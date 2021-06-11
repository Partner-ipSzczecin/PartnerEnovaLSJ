using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Soneta.Business;
using Soneta.Business.App;
using Soneta.Kadry;
using Soneta.Types;
using Soneta.Kasa;

/*
 * Struktura danych
 * 
 * "stepNationality": {
  "p0001": "Obywatelstwo"
},

"stepPersonalData": {
  "p0002": "Nazwisko",
  "p0003": "Imię",
  "p0004": "Drugie imię",
  "p0005": "Nazwisko rodowe",
  "p0006": "Imię ojca",
  "p0007": "Imię matki",
  "p0008": "Data urodzenia",
  "p0009": "Miejsce urodzenia",
  "p0010": "Nr PESEL",
  "p0011": "Nr PESEL lub nr paszportu jeżeli nie posiadasz nr PESEL",
  "p0012": "Przynależność do oddziału NFZ",
  "p0013": "Przynależność do Urzędu Skarbowego",
  "p0065": "Wykształcenie",
  "p0014": "Jestem osobą prowadzącą działalność gospodarczą",
  "p0015": "NIP",
  "p0016": "Czy posiada Pan/Pani emeryturę",
  "p0017": "Nr świadczenia",
  "p0018": "Czy posiada Pan/Pani rentę",
  "p0019": "Nr świadczenia",
  "p0020": "Orzeczenie o niepełnosprawności",
  "p0021": "Stopień niepełnosprawności",
  "p0022": "Orzeczenie o niepełnosprawności, data od",
  "p0023": "Orzeczenie o niepełnosprawności, data do",
  "p0024": "Orzeczenie o stopniu niezdolności do pracy",
  "p0025": "Orzeczenie o stopniu niezdolności do pracy, data od",
  "p0026": "Orzeczenie o stopniu niezdolności do pracy, data do",
  "p0027": "Nr orzeczenia"
},

"step3": {
  "p0028": "Kod pocztowy",
  "p0029": "Miejscowość",
  "p0030": "Ulica",
  "p0031": "Nr domu",
  "p0032": "Nr lokalu",
  "p0033": "Gmina",
  "p0034": "Powiat",
  "p0035": "Województwo",
  "p0036": "Państwo",
  "p0066": "Obwód",
  "p0067": "Rejon"
},

"stepAddress": {
  "p0037": "Czy adres zamieszkania jest inny od adresu zameldowania",
  "p0038": "Kod pocztowy",
  "p0039": "Miejscowość",
  "p0040": "Ulica",
  "p0041": "Nr domu",
  "p0042": "Nr lokalu",
  "p0043": "Gmina",
  "p0044": "Powiat",
  "p0045": "Województwo",
  "p0046": "Państwo",
  "p0068": "Obwód",
  "p0069": "Rejon"
},

"stepCorrespondenceAddress": {
  "t0001": "Kopiuj adres zameldowania",
  "t0002": "Kopiuj adres zamieszkania",

  "p0047": "Kod pocztowy",
  "p0048": "Miejscowość",
  "p0049": "Ulica",
  "p0050": "Nr domu",
  "p0051": "Nr lokalu",
  "p0052": "Gmina",
  "p0070": "Powiat",
  "p0071": "Województwo",
  "p0064": "Państwo"
},

"stepIdCard": {
  "p0053": "Rodzaj dokumentu tożsamości",
  "p0054": "Wydany przez",
  "p0055": "Seria i nr"
},

"stepContactData": {
  "t0001": "Osoba, którą należy zawiadomić w razie wypadku",

  "p0056": "Telefon kontaktowy",
  "p0057": "Adres mailowy",
  "p0058": "Imię, nazwisko",
  "p0059": "Adres",
  "p0060": "Telefon"
},

"stepBankAccount": {
  "p0061": "Nazwa banku",
  "p0062": "Numer konta bankowego",
  "p0072": "Numer konta bankowego",
  "p0063": "Dane adresowe do przelewu uwzględnić z",
  "p0077": "Wynagrodzenie i inne świadczenia pieniężne ze stosunku pracy przekazywać"
},

"stepStatements": {
  "p0073": "Wykonywałem pracę tymczasową na rzecz Pracodawcy - Użytkownika, u którego będzie powierzone wykonywanie pracy tymczasowej",
  "p0074": "Wyrażam zgodę na nie wypłacanie przez Pracodawcę ekwiwalentu za urlop wypoczynkowy, do którego nabywam prawo w trakcie zatrudnienia na podstawie kolejnych, zawieranych bezpośrednio po sobie umów o pracę",
  "p0075": "Uprawnienie do urlopu wypoczynkowego nabyte przez Pracownika tymczasowego za przepracowany okres, zostanie zrealizowane w trakcie kontynuowanego zatrudnienia",
  "p0076": "Niniejsze porozumienie dotyczy wszystkich umów, stanowiących kontynuację zatrudnienia na rzecz tego samego Pracodawcy Użytkownika (tj. umów o pracę zawieranych bezpośrednio po zakończeniu poprzedniej). Podstawa prawna art. 171 § 3 K.p."
},

"stepParentStatement": {
  "p0078": "Jestem rodzicem (opiekunem) dziecka do lat 4 (imię, nazwisko i data urodzenia dziecka)",
  "p0079": "Praca powyżej 8h na dobę (dot. systemu równoważnego czasu pracy, systemu skróconego tygodnia pracy, systemu pracy weekendowej) w dniach przedłużonego dziennego wymiaru czasu pracy (art. 148 pkt 3 K.p.)",
  "p0080": "Praca w godzinach nadliczbowych",
  "p0081": "Praca w porze nocnej (art. 178 par. 2 K.p.)",
  "p0082": "Praca w systemie przerywanego czasu pracy (art. 178 par. 2 K.p.)",
  "p0083": "Delegowanie poza stałe miejsce pracy (art. 178 par. 2 K.P.)"
},

 */

namespace PartnerEnovaNormaPraca
{
    public partial class frmImportZRejestracji : Form
    {
        Login login;
        string importUser = "";
        String username = "";
        String password = "";
        string token = "";
        string status = "";
        string nazwisko = "";
        string imie = "";
        DateTime dataOd = DateTime.MinValue;
        DateTime dataDo = DateTime.MaxValue;
        string obywatelstwo = "";
        string nrTelefonu = "";
        string email = "";
        string pesel = "";
        string liczbaWierszy = "100";

        List<Aplikacja> Aplikacje = new List<Aplikacja>();
        DaneSzczegolowe daneSzczegolowe = null;

        internal void InitContext(Context cx)
        {
            //obiekt logowania przejęty z kontekstu
            login = (Login)cx[typeof(Login)];
        }

        public frmImportZRejestracji()
        {
            InitializeComponent();
        }

        private void frmImportZREjestracji_Load(object sender, EventArgs e)
        {
            //Sprawdzamy czy w rejestrach jest klucz z danymi połaczenia
            RegistryKey rKey = null;
            try
            {
                rKey = Registry.CurrentUser.OpenSubKey("Software\\Partner-ip\\EnovaLSJ\\Import", true);
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
                    importUser = (string)rKey.GetValue("User");
                }
                catch (Exception ex) { }
            }

            txtLogin.Text = importUser;
            dataOd = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dataDo = dataOd.AddMonths(1).AddDays(-1);
            dtpDataOd.Value = dataOd;
            dtpDataDo.Value = dataDo;

            //btnImportuj.Enabled = false;
            btnFiltruj.Enabled = false;

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add("username", "username");
            dataGridView1.Columns.Add("status", "Status");
            dataGridView1.Columns.Add("imieNazwisko", "Imię i Nazwisko");
            dataGridView1.Columns.Add("obywatelstwo", "Obywatelstwo");
            dataGridView1.Columns.Add("wiek", "Wiek");
            dataGridView1.Columns.Add("email", "E-mail");
            dataGridView1.Columns.Add("telefon", "Telefon");
            dataGridView1.Columns.Add("dataRej", "Data dodania");
            dataGridView1.Columns[0].DataPropertyName = "username";
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].DataPropertyName = "status";
            dataGridView1.Columns[2].DataPropertyName = "imieNazwisko";
            dataGridView1.Columns[3].DataPropertyName = "obywatelstwo";
            dataGridView1.Columns[4].DataPropertyName = "wiek";
            dataGridView1.Columns[5].DataPropertyName = "email";
            dataGridView1.Columns[6].DataPropertyName = "telefon";
            dataGridView1.Columns[7].DataPropertyName = "dataRej";

            int ilCol = dataGridView1.Columns.Count;

            for (int i = 0; i < ilCol; i++)
            {
                dataGridView1.Columns[i].ReadOnly = true;
            }

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {

                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            username = txtLogin.Text;
            password = txtPassword.Text;
            String base64 = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            HttpWebRequest reqLogin = (HttpWebRequest)WebRequest.Create("https://rejestracja.lsj.pl/api/token");
            reqLogin.Headers.Add("Authorization", "Basic " + base64);
            Stream objStream;
            objStream = reqLogin.GetResponse().GetResponseStream();
            using (StreamReader objReader = new StreamReader(objStream))
            {
                string sLine = "";
                int i = 0;
                while (sLine != null)
                {
                    i++;
                    sLine = objReader.ReadLine();
                    //if (sLine != null)
                    //    listBox1.Items.Add(i + ":" + sLine);
                    if (i == 1)
                    {
                        JObject jsonObject = JObject.Parse(sLine);
                        //listBox1.Items.Add("role: " + jsonObject["role"]);
                        //listBox1.Items.Add("tokenJ: " + jsonObject["token"]);
                        if (token == "")
                        {
                            token = jsonObject["token"].ToString();
                            listBox1.Items.Add("Zalogowano");
                            btnFiltruj.Enabled = true;
                        }
                    }
                }
            }
        }

        private void btnFiltruj_Click(object sender, EventArgs e)
        {
            
            Aplikacje.Clear();

            if (btnImportuj.Enabled)
                btnImportuj.Enabled = false;
            listBox1.Items.Clear();
            status = cbxStatus.Text;
            nazwisko = txtNazwisko.Text;
            imie = txtImie.Text;
            dataOd = dtpDataOd.Value;
            dataDo = dtpDataDo.Value;
            if (cbxObywatelstwo.Text != "Dowolne")
                obywatelstwo = cbxObywatelstwo.Text;
            else
                obywatelstwo = "";
            nrTelefonu = txtNrTelefonu.Text;
            email = txtEmail.Text;
            pesel = txtPesel.Text;
            liczbaWierszy = nudLiczbaWierszy.Value.ToString();

            //" + liczbaWierszy + "
            HttpWebRequest reqLista = (HttpWebRequest)WebRequest.Create("https://rejestracja.lsj.pl/reports?status=" + status +
                "&nazwisko=" + nazwisko + "&imie=" + imie +
                "&obywatelstwo=" + obywatelstwo + "&telefon=" + nrTelefonu + "&email=" + email + "&pesel=" + pesel +
                "&nip=&maxRowsCount=" + liczbaWierszy + "&dataDodaniaOd=" + dataOd.ToShortDateString() +
                "&dataDodaniaDo=" + dataDo.ToShortDateString());
            reqLista.Headers.Add("Authorization", "Bearer " + token);
            //listBox1.Items.Add("token: " + token);
            WebResponse response = reqLista.GetResponse();
            listBox1.Items.Add(((HttpWebResponse)response).StatusDescription);
            Stream dataStream = response.GetResponseStream();
            using (StreamReader objReader = new StreamReader(dataStream))
            {
                var json = objReader.ReadToEnd();
                var Osoby = JsonConvert.DeserializeObject<List<Aplikacja>>(json);
                Aplikacje = Osoby;
                dataGridView1.DataSource = Aplikacje;

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnImportuj.Enabled = true;
            string username = "";
            try
            {
                username = (string)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells["username"].Value;
            }
            catch (Exception ex) { }

            if (username != "")
            {
                HttpWebRequest reqLista = (HttpWebRequest)WebRequest.Create("https://rejestracja.lsj.pl/answer/" + username);
                reqLista.Headers.Add("Authorization", "Bearer " + token);
                //listBox1.Items.Add("token: " + token);
                WebResponse response = reqLista.GetResponse();
                Stream dataStream = response.GetResponseStream();
                using (StreamReader objReader = new StreamReader(dataStream))
                {
                    var json = objReader.ReadToEnd();
                    int poczatek = json.IndexOf("answers\":[") + 9;
                    string jsonS = "";
                    try
                    {
                        jsonS = json.Substring(poczatek, json.Length - poczatek - 1);
                    }
                    catch (Exception ex) { }
                    //if (jsonS != "")
                    //{
                    //    listBox1.Items.Add(jsonS);
                    //    listBox1.Items.Add(jsonS.Length);
                    //    listBox1.Items.Add(jsonS.Substring(jsonS.Length - 1, 1));
                    //}
                    //listBox1.Items.Add(json);

                    var daneSlownik = JsonConvert.DeserializeObject<List<DaneSlownik>>(jsonS);
                    List<DaneSlownik> dane = daneSlownik;
                    daneSzczegolowe = new DaneSzczegolowe();
                    foreach (var d in dane)
                    {
                        try { if (d.questionId == "p0001") { daneSzczegolowe.p0001 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0002") { daneSzczegolowe.p0002 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0003") { daneSzczegolowe.p0003 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0004") { daneSzczegolowe.p0004 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0005") { daneSzczegolowe.p0005 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0006") { daneSzczegolowe.p0006 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0007") { daneSzczegolowe.p0007 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0008") { daneSzczegolowe.p0008 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0009") { daneSzczegolowe.p0009 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0010") { daneSzczegolowe.p0010 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0011") { daneSzczegolowe.p0011 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0012") { daneSzczegolowe.p0012 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0013") { daneSzczegolowe.p0013 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0014") { daneSzczegolowe.p0014 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0015") { daneSzczegolowe.p0015 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0016") { daneSzczegolowe.p0016 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0017") { daneSzczegolowe.p0017 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0018") { daneSzczegolowe.p0018 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0019") { daneSzczegolowe.p0019 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0020") { daneSzczegolowe.p0020 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0021") { daneSzczegolowe.p0021 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0022") { daneSzczegolowe.p0022 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0023") { daneSzczegolowe.p0023 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0024") { daneSzczegolowe.p0024 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0025") { daneSzczegolowe.p0025 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0026") { daneSzczegolowe.p0026 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0027") { daneSzczegolowe.p0027 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0028") { daneSzczegolowe.p0028 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0029") { daneSzczegolowe.p0029 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0030") { daneSzczegolowe.p0030 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0031") { daneSzczegolowe.p0031 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0032") { daneSzczegolowe.p0032 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0033") { daneSzczegolowe.p0033 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0034") { daneSzczegolowe.p0034 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0035") { daneSzczegolowe.p0035 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0036") { daneSzczegolowe.p0036 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0037") { daneSzczegolowe.p0037 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0038") { daneSzczegolowe.p0038 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0039") { daneSzczegolowe.p0039 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0040") { daneSzczegolowe.p0040 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0041") { daneSzczegolowe.p0041 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0042") { daneSzczegolowe.p0042 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0043") { daneSzczegolowe.p0043 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0044") { daneSzczegolowe.p0044 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0045") { daneSzczegolowe.p0045 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0046") { daneSzczegolowe.p0046 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0047") { daneSzczegolowe.p0047 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0048") { daneSzczegolowe.p0048 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0049") { daneSzczegolowe.p0049 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0050") { daneSzczegolowe.p0050 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0051") { daneSzczegolowe.p0051 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0052") { daneSzczegolowe.p0052 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0053") { daneSzczegolowe.p0053 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0054") { daneSzczegolowe.p0054 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0055") { daneSzczegolowe.p0055 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0056") { daneSzczegolowe.p0056 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0057") { daneSzczegolowe.p0057 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0058") { daneSzczegolowe.p0058 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0059") { daneSzczegolowe.p0059 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0060") { daneSzczegolowe.p0060 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0061") { daneSzczegolowe.p0061 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0062") { daneSzczegolowe.p0062 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0063") { daneSzczegolowe.p0063 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0064") { daneSzczegolowe.p0064 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0065") { daneSzczegolowe.p0065 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0066") { daneSzczegolowe.p0066 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0067") { daneSzczegolowe.p0067 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0068") { daneSzczegolowe.p0068 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0069") { daneSzczegolowe.p0069 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0070") { daneSzczegolowe.p0070 = d.valuePl; } } catch (Exception ex) { }
                        try { if (d.questionId == "p0071") { daneSzczegolowe.p0071 = d.valuePl; } } catch (Exception ex) { }

                    }
                    WyswietlDane(0);
                }
            }
        }

        /// <summary>
        /// Wyświetlanie danych
        /// </summary>
        /// <param name="tryb">
        /// 0 - inport
        /// 1 - szczegółowe
        /// </param>
        private void WyswietlDane(int tryb)
        {
            listBox1.Items.Clear();

            try { listBox1.Items.Add("p0001: Obywatelstwo: " + daneSzczegolowe.p0001); } catch { }
            try { listBox1.Items.Add("p0002: Nazwisko: " + daneSzczegolowe.p0002); } catch { }
            try { listBox1.Items.Add("p0003: Imię: " + daneSzczegolowe.p0003); } catch { }
            try { listBox1.Items.Add("p0004: Drugie imię: " + daneSzczegolowe.p0004); } catch { }
            try { listBox1.Items.Add("p0005: Nazwisko rodowe: " + daneSzczegolowe.p0005); } catch { }
            try { listBox1.Items.Add("p0006: Imię ojca: " + daneSzczegolowe.p0006); } catch { }
            try { listBox1.Items.Add("p0007: Imię matki: " + daneSzczegolowe.p0007); } catch { }
            try { listBox1.Items.Add("p0008: Data urodzenia: " + daneSzczegolowe.p0008); } catch { }
            try { listBox1.Items.Add("p0009: Miejsce urodzenia: " + daneSzczegolowe.p0009); } catch { }
            try { listBox1.Items.Add("p0010: Nr PESEL: " + daneSzczegolowe.p0010); } catch { }
            try { listBox1.Items.Add("p0011: Nr paszportu: " + daneSzczegolowe.p0011); } catch { }
            try { listBox1.Items.Add("p0012: Przynależność do oddziału NFZ: " + daneSzczegolowe.p0012); } catch { }
            try { listBox1.Items.Add("p0013: Przynależność do Urzędu Skarbowego: " + daneSzczegolowe.p0013); } catch { }
            if (tryb == 1)
                try { listBox1.Items.Add("p0014: " + daneSzczegolowe.p0014); } catch { }
            try { listBox1.Items.Add("p0028: NIP: " + daneSzczegolowe.p0015); } catch { }
            if (tryb == 1)
            {
                try { listBox1.Items.Add("p0016: " + daneSzczegolowe.p0016); } catch { }
                try { listBox1.Items.Add("p0017: " + daneSzczegolowe.p0017); } catch { }
                try { listBox1.Items.Add("p0018: " + daneSzczegolowe.p0018); } catch { }
                try { listBox1.Items.Add("p0019: " + daneSzczegolowe.p0019); } catch { }
                try { listBox1.Items.Add("p0020: " + daneSzczegolowe.p0020); } catch { }
                try { listBox1.Items.Add("p0021: " + daneSzczegolowe.p0021); } catch { }
                try { listBox1.Items.Add("p0022: " + daneSzczegolowe.p0022); } catch { }
                try { listBox1.Items.Add("p0023: " + daneSzczegolowe.p0023); } catch { }
                try { listBox1.Items.Add("p0024: " + daneSzczegolowe.p0024); } catch { }
                try { listBox1.Items.Add("p0025: " + daneSzczegolowe.p0025); } catch { }
            }
            try { listBox1.Items.Add(""); } catch { }
            try { listBox1.Items.Add("Adres zameldowania"); } catch { }
            try { listBox1.Items.Add("p0028: Kod pocztowy: " + daneSzczegolowe.p0028); } catch { }
            try { listBox1.Items.Add("p0029: Miejscowość: " + daneSzczegolowe.p0029); } catch { }
            try { listBox1.Items.Add("p0030: Ulica: " + daneSzczegolowe.p0030 + " " + daneSzczegolowe.p0031 + "/" + daneSzczegolowe.p0032); } catch { }
            if (tryb == 1)
            {
                try { listBox1.Items.Add("p0031: Nr domu: " + daneSzczegolowe.p0031); } catch { }
                try { listBox1.Items.Add("p0032: " + daneSzczegolowe.p0032); } catch { }
            }
            try { listBox1.Items.Add("p0033: Gmina: " + daneSzczegolowe.p0033); } catch { }
            try { listBox1.Items.Add("p0034: Powiat: " + daneSzczegolowe.p0034); } catch { }
            try { listBox1.Items.Add("p0035: Województwo: " + daneSzczegolowe.p0035); } catch { }
            try { listBox1.Items.Add("p0036: Państwo: " + daneSzczegolowe.p0036); } catch { }
            try { listBox1.Items.Add(""); } catch { }
            try { listBox1.Items.Add("Adres zamieszkania"); } catch { }
            if (tryb == 1)
                try { listBox1.Items.Add("p0037: Adres zamieszkania inny od adresu zameldowania: " + daneSzczegolowe.p0037); } catch { }
            try { listBox1.Items.Add("p0038: Kod pocztowy: " + daneSzczegolowe.p0038); } catch { }
            try { listBox1.Items.Add("p0039: Miejscowość: " + daneSzczegolowe.p0039); } catch { }
            try { listBox1.Items.Add("p0040: Ulica p0040: " + daneSzczegolowe.p0040 + " " + daneSzczegolowe.p0041 + "/" + daneSzczegolowe.p0042); } catch { }
            if (tryb == 1)
            {
                try { listBox1.Items.Add("p0041: Nr domu: " + daneSzczegolowe.p0041); } catch { }
                try { listBox1.Items.Add("p0042: " + daneSzczegolowe.p0042); } catch { }
                try { listBox1.Items.Add("p0043: " + daneSzczegolowe.p0043); } catch { }
                try { listBox1.Items.Add("p0044: " + daneSzczegolowe.p0044); } catch { }
                try { listBox1.Items.Add("p0045: " + daneSzczegolowe.p0045); } catch { }
            }
            try { listBox1.Items.Add("p0046: Państwo: " + daneSzczegolowe.p0046); } catch { }
            try { listBox1.Items.Add(""); } catch { }
            try { listBox1.Items.Add("Adres korespondencyjny"); } catch { }
            try { listBox1.Items.Add("p0047: Kod pocztowy: " + daneSzczegolowe.p0047); } catch { }
            try { listBox1.Items.Add("p0048: Miejscowość: " + daneSzczegolowe.p0048); } catch { }
            try { listBox1.Items.Add("p0049: Ulica: " + daneSzczegolowe.p0049 + " " + daneSzczegolowe.p0050 + "/" + daneSzczegolowe.p0051); } catch { }
            if (tryb == 1)
            {
                try { listBox1.Items.Add("p0050: Nr domu: " + daneSzczegolowe.p0050); } catch { }
                try { listBox1.Items.Add("p0051: " + daneSzczegolowe.p0051); } catch { }
            }
            try { listBox1.Items.Add("p0052: Gmina: " + daneSzczegolowe.p0052); } catch { }
            try { listBox1.Items.Add(""); } catch { }
            try { listBox1.Items.Add("p0053: Rodzaj dokumentu tożsamości: " + daneSzczegolowe.p0053); } catch { }
            try { listBox1.Items.Add("p0054: Wydany przez: " + daneSzczegolowe.p0054); } catch { }
            try { listBox1.Items.Add("p0055: Seria i nr: " + daneSzczegolowe.p0055); } catch { }
            try { listBox1.Items.Add("p0056: Telefon kontaktowy: " + daneSzczegolowe.p0056); } catch { }
            try { listBox1.Items.Add("p0057: Adres mailowy: " + daneSzczegolowe.p0057); } catch { }
            try { listBox1.Items.Add("p0058: Imię, nazwisko: " + daneSzczegolowe.p0058); } catch { }
            try { listBox1.Items.Add("p0059: Adres: " + daneSzczegolowe.p0059); } catch { }
            try { listBox1.Items.Add("p0060: Telefon: " + daneSzczegolowe.p0060); } catch { }
            try { listBox1.Items.Add("p0061: Nazwa banku: " + daneSzczegolowe.p0061); } catch { }
            try { listBox1.Items.Add("p0062: Numer konta bankowego: " + daneSzczegolowe.p0062); } catch { }
            try { listBox1.Items.Add("p0063: Dane adresowe do przelewu uwzględnić z: " + daneSzczegolowe.p0063); } catch { }
            try { listBox1.Items.Add("p0064: Państwo: " + daneSzczegolowe.p0064); } catch { }
            try { listBox1.Items.Add("p0065: Wykształcenie: " + daneSzczegolowe.p0065); } catch { }
        }

        private void btnImportuj_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            bool jestPracownik = false;
            using (Session session = login.CreateSession(false, false))
            {
                KadryModule km = KadryModule.GetInstance(session);
                KasaModule kasaM = KasaModule.GetInstance(session);
                try
                {
                    using (ITransaction trans = session.Logout(true))
                    {
                        string pesel = "";
                        string nrPaszportu = "";

                        try
                        {
                            pesel = daneSzczegolowe.p0010;
                        }
                        catch { }
                        // Polee z peselem obecnie nie jest wykorzystywane, sprawdzamy pole z numerem paszportu,
                        // w którym obecnie wprowadzany jest także nr pesel
                        if(daneSzczegolowe.p0011 != null)
                        {
                            if (pesel != "")
                            {
                                nrPaszportu = daneSzczegolowe.p0011;
                            }
                            else if (daneSzczegolowe.p0011.Length == 11) // Nr pesel ma 11 cyfr. Paszporty mają inne długości i format
                            {
                                int testPesel = 0;
                                try
                                {
                                    // Sprawdzamy czy wprowadzony ciąg 
                                    testPesel = int.Parse(daneSzczegolowe.p0011);
                                }
                                catch { }
                                if (testPesel != 0)
                                    pesel = daneSzczegolowe.p0011;
                                else
                                {
                                    nrPaszportu = daneSzczegolowe.p0011;
                                }
                            }
                        }

                        foreach (Pracownik p in km.Pracownicy)
                        {
                            try
                            {
                                // Sprawdzamy czy w bazie nie ma już osoby takich samych danych
                                if ((pesel != "" && p.PESEL == pesel) /*daneSzczegolowe.p0010*/ ||
                                   (p.NazwiskoImię.ToUpper() == (daneSzczegolowe.p0002 + " " + daneSzczegolowe.p0003).ToUpper()) ||
                                    p.Last.Dokument.SeriaNumer.ToUpper() == daneSzczegolowe.p0055.ToUpper())
                                {
                                    string komunikat = "W enovej istnieje już pracownik: " + p.NazwiskoImię + " \r\n" +
                                        "PESEL: " + p.PESEL + ", nr dok.: " + p.Last.Dokument.SeriaNumer + " \r\n" +
                                        "Czy dodać tego pracownika?";
                                    DialogResult result = MessageBox.Show(komunikat, "Uwaga", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                    if (result == DialogResult.Cancel)
                                        jestPracownik = true;
                                    break;
                                }
                            }catch(Exception ex)
                            {

                            }
                        }

                        Pracownik prac = new PracownikFirmy();
                        km.Pracownicy.AddRow(prac);
                        PracHistoria ph = prac.Last;
                        if (daneSzczegolowe.p0001 != null)
                            ph.Obywatelstwo.Nazwa = daneSzczegolowe.p0001;
                        if (daneSzczegolowe.p0002 != null)
                            ph.Nazwisko = daneSzczegolowe.p0002;
                        if (daneSzczegolowe.p0003 != null)
                            ph.Imie = daneSzczegolowe.p0003;
                        if (daneSzczegolowe.p0004 != null)
                            ph.ImieDrugie = daneSzczegolowe.p0004;
                        if (daneSzczegolowe.p0005 != null)
                            ph.NazwiskoRodowe = daneSzczegolowe.p0005;
                        if (daneSzczegolowe.p0006 != null)
                            ph.ImieOjca = daneSzczegolowe.p0006;
                        if (daneSzczegolowe.p0007 != null)
                            ph.ImieMatki = daneSzczegolowe.p0007;

                        if (daneSzczegolowe.p0008 != null)
                        {
                            string dataUrS = daneSzczegolowe.p0008;
                            Date dataUr = new Date(int.Parse(dataUrS.Substring(0, 4)), int.Parse(dataUrS.Substring(5, 2)), int.Parse(dataUrS.Substring(8, 2)));
                            ph.Urodzony.Data = dataUr;
                        }
                        if (daneSzczegolowe.p0009 != null)
                            ph.Urodzony.Miejsce = daneSzczegolowe.p0009;
                        // Po jakichś ostatnich zmianach
                        //if (daneSzczegolowe.p0010 != null)
                        //    ph.PESEL = daneSzczegolowe.p0010;
                        if (pesel != "")
                            ph.PESEL = pesel;
                        if (daneSzczegolowe.p0015 != null)
                            ph.NIP = daneSzczegolowe.p0015;
                        if (daneSzczegolowe.p0053 != null)
                        {
                            switch (daneSzczegolowe.p0053)
                            {
                                case "dowód osobisty":
                                    ph.Dokument.Rodzaj = KodRodzajuDokumentu.DowodOsobisty;
                                    if (daneSzczegolowe.p0055 != null)
                                        ph.Dokument.SeriaNumer = daneSzczegolowe.p0055;
                                    if (daneSzczegolowe.p0054 != null)
                                        ph.Dokument.WydanyPrzez = daneSzczegolowe.p0054;
                                    break;
                                case "paszport":
                                    ph.Dokument.Rodzaj = KodRodzajuDokumentu.Paszport;
                                    if (daneSzczegolowe.p0055 != null)
                                        ph.Dokument.SeriaNumer = daneSzczegolowe.p0011;
                                    break;
                                default:
                                    break;
                            }
                        }
                        // Przerzucone do if powyżej
                        //if (ph.Dokument.Rodzaj != KodRodzajuDokumentu.Niezdefiniowany)
                        //{
                        //    if (daneSzczegolowe.p0055 != null)
                        //        ph.Dokument.SeriaNumer = daneSzczegolowe.p0055;
                        //    if (daneSzczegolowe.p0054 != null)
                        //        ph.Dokument.WydanyPrzez = daneSzczegolowe.p0054;
                        //}

                        //ph.OddzialNFZ.Kod //
                        //ph.Wyksztalcenie.

                        //Adres zameldowania
                        if (daneSzczegolowe.p0028 != null)
                            ph.AdresZameldowania.KodPocztowyS = daneSzczegolowe.p0028;
                        if (daneSzczegolowe.p0029 != null)
                            ph.AdresZameldowania.Miejscowosc = daneSzczegolowe.p0029;
                        if (daneSzczegolowe.p0030 != null)
                            ph.AdresZameldowania.Ulica = daneSzczegolowe.p0030;
                        if (daneSzczegolowe.p0031 != null)
                            ph.AdresZameldowania.NrDomu = daneSzczegolowe.p0031;
                        if (daneSzczegolowe.p0032 != null)
                            ph.AdresZameldowania.NrLokalu = daneSzczegolowe.p0032;
                        if (daneSzczegolowe.p0034 != null)
                            ph.AdresZameldowania.Powiat = daneSzczegolowe.p0034;
                        if (daneSzczegolowe.p0033 != null)
                            ph.AdresZameldowania.Gmina = daneSzczegolowe.p0033;
                        if (daneSzczegolowe.p0035 != null)
                        {
                            switch (daneSzczegolowe.p0035)
                            {
                                case "dolnośląskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.dolnośląskie;
                                    break;
                                case "kujawsko-pomorskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.kujawsko_pomorskie;
                                    break;
                                case "lubelskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.lubelskie;
                                    break;
                                case "lubuskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.lubuskie;
                                    break;
                                case "mazowieckie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.mazowieckie;
                                    break;
                                case "małopolskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.małopolskie;
                                    break;
                                case "opolskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.opolskie;
                                    break;
                                case "podlaskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.podlaskie;
                                    break;
                                case "pomorskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.pomorskie;
                                    break;
                                case "warmińsko-mazurskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.warmińsko_mazurskie;
                                    break;
                                case "wielkopolskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.wielkopolskie;
                                    break;
                                case "zachodniopomorskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.zachodniopomorskie;
                                    break;
                                case "łódzkie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.łódzkie;
                                    break;
                                case "śląskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.śląskie;
                                    break;
                                case "świętokrzyskie":
                                    ph.AdresZameldowania.Wojewodztwo = Soneta.Core.Wojewodztwa.świętokrzyskie;
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (daneSzczegolowe.p0036 != null)
                            ph.AdresZameldowania.Kraj = daneSzczegolowe.p0036;


                        //Adres zamieszkania
                        if (daneSzczegolowe.p0038 != null)
                            ph.AdresZamieszkania.KodPocztowyS = daneSzczegolowe.p0038;
                        if (daneSzczegolowe.p0039 != null)
                            ph.AdresZamieszkania.Miejscowosc = daneSzczegolowe.p0039;
                        if (daneSzczegolowe.p0040 != null)
                            ph.AdresZamieszkania.Ulica = daneSzczegolowe.p0040;
                        if (daneSzczegolowe.p0041 != null)
                            ph.AdresZamieszkania.NrDomu = daneSzczegolowe.p0041;
                        if (daneSzczegolowe.p0042 != null)
                            ph.AdresZamieszkania.NrLokalu = daneSzczegolowe.p0042;
                        if (daneSzczegolowe.p0046 != null)
                            ph.AdresZamieszkania.Kraj = daneSzczegolowe.p0046;

                        //Adres korespondencyjny
                        if (daneSzczegolowe.p0047 != null)
                            ph.AdresDoKorespondencji.KodPocztowyS = daneSzczegolowe.p0047;
                        if (daneSzczegolowe.p0048 != null)
                            ph.AdresDoKorespondencji.Miejscowosc = daneSzczegolowe.p0048;
                        if (daneSzczegolowe.p0049 != null)
                            ph.AdresDoKorespondencji.Ulica = daneSzczegolowe.p0049;
                        if (daneSzczegolowe.p0050 != null)
                            ph.AdresDoKorespondencji.NrDomu = daneSzczegolowe.p0050;
                        if (daneSzczegolowe.p0051 != null)
                            ph.AdresDoKorespondencji.NrLokalu = daneSzczegolowe.p0051;
                        if (daneSzczegolowe.p0052 != null)
                            ph.AdresDoKorespondencji.Gmina = daneSzczegolowe.p0052;
                        if (daneSzczegolowe.p0060 != null)
                            ph.Powiadomic.Telefon = daneSzczegolowe.p0060;// Zmieniono na telefon do osoby, którą należy powiadomić w razie wypadku
                        //ph.AdresDoKorespondencji.Telefon = daneSzczegolowe.p0060;

                        if (daneSzczegolowe.p0056 != null)
                            ph.Kontakt.TelefonKomorkowy = daneSzczegolowe.p0056;
                        if (daneSzczegolowe.p0057 != null)
                            ph.Kontakt.EMAIL = daneSzczegolowe.p0057;

                        if (daneSzczegolowe.p0062 != null)
                        {
                            RachunekBankowyPracownika rachunek = new RachunekBankowyPracownika(prac);
                            kasaM.RachBankPodmiot.AddRow(rachunek);
                            rachunek.Rachunek.Numer.Pełny = daneSzczegolowe.p0062;
                        }
                        //rbp.
                    
                        if (!jestPracownik)
                            trans.Commit();
                        else
                            trans.Dispose();
                    }
                    if (!jestPracownik)
                    {
                        session.Save();
                        MessageBox.Show("Dodano pracownika");
                    }
                }
                catch (Exception ex) { listBox1.Items.Add(ex.ToString()); }
            }

        }

        private void frmImportZRejestracji_FormClosing(object sender, FormClosingEventArgs e)
        {
            importUser = txtLogin.Text;

            //Sprawdzamy czy jest klucz w rejestrze i jeśli takiego nie ma to tworzymy
            RegistryKey rKey = null;
            try
            {
                rKey = Registry.CurrentUser.OpenSubKey("Software\\Partner-ip\\EnovaLSJ\\Import", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (rKey == null)
            {
                rKey = Registry.CurrentUser.OpenSubKey("Software", true);
                rKey.CreateSubKey("Partner-ip\\EnovaLSJ\\Import");
                rKey.Close();
            }

            //Zapisujemy do rejestru dane połączenia z bazą
            RegistryKey rKeyValue = null;
            try
            {
                rKeyValue = Registry.CurrentUser.OpenSubKey("Software\\Partner-ip\\EnovaLSJ\\Import", true);
                rKeyValue.SetValue("User", importUser);
                rKeyValue.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void pokazDaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WyswietlDane(1);
        }

        private void zamknijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Kopiowanie zawartości listbox'a do schowka
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kopiujZawartoscToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s1 = "";
            foreach (object item in listBox1.Items) s1 += item.ToString() + "\r\n";
            Clipboard.SetText(s1);
        }
    }

    public class Aplikacja
    {
        public string username { get; set; }
        public string status { get; set; }
        public string dataRej { get; set; }
        public string dataDruk { get; set; }
        public string dataZablokowany { get; set; }
        public string dataNowy { get; set; }
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public string imieNazwisko { get; set; }
        public string wiek { get; set; }
        public string obywatelstwo { get; set; }
        public string dataUrodz { get; set; }
        public string email { get; set; }
        public string telefon { get; set; }
        public string pesel { get; set; }
        public string nip { get; set; }
        public string id { get; set; }
    }

    //public class DaneZwrot
    //{
    //    public string username { get; set; }
    //    public string answers { get; set; }
    //}

    public class DaneSlownik
    {
        public string questionId { get; set; }
        public string dictElemId { get; set; }
        public string valuePl { get; set; }
    }

    public class DaneSzczegolowe
    {
        public string p0001 { get; set; } //Obywatelstwo
        public string p0002 { get; set; } //Nazwisko
        public string p0003 { get; set; } //Imię
        public string p0004 { get; set; } //Drugie imię
        public string p0005 { get; set; } //Nazwisko rodowe
        public string p0006 { get; set; } //Imię ojca
        public string p0007 { get; set; } //Imię matki
        public string p0008 { get; set; } //Data urodzenia
        public string p0009 { get; set; } //Miejsce urodzenia
        public string p0010 { get; set; } //Nr PESEL
        public string p0011 { get; set; } //Nr PESEL lub nr paszportu jeżeli nie posiadasz nr PESEL
        public string p0012 { get; set; } //Przynależność do oddziału NFZ
        public string p0013 { get; set; } //Przynależność do Urzędu Skarbowego
        public string p0014 { get; set; }
        public string p0015 { get; set; }
        public string p0016 { get; set; }
        public string p0017 { get; set; }
        public string p0018 { get; set; }
        public string p0019 { get; set; }
        public string p0020 { get; set; }
        public string p0021 { get; set; }
        public string p0022 { get; set; }
        public string p0023 { get; set; }
        public string p0024 { get; set; }
        public string p0025 { get; set; }
        public string p0026 { get; set; }
        public string p0027 { get; set; }
        public string p0028 { get; set; } //Kod pocztowy
        public string p0029 { get; set; } //Miejscowość
        public string p0030 { get; set; } //Ulica
        public string p0031 { get; set; } //Nr domu
        public string p0032 { get; set; } 
        public string p0033 { get; set; } //Gmina
        public string p0034 { get; set; } //Powiat
        public string p0035 { get; set; } //Województwo
        public string p0036 { get; set; } //Państwo
        public string p0037 { get; set; } //Czy adres zamieszkania jest inny od adresu zameldowania
        public string p0038 { get; set; } //Kod pocztowy
        public string p0039 { get; set; } //Miejscowość
        public string p0040 { get; set; } //Ulica
        public string p0041 { get; set; } //Nr domu
        public string p0042 { get; set; }
        public string p0043 { get; set; }
        public string p0044 { get; set; }
        public string p0045 { get; set; }
        public string p0046 { get; set; } //Państwo
        public string p0047 { get; set; } //Kod pocztowy
        public string p0048 { get; set; } //Miejscowość
        public string p0049 { get; set; } //Ulica
        public string p0050 { get; set; } //Nr domu
        public string p0051 { get; set; }
        public string p0052 { get; set; } //Gmina
        public string p0053 { get; set; } //Rodzaj dokumentu tożsamości
        public string p0054 { get; set; } //Wydany przez
        public string p0055 { get; set; } //Seria i nr
        public string p0056 { get; set; } //Telefon kontaktowy
        public string p0057 { get; set; } //Adres mailowy
        public string p0058 { get; set; } //Imię, nazwisko
        public string p0059 { get; set; } //Adres
        public string p0060 { get; set; } //Telefon
        public string p0061 { get; set; } //Nazwa banku
        public string p0062 { get; set; } //Numer konta bankowego
        public string p0063 { get; set; } //Dane adresowe do przelewu uwzględnić z
        public string p0064 { get; set; } //Państwo
        public string p0065 { get; set; } //Wykształcenie
        public string p0066 { get; set; }
        public string p0067 { get; set; }
        public string p0068 { get; set; } //Obwód
        public string p0069 { get; set; } //Rejon
        public string p0070 { get; set; } //Powiat
        public string p0071 { get; set; } //Województwo
    }
}

