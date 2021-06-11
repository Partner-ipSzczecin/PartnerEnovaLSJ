using System;
using System.Collections;
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
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Xml;
using System.Globalization;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using static PartnerEnovaNormaPraca.classes;

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
    public partial class frmKonwersja : Form
    {
        private Thread trd;

        string server = "";
        string user = "";
        string password = "";
        string database = "";
        private string connectionString;

        List<PracownikZestawienie> pracownicyKody = null;
        string fileDirectory = "";
        List<string> filePath = new List<string>();
        string fileName = "";
        string fileKK = "";
        Excel.Application xlApp = null;
        Excel.Workbook xlWorkbook = null;
        Excel.Worksheet xlWorkSheetHarmonogram = null;// Arkusz z harmonogramem
        Excel.Worksheet xlWorkSheetCzasPracy = null;// Arkusz z czasem pracy

        string firma = "";

        bool log = false;

        //Excel.Application xlApp;
        //Excel.Workbook xlWorkBook;
        //Excel.Worksheet xlWorkSheet = null;

        public frmKonwersja()
        {
            InitializeComponent();
        }


        private void frmKonwersja_Load(object sender, EventArgs e)
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
            cbxHarmonogram.Enabled = false;
            cbxCzasPracy.Enabled = false;
            btnKonwertuj.Enabled = false;

            listBox1.Items.Clear();
            btnLog.Enabled = false;
            log = cbxLog.Checked;
            filePath.Clear();
            try
            {
                firma = cbxFirma.Text;
            }
            catch (Exception ex) { }
            if (firma != "")
            {
                switch (firma)
                {
                    case "Ammega":
                        openFileDialog1.Multiselect = true;
                        openFileDialog1.Filter = "Pliki XML (*.XML)|*.XML|" +
                            "Wszystkie pliki (*.*)|*.*";
                        break;
                    case "KK Wind Solutions":
                        openFileDialog1.Multiselect = false;
                        openFileDialog1.Filter = "Pliki Excel (*.XLS, *.XLSX)|*.XLS;*.XLSX|" +
                            "Wszystkie pliki (*.*)|*.*";
                        break;
                    case "Momox":
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
                //listBox1.Items.Add(fileDirectory);

                foreach (String file in openFileDialog1.FileNames)
                {
                    filePath.Add(file);
                    //filePath = openFileDialog1.FileName;
                    listBox1.Items.Add("Plik: " + file);


                    //fileName = Path.GetFileName(openFileDialog1.FileName);

                    //listBox1.Items.Add(file);
                }

                Thread trd = new Thread(new ThreadStart(this.ThreadTask));
                trd.IsBackground = true;
                trd.Start();

            }
        }

        private void ThreadTask()
        {

            if (firma != "")
            {
                try
                {
                    listBox1.Items.Add(firma);
                    switch (firma)
                    {
                        case "Ammega":
                            foreach (string file in filePath)
                            {
                                KonwersjaAmmega(file);
                            }
                            break;
                        case "KK Wind Solutions":
                            foreach (string file in filePath)
                            {
                                fileKK = file;
                                KonwersjaKK(fileKK);
                            }
                            break;
                        case "Momox":
                            foreach (string file in filePath)
                            {
                                KonversjaMomox(file);
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch(Exception ex) { ex.ToString(); }
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

        private void KonwersjaAmmega(string file)
        {
            Hashtable pracownicyKody = new Hashtable();//Hashtable na kody pracowników pobrane z bazy
            string zawartosc = "";
            bool brak = false;

            string sql = "SELECT Pracownicy.Kod, P_PracKody.Kod AS PracKod " +
                "FROM P_PracKody INNER JOIN Pracownicy ON P_PracKody.PracID = Pracownicy.ID " +
                "LEFT OUTER JOIN Wydzialy ON P_PracKody.WydzID = Wydzialy.ID WHERE (Wydzialy.Kod = N'Ammega')";

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
                        pracownicyKody.Add((string)dt.Rows[i]["PracKod"], (string)dt.Rows[i]["Kod"]);
                    }
                    catch (Exception ex) { }
                }
            }

            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    string text = sr.ReadToEnd();
                    string[] lines = text.Split('\r');
                    foreach (string l in lines)
                    {
                        string s = "";// l;


                        if (l.Contains("<Pracownik>") == true)
                        {
                            string prac = "";
                            string kod = "";
                            int index1 = l.IndexOf("<Pracownik>") + 11;
                            int index2 = l.IndexOf("</Pracownik>");
                            //listBox1.Items.Add("index1: " +index1 + ", index2: " + index2 + ", lenght: " + (index2 - index1).ToString());
                            try
                            {
                                prac = l.Substring(index1, index2 - index1);
                            }
                            catch (Exception ex) { }
                            try
                            {
                                kod = pracownicyKody[prac].ToString();
                            }
                            catch (Exception ex) { }
                            //listBox1.Items.Add("pracownik: " + prac + " - " + kod);
                            if (kod != "")
                            {
                                s = l.Replace(prac, kod);
                            }
                            else
                            {
                                brak = true;
                                listBox1.Items.Add("Nie znaleziono w enovej kodu pracownika: " + prac);
                            }
                        }

                        if (chkZamienUrlop.Checked)
                        {
                            if (l.Contains("<Definicja>Urlop wypoczynkowy</Definicja>"))
                            {
                                s = l.Replace("<Definicja>Urlop wypoczynkowy</Definicja>", "<Definicja>Urlop wypoczynkowy prac.tymcz.</Definicja>");
                            }
                        }

                        if (s != "")
                            zawartosc += s + "\r";
                        else
                            zawartosc += l + "\r";
                    }
                    //listBox1.Items.Add(s);
                }
            }
            catch (Exception ex) { listBox1.Items.Add("4: " + ex.ToString()); }

            try
            {
                if (!brak && zawartosc != "")
                {
                    listBox1.Items.Add("Generowanie pliku");
                    try
                    {
                        file = file.ToLower().Replace(".xml", "_konwersja.xml");
                        listBox1.Items.Add(file);
                        File.WriteAllText(file, zawartosc, Encoding.Unicode);
                        //listBox1.Items.Add(fileDirectory);
                        //listBox1.Items.Add(fileName);
                        //using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                        //{

                        //    file.WriteLine(zawartosc);
                        //}
                    }
                    catch (Exception ex) { listBox1.Items.Add("1: " + ex.ToString()); }

                }

            }
            catch (Exception ex) { listBox1.Items.Add("3: " + ex.ToString()); }

            btnLog.Enabled = true;

            listBox1.Items.Add("--- Zakończono ---");
        }

        private void KonwersjaKK(string file)
        {
            // Pobieramy kody pracowników z bazy danych
            pracownicyKody = null;
            pracownicyKody = new List<PracownikZestawienie>();//PracownicyKody("KK");
            try
            {
                pracownicyKody = PracownicyKody("KK");
            }
            catch (Exception ex) { listBox1.Items.Add(ex.Message); }
            //listBox1.Items.Add("Ilość pracowników z enovej: " + pracownicyKody.Count);

            // Otwieramy skoroszyt excela
            fileDirectory = Path.GetDirectoryName(fileKK);
            xlApp = new Excel.Application();
            xlWorkbook = xlApp.Workbooks.Open(fileKK);

            cbxHarmonogram.Items.Clear();
            cbxCzasPracy.Items.Clear();
            cbxHarmonogram.Items.Add("");
            cbxCzasPracy.Items.Add("");

            foreach (Excel.Worksheet w in xlWorkbook.Worksheets)
            {
                cbxHarmonogram.Items.Add(w.Name);
                cbxCzasPracy.Items.Add(w.Name);
            }

            cbxHarmonogram.Enabled = true;
            cbxCzasPracy.Enabled = true;
            btnKonwertuj.Enabled = true;
        }

        private void btnKonwertuj_Click(object sender, EventArgs e)
        {
            if (xlWorkbook != null)
            {
                if (cbxHarmonogram.Text == "" && cbxCzasPracy.Text == "")
                {
                    MessageBox.Show("Nie wybrano żadnego arkusza.");
                }
                else
                {
                    KonwersjaKK2();
                }
            }
            else
            {
                MessageBox.Show("Proszę ponownie wskazać skoroszyt excel.");
            }
        }

        private void KonwersjaKK2()
        {
            // Zmienna kontrolna ustawiana na false jeśli brak jest powiązania kodu pracownika u kontrahenta i w enovej
            bool kodyOk = true;

            foreach (Excel.Worksheet w in xlWorkbook.Worksheets)
            {
                // Identyfikujemy arkusze z harmonogramem i czasem pracy
                string arkuszNazwa = w.Name.ToLower();
                if (cbxHarmonogram.Text != "")
                    if (arkuszNazwa.IndexOf(cbxHarmonogram.Text) != -1)
                    {
                        try
                        {
                            xlWorkSheetHarmonogram = w;
                            listBox1.Items.Add("Arkusz z harmonogramem: " + xlWorkSheetHarmonogram.Name);
                        }
                        catch { }

                    }
                if (cbxCzasPracy.Text != "")
                    if (arkuszNazwa.IndexOf(cbxCzasPracy.Text) != -1)
                    {
                        try
                        {
                            xlWorkSheetCzasPracy = w;
                            listBox1.Items.Add("Arkusz z czasem pracy: " + xlWorkSheetCzasPracy.Name);
                        }
                        catch { }
                    }
            }

            listBox1.Items.Add("Przetwarzanie w toku. Proszę czekać.");

            // Wczytanie planu pracy z arkusza
            if (xlWorkSheetHarmonogram != null)
            {
                //listBox1.Items.Add("Arkusz z harmonogramem");
                Excel.Range range;
                range = xlWorkSheetHarmonogram.UsedRange;
                int rw = 0;// Ikość wierszy
                int cl = 0;// Ilość kolumn
                int rCnt;
                int cCnt;

                // Kolejność interesujących nas kolumn
                int cPracKod = 0;
                int cPracownik = 0;
                int cData = 0;
                int cOdGodziny = 0;
                int cCzas = 0;

                rw = range.Rows.Count;// Ilość wierszy
                cl = range.Columns.Count;// Ilość kolumn
                //listBox1.Items.Add("Wierszy: " + rw);
                //listBox1.Items.Add("Kolumn: " + cl);

                for (cCnt = 1; cCnt <= cl; cCnt++)
                {
                    // Sprawdzamy kolejność kolumn
                    string wartoscKonmorki = (string)(range.Cells[1, cCnt] as Excel.Range).Value2;
                    //listBox1.Items.Add("Nagłówek: " + wartoscKonmorki);
                    switch (wartoscKonmorki)
                    {
                        case "EMPLID":// Id pracownika u klienta
                            cPracKod = cCnt;
                            //listBox1.Items.Add("cPracKod: " + cPracKod.ToString());
                            break;
                        case "EmployeeName":// Nazwisko i imię pracownika
                            cPracownik = cCnt;
                            //listBox1.Items.Add("cPracownik: " + cPracownik.ToString());
                            break;
                        case "TRANSDATE":// Data
                            cData = cCnt;
                            //listBox1.Items.Add("cData: " + cData.ToString());
                            break;
                        case "StartTime":// Od godziny
                            cOdGodziny = cCnt;
                            //listBox1.Items.Add("cOdGodziny: " + cOdGodziny.ToString());
                            break;
                        case "Duration":// Czas planowany
                            cCzas = cCnt;
                            //listBox1.Items.Add("cCzas: " + cCzas.ToString());
                            break;

                    }


                }

                for (rCnt = 2; rCnt <= rw; rCnt++)
                {
                    tssLabel1.Text = "Przetwarzanie harmonogramu, wiersz: " + rCnt;
                    DzienPlanu dzienPlanu = new DzienPlanu();

                    string pracKod = null;// (string)(range.Cells[rCnt, cPracKod] as Excel.Range).Value2;
                    string pracownik = "";// (string)(range.Cells[rCnt, cPracownik] as Excel.Range).Value2;
                    dzienPlanu.Data = "";// (string)(range.Cells[rCnt, cData] as Excel.Range).Value2;
                    dzienPlanu.OdGodziny = "";// (string)(range.Cells[rCnt, cOdGodziny] as Excel.Range).Value2;
                    dzienPlanu.Czas = "";// (string)(range.Cells[rCnt, cCzas] as Excel.Range).Value2;

                    try
                    {
                        pracKod = (string)(range.Cells[rCnt, cPracKod] as Excel.Range).Value2;
                    }
                    catch (Exception ex) { listBox1.Items.Add("Harmonogram.pracKod, wiersz: " + rCnt + ", " + ex.Message); }
                    if (pracKod != null)
                    {
                        try
                        {
                            pracownik = (string)(range.Cells[rCnt, cPracownik] as Excel.Range).Value2;
                        }
                        catch (Exception ex) { listBox1.Items.Add("Harmonogram.pracownik, wiersz: " + rCnt + ", " + ex.Message); }
                        try
                        {
                            dzienPlanu.Data = (string)(range.Cells[rCnt, cData] as Excel.Range).Value2;
                        }
                        catch (Exception ex) { listBox1.Items.Add("Harmonogram.Data, wiersz: " + rCnt + ", " + ex.Message); }
                        try
                        {
                            dzienPlanu.OdGodziny = (string)(range.Cells[rCnt, cOdGodziny] as Excel.Range).Value2;
                        }
                        catch (Exception ex) { listBox1.Items.Add("Harmonogram.OdGodziny, wiersz: " + rCnt + ", " + ex.Message); }
                        try
                        {
                            dzienPlanu.Czas = ((double)(range.Cells[rCnt, cCzas] as Excel.Range).Value2).ToString();
                        }
                        catch (Exception ex) { listBox1.Items.Add("Harmonogram.Czas, wiersz: " + rCnt + ", " + ex.Message); }

                        if (log)
                        {
                            try
                            {
                                listBox1.Items.Add("kodDl: " + pracKod.Length);
                            }
                            catch (Exception ex) { listBox1.Items.Add("Harmonogram, pracKod.Length , wiersz: " + rCnt + " " + ex.Message); }
                            try
                            {
                                listBox1.Items.Add("Harmonogram, wiersz: " + rCnt + "; " + pracKod + " (" + pracKod.Length + "); " + pracownik + " (" + pracownik.Length + "); " +
                                                dzienPlanu.Data + "; " + dzienPlanu.Czas.ToString());
                            }
                            catch (Exception ex) { listBox1.Items.Add("Czas pracy, wiersz: " + rCnt + " " + ex.Message); }

                        }

                        //listBox1.Items.Add(                           "rekord: " + pracKod + " " + pracownik + " " + dzienPlanu.Data + " " + dzienPlanu.OdGodziny + " " + dzienPlanu.Czas);

                        var index = pracownicyKody.FindIndex(c => c.KodUKontrahenta == pracKod);
                        if (index != -1)
                        {
                            pracownicyKody[index].DniPlanu.Add(dzienPlanu);
                        }
                        else
                        {
                            PracownikZestawienie pracownikZestawienie = new PracownikZestawienie();
                            //pracownikZestawienie.KodEnova = "";
                            pracownikZestawienie.KodUKontrahenta = pracKod;
                            pracownikZestawienie.NazwiskoImie = pracownik;
                            pracownikZestawienie.DniPlanu.Add(dzienPlanu);
                            pracownicyKody.Add(pracownikZestawienie);
                        }
                    }


                }
            }

            if (xlWorkSheetCzasPracy != null)
            {
                //listBox1.Items.Add("Arkusz z czasem pracy");
                Excel.Range range;
                range = xlWorkSheetCzasPracy.UsedRange;
                int rw = 0;// Ikość wierszy
                int cl = 0;// Ilość kolumn
                int rCnt;
                int cCnt;

                // Kolejność interesujących nas kolumn
                int cPracKod = 0;
                int cPracownik = 0;
                int cData = 0;
                int cOdGodziny = 0;
                int cCzas = 0;
                int cDescription = 0;

                rw = range.Rows.Count;// Ilość wierszy
                cl = range.Columns.Count;// Ilość kolumn
                //listBox1.Items.Add("Wierszy: " + rw);
                //listBox1.Items.Add("Kolumn: " + cl);

                for (cCnt = 1; cCnt <= cl; cCnt++)
                {
                    // Sprawdzamy kolejność kolumn
                    string wartoscKonmorki = (string)(range.Cells[1, cCnt] as Excel.Range).Value2;
                    //listBox1.Items.Add("Nagłówek: " + wartoscKonmorki);
                    switch (wartoscKonmorki)
                    {
                        case "EMPLID":// Id pracownika u klienta
                            cPracKod = cCnt;
                            //listBox1.Items.Add("cPracKod: " + cPracKod.ToString());
                            break;
                        case "EmployeeName":// Nazwisko i imię pracownika
                            cPracownik = cCnt;
                            //listBox1.Items.Add("cPracownik: " + cPracownik.ToString());
                            break;
                        case "TRANSDATE":// Data
                            cData = cCnt;
                            //listBox1.Items.Add("cData: " + cData.ToString());
                            break;
                        case "StartTime":// Od godziny
                            cOdGodziny = cCnt;
                            //listBox1.Items.Add("cOdGodziny: " + cOdGodziny.ToString());
                            break;
                        case "Duration":// Czas planowany
                            cCzas = cCnt;
                            //listBox1.Items.Add("cCzas: " + cCzas.ToString());
                            break;
                        case "DESCRIPTION":// Opis zawierający symbole niobecności
                            cDescription = cCnt;
                            //listBox1.Items.Add("cCzas: " + cCzas.ToString());
                            break;

                    }
                }

                for (rCnt = 2; rCnt <= rw; rCnt++)
                {
                    tssLabel1.Text = "Przetwarzenie czasu pracy, wiersz: " + rCnt;
                    DzienPracyW dzienPracy = new DzienPracyW();
                    Nieobecnosc nieobecnosc = null;

                    string pracKod = null;// (string)(range.Cells[rCnt, cPracKod] as Excel.Range).Value2;
                    string pracownik = "";// (string)(range.Cells[rCnt, cPracownik] as Excel.Range).Value2;
                    dzienPracy.Data = "";// (string)(range.Cells[rCnt, cData] as Excel.Range).Value2;
                    dzienPracy.OdGodziny = "";// (string)(range.Cells[rCnt, cOdGodziny] as Excel.Range).Value2;
                    dzienPracy.Czas = "";// (string)(range.Cells[rCnt, cCzas] as Excel.Range).Value2;

                    try
                    {
                        pracKod = (string)(range.Cells[rCnt, cPracKod] as Excel.Range).Value2;
                    }
                    catch (Exception ex) { listBox1.Items.Add("CzasPracy.pracKod, wiersz: " + rCnt + ", " + ex.Message); }
                    if (pracKod != null)
                    {
                        try
                        {
                            pracownik = (string)(range.Cells[rCnt, cPracownik] as Excel.Range).Value2;
                        }
                        catch (Exception ex) { listBox1.Items.Add("CzasPracy.pracownik, wiersz: " + rCnt + ", " + ex.Message); }
                        try
                        {
                            dzienPracy.Data = (string)(range.Cells[rCnt, cData] as Excel.Range).Value2;
                        }
                        catch (Exception ex) { listBox1.Items.Add("CzasPracy.Data, wiersz: " + rCnt + ", " + ex.Message); }
                        try
                        {
                            dzienPracy.OdGodziny = (string)(range.Cells[rCnt, cOdGodziny] as Excel.Range).Value2;
                        }
                        catch (Exception ex) { listBox1.Items.Add("CzasPracy.OdGodziny, wiersz: " + rCnt + ", " + ex.Message); }
                        try
                        {
                            dzienPracy.Czas = ((double)(range.Cells[rCnt, cCzas] as Excel.Range).Value2).ToString();
                        }
                        catch (Exception ex) { listBox1.Items.Add("CzasPracy.Czas , wiersz: " + rCnt + ", " + ex.Message); }
                        try
                        {
                            string description = (string)(range.Cells[rCnt, cDescription] as Excel.Range).Value2;
                            if (description == "NN"
                                || description == "CH"
                                || description == "UB"
                                || description == "UW"
                                || description == "UW1"
                                || description == "E"
                                || description == "OP"
                                || description == "Uwych"
                                || description == "KRW"
                                || description == "UO"
                                )
                            {
                                nieobecnosc = new Nieobecnosc();
                                nieobecnosc.Okres = dzienPracy.Data;
                                switch (description)
                                {
                                    case "NN":
                                        nieobecnosc.Definicja = "Nieobec.nieusprawiedliwiona";
                                        break;
                                    //case "CH":
                                    //    nieobecnosc.Definicja = "Nieobec.usprawiedliwiona niepł";
                                    //    break;
                                    case "UB":
                                        nieobecnosc.Definicja = "Urlop bezpłatny (art 174 kp)";
                                        break;
                                    //case "UW":
                                    //    nieobecnosc.Definicja = "Urlop wypoczynkowy prac.tymcz.";
                                    //    nieobecnosc.PrzyczynaUrlopu = "Planowy";
                                    //    break;
                                    //case "UW1":
                                    //    nieobecnosc.Definicja = "Urlop wypoczynkowy prac.tymcz.";
                                    //    nieobecnosc.PrzyczynaUrlopu = "NaŻądanie";
                                    //    break;
                                    case "E":
                                        nieobecnosc.Definicja = "Wolne za nadgodziny";
                                        break;
                                    case "OP":
                                        nieobecnosc.Definicja = "Urlop opiekuńczy (art 188 kp, dni)";
                                        break;
                                    case "Uwych":
                                        nieobecnosc.Definicja = "Urlop wychowawczy (ur.macierzyński)";
                                        break;
                                    case "KRW":
                                        nieobecnosc.Definicja = "Krew - Nieobec.usprawiedliwiona niepł";
                                        break;
                                    case "UO":
                                        nieobecnosc.Definicja = "Okolicz - Nieobec.usprawiedliwiona niepł";
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        catch (Exception ex) { listBox1.Items.Add("Description , wiersz: " + rCnt + ", " + ex.Message); }

                        if (log)
                        {
                            try
                            {
                                listBox1.Items.Add("kodDl: " + pracKod.Length);
                            }
                            catch (Exception ex) { listBox1.Items.Add("Czas pracy, pracKod.Length , wiersz: " + rCnt + " " + ex.Message); }

                            try
                            {
                                listBox1.Items.Add("Czas pracy, wiersz: " + rCnt + "; " + pracKod + " (" + pracKod.Length + "); " + pracownik + " (" + pracownik.Length + "); " +
                                                dzienPracy.Data + "; " + dzienPracy.Czas.ToString());
                            }
                            catch (Exception ex) { listBox1.Items.Add("Czas pracy, wiersz: " + rCnt + " " + ex.Message); }
                        }

                        //listBox1.Items.Add("rekord: " + pracKod + " " + pracownik + " " + dzienPlanu.Data + " " + dzienPlanu.OdGodziny + " " + dzienPlanu.Czas);


                        // Sprawdzamy czy dla tego pracownika jest już zestawienie
                        var index = pracownicyKody.FindIndex(c => c.KodUKontrahenta == pracKod);
                        if (index != -1)
                        {
                            // Jest zestawienie

                            if (nieobecnosc != null)
                            {
                                pracownicyKody[index].Nieobecności.Add(nieobecnosc);
                            }
                            else
                            {
                                var dzienIndex = pracownicyKody[index].CzasPracy.FindIndex(d => d.Data == dzienPracy.Data);
                                if (dzienIndex != -1)
                                {
                                    StrefaPracyW strefaPracy = new StrefaPracyW();
                                    strefaPracy.Definicja = "Praca w normie";
                                    strefaPracy.OdGodziny = dzienPracy.OdGodziny;
                                    strefaPracy.Czas = dzienPracy.Czas;
                                    pracownicyKody[index].CzasPracy[dzienIndex].Strefy.Add(strefaPracy);
                                    pracownicyKody[index].CzasPracy[dzienIndex].LicznikStref++;
                                    if (log)
                                    {
                                        listBox1.Items.Add("Istniejący dzień - Strefa pracy: " + strefaPracy.OdGodziny + " " + strefaPracy.Czas);
                                    }

                                }
                                else
                                {
                                    StrefaPracyW strefaPracy = new StrefaPracyW();
                                    strefaPracy.Definicja = "Praca w normie";
                                    strefaPracy.OdGodziny = dzienPracy.OdGodziny;
                                    strefaPracy.Czas = dzienPracy.Czas;
                                    dzienPracy.LicznikStref++;
                                    dzienPracy.Strefy.Add(strefaPracy);
                                    pracownicyKody[index].CzasPracy.Add(dzienPracy);
                                    if (log)
                                    {
                                        listBox1.Items.Add("Nowy dzień - Strefa pracy: " + strefaPracy.OdGodziny + " " + strefaPracy.Czas);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Tworzymy nowe zestawienie

                            PracownikZestawienie pracownikZestawienie = new PracownikZestawienie();
                            //pracownikZestawienie.KodEnova = "";
                            pracownikZestawienie.KodUKontrahenta = pracKod;
                            pracownikZestawienie.NazwiskoImie = pracownik;

                            if (nieobecnosc != null)
                            {
                                pracownikZestawienie.Nieobecności.Add(nieobecnosc);
                            }
                            else
                            {
                                StrefaPracyW strefaPracy = new StrefaPracyW();
                                strefaPracy.Definicja = "Praca w normie";
                                strefaPracy.OdGodziny = dzienPracy.OdGodziny;
                                strefaPracy.Czas = dzienPracy.Czas;
                                dzienPracy.LicznikStref++;
                                dzienPracy.Strefy.Add(strefaPracy);
                                pracownikZestawienie.CzasPracy.Add(dzienPracy);
                            }
                            pracownicyKody.Add(pracownikZestawienie);
                        }
                    }


                }

            }

            string xmlText = "<?xml version=\"1.0\" encoding=\"Unicode\" ?>" + Environment.NewLine +
                             "<Root xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" + Environment.NewLine;
            string xmlTextNieobecnosci = "<?xml version=\"1.0\" encoding=\"Unicode\" ?>" + Environment.NewLine +
                             "<Root xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" + Environment.NewLine;

            bool jestNorma = false;
            bool jestCzas = false;
            bool jestNieob = false;


            if (log)
                listBox1.Items.Add("Generowanie planu");
            // Tworzymy zawartość xml dotyczącą normy
            foreach (PracownikZestawienie pz in pracownicyKody)
            {
                string kodKonr = pz.KodUKontrahenta != "" ? pz.KodUKontrahenta : "- brak kodu u kontrahenta -";
                string kodEnova = "";
                try
                {
                    kodEnova = pz.KodEnova.Substring(0, 1) == "L" ? pz.KodEnova : "- brak poviązania z enovą -";
                }
                catch (Exception ex)
                {
                    //listBox1.Items.Add("Nazwisko i imię: " + pz.NazwiskoImie + " - przypisanie kodu enova: " + ex.Message);
                    kodEnova = "- brak poviązania z enovą -";
                }
                if (kodEnova == "- brak poviązania z enovą -")
                    kodyOk = false;

                string nazwiskoImie = pz.NazwiskoImie;

                if (pz.DniPlanu.Count > 0 && kodEnova != "- brak poviązania z enovą -")
                {
                    listBox1.Items.Add("Nazwisko i imię: " + nazwiskoImie + ", kod enova " + kodEnova + ", kod u kontrahenta " + kodKonr);
                    if (!jestNorma)
                    {
                        xmlText += "    <DniPlanu>" + Environment.NewLine;
                        jestNorma = true;
                    }

                    foreach (DzienPlanu dp in pz.DniPlanu)
                    {
                        try
                        {
                            tssLabel1.Text = "Generowanie planu, pracownik: " + pz.NazwiskoImie + " " + dp.Data;
                        }
                        catch { }
                        if (log)
                            try
                            {
                                listBox1.Items.Add("Pracownik: " + pz.NazwiskoImie + " - " + pz.KodEnova + " " + pz.KodUKontrahenta + ", Data: " + dp.Data + ", Definicja: " + dp.Definicja + ", OdGodziny: " + dp.OdGodziny + ", Czas: " + dp.Czas);
                            }
                            catch (Exception ex) { listBox1.Items.Add(ex.Message); }

                        //listBox1.Items.Add(dp.Data + " " + dp.OdGodziny + " " + dp.Czas);
                        xmlText += "        <DzienPlanu>" + Environment.NewLine;
                        xmlText += "            <Pracownik>" + kodEnova + "</Pracownik>" + Environment.NewLine;
                        xmlText += "            <Data>" + dp.Data + "</Data>" + Environment.NewLine;
                        if (dp.Czas == "0")
                        {
                            xmlText += "            <Definicja>Wolny</Definicja>" + Environment.NewLine;
                        }
                        else
                        {
                            xmlText += "            <Definicja>Pracy</Definicja>" + Environment.NewLine;
                            xmlText += "            <OdGodziny>" + dp.OdGodziny + "</OdGodziny>" + Environment.NewLine;
                            double czasDouble = 0;
                            try
                            {
                                czasDouble = double.Parse(dp.Czas);
                            }
                            catch (Exception ex) { listBox1.Items.Add(dp.Czas + " " + ex.Message); }
                            TimeSpan czas = TimeSpan.FromHours(czasDouble);
                            xmlText += "            <Czas>" + czas.ToString("h\\:mm") + "</Czas>" + Environment.NewLine;
                        }
                        xmlText += "        </DzienPlanu>" + Environment.NewLine;
                    }
                }

            }

            if (jestNorma)
                xmlText += "    </DniPlanu>" + Environment.NewLine;

            if (log)
                listBox1.Items.Add("Generowanie czasu pracy");

            // Tworzymy zawartość xml dotyczącą czasu pracy
            foreach (PracownikZestawienie pz in pracownicyKody)
            {
                string kodKonr = pz.KodUKontrahenta != "" ? pz.KodUKontrahenta : "- brak kodu u kontrahenta -";
                string kodEnova = "";
                try
                {
                    kodEnova = pz.KodEnova.Substring(0, 1) == "L" ? pz.KodEnova : "- brak poviązania z enovą -";
                }
                catch (Exception ex)
                {
                    //listBox1.Items.Add("Nazwisko i imię: " + pz.NazwiskoImie + " - przypisanie kodu enova: " + ex.Message);
                    kodEnova = "- brak poviązania z enovą -";
                }
                if (kodEnova == "- brak poviązania z enovą -")
                    kodyOk = false;


                string nazwiskoImie = pz.NazwiskoImie;
                if (pz.CzasPracy.Count > 0 && kodEnova != "- brak poviązania z enovą -")
                {

                    if (!jestCzas)
                    {
                        xmlText += "    <CzasPracy>" + Environment.NewLine;
                        jestCzas = true;
                    }
                    listBox1.Items.Add("Nazwisko i imię: " + nazwiskoImie + ", kod enova " + kodEnova + ", kod u kontrahenta " + kodKonr);

                    foreach (DzienPracyW dp in pz.CzasPracy)
                    {
                        try
                        {
                            tssLabel1.Text = "Generowanie czasu, pracownik: " + pz.NazwiskoImie + " " + dp.Data;
                        }
                        catch { }
                        if (log)
                            try
                            {
                                listBox1.Items.Add("Pracownik: " + pz.NazwiskoImie + " - " + pz.KodEnova + " " + pz.KodUKontrahenta + ", Data: " + dp.Data + ", OdGodziny: " + dp.OdGodziny + ", Czas: " + dp.Czas);
                            }
                            catch (Exception ex) { listBox1.Items.Add(ex.Message); }
                        //listBox1.Items.Add(dp.Data + " " + dp.OdGodziny + " " + dp.Czas);
                        xmlText += "        <DzienPracy>" + Environment.NewLine;
                        xmlText += "            <Pracownik>" + kodEnova + "</Pracownik>" + Environment.NewLine;
                        xmlText += "            <Data>" + dp.Data + "</Data>" + Environment.NewLine;

                        if (dp.LicznikStref == 1)
                        {
                            if (dp.Czas != "0")
                            {
                                xmlText += "            <OdGodziny>" + dp.OdGodziny + "</OdGodziny>" + Environment.NewLine;
                                double czasDouble = 0;
                                try
                                {
                                    czasDouble = double.Parse(dp.Czas);
                                }
                                catch (Exception ex) { listBox1.Items.Add(dp.Czas + " " + ex.Message); }
                                TimeSpan czas = TimeSpan.FromHours(czasDouble);
                                xmlText += "            <Czas>" + czas.ToString("h\\:mm") + "</Czas>" + Environment.NewLine;
                            }
                        }
                        else
                        {
                            xmlText += "            <Strefy>" + Environment.NewLine;
                            foreach (StrefaPracyW sp in dp.Strefy)
                            {
                                double czasDouble = 0;
                                try
                                {
                                    czasDouble = double.Parse(sp.Czas);
                                }
                                catch (Exception ex) { listBox1.Items.Add(sp.Czas + " " + ex.Message); }

                                if (czasDouble > 0 && czasDouble < 24)
                                {
                                    TimeSpan czas = TimeSpan.FromHours(czasDouble);
                                    xmlText += "                <StrefaPracy Definicja=\"" + sp.Definicja + "\" OdGodziny=\"" + sp.OdGodziny + "\" Czas=\"" + czas.ToString("h\\:mm") + "\"></StrefaPracy>" + Environment.NewLine;
                                }
                            }
                            xmlText += "            </Strefy>" + Environment.NewLine;
                        }
                        xmlText += "        </DzienPracy>" + Environment.NewLine;
                    }// koniec foreach (DzienPracy dp in pz.CzasPracy)

                }

                if (pz.Nieobecności.Count > 0 && kodEnova != "- brak poviązania z enovą -")
                {
                    foreach (Nieobecnosc nie in pz.Nieobecności)
                    {
                        if (!jestNieob)
                        {
                            xmlTextNieobecnosci += "    <CzasPracy>" + Environment.NewLine;
                            jestNieob = true;
                        }
                        xmlTextNieobecnosci += "        <Nieobecnosc>" + Environment.NewLine;
                        xmlTextNieobecnosci += "            <Pracownik>" + kodEnova + "</Pracownik>" + Environment.NewLine;
                        xmlTextNieobecnosci += "            <Okres>" + nie.Okres + "..." + nie.Okres.Substring(8, 2) + "</Okres>" + Environment.NewLine;
                        xmlTextNieobecnosci += "            <Definicja>" + nie.Definicja + "</Definicja>" + Environment.NewLine;
                        if (nie.PrzyczynaUrlopu != "")
                        {
                            xmlTextNieobecnosci += "            <PrzyczynaUrlopu>" + nie.PrzyczynaUrlopu + "</PrzyczynaUrlopu>" + Environment.NewLine;
                        }
                        xmlTextNieobecnosci += "        </Nieobecnosc>" + Environment.NewLine;
                    }// koniec foreach (Nieobecnosc nie in pz.Nieobecności)
                }

            }// koniec foreach (PracownikZestawienie pz in pracownicyKody)

            if (jestCzas)
                xmlText += "    </CzasPracy>" + Environment.NewLine;

            if (jestNieob)
                xmlTextNieobecnosci += "    </CzasPracy>" + Environment.NewLine;

            xmlText += "</Root>" + Environment.NewLine;
            xmlTextNieobecnosci += "</Root>" + Environment.NewLine;

            xlWorkbook.Close(0);
            xlApp.Quit();

            if (kodyOk || chkPominBrakiKodow.Checked)
            {
                listBox1.Items.Add("Generowanie pliku");
                try
                {
                    string fileNieob = fileKK.ToLower().Replace(".xlsx", "Nieob_konwersja.xml").Replace(".xls", "Nieob_konwersja.xml");
                    fileKK = fileKK.ToLower().Replace(".xlsx", "_konwersja.xml").Replace(".xls", "_konwersja.xml");

                    listBox1.Items.Add(fileKK);
                    if (!kodyOk && chkPominBrakiKodow.Checked)
                        listBox1.Items.Add("Pominięto niektórych pracowników z powodu brakujących kodów.");

                    File.WriteAllText(fileKK, xmlText, Encoding.Unicode);
                    if (jestNieob)
                        File.WriteAllText(fileNieob, xmlTextNieobecnosci, Encoding.Unicode);

                    //listBox1.Items.Add(fileDirectory);
                    //listBox1.Items.Add(fileName);
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                    //{

                    //    file.WriteLine(zawartosc);
                    //}
                }
                catch (Exception ex) { listBox1.Items.Add("1: " + ex.ToString()); }
            }
            else
            {
                listBox1.Items.Add("Brak kodów u niektórych pracowników.");
            }

            tssLabel1.Text = "Zakończono";

            int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
            listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);

            btnLog.Enabled = true;

            listBox1.Items.Add("--- Zakończono ---");
        }

        private void KonversjaMomox(string file)
        {
            List<PracownikZestawienie> ZestawieniePracownicy = new List<PracownikZestawienie>();
            bool brak = false;

            //Odczytujemy plik csv
            using (TextFieldParser parser = new TextFieldParser(file))
            {
                parser.TextFieldType = FieldType.Delimited;

                /*
                 * Być może trzeba będzie obsłużyć wybór średnika jako separatora
                 */
                // Separator pól
                parser.SetDelimiters(",");
                bool dni = false;//zmienna definiująca czy wiersz należy do zestawienia pracownika.
                int raportWiersz = 0;
                string raportData = "";
                string rok = "";
                string miesiac = "";
                PracownikZestawienie zestawienie = null;
                Dzien zestawienieDzien = null;

                while (!parser.EndOfData)
                {
                    
                    //Process row
                    string[] fields = null;
                    try
                    {
                        fields = parser.ReadFields();
                    }
                    catch (MalformedLineException ex)
                    {
                        if (parser.ErrorLine.StartsWith("\""))
                        {
                            var line = parser.ErrorLine.Substring(1, parser.ErrorLine.Length - 2);
                            fields = line.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    string wiersz = "";
                    bool dniOn = false;
                    int i = 0;
                    string field1 = fields[0];


                    int field1Length = field1.Length;
                    //Szukamy początku zestawienia dla każdego pracownika
                    if (field1Length > 37)
                        if (field1.Substring(0, 37) == "Wykaz miesięczny (Monatsübersicht)   ")
                        {
                            if (zestawienie != null)
                            {
                                ZestawieniePracownicy.Add(zestawienie);
                                zestawienie = null;
                            }
                            zestawienie = new PracownikZestawienie();

                            zestawienie.NazwiskoImie = field1.Substring(37, field1.Length - 37);
                            int NazwiskoImieDo = zestawienie.NazwiskoImie.IndexOf(" (");
                            if (NazwiskoImieDo > 0)
                            {
                                zestawienie.KodUKontrahenta = zestawienie.NazwiskoImie.Substring(NazwiskoImieDo + 2, zestawienie.NazwiskoImie.Length - NazwiskoImieDo - 3);
                                zestawienie.NazwiskoImie = zestawienie.NazwiskoImie.Substring(0, NazwiskoImieDo);
                            }
                            else
                            {
                                zestawienie.KodUKontrahenta = "brak";
                            }

                            raportWiersz = 0;

                            //Sprawdzenie okresu za jaki jest raport
                            //Do numeru dnia dodawany będzie rok i miesiąc
                            if (raportData == "")
                            {
                                raportData = fields[19];
                                listBox1.Items.Add("Okres raportu: " + raportData);
                                //Sprawdzamy w którym miejscu w dacie jest rok.
                                int rokPoz = raportData.IndexOf("20");
                                rok = raportData.Substring(rokPoz, 4);
                                
                                switch (rokPoz)
                                {
                                    case 0:
                                        miesiac = raportData.Substring(5, 2);
                                        break;
                                    case 6:
                                        miesiac = raportData.Substring(rokPoz - 3, 2);
                                        break;
                                    default:
                                        break;
                                }

                                listBox1.Items.Add("Miesiąc raportu: " + rok + "-" + miesiac);
                                //miesiac = 
                            }

                            try
                            {
                                wiersz = i + " " + field1;
                            }
                            catch (Exception ex) { }
                        }

                    //Pole dzięki któremu stwierdzam, że kolejne wiersze nie należą do zestawienia pracownika
                    if (field1 == "Suma (Summen)")
                    {
                        try
                        {
                            dni = false;
                        }
                        catch (Exception ex) { }
                    }

                    foreach (string field in fields)
                    {
                        if (dni)
                        {
                            wiersz += " " + i + " " + field + "|";
                            switch (i)
                            {
                                case 0://data
                                    zestawienieDzien = null;
                                    zestawienieDzien = new Dzien();
                                    zestawienieDzien.DataS = rok + "-" + miesiac + "-" + field;
                                    break;
                                case 1:
                                    zestawienieDzien.DzienTygodnia = field;
                                    break;
                                case 2:
                                    zestawienieDzien.Praca = field;

                                    if (field != "")
                                    {
                                        TimeSpan ts = TimeSpan.Zero;
                                        try
                                        {
                                            int godziny = int.Parse(field.Substring(0, field.IndexOf(":")));
                                            int minuty = int.Parse(field.Substring(field.IndexOf(":") + 1, 2));
                                            ts = new TimeSpan(godziny,minuty,0);
                                        }
                                        catch (Exception ex) { }
                                        zestawienieDzien.PracaOd = ts;
                                    }
                                    break;
                                case 8://rodzaje nieobecności
                                    zestawienieDzien.NieobecnoscKod = field;
                                    switch (zestawienieDzien.NieobecnoscKod)
                                    {
                                        case "BEZ":
                                        case "UBP":
                                            zestawienieDzien.NieobecnoscEnova = "Urlop bezpłatny (art 174 kp)";
                                            break;
                                        case "CH":
                                            zestawienieDzien.NieobecnoscEnova = "Zwolnienie chorobowe";
                                            break;
                                        case "ICH":
                                        case "ION":
                                            zestawienieDzien.NieobecnoscEnova = "Nieobec.usprawiedliwiona niepł";
                                            break;
                                        case "NN":
                                        case "NUN":
                                            zestawienieDzien.NieobecnoscEnova = "Nieobec.nieusprawiedliwiona";
                                            break;
                                        case "NZ":
                                            zestawienieDzien.NieobecnoscEnova = "Urlop wypoczynkowy prac.tymcz.";
                                            zestawienieDzien.NieobecnoscEnovaPrzyczyna = "NaŻądanie";
                                            break;
                                        case "SP":
                                            zestawienieDzien.NieobecnoscEnova = "Spóźnienie";//na godziny przed właściwym czasem pracy
                                            break;
                                        case "U":
                                            zestawienieDzien.NieobecnoscEnova = "Urlop wypoczynkowy prac.tymcz.";
                                            zestawienieDzien.NieobecnoscEnovaPrzyczyna = "Planowy";
                                            break;
                                        case "WW":
                                            zestawienieDzien.NieobecnoscEnova = "Wyjście prywatne";//na godziny po właściwym czasie pracy
                                            break;
                                        case "WPP":
                                            zestawienieDzien.NieobecnoscEnova = "Wyjście wcześniejsze (brak pracy)";//na godziny po właściwym czasie pracy
                                            break;
                                        case "WWP":
                                        default:
                                            zestawienieDzien.NieobecnoscEnova = "";
                                            break;
                                    }
                                    break;
                                case 13://czas nieobecności
                                    zestawienieDzien.NieobecnoscEnovaCzas = GodzinaDoubleToSpan(field);
                                    break;
                                case 15://pracaCzas
                                    zestawienieDzien.PracaCzas = GodzinaDoubleToSpan(field);

                                    //string godziny = "";
                                    //try
                                    //{
                                    //    godziny = field.Substring(0, field.IndexOf(","));
                                    //}
                                    //catch (Exception ex) { }
                                    //string minuty = "";
                                    //try
                                    //{
                                    //    minuty = String.Format("{0:00}", double.Parse(field.Substring(field.IndexOf(",") + 1, 2)) /100 * 60);
                                    //}
                                    //catch (Exception ex) { }

                                    //zestawienieDzien.PracaCzas = godziny + ":" + minuty;
                                    break;
                                case 16://jak doszliśmy do tego pola to finito
                                    zestawienie.ZestawienieDni.Add(zestawienieDzien);
                                    break;
                                default:
                                    break;
                            }
                        }
                        i++;
                    }
                    

                    if (field1 == "Data (Datum)")
                    {
                        //wiersz = field;
                        dniOn = true;
                        //listBox1.Items.Add("Data");
                    }

                    if (dniOn)
                        dni = true;

                    //if (wiersz != "")
                    //    listBox1.Items.Add(raportWiersz + ": " + wiersz);
                    
                    raportWiersz++;
                }

                if (zestawienie != null)
                {
                    ZestawieniePracownicy.Add(zestawienie);
                }
            }

            Hashtable pracownicyKody = new Hashtable();//Hashtable na kody pracowników pobrane z bazy
            Hashtable pracownicyNazwiskaImiona = new Hashtable();

            string sql = "SELECT  Pracownicy.Kod, P_PracKody.Kod AS PracKod, Pracownicy.Nazwisko + ' ' + Pracownicy.Imie AS NazwiskoImie " +
                "FROM P_PracKody INNER JOIN Pracownicy ON P_PracKody.PracID = Pracownicy.ID " +
                "LEFT OUTER JOIN Wydzialy ON P_PracKody.WydzID = Wydzialy.ID " +
                "WHERE(Wydzialy.Kod LIKE 'Momox%') " +
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
                        pracownicyKody.Add((string)dt.Rows[i]["PracKod"], (string)dt.Rows[i]["Kod"]);
                    }
                    catch (Exception ex) { }
                }
            }

            foreach (PracownikZestawienie z in ZestawieniePracownicy)
            {
                z.KodEnova = "";
                //listBox1.Items.Add(z.NazwiskoImie + " " + z.KodUKontrahenta + "");
                if (z.KodUKontrahenta != "brak")
                {
                    try
                    {
                        z.KodEnova = pracownicyKody[z.KodUKontrahenta].ToString();
                    }
                    catch (Exception ex) { }

                    if (z.KodEnova == "")
                    {
                        brak = true;
                        listBox1.Items.Add("Nie znaleziono w enovej kodu pracownika: " + z.NazwiskoImie + ", kod Momox: " + z.KodUKontrahenta);
                    }
                }
                else
                {
                    
                    z.KodUKontrahenta = z.NazwiskoImie;
                    

                    try
                    {
                        z.KodEnova = pracownicyKody[z.KodUKontrahenta].ToString();
                    }
                    catch (Exception ex) { }

                    if (z.KodEnova == "")
                    {
                        brak = true;
                        listBox1.Items.Add("Nie znaleziono w enovej kodu pracownika: " + z.NazwiskoImie);
                        listBox1.Items.Add(z.NazwiskoImie + "   Brak kodu Momox w pliku źródłowym");
                        listBox1.Items.Add("   Proszę w enovej wprowadzić nazwisko i imię jako kod pracownika u kontrahenta");
                    }

                    //string nazwoskoImie = z.NazwiskoImie.ToUpper();
                    //string kodEnova = "";
                    //try
                    //{
                    //    kodEnova = pracownicyNazwiskaImiona[nazwoskoImie].ToString();
                    //}catch(Exception ex) {
                    //    listBox1.Items.Add("Dopasowanie nazwiska: " + ex.ToString());
                    //}

                    //if (kodEnova != "")
                    //{
                    //    listBox1.Items.Add("Dopasowano nazwisko i imię: " + nazwoskoImie + ", kod: " + kodEnova);
                    //    z.KodEnova = kodEnova;
                    //}
                    //else
                    //{
                    //    listBox1.Items.Add("Brak dopasowania, pracownik pominięty");
                    //    z.KodEnova = "brak";
                    //}
                }
            }

            string zawartosc = "";
            //List<string> lines = new List<string>();
            zawartosc = @"<?xml version = ""1.0"" encoding = ""Unicode"" ?>" + Environment.NewLine;
            zawartosc += @"<Root xmlns:xsd = ""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">" + Environment.NewLine;
            zawartosc += "  <CzasPracy>" + Environment.NewLine;

            string nieobecnosci = "";
            //nieobecnosci = @"<?xml version = ""1.0"" encoding = ""Unicode"" ?>" + Environment.NewLine;
            //nieobecnosci += @"<Root xmlns:xsd = ""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">" + Environment.NewLine;
            //nieobecnosci += "  <CzasPracy>" + Environment.NewLine;

            foreach (PracownikZestawienie z in ZestawieniePracownicy)
            {
                if (z.KodEnova != "brak" && z.KodEnova != "")
                {
                    //listBox1.Items.Add(z.NazwiskoImie + ", kod u kontrahenta:" + z.KodUKontrahenta + ", kod w enovej:" + z.KodEnova);

                    foreach (Dzien d in z.ZestawienieDni)
                    {
                        //listBox1.Items.Add("Data: " + d.DataS + ", " + d.DzienTygodnia + ", praca od: " +
                        //    d.PracaOd + ", praca czas: " + d.PracaCzas + ", nieobecność: " + d.NieobecnoscKod);
                        if (d.Praca != "")
                        {
                            zawartosc += "    <DzienPracy>" + Environment.NewLine;
                            zawartosc += "      <Pracownik>" + z.KodEnova + "</Pracownik>" + Environment.NewLine;
                            zawartosc += "        <Data>" + d.DataS + "</Data>" + Environment.NewLine;
                            zawartosc += "        <Strefy>" + Environment.NewLine;
                            zawartosc += "          <StrefaPracy Definicja=\"Praca w normie\" OdGodziny=\"" + GodzinaSpanToString(d.PracaOd) + 
                                "\" Czas=\"" + GodzinaSpanToString(d.PracaCzas) + "\">" + Environment.NewLine;
                            zawartosc += "            </StrefaPracy>" + Environment.NewLine;
                            zawartosc += "        </Strefy>" + Environment.NewLine;
                            zawartosc += "    </DzienPracy>" + Environment.NewLine;
                        }
                        if (d.NieobecnoscEnova != "")
                        {
                            nieobecnosci += "    <Nieobecnosc>" + Environment.NewLine;
                            nieobecnosci += "      <Pracownik>" + z.KodEnova + "</Pracownik>" + Environment.NewLine;
                            nieobecnosci += "        <Okres>" + d.DataS + "</Okres>" + Environment.NewLine;
                            nieobecnosci += "        <Definicja>" + d.NieobecnoscEnova + "</Definicja>" + Environment.NewLine;
                            if (d.NieobecnoscEnova == "Urlop wypoczynkowy prac.tymcz.")
                                nieobecnosci += "        <PrzyczynaUrlopu>" + d.NieobecnoscEnovaPrzyczyna + "</PrzyczynaUrlopu>" + Environment.NewLine;

                            if (GodzinaSpanToString(d.NieobecnoscEnovaCzas) != "0:00" && GodzinaSpanToString(d.NieobecnoscEnovaCzas) != "8:00")
                            {
                                nieobecnosci += "        <Norma>" + GodzinaSpanToString(d.NieobecnoscEnovaCzas) + "</Norma>" + Environment.NewLine;
                                listBox1.Items.Add("pracaOd: " + d.PracaOd + ", pracaCzas: " + d.PracaCzas + ", nieobCzas: " + d.NieobecnoscEnovaCzas);
                                if (d.NieobecnoscEnova != "Spóźnienie")
                                {
                                    TimeSpan nieOd = d.PracaOd.Add(d.PracaCzas);
                                    TimeSpan nieDo = nieOd.Add(d.NieobecnoscEnovaCzas);
                                    listBox1.Items.Add("nieOd: " + nieOd + ", nieDo: " + nieDo);
                                    nieobecnosci += "        <OdGodziny>" + GodzinaSpanToString(nieOd) + "</OdGodziny>" + Environment.NewLine;
                                    nieobecnosci += "        <DoGodziny>" + GodzinaSpanToString(nieDo) + "</DoGodziny>" + Environment.NewLine;
                                }
                                else
                                {
                                    TimeSpan nieOd = d.PracaOd.Subtract(d.NieobecnoscEnovaCzas);
                                    TimeSpan nieDo = d.PracaOd;
                                    listBox1.Items.Add("nieOd: " + nieOd + ", nieDo: " + nieDo);
                                    nieobecnosci += "        <OdGodziny>" + GodzinaSpanToString(nieOd) + "</OdGodziny>" + Environment.NewLine;
                                    nieobecnosci += "        <DoGodziny>" + GodzinaSpanToString(nieDo) + "</DoGodziny>" + Environment.NewLine;
                                }

                            }
                            //if (d.NieobecnoscEnovaCzas != "" || d.NieobecnoscEnovaCzas != )
                            nieobecnosci += "    </Nieobecnosc>" + Environment.NewLine;
                        }
                    }
                }
            }
            zawartosc += nieobecnosci;

            zawartosc += "  </CzasPracy>" + Environment.NewLine;
            zawartosc += "</Root>" + Environment.NewLine;

            //nieobecnosci += "  </CzasPracy>" + Environment.NewLine;
            //nieobecnosci += "</Root>" + Environment.NewLine;

            try
            {
                if (!brak && zawartosc != "" || chkPominBrakiKodow.Checked)
                {
                    listBox1.Items.Add("Generowanie pliku");
                    try
                    {
                        string file1 = file.ToLower().Replace(".csv", "_konwersja.xml");
                        listBox1.Items.Add(file1);
                        File.WriteAllText(file1, zawartosc, Encoding.Unicode);
                        //XmlDocument doc = new XmlDocument();
                        //doc.LoadXml(zawartosc);
                        //doc.Save(file);

                        //listBox1.Items.Add(fileDirectory);
                        //listBox1.Items.Add(fileName);
                        //using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                        //{

                        //    file.WriteLine(zawartosc);
                        //}
                    }
                    catch (Exception ex) { listBox1.Items.Add("1: " + ex.ToString()); }
                    //try
                    //{
                    //    string file1 = file.ToLower().Replace(".csv", "_czas_konw.xml");
                    //    listBox1.Items.Add(file1);
                    //    File.WriteAllText(file1, zawartosc, Encoding.Unicode);
                    //    //XmlDocument doc = new XmlDocument();
                    //    //doc.LoadXml(zawartosc);
                    //    //doc.Save(file);

                    //    //listBox1.Items.Add(fileDirectory);
                    //    //listBox1.Items.Add(fileName);
                    //    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                    //    //{

                    //    //    file.WriteLine(zawartosc);
                    //    //}
                    //}
                    //catch (Exception ex) { listBox1.Items.Add("1: " + ex.ToString()); }
                    //try
                    //{
                    //    string file2 = file.ToLower().Replace(".csv", "_nieob_konw.xml");
                    //    listBox1.Items.Add(file2);
                    //    File.WriteAllText(file2, nieobecnosci, Encoding.Unicode);
                    //    //XmlDocument doc = new XmlDocument();
                    //    //doc.LoadXml(zawartosc);
                    //    //doc.Save(file);

                    //    //listBox1.Items.Add(fileDirectory);
                    //    //listBox1.Items.Add(fileName);
                    //    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                    //    //{

                    //    //    file.WriteLine(zawartosc);
                    //    //}
                    //}
                    //catch (Exception ex) { listBox1.Items.Add("1: " + ex.ToString()); }

                }

            }
            catch (Exception ex) { listBox1.Items.Add("3: " + ex.ToString()); }

            btnLog.Enabled = true;
        }

        private TimeSpan GodzinaDoubleToSpan(string godzinaDouble)
        {
            godzinaDouble = godzinaDouble.Replace("-","");
            double sekundy = 0;
            try
            {
                sekundy = double.Parse(godzinaDouble.Substring(0, godzinaDouble.IndexOf(","))) * 3600;
            }
            catch (Exception ex) { }

            sekundy = Math.Round(sekundy / 60, 0) * 60;

            TimeSpan godzinaTime = TimeSpan.FromSeconds(sekundy);
            return godzinaTime;
        }

        private string GodzinaSpanToString(TimeSpan godzina)
        {
            return string.Format("{0}:{1:00}", godzina.Hours, godzina.Minutes);
        }

        private void cbxFirma_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void btnLog_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Zapisz log";
            saveFileDialog1.FileName = "log.txt";
            DialogResult dialog = saveFileDialog1.ShowDialog();
            if (dialog == DialogResult.OK)
            {
                StreamWriter SaveFile = new System.IO.StreamWriter(saveFileDialog1.FileName);
                foreach (var item in listBox1.Items)
                {
                    SaveFile.WriteLine(item.ToString());
                }
                SaveFile.Close();
                MessageBox.Show("Zakończono");
            }
        }
    }
}
