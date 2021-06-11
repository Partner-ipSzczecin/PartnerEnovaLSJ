using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Security.Cryptography;
using Soneta.Business;
using Soneta.Business.App;
using Soneta.Kadry;
using Soneta.Kalend;
using Soneta.Types;
using Soneta.Kasa;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using static PartnerEnovaNormaPraca.classes;
using static PartnerEnovaNormaPraca.EnovaLSJ;

/*
     <DzienPracy>
        <Pracownik>006</Pracownik>
        <Data>2015-09-15</Data>
        <OdGodziny>07:30</OdGodziny>
        <Czas>08:30</Czas>
     </DzienPracy>

    <DzienPracy>
        <Pracownik>006</Pracownik>
        <Data>2015-09-16</Data>
        <Strefy>
            <StrefaPracy Definicja="Praca w normie" OdGodziny="08:00" Czas="04:00" ></StrefaPracy>
            <StrefaPracy Definicja="Dyżur w pracy" OdGodziny="13:00" Czas="04:00" ></StrefaPracy>
        </Strefy>
    </DzienPracy>
     */

namespace PartnerEnovaNormaPraca
{
    public partial class frmImportCzasu : Form
    {

        /// <summary>
        /// To jest pole zawierające informacje o loginie do bazy danych,
        /// na którym będą robione wszystkie operacje.
        /// </summary>
        Login login;
        Log log;

        private Thread trd;
        string server = "";
        string user = "";
        string password = "";
        string database = "";
        private string connectionString;
        string fileDirectory = "";
        List<string> filePath = new List<string>();
        string fileName = "";
        string firma = "";
        
        internal void InitContext(Context cx)
        {
            //obiekt logowania przejęty z kontekstu
            login = (Login)cx[typeof(Login)];
        }
        public frmImportCzasu()
        {
            InitializeComponent();
        }

        private void frmImportCzasu_Load(object sender, EventArgs e)
        {
            lblVersion.Text = Info.Version;

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


        }

        private void btnPlik_Click(object sender, EventArgs e)
        {
            log = new Log("Import czasu pracy MOMOX", true);

            filePath.Clear();
            filePath.Clear();
            // Pobieramy firmę dla której robimy import
            try
            {
                firma = cbxFirma.Text;
            }
            catch (Exception ex) { }

            if (firma != "")
            {
                switch (firma)
                {
                    //case "Ammeraal":
                    //    openFileDialog1.Multiselect = true;
                    //    openFileDialog1.Filter = "Pliki XML (*.XML)|*.XML|" +
                    //        "Wszystkie pliki (*.*)|*.*";
                    //    break;
                    case "Incom":
                        openFileDialog1.Multiselect = false;
                        openFileDialog1.Filter = "Pliki Excel (*.XLS, *.XLSX)|*.XLS;*.XLSX|" +
                            "Wszystkie pliki (*.*)|*.*";
                        break;
                    //case "KK Wind Solutions":
                    //    openFileDialog1.Multiselect = false;
                    //    openFileDialog1.Filter = "Pliki Excel (*.XLS, *.XLSX)|*.XLS;*.XLSX|" +
                    //        "Wszystkie pliki (*.*)|*.*";
                    //    break;
                    case "Momox":
                        openFileDialog1.Multiselect = false;
                        openFileDialog1.Filter = "Pliki CSV (*.CSV)|*.CSV|" +
                            "Wszystkie pliki (*.*)|*.*";
                        break;
                    case "Momox-M":
                        openFileDialog1.Multiselect = false;
                        openFileDialog1.Filter = "Pliki CSV (*.CSV)|*.CSV|" +
                            "Wszystkie pliki (*.*)|*.*";
                        break;
                    default:
                        break;
                }
            }

            openFileDialog1.InitialDirectory = fileDirectory;
            DialogResult dialog = openFileDialog1.ShowDialog();
            if (dialog == DialogResult.OK)
            {
                //fileDirectory = Path.GetDirectoryName(openFileDialog1.FileName);

                foreach (String file in openFileDialog1.FileNames)
                {
                    filePath.Add(file);
                    //filePath = openFileDialog1.FileName;
                    log.WriteLine("Plik: " + file);


                    //fileName = Path.GetFileName(openFileDialog1.FileName);

                }
                btnImportuj.Enabled = true;
            }

        }

        private void btnImportuj_Click(object sender, EventArgs e)
        {
            btnImportuj.Enabled = false;
                Thread trd = new Thread(new ThreadStart(this.ThreadTask));
                trd.IsBackground = true;
                trd.Start();
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

        private void ThreadTask()
        {
            if (firma != "")
            {
                try
                {
                    log.WriteLine(firma);
                    switch (firma)
                    {
                        //case "Ammeraal":
                        //    foreach (string file in filePath)
                        //    {
                        //        KonwersjaAmmeraal(file);
                        //    }
                        //    break;
                        //case "KK Wind Solutions":
                        //    foreach (string file in filePath)
                        //    {
                        //        KonwersjaKK(file);
                        //    }
                        //    break;
                        case "Incom":
                            foreach (string file in filePath)
                            {
                                ImportIncom(file);
                            }
                            break;
                        case "Momox":
                            foreach (string file in filePath)
                            {
                                ImportMomox(file);
                            }
                            break;
                        //case "Momox-F":
                        //    foreach (string file in filePath)
                        //    {
                        //        ImportMomox(file);
                        //    }
                        //    break;
                        //case "Momox-M":
                        //    foreach (string file in filePath)
                        //    {
                        //        ImportMomox(file);
                        //    }
                        //    break;
                        default:
                            break;
                    }
                }
                catch (Exception ex) { ex.ToString(); }
            }

            /*
             // Przykład importu czasu pracy
            try
            {
                using (Session session = login.CreateSession(false, false))
                {
                    KadryModule kadry = KadryModule.GetInstance(session);
                    KalendModule kalend = KalendModule.GetInstance(session);
                    using (ITransaction trans = session.Logout(true))
                    {
                        Pracownik pracownik = kadry.Pracownicy.WgKodu["L00188"];
                        if (pracownik == null)
                            throw new BusException("Pracownik o kodzie 'L00188' nie został znaleziony. Uzupełnienie nieobecności nie jest możliwe.");
                        Soneta.Kalend.DzienPracy dzienPracy = new Soneta.Kalend.DzienPracy(pracownik, Date.Today);
                        kalend.DniPracy.AddRow(dzienPracy);
                        dzienPracy.Praca.OdGodziny = new Time(7, 15);
                        dzienPracy.Praca.Czas = new Time(8, 30);
                        trans.Commit();
                    }
                    session.Save();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            */
        }

        private void ImportIncom(string file)
        {
            // Zmienna kontrolna ustawiana na false jeśli brak jest powiązania kodu pracownika u kontrahenta i w enovej
            bool kodyOk = true;
            // Pobieramy kody pracowników z bazy danych
            List<PracownikZestawienie> pracownicyKody = new List<PracownikZestawienie>();//PracownicyKody("KK");
            try
            {
                pracownicyKody = PracownicyKody("INCOM");
            }
            catch (Exception ex) { log.WriteLine(ex.Message); }

            // Pobieramy reguły wygładzania rcp
            Dictionary<string, int> regulyRCP = RegulyRCP();
            int wejscieMinus = 0;
            int wejsciePlus = 0;
            int wyjscieMinus = 0;
            int wyjsciePlus = 0;
            try { wejscieMinus = regulyRCP["Wejście minus"]; } catch { }
            try { wejsciePlus = regulyRCP["Wejście plus"]; } catch { }
            try { wyjscieMinus = regulyRCP["Wyjście minus"]; } catch { }
            try { wyjsciePlus = regulyRCP["Wyjście plus"]; } catch { }

            // Otwieramy skoroszyt excela
            fileDirectory = Path.GetDirectoryName(file);
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(file);
            Excel.Worksheet xlWorkSheetCzasPracy = null;// Arkusz z czasem pracy

            foreach (Excel.Worksheet w in xlWorkbook.Worksheets)
            {
                if (xlWorkSheetCzasPracy == null)
                {
                    xlWorkSheetCzasPracy = w;
                    log.WriteLine("Arkusz z harmonogramem: " + xlWorkSheetCzasPracy.Name);
                }
            }

            log.WriteLine("Przetwarzanie w toku. Proszę czekać.");
            log.WriteLine("Wejście minus: " + wejscieMinus.ToString());
            log.WriteLine("Wejście plus: " + wejsciePlus.ToString());
            log.WriteLine("Wyjście minus: " + wyjscieMinus.ToString());
            log.WriteLine("Wyjście plus: " + wyjsciePlus.ToString());


            if (xlWorkSheetCzasPracy != null)
            {
                Excel.Range range;
                range = xlWorkSheetCzasPracy.UsedRange;
                int rw = 0;// Ikość wierszy
                int cl = 0;// Ilość kolumn
                int rCnt;
                int cCnt;

                rw = range.Rows.Count;// Ilość wierszy
                cl = range.Columns.Count;// Ilość kolumn
                log.WriteLine("Wierszy: " + rw);
                log.WriteLine("Kolumn: " + cl);

                string ostatniKod = "";
                string kodEnova = "";
                int pracownikIndex = -1;

                for (rCnt = 4; rCnt <= rw; rCnt++)
                {
                    string numerRcp = "";
                    DateTime dzienData = new DateTime(1900,1,1);
                    TimeSpan dzienNorma = new TimeSpan();
                    TimeSpan dzienZapisRcp = new TimeSpan();
                    int dl = 0;
                    DateTime normaOd = new DateTime(1900, 1, 1, 0, 0, 0);
                    DateTime normaDo = new DateTime(1900, 1, 1, 0, 0, 0);
                    DateTime rcpOd = new DateTime(1900, 1, 1, 0, 0, 0);
                    DateTime rcpDo = new DateTime(1900, 1, 1, 0, 0, 0);
                    DateTime pracaOd;
                    DateTime pracaDo;


                    try
                    {
                        // Odczytujemy nr rcp z arkusza - kol 2
                        numerRcp = ((double)(range.Cells[rCnt, 2] as Excel.Range).Value2).ToString();
                    }
                    catch (Exception ex) { /*log.WriteLine("CzasPracy.pracKod, wiersz: " + rCnt + ", " + ex.Message);*/ }

                    // Sprawdzazmy czy nastąpiła zmiana numeru czyli kolejny pracownik
                    if (ostatniKod != numerRcp && numerRcp != "")
                    {
                        ostatniKod = numerRcp;
                        // Sprawdzamy czy dla tego pracownika jest już zestawienie
                        int index = pracownicyKody.FindIndex(c => c.KodUKontrahenta == ostatniKod);
                        if (index != -1)
                        {
                            // Jest zestawienie
                            kodEnova = pracownicyKody[index].KodEnova;
                            pracownikIndex = index;
                            log.WriteLine("Kod u kontrahenta: " + ostatniKod + ", kod enova: " + kodEnova);
                        }
                        else
                        {
                            kodEnova = "";
                            pracownikIndex = -1;
                            log.WriteLine("Kod u kontrahenta: " + ostatniKod + " - brak kodu w enovej");
                        }
                    }

                    // Odczytanie daty z arkusza - kol 3
                    try
                    {
                        double SerialDate = (double)(range.Cells[rCnt, 3] as Excel.Range).Value2;
                        if(SerialDate > 59) SerialDate -= 1;
                        dzienData = new DateTime(1899, 12, 31).AddDays(SerialDate);
                    }
                    catch (Exception ex) { /*log.WriteLine("CzasPracy.pracKod, wiersz: " + rCnt + ", " + ex.Message);*/ }
                    try
                    {
                        // Kolumna 5 zawiera normę danego dnia
                        string dzienNormaStr = "";
                        try
                        {
                            dzienNormaStr = (string)(range.Cells[rCnt, 5] as Excel.Range).Value2;
                        }
                        catch (Exception ex) { log.WriteLine("dzienNormaStr: " + ex.Message); }
                        if (dzienNormaStr.Length != 0)
                        {
                            // Sprawdzamy czy na dany dzień jest norma
                            int indexNorma = dzienNormaStr.IndexOf("icpl prod ");
                            if (indexNorma != -1)
                            {
                                dzienNormaStr = dzienNormaStr.Replace("icpl prod ", "");
                                // Pobieramy normęOd
                                try
                                {
                                    string normaOdStr = dzienNormaStr.Substring(0, dzienNormaStr.IndexOf("-"));
                                    normaOd = normaOd.AddHours(int.Parse(normaOdStr));
                                }
                                catch (Exception ex) { log.WriteLine("normaOd " + ex.Message); }
                                // Pobieramy normęDo
                                try
                                {
                                    string normaDoStr = dzienNormaStr.Substring(dzienNormaStr.IndexOf("-") + 1, dzienNormaStr.Length - dzienNormaStr.IndexOf("-") - 1);
                                    normaDo = normaDo.AddHours(int.Parse(normaDoStr));
                                }
                                catch (Exception ex) { log.WriteLine("normaDo " + ex.Message); }
                            }
                        }
                    }
                    catch (Exception ex) { /*log.WriteLine("Norma, wiersz: " + rCnt + ", " + ex.Message);*/ }
                    try
                    {
                        // Kolumna 6 zawiera wejścia i wyjścia
                        string dzienZapisRcpStr = "";
                        try
                        {
                            dzienZapisRcpStr = (string)(range.Cells[rCnt, 6] as Excel.Range).Value2;
                        }
                        catch (Exception ex) { log.WriteLine("dzienZapisRcpStr: " + ex.Message); }
                        try
                        {
                            if (dzienZapisRcpStr != "")
                            {
                                //log.WriteLine(dzienZapisRcpStr);
                                dl = dzienZapisRcpStr.Length;
                            }
                        }
                        catch { }

                        if (dzienZapisRcpStr.Length != 0)
                        {
                            dzienZapisRcpStr = dzienZapisRcpStr.Replace("+", "");
                            // Szukamy godziny wejścia
                            int indexRcpOd = dzienZapisRcpStr.ToLower().IndexOf(" i ");
                            string rcpOdStr = "";
                            if (indexRcpOd != -1)
                                rcpOdStr = dzienZapisRcpStr.Substring(dzienZapisRcpStr.ToLower().IndexOf(" i ") + 3, 5);

                            if (indexRcpOd == -1)
                            {
                                indexRcpOd = dzienZapisRcpStr.ToLower().IndexOf(".69 i ");
                                if (indexRcpOd != -1)
                                    rcpOdStr = dzienZapisRcpStr.Substring(dzienZapisRcpStr.ToLower().IndexOf(".69 i ") + 6, 5);
                            }
                            try { rcpOd = DateTime.Parse("1900-01-01 " + rcpOdStr); }
                            catch (Exception ex) { log.WriteLine(ex.Message); }

                            // Szukamy godziny wyjścia
                            // Praca w jednej strefie bez przerw
                            int indexRcpDo = dzienZapisRcpStr.ToLower().LastIndexOf("... o ");// dzienZapisRcpStr.ToLower().IndexOf(".70 c ");
                            string rcpDoStr = "";
                            if (indexRcpDo != -1)
                                rcpDoStr = dzienZapisRcpStr.Substring(dzienZapisRcpStr.ToLower().LastIndexOf("... o ") + 6, 5);
                            if (indexRcpDo == -1)
                            {
                                indexRcpDo = dzienZapisRcpStr.ToLower().LastIndexOf(".70 c ");
                                if (indexRcpDo != -1)
                                    rcpDoStr = dzienZapisRcpStr.Substring(dzienZapisRcpStr.ToLower().LastIndexOf(".70 c ") + 6, 5);
                            }
                            if (indexRcpDo == -1)
                            {
                                indexRcpDo = dzienZapisRcpStr.ToLower().LastIndexOf(".69 c ");
                                if (indexRcpDo != -1)
                                    rcpDoStr = dzienZapisRcpStr.Substring(dzienZapisRcpStr.ToLower().LastIndexOf(".69 c ") + 6, 5);
                            }
                            try { rcpDo = DateTime.Parse("1900-01-01 " + rcpDoStr); }
                            catch (Exception ex) { log.WriteLine(ex.Message); }


                            //log.WriteLine("rcpOdStr: " + rcpOdStr + ", rcpDoStr: " + rcpDoStr);
                        }
                    }
                    catch (Exception ex) { /*log.WriteLine("Czas, wiersz: " + rCnt + ", " + ex.Message);*/ }

                    // Wyliczamy czas normy
                    if (normaDo < normaOd)
                        dzienNorma = normaDo.AddDays(1) - normaOd;
                    else
                        dzienNorma = normaDo - normaOd;

                    string norma = "";
                    if (normaOd == new DateTime(1900,1,1,0,0,0))
                        norma = "wolne";
                    else
                        norma = normaOd.ToString("HH:mm") + " - " + normaDo.ToString("HH:mm");

                    string rcp = "";
                    //if (rcpOd != new DateTime(1900, 1, 1, 0, 0, 0))
                    //    rcp = rcpOd.ToString("HH:mm") + " - " + rcpDo.ToString("HH:mm");

                    //if (dl != 0)
                    //    log.WriteLine("    " + dzienData.ToShortDateString() + ", norma: " + norma + " " + dzienNorma.ToString("HH:mm") +
                    //        ", rcp: " + rcp);
                    if (dl != 0)
                    {
                        //try { log.WriteLine("    data: " + dzienData.ToShortDateString()); } catch (Exception ex) { log.WriteLine("dzienData: " + ex.Message); }
                        //try { log.WriteLine("    normaOd: " + normaOd.ToString("HH:mm")); } catch (Exception ex) { log.WriteLine("normaOd: " + ex.Message); }
                        //try { log.WriteLine("    normaDo: " + normaDo.ToString("HH:mm")); } catch (Exception ex) { log.WriteLine("normaDo: " + ex.Message); }
                        //try { log.WriteLine("    dzienNorma: " + new DateTime(dzienNorma.Ticks).ToString("HH:mm")); } catch (Exception ex) { log.WriteLine("dzienNorma: " + ex.Message); }
                        //try { log.WriteLine("    norma: " + norma); } catch (Exception ex) { log.WriteLine("norma: " + ex.Message); }
                        //try { log.WriteLine("    rcpOd: " + rcpOd.ToString("HH:mm") + " " + rcpOd.Hour.ToString() + " " + rcpOd.Minute.ToString()); } catch (Exception ex) { log.WriteLine("rcpOd: " + ex.Message); }
                        //try { log.WriteLine("    rcpDo: " + rcpDo.ToString("HH:mm")); } catch (Exception ex) { log.WriteLine("rcpDo: " + ex.Message); }

                        // Dopisujemy dzień pracy do zestawienia pracownika
                        if (kodEnova != "")
                        {
                            TimeSpan roznicaWe = TimeSpan.Zero; 
                            TimeSpan roznicaWy = TimeSpan.Zero;
                            if (norma != "wolne")
                            {
                                roznicaWe = normaOd - rcpOd;
                                roznicaWy = normaDo - rcpDo;
                            }

                            // Wygładzamy wejścia zgodnie z regułami
                            if (norma != "wolne")
                            {
                                if (roznicaWe.TotalMinutes <= wejscieMinus && roznicaWe.TotalMinutes >= (wejsciePlus * -1))
                                    pracaOd = normaOd;
                                else
                                    pracaOd = rcpOd;
                            }
                            else
                                pracaOd = rcpOd;

                            // Wygładzamy wyjścia z=godnie z regułami
                            if (norma != "wolne")
                            {
                                if (roznicaWy.TotalMinutes <= wyjscieMinus && roznicaWy.TotalMinutes >= (wyjsciePlus * -1))
                                    pracaDo = normaDo;
                                else
                                    pracaDo = rcpDo;
                            }
                            else
                                pracaDo = rcpDo;

                            log.WriteLine("We/wy: " + rcpOd.ToString("HH:mm") + " - " + rcpDo.ToString("HH:mm") +
                                                " norma: " + normaOd.ToString("HH:mm") + " - " + normaDo.ToString("HH:mm") +
                                                " różnica we/wy" + roznicaWe.TotalMinutes + " - " + roznicaWy.TotalMinutes);

                            DzienPracyImport dzien = new DzienPracyImport();
                            dzien.Data = dzienData;
                            dzien.OdGodziny = new Time(pracaOd.Hour, pracaOd.Minute);
                            TimeSpan dzienPraca;
                            if (pracaDo < pracaOd)
                                dzienPraca = pracaDo.AddDays(1) - pracaOd;
                            else
                                dzienPraca = pracaDo - pracaOd;
                            dzien.Czas = new Time(dzienPraca.Hours, dzienPraca.Minutes);
                            pracownicyKody[pracownikIndex].CzasPracyImport.Add(dzien);
                            log.WriteLine("    wiersz: " + rCnt.ToString() + ", data: " + dzien.Data.ToShortDateString() + ", norma: " + norma + 
                                ", rcp od " + rcpOd.ToString("HH:mm") + ", rcp do: " + rcpDo.ToString("HH:mm") +
                                ", praca od: " + dzien.OdGodziny.ToString() +
                                ", czas pracy: " + dzien.Czas.ToString());

                        }
                    }
                }


                xlWorkbook.Close(0);
                xlApp.Quit();
                if (chkCzas.Checked)
                {
                    log.WriteLine("");
                    log.WriteLine("Importowanie czasu pracy");
                    foreach (PracownikZestawienie pz in pracownicyKody)
                    {
                        if (pz.CzasPracyImport.Count != 0)
                        {
                            log.WriteLine(pz.KodUKontrahenta + " - " + pz.KodEnova);

                            try
                            {
                                using (Session session = login.CreateSession(false, false))
                                {
                                    KadryModule kadry = KadryModule.GetInstance(session);
                                    KalendModule kalend = KalendModule.GetInstance(session);
                                    using (ITransaction trans = session.Logout(true))
                                    {
                                        Pracownik pracownik = kadry.Pracownicy.WgKodu[pz.KodEnova];

                                        if (pracownik == null)
                                            log.WriteLine("Pracownik o kodzie '" + ostatniKod + " - " + kodEnova + "' nie został znaleziony. Uzupełnienie czasu pracy nie jest możliwe.");
                                        foreach (DzienPracyImport dp in pz.CzasPracyImport)
                                        {

                                            Soneta.Kalend.DzienPracy dzienPracy = null;
                                            try
                                            {
                                                dzienPracy = (DzienPracy)pracownik.DniPracy[dp.Data];
                                            }
                                            catch { }

                                            bool pomin = rbPomin.Checked;

                                            if ((dzienPracy != null && !pomin) || dzienPracy == null)
                                            {
                                                if (dzienPracy != null)
                                                    dzienPracy.Delete();

                                                dzienPracy = new Soneta.Kalend.DzienPracy(pracownik, dp.Data);

                                                kalend.DniPracy.AddRow(dzienPracy);
                                                dzienPracy.Praca.OdGodziny = dp.OdGodziny;// new Time(7, 15);
                                                dzienPracy.Praca.Czas = dp.Czas;// new Time(8, 30);
                                                log.WriteLine("    data: " + dzienPracy.Data.ToString() + ", od godziny:  " + dzienPracy.OdGodziny.ToString() + ", czas pracy: " + dzienPracy.Czas.ToString());
                                            }
                                            else
                                            {
                                                log.WriteLine("    pominięto: " + dp.Data.ToShortDateString());
                                            }
                                        }


                                        trans.Commit();
                                    }
                                    session.Save();
                                }
                            }
                            catch (Exception ex)
                            {
                                log.WriteLine(ex.ToString());
                            }

                            //try
                            //{
                            //    using (Session session = login.CreateSession(false, false))
                            //    {
                            //        KadryModule kadry = KadryModule.GetInstance(session);
                            //        KalendModule kalend = KalendModule.GetInstance(session);
                            //        using (ITransaction trans = session.Logout(true))
                            //        {
                            //            Pracownik pracownik = kadry.Pracownicy.WgKodu["L00188"];
                            //            if (pracownik == null)
                            //                throw new BusException("Pracownik o kodzie 'L00188' nie został znaleziony. Uzupełnienie nieobecności nie jest możliwe.");
                            //            Soneta.Kalend.DzienPracy dzienPracy = new Soneta.Kalend.DzienPracy(pracownik, Date.Today);
                            //            kalend.DniPracy.AddRow(dzienPracy);
                            //            dzienPracy.Praca.OdGodziny = new Time(7, 15);
                            //            dzienPracy.Praca.Czas = new Time(8, 30);
                            //            trans.Commit();
                            //        }
                            //        session.Save();
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    MessageBox.Show(ex.ToString());
                            //}
                        }
                    }
                }
            }

        }

        private void ImportMomox(string file)
        {
            // Zmienna kontrolna ustawiana na false jeśli brak jest powiązania kodu pracownika u kontrahenta i w enovej
            bool kodyOk = true;
            // Pobieramy kody pracowników z bazy danych
            List<PracownikZestawienie> pracownicyKody = new List<PracownikZestawienie>();
            try
            {
                pracownicyKody = PracownicyKody(firma);
            }
            catch (Exception ex) { 
                log.WriteLine("Pobranie kodów pracowników: " + ex.Message);
            }

            List<PracownikZestawienie> ZestawieniePracownicy = new List<PracownikZestawienie>();
            List<string> PracownicyBezKodow = new List<string>();

            //Odczytujemy plik csv
            using (TextFieldParser parser = new TextFieldParser(file))
            {
                
                parser.TextFieldType = FieldType.Delimited;
                // Separator pól
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string linia = parser.ReadLine();

                    //log.WriteLine("");
                    //log.WriteLine(linia);

                    // Usuwamy zbędne " na początku i na końcu linii
                    if (linia[0] == '\"')// linia.Replace("\"\"", "'").Replace("\"", "");
                    {
                        try
                        {
                            linia = linia.Substring(1, linia.Count() - 1);
                        }
                        catch { }
                    }
                    if (linia[linia.Length - 1] == '\"')
                    {
                        try
                        {
                            linia = linia.Substring(0, linia.Length - 1);
                        }
                        catch { }
                    }

                    // Zamieniamy podwójny dwa cudzysłowy "" na jeden "
                    linia = linia.Replace("\"\"", "\"");
                    bool tekst = false;

                    //log.WriteLine(linia);

                    // Zamiana separatora pól "," na ";" pomijając przecinek w godzinach otoczonych cudzysłowem
                    for (int i = 0; i < linia.Length; i++)
                    {
                        if (linia[i] == '\"')
                            if (!tekst)
                                tekst = true;
                            else
                                tekst = false;
                        if (linia[i] == ',' && !tekst)
                        {
                            StringBuilder sb = new StringBuilder(linia);
                            sb[i] = ';';
                            linia = sb.ToString();
                        }
                    }

                    // Pozbywamy się separatowa tekstu
                    linia = linia.Replace("\"", "");

                    // Rozbijamy linięna pola
                    string[] fields = null;
                    try
                    {
                        fields = linia.Split(';');
                    }
                    catch { }
                    /*
                     * Po zmianie struktury w momox, żeby nie zmieniać zbyt dużo
                     * dodaję jedno pole żeby ilość i kolejność pól była taka sama jak 
                     * poprzednio
                     */
                    if (fields[0].Length > 6)
                    {
                        //log.WriteLine("2");
                        List<string> fieldsWork = fields.ToList();
                        fieldsWork.Insert(1, "");
                        fields = null;
                        fields = fieldsWork.ToArray();
                    }

                    if (fields[0] != "Datum" && fields[0] != "Date" && fields[0] != "Summen")
                    {
                        string kodKontrahenta = fields[5];
                        string test = "";
                        test = fields[0] + " " + fields[5];

                        int index = pracownicyKody.FindIndex(c => c.KodUKontrahenta == kodKontrahenta);
                        if (index != -1)
                        {
                            DzienPracyImport dzien = null;
                            /*
                             * Jeśli w danym dniu nie ma zarejestrowanej nieobecności
                             * to przyjmujemy, że przepracowane zostało 8h
                             * Jeśli zmiana poranna to przyjmujemy 8h od 6:00
                             * Jeśli zmiana wieczorna to przyjmujemy 8h od 14:20
                             */
                            if (fields[6] != "" || fields[11] != "")
                            {
                                if (cbxLog.Checked)
                                {
                                    string test1 = "";
                                    foreach (string s in fields)
                                    {
                                        test1 += s + " | ";
                                    }
                                    log.WriteLine(test1);
                                }
                                try
                                {
                                    string godzOdS = fields[6];// Godzina wejścia
                                    string godzDoS = fields[7];// Godzina wyjścia
                                    string planS = fields[8];// Godziny planowane
                                    double planD = 0;
                                    string nieob = fields[11];// nieobecność
                                    string czasS = fields[15];// Czas przepracowany
                                    double czasD = 0;
                                    string n50S = fields[16];// Nadg 50%
                                    double n50D = 0;
                                    string n100S = fields[17];// Nadg 100%
                                    double n100D = 0;
                                    string nocneS = fields[18];// nocne

                                    TimeSpan godzOd = new TimeSpan();
                                    TimeSpan godzDo = new TimeSpan();
                                    TimeSpan czas = new TimeSpan();

                                    try
                                    {
                                        czasD = double.Parse(czasS);//.Replace(",", "."));
                                    }
                                    catch { }
                                    try
                                    {
                                        czas = TimeSpan.FromHours(czasD);
                                        //log.WriteLine("czas: " + czas.ToString());
                                    }
                                    catch { }
                                    try
                                    {
                                        godzOd = MomoxGodzStrDoTime(godzOdS);//TimeSpan.Parse(godzOdS);
                                    }
                                    catch { }
                                    try
                                    {
                                        godzDo = MomoxGodzStrDoTime(godzDoS);//TimeSpan.Parse(godzDoS);
                                    }
                                    catch { }
                                    try
                                    {
                                        if (planS != "")
                                            planD = double.Parse(planS);
                                    }
                                    catch { }
                                    try
                                    {
                                        if (n50S != "")
                                            n50D = double.Parse(n50S);
                                    }
                                    catch { }
                                    try
                                    {
                                        if (n100S != "")
                                            n100D = double.Parse(n100S);
                                    }
                                    catch { }

                                    /*
                                     * Jeśli są nadgodziny lub nieobecność to bierzemy cały czas przepracowany
                                     * jeśli nie to plan
                                     */
                                    if (planD != 0)
                                    {
                                        if (nieob == "" && n50D == 0 && n100D == 0)
                                        {
                                            // czas przepracowany = plan
                                            try
                                            {
                                                czas = TimeSpan.FromHours(planD);
                                            }
                                            catch { }
                                        }

                                        // Sprawdzamy nieobecności, które pmniejszają czas pracy
                                        if (nieob != "")
                                        {
                                            switch(nieob)
                                            {
                                                // Czas pracy = 0
                                                case "BEZ":// Urlop bezpłatny
                                                case "CH":
                                                case "ICH":
                                                case "ION":
                                                case "NN":
                                                case "NOP":
                                                case "NUN":
                                                case "NZ":
                                                case "OP":
                                                case "U":
                                                case "UBP":// Urlop bezpłatny z zachowaniem premii frekwencyjnej
                                                    czas = TimeSpan.Zero;
                                                    break;
                                                    // czas pracy pomniejszony
                                                case "SP":
                                                case "WW":
                                                    break;
                                                    // czas pracy nie pomniejszany
                                                case "WPP":
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }

                                    /*
                                     * Jesli są godziny nocne to wyliczamy godzinę wejścia odejmując 
                                     * czas przepracownamy od godziny wyjścia
                                     */
                                    if (nocneS != "")
                                    {
                                        try
                                        {
                                            godzOd = godzDo - czas;
                                        }
                                        catch { }
                                    }

                                    #region Stary kod
                                    /*
                                    //log.WriteLine("1. " + fields[0] + " godzOdS: " + godzOdS + ", godzDoS: " + godzDoS + ", czasS: " + czasS + ", czasD: " + czasD.ToString() + " planS: " + planS + ", czas: " + czas.ToString());
                                    if (fields[11] != "")
                                    {
                                        try
                                        {
                                            czasD = double.Parse(czasS);//.Replace(",", "."));
                                                                        //log.WriteLine("czasD: " + czasD.ToString());
                                        }
                                        catch { }
                                        try
                                        {
                                            godzDo = MomoxGodzStrDoTime(godzDoS);//TimeSpan.Parse(godzDoS);
                                                                                 //log.WriteLine("godzDo: " + godzDo.ToString());
                                        }
                                        catch { }
                                        try
                                        {
                                            czas = TimeSpan.FromHours(czasD);
                                            //log.WriteLine("czas: " + czas.ToString());
                                        }
                                        catch { }
                                        //log.WriteLine("2. " + fields[0] + " godzOdS: " + godzOdS + ", godzDoS: " + godzDoS + ", czasS: " + czasS + ", czasD: " + czasD.ToString() + " planS: " + planS + ", czas: " + czas.ToString());
                                        if (fields[18] != "")
                                        {
                                            try
                                            {
                                                godzOd = godzDo - czas;
                                                //godzOdS = godzOd.ToString("hh':'mm");
                                                //log.WriteLine("godzDo: " + godzDo.ToString() + " czas: " + czas.ToString() + " godzOdS: " + godzOdS);
                                            }
                                            catch (Exception ex)
                                            {
                                                //log.WriteLine("Czas do: " + ex.Message);
                                                //log.WriteLine("Czas do: " + ex.Message); 
                                            }
                                        }
                                        else
                                        {
                                            godzOd = MomoxGodzStrDoTime(godzOdS);
                                            //godzOdS = MomoxGodzStrDoTime(godzOdS).ToString("hh':'mm");
                                        }
                                    }
                                    else
                                    {
                                        godzOd = MomoxGodzStrDoTime(godzOdS);
                                        try
                                        {
                                            czas = TimeSpan.FromHours(double.Parse(planS));
                                        }
                                        catch
                                        {
                                            czas = TimeSpan.Zero;
                                        }
                                        //log.WriteLine("3. " + fields[0] + " godzOdS: " + godzOdS + ", godzDoS: " + godzDoS + ", czasS: " + czasS + ", czasD: " + czasD.ToString() + " planS: " + planS + ", czas: " + czas.ToString());
                                    }

                                    try
                                    {
                                        if (planS != "")
                                            planD = double.Parse(planS);
                                        else
                                            czas = TimeSpan.Zero;
                                    }
                                    catch { }
                                    */
                                    #endregion Stary kod

                                    dzien = new DzienPracyImport();
                                    if (fields[1] != "")
                                    {
                                        // Format ze starego pliku momox
                                        dzien.Data = MomoxData(fields[0], fields[1]);// Funkcja zwracająca sformatowaną datę z pliku momox
                                    }
                                    else
                                    {
                                        if (fields[0].IndexOf(".") == 2)
                                        {
                                            try
                                            {
                                                dzien.Data = new DateTime(int.Parse(fields[0].Substring(6, 4)), int.Parse(fields[0].Substring(3, 2)), int.Parse(fields[0].Substring(0, 2)));
                                            }
                                            catch (Exception ex) 
                                            {
                                                try
                                                {
                                                    dzien.Data = new DateTime(int.Parse(fields[0].Substring(0, 4)), int.Parse(fields[0].Substring(5, 2)), int.Parse(fields[0].Substring(8, 2)));
                                                }
                                                catch { }
                                            }
                                        }
                                    }

                                    try
                                    {
                                        dzien.OdGodziny = (Time)godzOd;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.WriteLine("dzien.OdGodziny: " + godzOdS + " " + ex.Message);
                                    }
                                    try { 
                                        dzien.DoGodziny = (Time)godzDo;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.WriteLine("dzien.DoGodziny: " + ex.Message);
                                    }
                                    try { 
                                        dzien.Plan = (Time)TimeSpan.FromHours(planD);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.WriteLine("dzien.Plan: " + ex.Message);
                                    }
                                    try{ 
                                        dzien.Czas = (Time)czas;//.ToString("hh':'mm");
                                                                //dzien.Work = "godzOd " + godzOdS + ", godzDo " + godzDoS + ", czas " + dzien.Czas;
                                        //log.WriteLine(dzien.Data.ToShortDateString() + ", OdGodziny: " + dzien.OdGodziny + ", czas: " + dzien.Czas);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.WriteLine("dzien.Czas: " + ex.Message);
                                    }
                                    if (cbxLog.Checked)
                                        log.WriteLine("dzien.Data: " + dzien.Data.ToShortDateString() + " dzien.OdGodziny: " + dzien.OdGodziny.ToString() +
                                                    " dzien.Czas: " + dzien.Czas.ToString());
                                    pracownicyKody[index].CzasPracyImport.Add(dzien);
                                }
                                catch (Exception ex) {
                                    log.WriteLine("4. Przetwarzanie dnia pracy: " + ex.Message);
                                }
                            }

                            // Nieobecność
                            if (cbxNieobecosci.Checked)
                            {
                                if (fields[11] != "")
                                {
                                    if (cbxLog.Checked)
                                        log.WriteLine("nieob: " + fields[11]);
                                    classes.Nieobecnosc nieobecnosc = new classes.Nieobecnosc();
                                    try
                                    {
                                        switch (fields[11])
                                        {
                                            case "BEZ":
                                            case "UBP":
                                                nieobecnosc.Definicja = "Urlop bezpłatny (art 174 kp)";
                                                break;
                                            case "CH":
                                                nieobecnosc.Definicja = "Zwolnienie chorobowe";
                                                break;
                                            case "ICH":
                                            case "ION":
                                                nieobecnosc.Definicja = "Nieobec.usprawiedliwiona niepł";
                                                break;
                                            case "NN":
                                            case "NUN":
                                                nieobecnosc.Definicja = "Nieobec.nieusprawiedliwiona";
                                                break;
                                            case "NOP":
                                                nieobecnosc.Definicja = "Nieobec.usprawiedliwiona płatn";
                                                break;
                                            case "NZ":
                                                nieobecnosc.Definicja = "Urlop wypoczynkowy prac.tymcz.";
                                                nieobecnosc.PrzyczynaUrlopu = "NaŻądanie";
                                                break;
                                            case "OP":
                                                nieobecnosc.Definicja = "Urlop opiekuńczy (art 188 kp, dni)";
                                                break;
                                            case "SP":
                                                nieobecnosc.Definicja = "Spóźnienie";//na godziny przed właściwym czasem pracy
                                                if (dzien.Plan != Time.Zero && dzien.Czas != Time.Zero)
                                                {
                                                    nieobecnosc.DoGodziny = dzien.OdGodziny;
                                                    nieobecnosc.OdGodziny = nieobecnosc.DoGodziny - (dzien.Plan - dzien.Czas);
                                                }
                                                break;
                                            case "U":
                                                nieobecnosc.Definicja = "Urlop wypoczynkowy prac.tymcz.";
                                                nieobecnosc.PrzyczynaUrlopu = "Planowy";
                                                break;
                                            case "WPP":
                                                nieobecnosc.Definicja = "Wyjście wcześniejsze (brak pracy)";//na godziny po właściwym czasie pracy
                                                if (dzien.Plan != Time.Zero && dzien.Czas != Time.Zero)
                                                {
                                                    nieobecnosc.OdGodziny = dzien.DoGodziny;
                                                    nieobecnosc.DoGodziny = nieobecnosc.OdGodziny + (dzien.Plan - dzien.Czas);
                                                }
                                                break;
                                            case "WW":
                                                nieobecnosc.Definicja = "Wyjście prywatne";//na godziny po właściwym czasie pracy
                                                if (dzien.Plan != Time.Zero && dzien.Czas != Time.Zero)
                                                {
                                                    nieobecnosc.OdGodziny = dzien.DoGodziny;
                                                    nieobecnosc.DoGodziny = nieobecnosc.OdGodziny + (dzien.Plan - dzien.Czas);
                                                }
                                                break;
                                            //case "WWP":// Tu jeszcze do zrobienia, co to za nieobecność
                                            default:
                                                nieobecnosc.Definicja = "";
                                                break;
                                        }
                                    }
                                    catch (Exception ex) { 
                                        log.WriteLine("Przetwarzanie nieobecnności: " + ex.Message);
                                    }
                                    if(cbxLog.Checked)
                                        log.WriteLine(nieobecnosc.Definicja);
                                    nieobecnosc.Definicja = nieobecnosc.Definicja;

                                    if (fields[1] != "")
                                    {
                                        //if (cbxLog.Checked)
                                        //    log.WriteLine("1");
                                        // Format ze starego pliku momox
                                        nieobecnosc.OkresOd = MomoxData(fields[0], fields[1]);// Funkcja zwracająca sformatowaną datę z pliku momox
                                    }
                                    else
                                    {
                                        if (fields[0].IndexOf(".") == 2 && fields[0].Length == 10)
                                        {
                                            //if (cbxLog.Checked)
                                            //    log.WriteLine("2 " + fields[0]);
                                            try
                                            {
                                                //if (cbxLog.Checked)
                                                //    log.WriteLine("3");
                                                nieobecnosc.OkresOd = new DateTime(int.Parse(fields[0].Substring(6, 4)), int.Parse(fields[0].Substring(3, 2)), int.Parse(fields[0].Substring(0, 2)));
                                            }
                                            catch (Exception ex)
                                            {
                                                try
                                                {
                                                    //if (cbxLog.Checked)
                                                    //    log.WriteLine("4");
                                                    nieobecnosc.OkresOd = new DateTime(int.Parse(fields[0].Substring(0, 4)), int.Parse(fields[0].Substring(5, 2)), int.Parse(fields[0].Substring(8, 2)));
                                                }
                                                catch (Exception ex1) { }
                                            }
                                        }
                                        else
                                        {
                                            log.WriteLine("Nieprawidłowa data: " + fields[0]);
                                        }
                                    }
                                    try
                                    {
                                        nieobecnosc.OkresDo = nieobecnosc.OkresOd;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.WriteLine("Ustawienie dat nieobecności: " + ex.Message);
                                    }
                                    if (nieobecnosc.Definicja != "")
                                    {
                                        pracownicyKody[index].Nieobecności.Add(nieobecnosc);
                                    }
                                }
                            }
                        }
                        else
                        {
                            string pracBez = fields[3] + " " + fields[4] + " " + fields[5];
                            if (!PracownicyBezKodow.Contains(pracBez))
                                PracownicyBezKodow.Add(pracBez);
                            //test += " - brak kodu w enovej";
                        }
                        //foreach (string l in fields)
                        //{

                        //}
                        //log.WriteLine(test);
                    }
                }

            }
            // Pracownicy bez kodu w enovej
            log.WriteLine("Pracownicy bez przypisanego kodu kontrahenta w enovej");
            foreach(string pr in PracownicyBezKodow)
            {
                log.WriteLine(pr);
                //log.WriteLine(pr);
            }

            if (chkCzas.Checked)
            {
                log.WriteLine("");
                // Przetwarzane czasu pracy
                log.WriteLine("Przetwarzane czasu pracy ");
                foreach (PracownikZestawienie pz in pracownicyKody)
                {
                    if (pz.CzasPracyImport.Count != 0)
                    {
                        log.WriteLine(pz.NazwiskoImie + " - " + pz.KodUKontrahenta + " - " + pz.KodEnova);

                        try
                        {
                            using (Session session = login.CreateSession(false, false))
                            {
                                KadryModule kadry = KadryModule.GetInstance(session);
                                KalendModule kalend = KalendModule.GetInstance(session);
                                using (ITransaction trans = session.Logout(true))
                                {
                                    Pracownik pracownik = kadry.Pracownicy.WgKodu[pz.KodEnova];

                                    if (pracownik == null)
                                        log.WriteLine("Pracownik o kodzie '" + pz.KodEnova + "' nie został znaleziony. Uzupełnienie czasu pracy nie jest możliwe.");
                                    //else
                                    //    log.WriteLine("Pracownik: " + pracownik.Kod);


                                    foreach (DzienPracyImport dp in pz.CzasPracyImport)
                                    {
                                        //log.WriteLine("    " + dp.Data + ", godzOd " + dp.OdGodziny + ", czas " + dp.Czas);

                                        if (dp.Czas != Time.Zero)
                                        {
                                            Soneta.Kalend.DzienPracy dzienPracy = null;
                                            try
                                            {
                                                dzienPracy = (DzienPracy)pracownik.DniPracy[dp.Data];
                                            }
                                            catch (Exception ex) { log.WriteLine("    " + pz.KodEnova + " " + dp.Data.ToShortDateString() + " " + ex.Message); }
                                            bool pomin = rbPomin.Checked;
                                            if ((dzienPracy != null && !pomin) || dzienPracy == null)
                                            {
                                                if (dzienPracy != null)
                                                    dzienPracy.Delete();

                                                dzienPracy = new Soneta.Kalend.DzienPracy(pracownik, dp.Data);
                                                //log.WriteLine("    dzień: " + dzienPracy.Data.ToString());

                                                kalend.DniPracy.AddRow(dzienPracy);
                                                dzienPracy.Praca.OdGodziny = dp.OdGodziny;// new Time(7, 15);
                                                dzienPracy.Praca.Czas = dp.Czas;// new Time(8, 30);
                                                log.WriteLine("    data: " + ((DateTime)dzienPracy.Data).ToShortDateString() + ", od godziny: " + dzienPracy.OdGodziny.ToString() + ", czas pracy: " + dzienPracy.Czas.ToString());
                                            }
                                            else
                                            {
                                                log.WriteLine("    pominięto: " + dp.Data.ToShortDateString());
                                            }
                                        }
                                        else
                                        {
                                            log.WriteLine("    pominięto: " + dp.Data.ToShortDateString() + " - zerowy czas pracy");
                                        }

                                        //int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
                                        //listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);
                                    }

                                    trans.Commit();
                                }
                                session.Save();
                            }
                        }
                        catch (Exception ex)
                        {
                            log.WriteLine(ex.Message);
                        }

                    }
                }
            }

            if (cbxNieobecosci.Checked)
            {
                log.WriteLine("");
                // Przetwarzanie nieobecności
                log.WriteLine("Przetwarzane nieobecności");
                foreach (PracownikZestawienie pz in pracownicyKody)
                {
                    if (pz.Nieobecności.Count != 0)
                    {
                        log.WriteLine(pz.NazwiskoImie + " - " + pz.KodUKontrahenta + " - " + pz.KodEnova + ", ilość nieobecności: " + pz.Nieobecności.Count.ToString());

                        try
                        {

                            foreach (classes.Nieobecnosc nie in pz.Nieobecności)
                            {
                                bool pomin = false;

                                if (nie.Definicja == "Zwolnienie chorobowe" && cbxPominChorobowe.Checked)
                                    pomin = true;

                                if (!pomin)
                                {
                                    using (Session session = login.CreateSession(false, false))
                                    {
                                        KadryModule kadry = KadryModule.GetInstance(session);
                                        KalendModule kalend = KalendModule.GetInstance(session);
                                        Pracownik pracownik = kadry.Pracownicy.WgKodu[pz.KodEnova];

                                        if (pracownik == null)
                                            log.WriteLine("Pracownik o kodzie '" + pz.KodEnova + "' nie został znaleziony. Uzupełnienie czasu pracy nie jest możliwe.");
                                        using (ITransaction trans = session.Logout(true))
                                        {
                                            try
                                            {
                                                //log.WriteLine("   - " + nie.Okres + " " + nie.Definicja + " " + nie.PrzyczynaUrlopu);


                                                Soneta.Kalend.Nieobecnosc nieobecność = new NieobecnośćPracownika(pracownik);
                                                kalend.Nieobecnosci.AddRow(nieobecność);
                                                nieobecność.Okres = new FromTo(nie.OkresOd, nie.OkresDo);
                                                DefinicjaNieobecnosci def = null;
                                                try
                                                {
                                                    def = (DefinicjaNieobecnosci)kalend.DefNieobecnosci.WgNazwy[nie.Definicja];
                                                    //if (def != null)
                                                    //    log.WriteLine(" - znaleziono definicję: " + def.Nazwa);
                                                    //else

                                                    //    log.WriteLine(" - nie znaleziono definicji " + nie.Definicja);
                                                }
                                                catch (Exception ex) { log.WriteLine(" - szukanie def nieob. - " + ex.Message); }
                                                nieobecność.Definicja = def;

                                                // Wstawienie czasów dla nieobecności na godziny
                                                switch (nie.Definicja)
                                                {
                                                    case "Wyjście prywatne":
                                                    case "Wyjście wcześniejsze (brak pracy)":
                                                    case "Spóźnienie":
                                                        nieobecność.OdGodziny = nie.OdGodziny;
                                                        nieobecność.DoGodziny = nie.DoGodziny;
                                                        break;
                                                }

                                                // Przyczyny urlopu
                                                switch (nie.PrzyczynaUrlopu)
                                                {
                                                    case "NaŻądanie":
                                                        nieobecność.Urlop.Przyczyna = PrzyczynaUrlopu.NaŻądanie;
                                                        break;
                                                    case "Planowy":
                                                        nieobecność.Urlop.Przyczyna = PrzyczynaUrlopu.Planowy;
                                                        break;
                                                    default:
                                                        break;
                                                }
                                                string info = "     okres: " + nieobecność.Okres.ToString() + " " + nieobecność.Definicja;

                                                switch (nie.Definicja)
                                                {
                                                    case "Urlop wypoczynkowy prac.tymcz.":
                                                        info += " " + nieobecność.Urlop.Przyczyna;
                                                        break;
                                                    case "Wyjście prywatne":
                                                    case "Wyjście wcześniejsze (brak pracy)":
                                                    case "Spóźnienie":
                                                        info += " " + nie.OdGodziny.ToString() + " - " + nie.DoGodziny.ToString();
                                                        break;
                                                    default:
                                                        break;
                                                }
                                                log.WriteLine(info);
                                            }
                                            catch (Exception ex) { log.WriteLine("     okres: " + nie.Okres + " " + nie.Definicja + " " + ex.Message); }

                                            //int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
                                            //listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);

                                            try
                                            {
                                                trans.Commit();
                                            }
                                            catch (Exception ex) { log.WriteLine("      " + ex.Message); }
                                        }
                                        try
                                        {
                                            session.Save();
                                        }
                                        catch (Exception ex) { log.WriteLine("      " + ex.Message); }

                                    }
                                }
                                else
                                {
                                    log.WriteLine("Pominięto zwolnienie chorobowe: " + new FromTo(nie.OkresOd, nie.OkresDo).ToString());
                                }
                            }
                        }
                        catch (Exception ex) { log.WriteLine("      " + ex.Message); }
                    }
                }
            }

            log.WriteLine("-- ZAKOŃCZONO --");
            this.Close();
        }

        /// <summary>
        /// Funkcja zamieniająca godzinę w formacie 2:30 PM na godzinę w formacie TimeSpan
        /// </summary>
        /// <param name="GodzinaString"></param>
        /// <returns></returns>
        private TimeSpan MomoxGodzStrDoTime (string GodzinaString)
        {
            string godzina = GodzinaString.Substring(0, GodzinaString.IndexOf(":"));
            string minuty = GodzinaString.Substring(GodzinaString.IndexOf(":") + 1, 2);

            TimeSpan godzinaT = new TimeSpan();

            try
            {
                godzinaT = new TimeSpan(int.Parse(godzina), int.Parse(minuty), 0);
            }
            catch { }

            try
            {
                if (GodzinaString.ToLower().Contains("pm"))
                    godzinaT = godzinaT + new TimeSpan(12, 0, 0);
            }
            catch { }

            return godzinaT;
        }

        /// <summary>
        /// Funkcja zwracająca sformatowaną datę dla importu z pliku momox
        /// </summary>
        /// <param name="DzienMies"></param>
        /// <param name="Rok"></param>
        /// <returns></returns>
        private DateTime MomoxData (string DzienMies, string Rok)
        {
            string miesiacS = "";
            int miesiac = 0;
            try
            {
                miesiacS = DzienMies.Substring(0, 3);
            }
            catch { }
            switch(miesiacS)
            {
                case "Jan":
                    miesiac = 1;
                    break;
                case "Feb":
                    miesiac = 2;
                    break;
                case "Mar":
                    miesiac = 3;
                    break;
                case "Apr":
                    miesiac = 4;
                    break;
                case "May":
                    miesiac = 5;
                    break;
                case "Jun":
                    miesiac = 6;
                    break;
                case "Jul":
                    miesiac = 7;
                    break;
                case "Aug":
                    miesiac = 8;
                    break;
                case "Sep":
                    miesiac = 9;
                    break;
                case "Oct":
                    miesiac = 10;
                    break;
                case "Nov":
                    miesiac = 11;
                    break;
                case "Dec":
                    miesiac = 12;
                    break;
            }
            int dzien = 0;
            try
            {
                dzien = int.Parse(DzienMies.Substring(4, DzienMies.Length - 4));
            }
            catch { }
            //if (int.Parse(dzien) < 10)
            //{
            //    dzien = "0" + dzien;
            //}
            int rok = 0;
            try
            {
                rok = int.Parse(Rok.Replace(" ", ""));
            }
            catch { }

            return new DateTime(rok,miesiac,dzien);// rok + "-" + miesiac + "-" + dzien;
        }

        // Funkcja pobierająca z bazy kody pracowników klienta i przypisane do nich kody LSJ
        // Funkcja zwraca HashTable z kodami
        private List<PracownikZestawienie> PracownicyKody(string wydzial)
        {
            List<PracownikZestawienie> pracownicyKody = new List<PracownikZestawienie>();

            string sql = "SELECT  Pracownicy.Kod, P_PracKody.Kod AS PracKod, Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS NazwiskoImie " +
                "FROM P_PracKody INNER JOIN Pracownicy ON P_PracKody.PracID = Pracownicy.ID " +
                "LEFT OUTER JOIN Wydzialy ON P_PracKody.WydzID = Wydzialy.ID " +
                "WHERE(Wydzialy.Kod LIKE '" + wydzial + "%') " +
                "GROUP BY Pracownicy.Kod, P_PracKody.Kod, Pracownicy.Nazwisko + ' ' + Pracownicy.Imie";

            SqlConnection connection;
            connection = new SqlConnection(connectionString);
            connection.Open();
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlCommand sCommand = new SqlCommand(sql, connection);
            SqlDataAdapter da = new SqlDataAdapter(sCommand);
            da.Fill(dt);

            connection.Close();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        PracownikZestawienie pracZest = new PracownikZestawienie();
                        pracZest.KodEnova = (string)dt.Rows[i]["Kod"];
                        pracZest.KodUKontrahenta = (string)dt.Rows[i]["PracKod"];
                        pracZest.NazwiskoImie = (string)dt.Rows[i]["NazwiskoImie"];
                        pracownicyKody.Add(pracZest);
                    }
                    catch (Exception ex) { }
                }
            }

            return pracownicyKody;
        }

        // Funkcja pobierająca reguły wygładzania rcp z konfiguracji
        private Dictionary<string,int> RegulyRCP()
        {
            Dictionary<string, int> regulyRCP = new Dictionary<string, int>();

            string sql = "SELECT ID, Node, Name, Type, StrValue, MemoValue, Stamp FROM CfgAttributes WHERE(Node = 272)";

            SqlConnection connection;
            connection = new SqlConnection(connectionString);
            connection.Open();
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlCommand sCommand = new SqlCommand(sql, connection);
            SqlDataAdapter da = new SqlDataAdapter(sCommand);
            da.Fill(dt);

            connection.Close();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        string regulaKey = (string)dt.Rows[i]["Name"];
                        string regulaValue = (string)dt.Rows[i]["StrValue"];
                        int regulaMinut = 0;
                        try
                        {
                            int godziny = int.Parse(regulaValue.Substring(0,regulaValue.IndexOf(":")));
                            int minuty = int.Parse(regulaValue.Substring(regulaValue.IndexOf(":") + 1, 2));
                            regulaMinut = godziny * 60 + minuty;
                        }
                        catch { }
                        regulyRCP.Add(regulaKey, regulaMinut);
                    }
                    catch (Exception ex) { }
                }
            }

            return regulyRCP;
        }

        private void rbPomin_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbxFirma_TextChanged(object sender, EventArgs e)
        {
        }

        private void cbxFirma_DropDownClosed(object sender, EventArgs e)
        {
            switch (cbxFirma.Text)
            {
                case "Incom":
                    chkCzas.Enabled = true;
                    cbxNieobecosci.Enabled = false;
                    break;
                case "Momox":
                    chkCzas.Enabled = true;
                    cbxNieobecosci.Enabled = true;
                    gbxImportNieobecnosci.Enabled = true;
                    break;
                //case "Momox-M":
                //    chkCzas.Enabled = false;
                //    break;
                default:
                    break;

            }
        }

        private void lblConnection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmConnection control = new frmConnection();
            control.Server = server;
            control.User = user;
            control.Database = database;
            control.Show();
        }

        private void chkNieobecosci_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxNieobecosci.Checked)
                gbxImportNieobecnosci.Enabled = true;
            else
                gbxImportNieobecnosci.Enabled = false;

        }

        private void cbxFirma_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
