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
using Soneta.Business;
using Soneta.Business.UI;
using Soneta.Business.App;
using Soneta.Deklaracje;
using Soneta.Deklaracje.PIT;
using Soneta.Kadry;
using Soneta.CRM;
using Soneta.Printer;
using Soneta.Tools;

namespace PartnerEnovaNormaPraca
{
    public partial class frmPit11ToPdf : Form
    {
        Login login;
        Context context;
        string folder = "";// Folder zapisu wydruków
        List<PIT> listaDeklaracji = new List<PIT>();// Lista deklaracji do wydruku
                                                    //string templateFileName = @"deklaracje\pit_11_23.aspx";// ścieżka do wzorca wydruku deklaracji
        bool podpisz = false;
        bool sygnatura = false;

        //string kod = "";
        public static string path = "";

        internal void InitContext(Context cx)
        {
            //obiekt logowania przejęty z kontekstu
            login = (Login)cx[typeof(Login)];
            context = cx;
        }

        //Lista deklaracji do wydruku
        public List<PIT> ListaDeklaracji
        {
            get { return listaDeklaracji; }
            set { listaDeklaracji = value; }
        }

        public string Folder
        {
            get { return folder; }
            set { folder = value; }
        }

        public bool Podpisz
        {
            get { return podpisz; }
            set { podpisz = value; }
        }

        public bool Sygnatura
        {
            get { return sygnatura; }
            set { sygnatura = value; }
        }

        public frmPit11ToPdf()
        {
            InitializeComponent();
        }

        private void frmPit11ToPdf_Load(object sender, EventArgs e)
        {
            // Wczytanie ustawień
            LoadSettings();

            foreach (PIT pit in listaDeklaracji)
            {
                //Obsługa deklaracji PIT11 w wersji 26
                if (pit as PIT11_26 == null)
                    throw new InvalidOperationException("Wydruk PIT-11 (26) może być drukowany wyłącznie dla deklaracji PIT-11 w wersji 26.");
                PIT11_26 pit11 = (PIT11_26)pit;
                Pracownik prac = (Pracownik)pit.Podmiot;

                string kod = prac.Kod;
                path = folder + "\\" + kod + ".pdf";

                var generator = new AspReportGenerator
                {
                    Name = "Test",
                    TemplateFileName = @"Deklaracje\pit_11_26.aspx",
                    AspDestination = AspDestinations.PDF,
                    //Destination = AspReportGenerator.Destinations.PDF,
                    OutputFileName = @"D:\temp\pit.pdf"//path
                };
                MessageBox.Show(path);
            }
        }

        private void LoadSettings()
        {
            // Odczyt ustawień
            try
            {
                folder = Properties.Settings.Default["Pit11ToPdfFolder"].ToString();
            }
            catch { }

            txtFolder.Text = folder;
        }

        private void frmPit11ToPdf_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Zapis ustawień
            Properties.Settings.Default["Pit11ToPdfFolder"] = folder;
            Properties.Settings.Default.Save();
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            // Wybór folderu docelowego
            FolderBrowserDialog choseFolder = new FolderBrowserDialog();
            if (folder != "")
                choseFolder.SelectedPath = folder;
            if (choseFolder.ShowDialog() == DialogResult.OK)
            {
                folder = choseFolder.SelectedPath;
                txtFolder.Text = folder;
            }
        }

        private void btnWydruk_Click(object sender, EventArgs e)
        {

        }

        //private static object ZapiszPlik(Stream stream)
        //{
        //    //var name = @"D:\MG\SprzedazREPX.pdf";
        //    //var name2 = @"D:\MG\SprzedazASPX.pdf";
        //    //string path;

        //    //path = flag == true ? name : name2;

        //    //var path = Path.Combine(temp, name);

        //    using (var file = File.Create(path))
        //    {
        //        CoreTools.StreamCopy(stream, file);
        //        file.Flush();
        //    }
        //    System.Diagnostics.Process.Start(path);
        //    return null;
        //}

    }
}
