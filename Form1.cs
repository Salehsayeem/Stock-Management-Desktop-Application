using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportingTool
{
    public partial class ManageProduct : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-DB29N8A;Initial Catalog=Stock;Integrated Security=True");
        public ManageProduct()
        {
            Thread t = new Thread(new ThreadStart(StartForm));
            t.Start();
            Thread.Sleep(5000);
            InitializeComponent();
            t.Abort();
        }

        private void ManageProduct_Load(object sender, EventArgs e)
        {
            
            label1.BackColor = Color.Cornsilk;
            btnDisplay.BackColor = Color.GhostWhite;
            btnAdd.BackColor = Color.Chartreuse;
            btnDelete.BackColor = Color.Red;
            btnUpdate.BackColor = Color.Yellow;
            DisplayComboBox();
            Date.Text = DateTime.Now.ToString("d");
        }

        public void StartForm()
        {
            Application.Run(new SplashScreen());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtProductName.Text == "")
            {
                MessageBox.Show("Please enter product Name");
            }
            else if (txtProductPrice.Text == "")
            {
                MessageBox.Show("Please enter product price");
            }
            else if (txtProductQuantity.Text == "")
            {
                MessageBox.Show("Please enter product quantity");
            }
            else
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Insert into[Product] values('" + txtProductName.Text + "','" + Convert.ToInt32(txtProductPrice.Text) +
                                  "','" + Convert.ToInt32(txtProductQuantity.Text) + "')";
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
                con.Close();
                DisplayData();
                ClearAll();
            }
        }
        private void btnSaveCustomerInformation_Click(object sender, EventArgs e)
        {
            if (txtCustomerName.Text == "")
            {
                MessageBox.Show("Please enter Customer Name");
            }
            else if (txtContact.Text == "")
            {
                MessageBox.Show("Please enter Customer Contact Details");
            }
            else
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Insert into[Customer] values('" + txtCustomerName.Text + "','" + txtCustomerAddress.Text +
                                  "','" + txtContact.Text + "')";
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                lblCustomerName.Text = txtCustomerName.Text.ToString();
                lblCustomerAddress.Text = txtCustomerAddress.Text.ToString();
                lblContact.Text = txtContact.Text.ToString();

                con.Close();
                ClearAll();
            }
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            
            if (txtProductName.Text == "")
            {
                MessageBox.Show("Please enter product Name to Delete ");
            }
            else
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Delete from [Product] where Name=('" + txtProductName.Text + "')";
                cmd.ExecuteNonQuery();
                con.Close();
                DisplayData();
                MessageBox.Show("Deleted Successfully");
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtProductName.Text == "")
            {
                MessageBox.Show("Please enter product Name to Update ");
            }
            else
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Update [Product] set Quantity='" + txtProductQuantity.Text + "'where Name = '" + txtProductName.Text + "'";
                cmd.ExecuteNonQuery();
                con.Close();
                DisplayData();
                ClearAll();
                txtProductName.Focus();
                MessageBox.Show("Quantity updated Successfully");
            }
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            DisplayData();
        }
        public void ClearAll()
        {
            txtProductName.Text = "";
            txtProductPrice.Text = "";
            txtProductQuantity.Text = "";
            txtCustomerName.Text = "";
            txtCustomerAddress.Text = "";
            txtContact.Text = "";
        }
        public void DisplayData()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from [Product]";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            productList.DataSource = dt;
            con.Close();
        }
        public void DisplayComboBox()
        {
            string con = (@"Data Source=DESKTOP-DB29N8A;Initial Catalog=Stock;Integrated Security=True");
            string query= "Select * from[Product]";
            SqlConnection conDatabase = new SqlConnection(con);
            SqlCommand cmdDatabase = new SqlCommand(query,conDatabase);
            SqlDataReader reader;
            try
            {
                conDatabase.Open();
                reader = cmdDatabase.ExecuteReader();
                while (reader.Read())
                {
                    string getString = reader.GetString(0);
                    comboAvailableProducts.Items.Add(getString);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //private void btnAvailableProducts_Click(object sender, EventArgs e)
        //{
        //    DisplayComboBox();
        //    //lblAvailableStocks = comboAvailableProducts.SelectedItem.ToString();
        //}
        private void comboBoxQuantity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboAvailableProducts.Text.Length > 0)
            {
                if (Convert.ToInt32(comboBoxQuantity.Text.Length) > 0)
                {
                    lblTotal.Text = (Convert.ToInt32(lblPrice.Text) * Convert.ToInt32(comboBoxQuantity.Text)).ToString();
                }
            }
            else
            {
                MessageBox.Show("Please Choose Product Name");
            }
            

            txtDiscount.Focus();
        }
        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            lblSubTotal.Text = txtDiscount.Text.Length > 0 ? (Convert.ToInt32(lblTotal.Text) - Convert.ToInt32(txtDiscount.Text)).ToString() : (Convert.ToInt32(lblTotal.Text)).ToString();
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            string[] arr = new string[6];
            arr[0] = comboAvailableProducts.SelectedItem.ToString();
            arr[1] = lblPrice.Text;
            arr[2] = comboBoxQuantity.Text;
            arr[3] = lblTotal.Text;
            arr[4] = txtDiscount.Text;
            arr[5] = lblSubTotal.Text;

            ListViewItem listViewItem = new ListViewItem(arr);
            listAddedProduct.Items.Add(listViewItem);
            int a=0;
            a = Convert.ToInt32(txtGrandTotal.Text);
            if (lblSubTotal.Text.Length > 0)
            {
                txtGrandTotal.Text = (Convert.ToInt32(lblSubTotal.Text) + a).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                txtGrandTotal.Text = (Convert.ToInt32(lblTotal.Text) + a).ToString(CultureInfo.InvariantCulture);
            }
            
            
            
        }
        private void comboAvailableProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string con = (@"Data Source=DESKTOP-DB29N8A;Initial Catalog=Stock;Integrated Security=True");
            string query = "Select * from [Product] where Name='"+comboAvailableProducts.Text+"'";
            SqlConnection conDatabase = new SqlConnection(con);
            SqlCommand cmdDatabase = new SqlCommand(query, conDatabase);
            SqlDataReader reader;
            try
            {
                conDatabase.Open();
                reader = cmdDatabase.ExecuteReader();
                while (reader.Read())
                {
                    if (comboAvailableProducts.SelectedIndex != -1)
                    {
                        string price = reader["Price"].ToString(); 
                        lblPrice.Text = price;
                        string quantity = reader["Quantity"].ToString();
                        lblAvailableStocks.Text = quantity;
                    }
                    else
                    {
                        MessageBox.Show("Combobox value has not changed");
                    }
                }

                comboBoxQuantity.Focus();
                comboBoxQuantity.Text = "";
                txtDiscount.Text = "";
                lblTotal.Text = "";
                lblSubTotal.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtGrandTotal.Text.Length > 0)
                {
                    txtRefund.Text = (float.Parse(txtPaidAmount.Text) - float.Parse(txtGrandTotal.Text))
                        .ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                txtRefund.Text = "";
            }
            
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listAddedProduct.SelectedItems.Count;)
            {
                if (listAddedProduct.SelectedItems[i].Selected)
                    txtGrandTotal.Text =
                        (Convert.ToInt32(txtGrandTotal.Text) -
                         Convert.ToInt32(listAddedProduct.SelectedItems[i].SubItems[5].Text)).ToString();
                listAddedProduct.SelectedItems[i].Remove();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            invoicePrintDialog.Document = invoicePrintDocument;

            DialogResult result = invoicePrintDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                invoicePrintDocument.Print();
            }

            PrintPreviewDialog dlg = new PrintPreviewDialog();
            dlg.Document = invoicePrintDocument;
            dlg.ShowDialog();
        }
        private void invoicePrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Ubuntu",14);
            float fontWeight = font.GetHeight();
            int headerX = 580;
            int startX = 100;
            int startY = 180;
            int offset = 30;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            graphics.DrawString("Md Maniruzzaman Shohag", font, new SolidBrush(Color.Black), headerX, 20);
            graphics.DrawString("Managing Director", new Font("Ubuntu", 12), new SolidBrush(Color.Gray), headerX, 38);
            graphics.DrawString("Shop No#140,Cumilla Tower", font, new SolidBrush(Color.Black), headerX, 60);
            graphics.DrawString("Shopping Complex, Laksham", font, new SolidBrush(Color.Black), headerX, 80);
            graphics.DrawString("Road,Kandirpar,Cumilla 3500", font, new SolidBrush(Color.Black), headerX, 100);
            graphics.DrawString("Invoice #", font, new SolidBrush(Color.Black), headerX, 140);
            graphics.DrawString("Date ", font, new SolidBrush(Color.Black), headerX, 160);
            graphics.DrawString(Date.Text, font, new SolidBrush(Color.Black), headerX +100, 160);
            graphics.DrawString("Name", font, new SolidBrush(Color.Black), startX, 180);
            graphics.DrawString(lblCustomerName.Text, font, new SolidBrush(Color.Black), startX+100, 180);

            graphics.DrawString("Address", font, new SolidBrush(Color.Black), startX, 200);
            graphics.DrawString(lblCustomerAddress.Text, font, new SolidBrush(Color.Black), startX + 100, 200);

            graphics.DrawString("Contact", font, new SolidBrush(Color.Black), startX, 220);
            graphics.DrawString(lblContact.Text, font, new SolidBrush(Color.Black), startX+100, 220);
            offset = offset + (int)fontWeight;
            graphics.DrawString("-------------------------------------------------------------------------------------------",font,new SolidBrush(Color.Black),startX,startY + offset );
            offset = offset + 30;
            graphics.DrawString("SL " +
                                "ItemDescription\t" +
                                "PRICE\t" +
                                "QUANTITY\t" +
                                "TOTAL", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            int serial = 1;
            for (int x = 0; x < listAddedProduct.Items.Count; x++)
            {
                if (listAddedProduct.Items[x].SubItems[4].Text.Length >0)
                {
                    graphics.DrawString((serial++) + 
                                        "\t" + listAddedProduct.Items[x].SubItems[0].Text + //name
                                        "\t" + listAddedProduct.Items[x].SubItems[1].Text + //price
                                        "\t" + listAddedProduct.Items[x].SubItems[2].Text + //quantity
                                        "\t\t" + listAddedProduct.Items[x].SubItems[5].Text
                        , font, new SolidBrush(Color.Black), startX, startY + offset);
                    graphics.DrawString("-----------------------------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset + 10);
                    offset = offset + 30;
                }
                else
                {
                    graphics.DrawString((serial++) + 
                                        "\t" + listAddedProduct.Items[x].SubItems[0].Text +
                                        "\t" + listAddedProduct.Items[x].SubItems[1].Text +
                                        "\t" + listAddedProduct.Items[x].SubItems[2].Text +
                                        "\t\t" + listAddedProduct.Items[x].SubItems[3].Text
                        , font, new SolidBrush(Color.Black), startX, startY + offset);
                    graphics.DrawString("-----------------------------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset + 10);
                    offset = offset + 30;
                }
                
            }
            offset = offset + (int)fontWeight;
            startY = startY + 100;
            graphics.DrawString("Grand Total = "+txtGrandTotal.Text+".00 Tk", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            offset = offset + (int)fontWeight;
            graphics.DrawString("Paid Amount = "+txtPaidAmount.Text+".00 Tk", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            offset = offset + (int)fontWeight;
            graphics.DrawString("Refund Amount = "+txtRefund.Text+".00 Tk", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            offset = offset + (int)fontWeight;
            graphics.DrawString("-----------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            offset = offset + (int)fontWeight;
            graphics.DrawString("Thank You For Visiting", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
        }

        
    }
}
