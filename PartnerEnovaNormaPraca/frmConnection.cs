using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using Microsoft.Win32;

namespace PartnerEnovaNormaPraca
{
    public partial class frmConnection : Form
    {
        private string server = "";
        private string database = "";
        private string user = "";
        private string password = "";

        public string Server
        {
            get { return server; }
            set { server = value; }
        }
        public string Database
        {
            get { return database; }
            set { database = value; }
        }
        public string User
        {
            get { return user; }
            set { user = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public frmConnection()
        {
            InitializeComponent();
        }

        private void frmConnection_Load(object sender, EventArgs e)
        {
            txtServer.Text = server;
            txtUser.Text = user;
            txtDatabase.Text = database;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            server = txtServer.Text;
            user = txtUser.Text;
            database = txtDatabase.Text;
            password = txtPassword.Text;

            //Sprawdzamy czy jest klucz w rejestrze i jeśli takiego nie ma to tworzymy
            RegistryKey rKey = null;
            try
            {
                rKey = Registry.CurrentUser.OpenSubKey("Software\\Partner-ip\\EnovaNormaPraca", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (rKey == null)
            {
                rKey = Registry.CurrentUser.OpenSubKey("Software", true);
                rKey.CreateSubKey("Partner-ip\\EnovaNormaPraca");
                rKey.Close();
            }

            //Zapisujemy do rejestru dane połączenia z bazą
            RegistryKey rKeyValue = null;
            try
            {
                rKeyValue = Registry.CurrentUser.OpenSubKey("Software\\Partner-ip\\EnovaNormaPraca", true);
                rKeyValue.SetValue("Server", server);
                rKeyValue.SetValue("User", user);
                rKeyValue.SetValue("Password", Encrypt(password));
                rKeyValue.SetValue("Database", database);
                rKeyValue.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        #region encryption
        //Funkcja szyfrująca
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "EIDMSU234MDNO34";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        //Funkcja deszyfrująca
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "EIDMSU234MDNO34";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion

    }
}
