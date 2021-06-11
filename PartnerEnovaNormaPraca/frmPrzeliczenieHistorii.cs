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
    public partial class frmPrzeliczenieHistorii : Form
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

        public frmPrzeliczenieHistorii()
        {
            InitializeComponent();
        }

        private void frmPrzeliczenieHistorii_Load(object sender, EventArgs e)
        {
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

        private void btnPrzelicz_Click(object sender, EventArgs e)
        {
            //Miesiąc na jaki wyliczamy dane
            int rok = int.Parse(cbxRok.Text);
            int mies = MiesiacToInt(cbxMiesiac.Text);
            Date dataOd = new Date(rok, mies, 1);
            Date dataDo = dataOd.LastDayMonth();

            connection = new SqlConnection(connectionString);
            connection.Open();

            sql = "SELECT ID, PracID, Year, Month, WydzialID, HistID, OkresOd, OkresDo, Norma, NormaMies, Praca, OdchylkiPlus, OdchylkiMinus, Stawka, Obywatelstwo FROM P_HistCzas WHERE (Year = " + rok.ToString() + ") AND (Month = " + mies.ToString() + ")";
            sCommand = new SqlCommand(sql, connection);
            da = new SqlDataAdapter(sCommand);
            cb = new SqlCommandBuilder(da);
            dt = new System.Data.DataTable();
            //da.Fill(dt);
            ds = new DataSet();
            da.Fill(ds, "Historia");
            dt = ds.Tables["Historia"];


            listBox1.Items.Add("Okres " + dataOd.ToString() + ".." + dataDo.ToString());
            listBox1.Items.Add("---");

            foreach (Object obj in arr)
            {
                Pracownik prac = (Pracownik)obj;
                Soneta.Kalend._Pracownik.CzasPracyEtatWorker cpw = new Soneta.Kalend._Pracownik.CzasPracyEtatWorker();
                cpw.Pracownik = prac;
                int pracId = prac.ID;

                listBox1.Items.Add(prac.Kod.ToString());
                foreach (PracHistoria ph in prac.Historia.GetIntersectedRows(new FromTo(dataOd,dataDo)))
                {
                    Date okresOd = ph.Aktualnosc.From;
                    Date okresDo = ph.Aktualnosc.To;
                    if (okresOd < dataOd)// || okresOd == Date.Empty)
                        okresOd = dataOd;
                    if (okresDo > dataDo)// || okresDo == Date.Empty)
                        okresDo = dataDo;
                    int histId = ph.ID;
                    string wydzial = "";
                    int wydzialId = 0;
                    FromTo okres = new FromTo();
                    double norma = 0;
                    double normaMies = 0;
                    double praca = 0;
                    double odchPl = 0;
                    double odchMi = 0;
                    decimal stawka = 0;
                    string obywatelstwo = "";
                    Soneta.Kalend.Kalendarz kal = ph.Etat.Kalendarz;
                    //listBox1.Items.Add("okresOd: " + okresOd.ToString() + " okresDo: " + okresDo.ToString());
                    //try { 
                    //listBox1.Items.Add("wydział: " + ph.Etat.Wydzial.Kod);
                    //}
                    //catch (Exception ex) { }

                    try
                    {
                        wydzialId = ph.Etat.Wydzial.ID;
                        //wydzial = ph.Etat.Wydzial.Kod;
                    }
                    catch (Exception ex) { }
                    try
                    {
                        //wydzialId = ph.Etat.Wydzial.ID;
                        wydzial = ph.Etat.Wydzial.Kod;
                    } catch (Exception ex) { }

                    try
                    {
                        okres = ph.Etat.Okres;
                        if (okresOd < okres.From)
                            okresOd = okres.From;
                        if (okresDo > okres.To)
                            okresDo = okres.To;
                    }
                    catch (Exception ex) { }

                    Soneta.Kalend._Pracownik.CzasPracyWorker cpw2 = new Soneta.Kalend._Pracownik.CzasPracyWorker();
                    cpw2.Pracownik = prac;
                    cpw2.Okres = new FromTo(dataOd, dataDo);
                    normaMies = cpw2.Norma.Czas.TotalHours;


                    if (wydzial != "" && okresOd <= okresDo)
                    {
                        listBox1.Items.Add("  wydział " + wydzial);
                        listBox1.Items.Add("  okres " + okresOd.ToString() + " " + okresDo.ToString());
                        //cpw.Okres = new FromTo(okresOd.FirstDayMonth(), okresDo.LastDayMonth());
                        //normaMies = cpw.Norma.Czas.TotalHours;
                        cpw.Okres = new FromTo(okresOd, okresDo);
                        norma = cpw.Norma.Czas.TotalHours;
                        praca = cpw.Praca.Czas.TotalHours;
                        odchPl = cpw.Odchylka.Plus.TotalHours;
                        odchMi = cpw.Odchylka.Minus.TotalHours;
                        stawka = ph.Etat.Zaszeregowanie.Stawka.Value;
                        obywatelstwo = ph.Obywatelstwo.Nazwa;

                        listBox1.Items.Add("   norma miesiąca " + normaMies.ToString());
                        listBox1.Items.Add("   norma " + norma.ToString());
                        listBox1.Items.Add("   praca " + praca.ToString());
                        listBox1.Items.Add("   odch + " + odchPl.ToString());
                        listBox1.Items.Add("   odch - " + odchMi.ToString());
                        listBox1.Items.Add("   stawka " + stawka.ToString());

                        

                        bool contains = dt.AsEnumerable().Any(row => histId == row.Field<int>("HistID"));
                        if (contains)
                        {
                            DataRow[] dr = ds.Tables["Historia"].Select("HistID = '" + histId.ToString() + "'");
                            dr[0]["Norma"] = norma;
                            dr[0]["NormaMies"] = normaMies;
                            dr[0]["Praca"] = praca;
                            dr[0]["WydzialID"] = wydzialId;
                            dr[0]["OkresOd"] = (DateTime)okresOd;
                            dr[0]["OkresDo"] = (DateTime)okresDo;
                            dr[0]["Norma"] = norma;
                            dr[0]["Praca"] = praca;
                            dr[0]["OdchylkiPlus"] = odchPl;
                            dr[0]["OdchylkiMinus"] = odchMi;
                            dr[0]["Stawka"] = stawka;
                            dr[0]["Obywatelstwo"] = obywatelstwo;
                        }
                        else
                        {
                            DataRow dr = dt.NewRow();
                            dr["PracID"] = pracId;
                            dr["Year"] = rok;
                            dr["Month"] = mies;
                            dr["HistID"] = histId;
                            dr["WydzialID"] = wydzialId;
                            dr["OkresOd"] = (DateTime)okresOd;
                            dr["OkresDo"] = (DateTime)okresDo;
                            dr["Norma"] = norma;
                            dr["NormaMies"] = normaMies;
                            dr["Praca"] = praca;
                            dr["OdchylkiPlus"] = odchPl;
                            dr["OdchylkiMinus"] = odchMi;
                            dr["Stawka"] = stawka;
                            dr["Obywatelstwo"] = obywatelstwo;
                            dt.Rows.Add(dr);
                        }

                        try
                        {
                            da.UpdateCommand = cb.GetUpdateCommand();
                            da.Update(dt);
                        }
                        catch (Exception ex)
                        {
                            listBox1.Items.Add(ex.ToString());
                        }
                        connection.Close();
                        listBox1.Items.Add("==== KONIEC ====");
                    }

                    //FromTo efektywnyOkres = new FromTo();
                    //FromTo okresZatrudnienia = new FromTo();


                    //try
                    //{
                    //    efektywnyOkres = ph.Etat.EfektywnyOkres;
                    //}
                    //catch (Exception ex) { }


                    //try
                    //{
                    //    okresZatrudnienia = ph.Etat.OkresZatrudnienia;
                    //}
                    //catch (Exception ex) { }


                    //listBox1.Items.Add("  ID " + histId.ToString() + " efektywny okres " + efektywnyOkres.ToString() + " etat okres " + okres.ToString() + " okres zatrudnienia " + okresZatrudnienia.ToString());

                }
                listBox1.Items.Add("---");
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

        private void lblConnection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmConnection control = new frmConnection();
            control.Server = server;
            control.User = user;
            control.Database = database;
            control.Show();
        }
    }
}
