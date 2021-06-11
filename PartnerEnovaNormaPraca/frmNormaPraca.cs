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
using System.Security.Cryptography;
using System.IO;
using Microsoft.Win32;
using Soneta.Tools;
using Soneta.Types;
using Soneta.Business;
using Soneta.Business.Licence;
using Soneta.Business.Forms;
using Soneta.Business.App;
using Soneta.Kadry;
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
    public partial class frmNormaPraca : Form
    {
        Login login;
        Context context;
        ArrayList arr = new ArrayList();//Tablica z zaznaczonymi pracownikami
        string server = "";
        string user = "";
        string password = "";
        string database = "";

        private string connectionString;
        SqlConnection connection;
        string sql;
        SqlCommand sCommand;
        BindingSource source;
        DataView view;
        SqlDataAdapter da;
        SqlCommandBuilder cb;
        DataSet ds = new DataSet();
        DataTable dt;


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

        public frmNormaPraca()
        {
            InitializeComponent();
        }

        private void frmNormaPraca_Load(object sender, EventArgs e)
        {
            //listBox1.Items.Add(login.);

            //Sprawdzamy czy w rejestrach jest klucz z danymi połaczenia
            RegistryKey rKey = null;
            try
            {
                rKey = Registry.CurrentUser.OpenSubKey("Software\\Partner-ip\\EnovaNormaPraca", true);
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

            Date teraz = Date.Now.AddMonths(-1);
            cbxRok.Text = teraz.Year.ToString();
            cbxMiesiac.Text = MiesiacToString(teraz.Month);
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

        private void lblConnection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmConnection control = new frmConnection();
            control.Server = server;
            control.User = user;
            control.Database = database;
            control.Show();
        }

        private void btnPrzelicz_Click(object sender, EventArgs e)
        {
            //Miesiąc na jaki wyliczamy dane
            int rok = int.Parse(cbxRok.Text);
            int mies = MiesiacToInt(cbxMiesiac.Text);
            Date dataOd = new Date(rok, mies, 1);
            Date dataDo = dataOd.LastDayMonth();

            connection = new SqlConnection(connectionString);
            connection.Open();
            sql = "SELECT ID, PracID, Year, Month, Norma, Praca, N50, N100, NSW, Noc FROM P_NormaPraca WHERE (Year = " + rok.ToString() + ") AND (Month = " + mies.ToString() + ")";
            sCommand = new SqlCommand(sql, connection);
            da = new SqlDataAdapter(sCommand);
            cb = new SqlCommandBuilder(da);
            dt = new System.Data.DataTable();
            //da.Fill(dt);
            ds = new DataSet();
            da.Fill(ds, "Czasy");
            dt = ds.Tables["Czasy"];






            //context = Lo
            //listBox1.Items.Add("1");
            //Lecimy po pracownikach w tabeli
            foreach (Object obj in arr)
            {
                Pracownik prac = (Pracownik)obj;
                Soneta.Kalend._Pracownik.CzasPracyEtatWorker cpw = new Soneta.Kalend._Pracownik.CzasPracyEtatWorker();
                cpw.Pracownik = prac;
                cpw.Okres = new FromTo(dataOd, dataDo);
                int pracId = prac.ID;
                double norma = cpw.Norma.Czas.TotalHours;
                double praca = cpw.Praca.Czas.TotalHours;
                //double n50 = cpw.Nadgodziny.N50.TotalHours;
                //double n100 = cpw.Nadgodziny.N100.TotalHours;
                //double nSw = cpw.Nadgodziny.NSW.TotalHours;
                //double noc = cpw.Nocne.TotalHours;
                double n50 = 0;
                double n100 = 0;
                double nSw = 0;
                double noc = 0;

                foreach (Wyplata wyp in prac.Wyplaty)
                {
                    foreach (WypElement el in wyp.Elementy)
                    {
                        string rodzaj = el.Definicja.RodzajZrodla.ToString();
                        if (el.Okres.To >= cpw.Okres.From && el.Okres.From <= cpw.Okres.To)
                            switch (rodzaj)
                        {
                            case "NadgodzinyI":
                                n50 += el.Czas.TotalHours;
                                break;
                            case "NadgodzinyII":
                            case "NadgodzinyŚw":
                                n100 += el.Czas.TotalHours;
                                break;
                            case "Nocne":
                                noc += el.Czas.TotalHours;
                                break;
                            default:
                                break;
                        }
                    }
                }

                //DataRow[] foundPrac = dt.Select("ID = '" + pracId + "'");
                bool contains = dt.AsEnumerable().Any(row => pracId == row.Field<int>("PracID"));
                listBox1.Items.Add(prac.NazwiskoImię + " (" + prac.Kod + ") norma: " + norma.ToString() + "; praca: " + praca.ToString() + "; n50: " +
                    n50.ToString() + "; n100: " + n100.ToString() + "; nśw: " + nSw.ToString() + "; nocne: " + noc.ToString());
                if (contains)
                {
                    DataRow[] dr = ds.Tables["Czasy"].Select("PracID = '" + pracId.ToString() + "'");
                    dr[0]["Norma"] = norma;
                    dr[0]["Praca"] = praca;
                    dr[0]["N50"] = n50;
                    dr[0]["N100"] = n100;
                    dr[0]["NSW"] = nSw;
                    dr[0]["Noc"] = noc;

                }
                else
                {
                    //listBox1.Items.Add("1");
                    DataRow dr = dt.NewRow();
                    dr["PracID"] = pracId;
                    dr["Year"] = rok;
                    dr["Month"] = mies;
                    dr["Norma"] = norma;
                    dr["Praca"] = praca;
                    dr["N50"] = n50;
                    dr["N100"] = n100;
                    dr["NSW"] = nSw;
                    dr["Noc"] = noc;
                    dt.Rows.Add(dr);
                }
            }

            try
            {
                da.UpdateCommand = cb.GetUpdateCommand();
                da.Update(dt);
            }catch(Exception ex)
            {
                listBox1.Items.Add(ex.ToString());
            }
            connection.Close();
            listBox1.Items.Add("==== KONIEC ====");

            //using (Session session = login.CreateSession(false, false))
            //{
            //    KadryModule km = KadryModule.GetInstance(session);
            //    using (ITransaction t = session.Logout(true))
            //    {
            //        FromTo okres = (FromTo)session.Login.[typeof(FromTo)];
            //        listBox1.Items.Add(okres.ToString());
            //    }
            //}
        }

        private int MiesiacToInt(string mies)
        {
            int miesiac = 0;
            switch (mies.ToString())
            {
                case "styczeń":
                    miesiac = 1;
                    break;
                case "luty":
                    miesiac = 2;
                    break;
                case "marzec":
                    miesiac = 3;
                    break;
                case "kwiecień":
                    miesiac = 4;
                    break;
                case "maj":
                    miesiac = 5;
                    break;
                case "czerwiec":
                    miesiac = 6;
                    break;
                case "lipiec":
                    miesiac = 7;
                    break;
                case "sierpień":
                    miesiac = 8;
                    break;
                case "wrzesień":
                    miesiac = 9;
                    break;
                case "październik":
                    miesiac = 10;
                    break;
                case "listopad":
                    miesiac = 11;
                    break;
                case "grudzień":
                    miesiac = 12;
                    break;
                default:
                    break;
            }

            return miesiac;
        }

        private string MiesiacToString(int mies)
        {
            string miesiac = "";
            switch (mies.ToString())
            {
                case "1":
                    miesiac = "styczeń";
                    break;
                case "2":
                    miesiac = "luty";
                    break;
                case "3":
                    miesiac = "marzec";
                    break;
                case "4":
                    miesiac = "kwiecień";
                    break;
                case "5":
                    miesiac = "maj";
                    break;
                case "6":
                    miesiac = "czerwiec";
                    break;
                case "7":
                    miesiac = "lipiec";
                    break;
                case "8":
                    miesiac = "sierpień";
                    break;
                case "9":
                    miesiac = "wrzesień";
                    break;
                case "10":
                    miesiac = "październik";
                    break;
                case "11":
                    miesiac = "listopad";
                    break;
                case "12":
                    miesiac = "grudzień";
                    break;
                default:
                    break;
            }

            return miesiac;
        }

    }
}
