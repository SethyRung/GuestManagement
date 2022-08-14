using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace GuestManagement
{
    public partial class FrmMain : Form
    {
        private bool isInsert;
        public FrmMain(User usr)
        {
            //usr = new("rungsethyhk@gmail.com", "Rs123451@", "Sethy", "Theavin", "ពិធីរៀបអាពាហ៍ពិពាហ៍", new DateTime(2030, 12, 31));
            InitializeComponent();

            cboGender.DataSource = new string[] { "ប្រុស", "ស្រី" };
            cboMType.DataSource = new string[] { "ប្រាក់រៀល", "ប្រាក់ដុល្លារ" };
            cboGender.SelectedIndex = -1;
            cboMType.SelectedIndex = -1;
            btnEdit.Enabled = false;

            this.Load += (s, e) =>
            {
                if (usr.Type.Equals("ពិធីរៀបអាពាហ៍ពិពាហ៍"))
                {
                    lblTitle.Text = "សិរីសួស្ដីពិធីអាពាហ៍ពិពាហ៍";
                    lblMaleTitle.Text = "កូនប្រុសនាម";
                    lblFemaleTitle.Text = "កូនស្រីនាម";
                    picTitle.Image = Properties.Resources.two_hearts_100;
                }
                else
                {
                    lblTitle.Text = "រីករាយពិធីឡើងគេហដ្ឋានថ្មី";
                    lblMaleTitle.Text = "លោក";
                    lblFemaleTitle.Text = "អ្នកស្រី";
                    picTitle.Image = Properties.Resources.home_100;
                }
                lblMaleName.Text = usr.MaleName;
                lblFemaleName.Text = usr.FemaleName;
                lblDate.Text = usr.Date.ToString("D");

                OnOff(false);
                LoadData(usr);
            };

            txtPhone.KeyPress += (s, e) =>
            {
                char ch = e.KeyChar;
                if (!char.IsDigit(ch) && ch != (char)8)
                    e.Handled = true;
            };

            btnAdd.Click += (s, e) =>
            {
                if (btnAdd.Text.Equals("បញ្ចូលភ្ញៀវថ្មី"))
                {
                    Clear();
                    OnOff(true);
                    btnAdd.Text = "បោះបង់";
                    btnEdit.Text = "រក្សារទុក";
                    btnEdit.Enabled = true;
                    isInsert = true;
                }
                else
                {
                    if (MessageBox.Show("Do you want to cancel?", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        btnAdd.Text = "បញ្ចូលភ្ញៀវថ្មី";
                        btnEdit.Text = "កែទិន្នន័យភ្ញៀវ";
                        btnEdit.Enabled = false;
                        OnOff(false);
                        Clear();
                    }
                }
            };

            btnEdit.Click += (s, e) =>
            {
                if (btnEdit.Text.Equals("កែទិន្នន័យភ្ញៀវ"))
                {
                    OnOff(true);
                    btnAdd.Text = "បោះបង់";
                    btnEdit.Text = "រក្សារទុក";
                    isInsert = false;
                }
                else
                {
                    if (checkControl())
                    {
                        if (isInsert)
                            InsertGuest(usr);
                        else
                            EditGuest(usr);
                        LoadData(usr);
                    }
                }
            };

            dgvGuest.CellClick += (s, e) =>
            {
                if (dgvGuest.RowCount > 0)
                {
                    if (btnAdd.Text.Equals("បោះបង់") && btnEdit.Text.Equals("រក្សារទុក")) return;
                    btnEdit.Enabled = true;
                    int row = dgvGuest.CurrentRow.Index;
                    txtGName.Text = dgvGuest.Rows[row].Cells[0].Value.ToString();
                    if (dgvGuest.Rows[row].Cells[1].Value.ToString().Equals("ប្រុស"))
                        cboGender.SelectedIndex = 0;
                    else
                        cboGender.SelectedIndex = 1;
                    txtPhone.Text = dgvGuest.Rows[row].Cells[2].Value.ToString();
                    txtAddress.Text = dgvGuest.Rows[row].Cells[3].Value.ToString();
                    if (dgvGuest.Rows[row].Cells[4].Value.ToString().Equals("ប្រាក់រៀល"))
                        cboMType.SelectedIndex = 0;
                    else
                        cboMType.SelectedIndex = 1;
                    txtMoney.Text = dgvGuest.Rows[row].Cells[5].Value.ToString();
                }

            };
            txtSearch.KeyUp += (s, e) =>
            {
                if (txtSearch.Text.Length != 0)
                {
                    try
                    {
                        List<Guest> lst = new();
                        List<Guest> lstGuest = ReadGuest(usr.Email);
                        foreach (Guest g in lstGuest)
                        {
                            if (SearchGuest(txtSearch.Text.Length - 1, g.Name))
                                lst.Add(g);
                        }
                        dgvGuest.DataSource = lst;
                    }
                    catch (Exception ex) { }
                }
                else LoadData(usr);
            };
        }
        private bool SearchGuest(int n, String name)
        {
            if (n == 0)
                return (name.IndexOf(txtSearch.Text[n]) != -1);
            else
                return (SearchGuest(n - 1, name) && (name.IndexOf(txtSearch.Text[n]) != -1));
        }
        private void WriteGuest(Guest g)
        {
            FileStream fs;
            List<Guest> lstGuest = new List<Guest>();
            string json;
            try
            {
                fs = new FileStream(@"C:\Sethy Program\guest.json", FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader sr = new StreamReader(fs);
                json = sr.ReadToEnd();
                if (!json.Equals(""))
                    lstGuest = JsonSerializer.Deserialize<List<Guest>>(json);

                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Reading: {ex.Message}", "Error");
            }

            lstGuest.Add(g);
            json = JsonSerializer.Serialize<List<Guest>>(lstGuest);

            try
            {
                fs = new FileStream(@"C:\Sethy Program\guest.json", FileMode.Open, FileAccess.Write, FileShare.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Writing: {ex.Message}", "Error");
                return;
            }
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(json);
            sw.Close();
            fs.Close();
        }
        private List<Guest> ReadGuest(string UserEmail)
        {
            List<Guest> lstGuest = new List<Guest>();
            List<Guest> lst = new List<Guest>();
            FileStream fs;
            try
            {
                fs = new FileStream(@"C:\Sethy Program\guest.json", FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader sr = new StreamReader(fs);
                string json = sr.ReadToEnd();
                if (!json.Equals(""))
                    lst = JsonSerializer.Deserialize<List<Guest>>(json);
                if (lst.Count > 0)
                    foreach (Guest g in lst)
                        if (g.UserEmail.Equals(UserEmail))
                            lstGuest.Add(g);

                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Reading: {ex.Message}", "Error");
            }
            return lstGuest;
        }
        private void LoadData(User usr)
        {
            double KhTotal = 0, UsTotal = 0;
            List<Guest> guests = ReadGuest(usr.Email);
            dgvGuest.DataSource = guests;
            dgvGuest.Columns["Name"].Width = 300;
            dgvGuest.Columns["Name"].HeaderText = "ឈ្មោះ";
            dgvGuest.Columns["Gender"].Width = 160;
            dgvGuest.Columns["Gender"].HeaderText = "ភេទ";
            dgvGuest.Columns["Phone"].Width = 250;
            dgvGuest.Columns["Phone"].HeaderText = "លេខទូរសព្ទ";
            dgvGuest.Columns["Address"].Width = 270;
            dgvGuest.Columns["Address"].HeaderText = "អាសយដ្ឋាន";
            dgvGuest.Columns["MoneyType"].Width = 250;
            dgvGuest.Columns["MoneyType"].HeaderText = "ប្រភេទក្រដាសប្រាក់";
            dgvGuest.Columns["Money"].Width = 240;
            dgvGuest.Columns["Money"].HeaderText = "ទឹកប្រាក់";
            dgvGuest.Columns["UserEmail"].Visible = false;
            foreach(Guest g in guests)
            {
                if (g.MoneyType.Equals("ប្រាក់រៀល"))
                    KhTotal += g.Money;
                else
                    UsTotal += g.Money;
            }
            txtKhTotal.Text = KhTotal.ToString() + " ៛";
            txtUsTotal.Text = UsTotal.ToString() + " $";
        }
        private bool checkControl()
        {
            bool isNotNull = false;
            if (string.IsNullOrEmpty(txtGName.Text.Trim()))
                MessageBox.Show("Please enter guest name", "Messing");
            else if (string.IsNullOrEmpty(cboGender.Text))
                MessageBox.Show("Please choose guest gender", "Messing");
            else if (string.IsNullOrEmpty(txtPhone.Text.Trim()))
                MessageBox.Show("Please enter guest phone", "Messing");
            else if (string.IsNullOrEmpty(txtAddress.Text.Trim()))
                MessageBox.Show("Please enter guest address", "Messing");
            else if (string.IsNullOrEmpty(cboMType.Text))
                MessageBox.Show("Please choose money type", "Messing");
            else if (string.IsNullOrEmpty(txtMoney.Text.Trim()))
                MessageBox.Show("Please enter money", "Messing");
            else
                isNotNull = true;
            return isNotNull;
        }
        private void InsertGuest(User usr)
        {
            string name = txtGName.Text.Trim();
            string gender = cboGender.Text;
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string mType = cboMType.Text;
            double money = double.Parse(txtMoney.Text.Trim());
            Guest g = new(name, gender, phone, address, mType, money, usr.Email);
            WriteGuest(g);
            btnAdd.Text = "បញ្ចូលភ្ញៀវថ្មី";
            btnEdit.Text = "កែទិន្នន័យភ្ញៀវ";
            btnEdit.Enabled = false;
            OnOff(false);
            Clear();
        }
        private void EditGuest(User usr)
        {
            string name = txtGName.Text.Trim();
            string gender = cboGender.Text;
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string mType = cboMType.Text;
            double money = double.Parse(txtMoney.Text.Trim());
            Guest g = new(name, gender, phone, address, mType, money, usr.Email);
            List<Guest> lstGuest = new List<Guest>();
            string json;
            FileStream fs;
            try
            {
                fs = new FileStream(@"C:\Sethy Program\guest.json", FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader sr = new StreamReader(fs);
                json = sr.ReadToEnd();
                if (!json.Equals(""))
                    lstGuest = JsonSerializer.Deserialize<List<Guest>>(json);
                sr.Close();
                fs.Close();
                
                int index = -1;
                foreach (Guest ge in lstGuest)
                {
                    if (ge.Name.Equals(g.Name) && ge.UserEmail.Equals(g.UserEmail))
                    {
                        index = lstGuest.IndexOf(ge);
                        break;
                    }
                }
                if (index > -1)
                {
                    lstGuest.RemoveAt(index);
                    lstGuest.Insert(index, g);
                }
                json = JsonSerializer.Serialize<List<Guest>>(lstGuest);
                try
                {
                    fs = new FileStream(@"C:\Sethy Program\guest.json", FileMode.Truncate, FileAccess.Write, FileShare.None);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(json);
                    sw.Close();
                    fs.Close();
                    btnAdd.Text = "បញ្ចូលភ្ញៀវថ្មី";
                    btnEdit.Text = "កែទិន្នន័យភ្ញៀវ";
                    btnEdit.Enabled = false;
                    OnOff(false);
                    Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Writing: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Reading: {ex.Message}", "Error");
            }
        }
        private void OnOff(bool b)
        {
            txtGName.Enabled = b;
            txtAddress.Enabled = b;
            txtPhone.Enabled = b;
            txtMoney.Enabled = b;
            cboGender.Enabled = b;
            cboMType.Enabled = b;
        }
        private void Clear()
        {
            txtGName.Text = null;
            txtAddress.Text = null;
            txtPhone.Text = null;
            txtMoney.Text = null;
            cboGender.Text = null;
            cboMType.Text = null;
        }

    }
}
