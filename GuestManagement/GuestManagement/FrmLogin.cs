using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace GuestManagement
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();



            btnPass.Click += (sender, e) =>
            {
                if (btnPass.Tag.Equals("Show"))
                {
                    btnPass.Image = Properties.Resources.invisible_24px;
                    btnPass.Tag = "UnShow";
                    txtPassword.PasswordChar = '\0';
                }
                else
                {
                    btnPass.Image = Properties.Resources.eye_24px;
                    btnPass.Tag = "Show";
                    txtPassword.PasswordChar = '*';
                }
            };

            this.Load += (sender, e) =>
            {
                DirectoryInfo dir = new DirectoryInfo(@"C:\Sethy Program");
                FileInfo guestFile = new FileInfo(@"C:\Sethy Program\guest.json");
                try
                {
                    if (!dir.Exists)
                        dir.Create();
                    if (!guestFile.Exists)
                        guestFile.Create();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnLogin.Click += (sender, e) =>
            {
                if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("Please enter email.", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmail.Focus();
                }
                else if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    MessageBox.Show("Please enter email.", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Focus();
                }
                else
                {
                    FileInfo usrFile = new FileInfo(@"C:\Sethy Program\user.json");
                    FileStream fs = null;
                    if (usrFile.Exists)
                    {
                        try
                        { 
                            fs = new FileStream(usrFile.FullName, FileMode.Open, FileAccess.Read, FileShare.None);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Cannot read file'user.json'{ex.Message}", "Error");
                            return;
                        }
                        StreamReader sr = new StreamReader(fs);
                        string json = sr.ReadToEnd();
                        sr.Close();
                        fs.Close();

                        //json and compare account
                        List<User> lstUser = JsonSerializer.Deserialize<List<User>>(json);
                        foreach(User user in lstUser)
                        {
                            if (user.Email.Equals(txtEmail.Text.Trim()) || user.Password.Equals(txtPassword.Text.Trim()))
                            {
                                this.Hide();
                                FrmMain main = new(user);
                                main.ShowDialog();
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Account isn't existed", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            };

            btnCreate.Click += (sender, e) =>
            {
                FrmCreate frmCreate = new FrmCreate(this);
                frmCreate.Show();
                this.Enabled = false;
            };
        }
    }
}
