using Soneta.Business;
using Soneta.Business.App;
using Soneta.Kadry;
using Soneta.Kalend;
using Soneta.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PartnerEnovaNormaPraca
{
    public partial class frmEksportNieobecLimitow : Form
    {
        Login login;
        Log log;
        Context context;
        ArrayList arr = new ArrayList();//Tablica z zaznaczonymi pracownikami
        DateTime dataOd = DateTime.Today;
        DateTime dataDo = DateTime.Today;

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

        public frmEksportNieobecLimitow()
        {
            InitializeComponent();
        }

        private void frmEksportNieobecLimitow_Load(object sender, EventArgs e)
        {
            dataOd = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dataDo = dataOd.AddMonths(1).AddDays(-1);
            dtpDataOd.Value = dataOd;
            dtpDataDo.Value = dataDo;
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            log = new Log("Eksport nieobecności i limitów", true);
            //log.WriteLine("Eksport nieobecności i limitów");

            List<NieobecnoscE> nieobecnosci = new List<NieobecnoscE>();
            List<LimitE> limity = new List<LimitE>();

            DateTime okresOd = dtpDataOd.Value;
            DateTime okresDo = dtpDataDo.Value;

            string zawartoscPliku = "";// Zawartość pliku eksportu

            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.DefaultExt = "xml";
            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.FileName = "Eksport_" + okresOd.ToShortDateString() + ".." + okresDo.ToShortDateString() + ".xml";
            System.Windows.Forms.DialogResult dialog = saveFileDialog1.ShowDialog();

            if (dialog == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (Session session = login.CreateSession(false, false))
                    {
                        KadryModule kadry = KadryModule.GetInstance(session);
                        KalendModule kalend = KalendModule.GetInstance(session);
                        using (ITransaction trans = session.Logout(true))
                        {
                            foreach (Object obj in arr)
                            {
                                Pracownik prac = (Pracownik)obj;

                                KalkulatorPracy kalkN = new KalkulatorPracy(prac, null);
                                foreach (OkresNieobecności n in kalkN.Nieobecnosci(new FromTo(okresOd, okresDo), true))
                                {
                                    if (n.Definicja.Nazwa == "Zwolnienie chorobowe" && chkZwolLekarskie.Checked)
                                    {
                                        NieobecnoscE nie = new NieobecnoscE();

                                        nie.Pracownik = prac.Kod;
                                        nie.Okres = n.Okres.ToString();
                                        nie.Definicja = n.Definicja.Nazwa;
                                        nie.KodChoroby = n.Zwolnienie.KodChoroby;
                                        nie.PrzyczynaZwolnienia = n.Zwolnienie.Przyczyna.ToString();
                                        nie.Kwarantanna = n.Zwolnienie.Kwarantanna.ToString();

                                        nieobecnosci.Add(nie);
                                        //log.WriteLine(nie.Pracownik + " " + nie.Okres + " " + nie.Definicja + " " + nie.KodChoroby + " " + nie.PrzyczynaZwolnienia);
                                    }
                                    if (n.Definicja.Nazwa == "Zwolnienie opieka (ZUS)" && chkZwolOpiekaZus.Checked)
                                    {
                                        NieobecnoscE nie = new NieobecnoscE();

                                        nie.Pracownik = prac.Kod;
                                        nie.Okres = n.Okres.ToString();
                                        nie.Definicja = n.Definicja.Nazwa;
                                        nie.PrzyczynaZwolnienia = n.Zwolnienie.Przyczyna.ToString();

                                        nieobecnosci.Add(nie);
                                        //log.WriteLine(nie.Pracownik + " " + nie.Okres + " " + nie.Definicja + " " + nie.KodChoroby + " " + nie.PrzyczynaZwolnienia);
                                    }
                                }

                                // Eksport limitów urlopów pracowników tymczasowych
                                if (chkLinitUrlPracTymcz.Checked)
                                {
                                    List<LimitNieobecnosci> limityPrac = new List<LimitNieobecnosci>();
                                    try
                                    {
                                        limityPrac = prac.Limity.Where(x => x.Definicja.Nazwa == "Urlop wypoczynkowy prac.tymcz." && 
                                                                            x.Okres.To.Year == okresDo.Year).ToList();
                                        limityPrac = limityPrac.OrderBy(x => x.Okres.From).ToList();

                                        if (limityPrac.Count != 0)
                                        {
                                            DateTime okresLimOd = limityPrac[0].Okres.From;
                                            DateTime okresLimDo = limityPrac[limityPrac.Count - 1].Okres.To;
                                            double dni = 0;
                                            double przeniesienie = 0;
                                            double pozostalo = 0;

                                            try
                                            {
                                                przeniesienie = limityPrac[0].Przeniesienie;
                                            }
                                            catch { }

                                            foreach (LimitNieobecnosci l in limityPrac)
                                            {
                                                dni += l.Limit;
                                                //log.WriteLine(prac.Kod + " " + l.Definicja.Nazwa + " " + l.Okres.ToString() + " " + l.PozostaloDni);
                                            }
                                            try
                                            {
                                                pozostalo = limityPrac[limityPrac.Count - 1].PozostaloDni;
                                            }
                                            catch { }

                                            LimitE limit = new LimitE();
                                            limit.Pracownik = prac.Kod;
                                            limit.Okres = new FromTo(okresLimOd, okresLimDo).ToString();
                                            limit.Definicja = "Urlop wypoczynkowy prac.tymcz.";
                                            limit.Limit = dni.ToString();
                                            limit.ZPrzeniesienia = przeniesienie.ToString();
                                            limit.Pozostalo = pozostalo.ToString();

                                            limity.Add(limit);
                                            //log.WriteLine("Limit Okres: " + new FromTo(okresLimOd, okresLimDo) + " " + dni.ToString() + 
                                            //            " z przeniesienia: " + przeniesienie.ToString() + " pozostało: " + pozostalo.ToString());
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { log.WriteLine(ex.Message); }
            }

            if (chkZwolLekarskie.Checked)
            {
                log.WriteLine("");
                log.WriteLine("-- Eksport nieobecności --");

                zawartoscPliku = "<?xml version=\"1.0\" encoding=\"Unicode\" ?>" + Environment.NewLine;
                zawartoscPliku += "<Root xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" + Environment.NewLine;
                zawartoscPliku += "    <CzasPracy>" + Environment.NewLine;

                foreach (NieobecnoscE nie in nieobecnosci)
                {
                    zawartoscPliku += "        <Nieobecnosc>" + Environment.NewLine;
                    zawartoscPliku += "            <Pracownik>" + nie.Pracownik + "</Pracownik>" + Environment.NewLine;
                    zawartoscPliku += "            <Okres>" + nie.Okres + "</Okres>" + Environment.NewLine;
                    zawartoscPliku += "            <Definicja>" + nie.Definicja + "</Definicja>" + Environment.NewLine;
                    if (nie.Definicja == "Zwolnienie chorobowe")
                        zawartoscPliku += "            <KodChoroby>" + nie.KodChoroby + "</KodChoroby>" + Environment.NewLine;
                    zawartoscPliku += "            <PrzyczynaZwolnienia>" + nie.PrzyczynaZwolnienia + "</PrzyczynaZwolnienia>" + Environment.NewLine;
                    if (nie.Definicja == "Zwolnienie chorobowe")
                        zawartoscPliku += "            <Kwarantanna>" + nie.Kwarantanna + "</Kwarantanna>" + Environment.NewLine;
                    zawartoscPliku += "        </Nieobecnosc>" + Environment.NewLine;

                    log.WriteLine(nie.Pracownik + " Nieobecność " + nie.Okres + " " + nie.Definicja + " " + nie.PrzyczynaZwolnienia);
                }

                zawartoscPliku += "    </CzasPracy>" + Environment.NewLine;
                zawartoscPliku += "</Root>" + Environment.NewLine;

                System.IO.File.WriteAllText(saveFileDialog1.FileName.Replace("Eksport_", "Eksport_nieob_"), zawartoscPliku, Encoding.Unicode);
            }

            if (chkLinitUrlPracTymcz.Checked)
            {
                log.WriteLine("");
                log.WriteLine("-- Eksport limitów nieobecności --");

                zawartoscPliku = "<?xml version=\"1.0\" encoding=\"Unicode\" ?>" + Environment.NewLine;
                zawartoscPliku += "<Root xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" + Environment.NewLine;
                zawartoscPliku += "    <Limity>" + Environment.NewLine;

                foreach(LimitE l in limity)
                {
                    zawartoscPliku += "        <Limit>" + Environment.NewLine;
                    zawartoscPliku += "            <Pracownik>" + l.Pracownik + "</Pracownik>" + Environment.NewLine;
                    zawartoscPliku += "            <Okres>" + l.Okres + "</Okres>" + Environment.NewLine;
                    zawartoscPliku += "            <Definicja>" + l.Definicja + "</Definicja>" + Environment.NewLine;
                    zawartoscPliku += "            <Limit>" + l.Limit + "</Limit>" + Environment.NewLine;
                    zawartoscPliku += "            <ZPrzeniesienia>" + l.ZPrzeniesienia + "</ZPrzeniesienia>" + Environment.NewLine;
                    zawartoscPliku += "            <Pozostalo>" + l.Pozostalo + "</Pozostalo>" + Environment.NewLine;
                    zawartoscPliku += "        </Limit>" + Environment.NewLine;

                    log.WriteLine(l.Pracownik + " limit: " + l.Definicja + " " + l.Okres + " dni: " + l.Limit + " pozostało: " + l.Pozostalo);
                }

                zawartoscPliku += "    </Limity>" + Environment.NewLine;
                zawartoscPliku += "</Root>" + Environment.NewLine;

                System.IO.File.WriteAllText(saveFileDialog1.FileName.Replace("Eksport_", "Eksport_limity_"), zawartoscPliku, Encoding.Unicode);
            }
            this.Close();
        }

        public class NieobecnoscE
        {
            public string Pracownik { get; set; }
            public string Okres { get; set; }
            public string Definicja { get; set; }
            public string KodChoroby { get; set; }
            public string PrzyczynaZwolnienia { get; set; }
            public string Kwarantanna { get; set; }
        }

        public class LimitE
        {
            public string Pracownik { get; set; }
            public string Okres { get; set; }
            public string Definicja { get; set; }
            public string Limit { get; set; }
            public string ZPrzeniesienia { get; set; }
            public string Pozostalo { get; set; }
        }
    }
}
