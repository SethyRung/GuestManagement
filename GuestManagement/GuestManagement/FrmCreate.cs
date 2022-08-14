using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace GuestManagement
{
    public partial class FrmCreate : Form
    {
        public FrmCreate(FrmLogin frmLogin)
        {
            InitializeComponent();

            cboType.DataSource = new string[] { "ពិធីរៀបអាពាហ៍ពិពាហ៍", "ពិធីឡើងគេហដ្ឋានថ្មី" };

            btnCreate.Click += (sender, e) =>
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
                    FileInfo gusFile = new FileInfo(@"C:\Sethy Program\guest.json");

                    try
                    {
                        if (!gusFile.Exists)
                            gusFile.Create();

                        DialogResult re = MessageBox.Show("Make sure all your data is accurate.Note: 'Account after creation can not be changed'Do you really want to create ? ", "Create Account", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (re == DialogResult.Yes)
                        {
                            User u = new(txtEmail.Text.Trim(), txtPassword.Text.Trim(), txtMaleName.Text.Trim(), txtFemaleName.Text.Trim(), cboType.Text, dtpDate.Value);
                            string json;
                            FileStream fs = null;
                            if (usrFile.Exists)
                            {
                                try
                                {
                                    fs = new FileStream(usrFile.FullName, FileMode.Open, FileAccess.Read, FileShare.None);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Cannot read file'user.json'", "Error");
                                    return;
                                }
                                StreamReader sr = new StreamReader(fs);
                                json = sr.ReadToEnd();
                                sr.Close();
                                fs.Close();

                                //json
                                List<User> lstUser = JsonSerializer.Deserialize<List<User>>(json);
                                lstUser.Add(u);
                                json = JsonSerializer.Serialize<List<User>>(lstUser);
                                
                                fs = null;
                                try
                                {
                                    fs = new FileStream(usrFile.FullName, FileMode.Open, FileAccess.Write, FileShare.None);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Open file for writing: {ex.Message}");
                                    return;
                                }

                                StreamWriter sw = new StreamWriter(fs);
                                sw.Write(json);
                                sw.Close();
                                fs.Close();
                            }
                            else
                            {
                                fs = null;
                                try
                                {
                                    fs = new FileStream(usrFile.FullName, FileMode.Create, FileAccess.Write, FileShare.None);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Open file for writing: {ex.Message}");
                                    return;
                                }
                                StreamWriter sw = new StreamWriter(fs);
                                List<User> lstUser = new();
                                lstUser.Add(u);
                                json = JsonSerializer.Serialize<List<User>>(lstUser);
                                sw.Write(json);
                                sw.Close();
                                fs.Close();
                                
                            }
                            MessageBox.Show("Successfully created account", "Create Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            frmLogin.Enabled = true;
                            this.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            btnCancel.Click += (sender, e) =>
            {
                if (MessageBox.Show("Do you want to cancel?", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    frmLogin.Enabled = true;
                    this.Close();
                }
            };

            cboType.SelectedIndexChanged += (sender, e) =>
            {
                if (cboType.SelectedIndex == 1)
                {
                    lblMaleTitle.Text = "លោក";
                    lblFemaleTitle.Text = "អ្នកស្រី";
                }
                else
                {
                    lblMaleTitle.Text = "ឈ្មោះកូនប្រុស";
                    lblFemaleTitle.Text = "ឈ្មោះកូនស្រី";
                }
            };

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
        }
    }
}
