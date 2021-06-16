using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Security.Cryptography;
using Soneta.Tools;
using Soneta.Types;
using Soneta.Business;
using Soneta.Business.Licence;
using Soneta.Business.Forms;
using Soneta.Business.App;
using Soneta.Core;
using Soneta.Kadry;
using Soneta.Place;

namespace PartnerEnovaNormaPraca
{
    public partial class frmPracKody : Form
    {
        Login login;
        Context context;
        ArrayList arr = new ArrayList();//Tablica z zaznaczonymi pracownikami
        List<PracownikKod> ListaPracownikow = new List<PracownikKod>();
        Wydzial wydz = null;

        string server = "";
        string user = "";
        string password = "";
        string database = "";

        private string connectionString;
        //SqlConnection connection;
        //string sql;
        //SqlCommand sCommand;
        //BindingSource source;
        //DataView view;
        //SqlDataAdapter da;
        //SqlCommandBuilder cb;
        //DataSet ds = new DataSet();
        //DataTable dt;
        bool edycja = false;

        public ArrayList Arr
        {
            get { return arr; }
            set { arr = value; }
        }

        internal void InitContext(Context cx)
        {
            //obiekt logowania przejęty z kontekstu
            login = (Login)cx[typeof(Login)];
            context = cx;
        }

        public frmPracKody()
        {
            InitializeComponent();
        }

        private void frmPracKody_Load(object sender, EventArgs e)
        {
            //Sprawdzamy czy w rejestrach jest klucz z danymi połaczenia
            RegistryKey rKey = null;
            try
            {
                rKey = Registry.CurrentUser.OpenSubKey("Software\\Partner-ip\\EnovaLSJ", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //Pobieramy dane połączenia
            if (rKey != null)
            {
                try
                {
                    server = (string)rKey.GetValue("Server");
                    user = (string)rKey.GetValue("User");
                    password = Decrypt((string)rKey.GetValue("Password"));
                    database = (string)rKey.GetValue("Database");
                }
                catch (Exception ex) { }
            }

            connectionString = @"Data source=" + server.ToString() + "; Initial Catalog=" + database.ToString() + "; User Id=" + user + "; Password=" + password + ";";

            // Pobieramy z kontekstu datę aktualności
            Date dataAktualnosci = Date.Empty;
            if (context.Contains(typeof(ActualDate)))
            {
                dataAktualnosci = ((ActualDate)context[typeof(ActualDate)]).Actual;
            }

            // Pobieramy z kontekstu okres
            FromTo okres = new FromTo(new DateTime(dataAktualnosci.Year, dataAktualnosci.Month, 1), new DateTime(dataAktualnosci.Year, dataAktualnosci.Month, 1).AddMonths(1).AddDays(-1));

            // Pobieramy z kontekstu wydział
            if (context.Contains(typeof(Wydzial)))
            {
                wydz = ((Wydzial)context[typeof(Wydzial)]);
            }

            label1.Text = dataAktualnosci.ToString() + " " + okres.ToString() + " " + wydz.Kod;

            string sql = "SELECT Pracownicy.ID, " +
                "Pracownicy.Kod, " +
                "Pracownicy.Nazwisko, " +
                "Pracownicy.Imie, " +
                "Wydzialy.ID AS WydzId, " +
                "Wydzialy.Kod AS WydzKod, " +
                "PracHistorie.AktualnoscFrom, " +
                "PracHistorie.AktualnoscTo, " +
                "ISNULL((SELECT TOP(1) Kod FROM P_PracKody WHERE (PracID = Pracownicy.ID) AND (P_PracKody.WydzID = " + wydz.ID + ")), '') AS PracKod, " +
                "ISNULL((SELECT TOP(1) ID FROM P_PracKody AS P_PracKody_1 WHERE (PracID = Pracownicy.ID) AND (P_PracKody_1.WydzID = " + wydz.ID + ")), 0) AS PracKodId, " +
                "PracHistorie.EtatOkresFrom, " +
                "PracHistorie.EtatOkresTo " +
                "FROM Pracownicy INNER JOIN PracHistorie ON Pracownicy.ID = PracHistorie.Pracownik INNER JOIN Wydzialy ON PracHistorie.EtatWydzial = Wydzialy.ID " +
                "WHERE(PracHistorie.AktualnoscFrom <= '" + dataAktualnosci.ToString("yyyy-MM-dd") + "') " +
                    "AND (PracHistorie.AktualnoscTo >= '" + dataAktualnosci.ToString("yyyy-MM-dd") + "') " +
                    "AND (Wydzialy.Kod = '" + wydz.Kod + "') " +
                    "AND (PracHistorie.EtatOkresFrom <= '" + okres.To.ToString("yyyy-MM-dd") + "') " +
                    "AND (PracHistorie.EtatOkresTo >= '" + okres.From.ToString("yyyy-MM-dd") + "')" +
                "ORDER BY Pracownicy.Nazwisko";

            SqlConnection connection;
            connection = new SqlConnection(connectionString);
            connection.Open();
            DataTable dt = new DataTable();
            SqlCommand sCommand = new SqlCommand(sql, connection);
            SqlDataAdapter da = new SqlDataAdapter(sCommand);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PracownikKod prk = new PracownikKod();
                    try
                    {
                        prk.ID = (int)dt.Rows[i]["ID"];
                    }
                    catch (Exception ex) { }
                    try
                    {
                        prk.Kod = (string)dt.Rows[i]["Kod"];
                    }
                    catch (Exception ex) { }
                    try
                    {
                        prk.Nazwisko = (string)dt.Rows[i]["Nazwisko"];
                    }
                    catch (Exception ex) { }
                    try
                    {
                        prk.Imie = (string)dt.Rows[i]["Imie"];
                    }
                    catch (Exception ex) { }
                    try
                    {
                        prk.WydzID = (int)dt.Rows[i]["WydzId"];
                    }
                    catch (Exception ex) { }
                    try
                    {
                        prk.WydzKod = (string)dt.Rows[i]["WydzKod"];
                    }
                    catch (Exception ex) { }
                    try
                    {
                        prk.PracKodID = (int)dt.Rows[i]["PracKodId"];
                    }
                    catch (Exception ex) { }
                    try
                    {
                        prk.PracKod = (string)dt.Rows[i]["PracKod"];
                    }
                    catch (Exception ex) { }
                    ListaPracownikow.Add(prk);
                }

                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.ReadOnly = false;
                dataGridView1.DataSource = ListaPracownikow;
                dataGridView1.Columns.Add("ID", "ID");
                dataGridView1.Columns.Add("Kod", "Kod");
                dataGridView1.Columns.Add("Nazwisko", "Nazwisko");
                dataGridView1.Columns.Add("Imie", "Imię");
                dataGridView1.Columns.Add("PracKodID", "PracKodiD");
                dataGridView1.Columns.Add("PracKod", "Kod u kotrah");
                dataGridView1.Columns[0].DataPropertyName = "ID";
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].DataPropertyName = "Kod";
                dataGridView1.Columns[1].Width = 60;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].DataPropertyName = "Nazwisko";
                dataGridView1.Columns[2].Width = 200;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].DataPropertyName = "Imie";
                dataGridView1.Columns[3].Width = 200;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].DataPropertyName = "PracKodID";
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].DataPropertyName = "PracKod";
                dataGridView1.Columns[5].Width = 150;
                dataGridView1.Columns[5].ReadOnly = false;

                //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

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

        /// <summary>
        /// 
        /// </summary>
        public class PracownikKod
        {
            public int ID { get; set; }// Id pracownika w enovej
            public string Kod { get; set; }// Kod LSJ
            public string Nazwisko { get; set; }
            public string Imie { get; set; }
            public int WydzID { get; set; }// Id wydziału
            public string WydzKod { get; set; }// Kod wydziału
            public int PracKodID { get; set; }// Id rekordu w tabeli kodów
            public string PracKod { get; set; }// Kod u kontrahenta
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("1");
            int id = 0;
            string pracKod = "";
            try
            {
                id = (int)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells["ID"].Value;
            }
            catch (Exception ex) { }

            try
            {
                pracKod = (string)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells["PracKod"].Value;
            }
            catch (Exception ex) { }
            //MessageBox.Show("id " + id.ToString() + " kod " + pracKod);
            if (id != 0)
            {
                //MessageBox.Show("zmiana");
                int index = ListaPracownikow.FindIndex(c => c.ID == id);
                ListaPracownikow[index].PracKod = pracKod;
                dataGridView1.DataSource = ListaPracownikow;
                edycja = true;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //int id = 0;
            //try
            //{
            //    id = (int)dataGridView1.CurrentCell.ColumnIndex;
            //}
            //catch(Exception ex) { }


        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            if (ListaPracownikow.Count > 0)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (PracownikKod prk in ListaPracownikow)
                    {
                        if (prk.PracKodID == 0)
                        {
                            if (prk.PracKod != "")
                            {
                                //MessageBox.Show("insert");
                                string sql = "INSERT INTO P_PracKody (PracID, WydzID, Kod) " +
                                    "VALUES ('" + prk.ID.ToString() + "','" + prk.WydzID.ToString() + "','" + prk.PracKod.ToString() + "')";
                                try
                                {
                                    SqlCommand comm = new SqlCommand(sql, conn);
                                    comm.ExecuteNonQuery();
                                }
                                catch (Exception ex) { MessageBox.Show("101: " + ex.Message.ToString() + "\r\n" + sql); }
                            }
                        }
                        else
                        {
                            //MessageBox.Show("update");
                            string sql = "UPDATE P_PracKody SET Kod = '" + prk.PracKod.ToString() + "' WHERE (ID = " + prk.PracKodID.ToString() + ")";
                            try
                            {
                                SqlCommand comm = new SqlCommand(sql, conn);
                                comm.ExecuteNonQuery();
                            }
                            catch (Exception ex) {MessageBox.Show("102: " + ex.Message.ToString() + "\r\n" + sql);}

                        }
                    }

                    conn.Close();
                }
            }

            this.Close();
        }
    }
}
