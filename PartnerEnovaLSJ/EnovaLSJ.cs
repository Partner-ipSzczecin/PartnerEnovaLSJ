using System;
using System.Collections;
using System.Windows.Forms;
using Soneta.Business;
using Soneta.Business.UI;
using Soneta.Business.App;
using Soneta.Kasa;
using Soneta.Kadry;
using Soneta.Deklaracje.PIT;
using Soneta.Place;
using Soneta.Tools;

// Czynności na liście pracowników
[assembly: Worker(
    typeof(PartnerEnovaNormaPraca.EnovaLSJ),
    typeof(Soneta.Kadry.Pracownicy))]

// Wydruk deklaracji PIT11 do PDF
[assembly: Worker(
    typeof(PartnerEnovaNormaPraca.PIT11),
    typeof(Soneta.Deklaracje.PIT.PIT11))]

// Wydruk deklaracji PIT11 do PDF
[assembly: Worker(
    typeof(PartnerEnovaNormaPraca.IFT1),
    typeof(Soneta.Deklaracje.PIT.IFT1))]

// Serwis zmieniający opis na przelewie do banku
[assembly: Service(typeof(Platnosc.IOpisPłatności),
    typeof(PartnerEnovaNormaPraca.OpisPłatnościService))]


namespace PartnerEnovaNormaPraca
{
    /// <summary>
    /// Czynności dostępne na liście pracowników
    /// </summary>

    public class EnovaLSJ
    {
        #region Norma i praca dla analiz
        [Action(
            "Norma i praca dla analiz",
            Description = "Norma i praca dla analiz",
            Priority = 1001,
            Mode = ActionMode.SingleSession,
            Target = ActionTarget.ToolbarWithText
            )]

        public void PrzeliczCzasy(Context cx)
        {
            ArrayList arr = new ArrayList();
            int i = 0;
            Soneta.Business.INavigatorContext cx1 = cx[typeof(Soneta.Business.INavigatorContext)] as Soneta.Business.INavigatorContext;
            foreach (object obj in cx1.SelectedRows)
            {

                Pracownik prac = (Pracownik)obj;
                //arr.Add(obj == null ? "null" : obj);
                arr.Add(prac);
                i = i + 1;
            }
            frmPrzeliczenieHistorii control = new frmPrzeliczenieHistorii();
            control.InitContext(cx);
            control.Arr = arr;
            control.Show();
        }
        #endregion

        #region Kody pracowników
        [Action(
        "Kody pracowników",
        Description = "Kody pracowników",
        Priority = 1001,
        Mode = ActionMode.SingleSession,
        Target = ActionTarget.ToolbarWithText
        )]

        public void PracownicyKody(Context cx)
        {

            ArrayList arr = new ArrayList();
            int i = 0;
            Soneta.Business.INavigatorContext cx1 = cx[typeof(Soneta.Business.INavigatorContext)] as Soneta.Business.INavigatorContext;
            foreach (object obj in cx1.SelectedRows)
            {

                Pracownik prac = (Pracownik)obj;
                //arr.Add(obj == null ? "null" : obj);
                arr.Add(prac);
                i = i + 1;
            }
            frmPracKody control = new frmPracKody();
            control.InitContext(cx);
            control.Arr = arr;
            control.Show();

        }
        #endregion

        #region Konwersja czasu pracy
        [Action(
        "Konwersja czasu pracy od kontrahentów",
        Description = "Konwersja plików z czasem pracy od kontrahentów",
        Priority = 1001,
        Mode = ActionMode.SingleSession,
        Target = ActionTarget.Menu
        )]

        public void KonwersjaCP(Context cx)
        {

            ArrayList arr = new ArrayList();
            int i = 0;
            Soneta.Business.INavigatorContext cx1 = cx[typeof(Soneta.Business.INavigatorContext)] as Soneta.Business.INavigatorContext;
            foreach (object obj in cx1.SelectedRows)
            {

                Pracownik prac = (Pracownik)obj;
                //arr.Add(obj == null ? "null" : obj);
                arr.Add(prac);
                i = i + 1;
            }
            frmKonwersja control = new frmKonwersja();
            //control.InitContext(cx);
            //control.Arr = arr;
            control.Show();

        }
        #endregion

        #region Import czasu pracy
        [Action(
        "Import czasu pracy od kontrahentów",
        Description = "Import plików z czasem pracy od kontrahentów",
        Priority = 1002,
        Mode = ActionMode.SingleSession,
        Target = ActionTarget.Menu
        )]

        public void ImportCP(Context cx)
        {

            ArrayList arr = new ArrayList();
            int i = 0;
            Soneta.Business.INavigatorContext cx1 = cx[typeof(Soneta.Business.INavigatorContext)] as Soneta.Business.INavigatorContext;
            foreach (object obj in cx1.SelectedRows)
            {

                Pracownik prac = (Pracownik)obj;
                //arr.Add(obj == null ? "null" : obj);
                arr.Add(prac);
                i = i + 1;
            }
            frmImportCzasu control = new frmImportCzasu();
            control.InitContext(cx);
            //control.Arr = arr;
            control.Show();

        }
        #endregion

        #region Eksport nieobecności i limitów 
        [Action(
        "Eksport nieobecności i limitów",
        Description = "Eksport nieobecności i limitów",
        Priority = 1003,
        Mode = ActionMode.SingleSession,
        Target = ActionTarget.Menu
        )]

        public void EksportZw(Context cx)
        {

            ArrayList arr = new ArrayList();
            int i = 0;
            Soneta.Business.INavigatorContext cx1 = cx[typeof(Soneta.Business.INavigatorContext)] as Soneta.Business.INavigatorContext;
            foreach (object obj in cx1.SelectedRows)
            {

                Pracownik prac = (Pracownik)obj;
                //arr.Add(obj == null ? "null" : obj);
                arr.Add(prac);
                i = i + 1;
            }
            frmEksportNieobecLimitow control = new frmEksportNieobecLimitow();
            control.InitContext(cx);
            control.Arr = arr;
            control.Show();

        }
        #endregion

        #region Import z rejestracji
        [Action(
        "Import z rejestracji",
        Description = "Import danych przecowników z systemu rejestracji",
        Priority = 1001,
        Mode = ActionMode.SingleSession,
        Target = ActionTarget.Menu
        )]
        public void ImportRejestracji(Context cx)
        {
            Login login = cx.Login;
            bool upr = false;
            try
            {
                upr = (bool)login.Operator.Features["Import z systemu rejestracji"];
            }catch(Exception ex) { }
            if (upr)
            {
                frmImportZRejestracji control = new frmImportZRejestracji();
                control.InitContext(cx);
                control.Show();
            }
            else
            {
                MessageBox.Show("Brak praw do importu");
            }
        }
        #endregion

    }

    /// <summary>
    /// Wydruk PIT11 do plików PDF
    /// </summary>

    class PIT11
    {
        [Context]
        public Context cx { get; set; }

        [Context]
        public PIT deklaracja { get; set; } // Pojedyncza deklaracja
        //public PIT[] Deklaracje { get; set; } // Tablica deklaracji

        //bool wykonane = false; // Zmienna dla opcji z tablicą deklaracji
        public static string path = "";
        string folder = "";
        bool wydruk = false;
        public static DialogResult result;

        //[Action("PITRepx", Priority = 0, Mode = ActionMode.OnlyForm, Target = ActionTarget.ToolbarWithText)]
        [Action(
        "Wydruk PIT11 do PDF",
        Description = "Wydruk PIT11 do PDF",
        Priority = 1001,
        Mode = ActionMode.SingleSession | ActionMode.OnlyTable,
        Target = ActionTarget.Menu
        )]

        public object Drukuj(Context cx)
        {
            if (!wydruk)
            {
                frmPit11ToPdf control = new frmPit11ToPdf();
                result = control.ShowDialog();

                if (result == DialogResult.OK)
                {
                    folder = control.Folder;
                }
                wydruk = true;
            }

            if (result == DialogResult.OK)
            {
                //cx.Set(new XtraReportSerialization.Pit1125.Params(cx)
                cx.Set(new XtraReportSerialization.Pit1126.Params(cx)
                {
                    PITR = false,
                    Upo = false
                });

                #region Opcja z pojedynczymi deklaracjami

                cx.Set(deklaracja);

                #region Obsługa repx

                var reportResult = new ReportResult()
                {
                    Context = cx,
                    DataType = deklaracja.GetType(),
                    TemplateType = typeof(XtraReportSerialization.Pit1126),
                    Format = ReportResultFormat.PDF
                };
                Pracownik prac = (Pracownik)deklaracja.Podmiot;
                string kod = prac.Kod;

                BusApplication.Instance.GetService(out IReportService service);
                var name = folder + "\\" + kod + ".pdf";

                using (var stream = service.GenerateReport(reportResult))
                {
                    var file = System.IO.File.Create(name);
                    CoreTools.StreamCopy(stream, file);
                    file.Flush();
                }
                //System.Diagnostics.Process.Start(name);

                #endregion

                #region Obsługa aspx

                //Pracownik prac = (Pracownik)deklaracja.Podmiot;
                //string kod = prac.Kod;

                //var name = @"D:\Temp\" + kod + ".pdf";
                //var generator = new AspGenerator
                //{
                //    TemplateFileName = @"Deklaracje\pit_11_24.aspx",
                //    Destination = AspGenerator.Destinations.PDF,
                //    OutputFileName = name,
                //    Sign = true,
                //    VisibleSignature = true
                //};

                //generator.Print(null, cx, false, null);

                #endregion

                #endregion

                #region Opcja z tablicą deklaracji

                //if (!wykonane)
                //{
                //    wykonane = true;
                //    foreach (var deklaracja in Deklaracje)
                //    {
                //        cx.Set(deklaracja);

                //        var reportResult = new ReportResult()
                //        {
                //            Context = cx,
                //            DataType = deklaracja.GetType(),
                //            TemplateType = typeof(XtraReportSerialization.Pit1124),
                //            Format = ReportResultFormat.PDF,
                //            Sign = true,
                //            VisibleSignature = true
                //        };
                //        Pracownik prac = (Pracownik)deklaracja.Podmiot;
                //        string kod = prac.Kod;

                //        BusApplication.Instance.GetService(out IReportService service);
                //        var name = @"D:\Temp\" + kod + ".pdf";

                //        using (var stream = service.GenerateReport(reportResult))
                //        {
                //            var file = System.IO.File.Create(name);
                //            CoreTools.StreamCopy(stream, file);
                //            file.Flush();
                //        }
                //        //System.Diagnostics.Process.Start(name);
                //    }
                //}

                #endregion

            }
            return null;
        }
    }

    /// <summary>
    /// Wydruk IFT-1 do plików PDF
    /// </summary>

    class IFT1
    {
        [Context]
        public Context cx { get; set; }

        [Context]
        public PIT deklaracja { get; set; } // Pojedyncza deklaracja

        public static string path = "";
        string folder = "";
        bool wydruk = false;
        public static DialogResult result;

        [Action(
        "Wydruk IFT-1R do PDF",
        Description = "Wydruk IFT-1R do PDF",
        Priority = 1001,
        Mode = ActionMode.SingleSession | ActionMode.OnlyTable,
        Target = ActionTarget.Menu
        )]

        public object Drukuj(Context cx)
        {
            if (!wydruk)
            {
                frmPit11ToPdf control = new frmPit11ToPdf();
                result = control.ShowDialog();

                if (result == DialogResult.OK)
                {
                    folder = control.Folder;
                }
                wydruk = true;
            }

            if (result == DialogResult.OK)
            {
                cx.Set(deklaracja);

                var reportResult = new ReportResult()
                {
                    Context = cx,
                    DataType = deklaracja.GetType(),
                    TemplateType = typeof(XtraReportSerialization.DeklaracjaIFT_1_15),
                    Format = ReportResultFormat.PDF
                };
                Pracownik prac = (Pracownik)deklaracja.Podmiot;
                string kod = prac.Kod;

                BusApplication.Instance.GetService(out IReportService service);
                var name = folder + "\\" + kod + ".pdf";

                using (var stream = service.GenerateReport(reportResult))
                {
                    var file = System.IO.File.Create(name);
                    CoreTools.StreamCopy(stream, file);
                    file.Flush();
                }

            }

            return null;
        }
    }

    public class OpisPłatnościService : Platnosc.IOpisPłatności
    {
        public string OpisPłatności(Platnosc płatność, string opis)
        {
            Soneta.Place.Wyplata w = płatność.Dokument as Soneta.Place.Wyplata;
            //opis = string.Format("Wynagrodzenie");
            string nrListy = "";
            try
            {
                nrListy = " " + w.ListaPlac.Numer.NumerPelny.Replace("/?", "");
            }
            catch (Exception ex) { }

            //if (w != null)
            //    opis = nrListy;

            try
            {
                opis = string.Format("wynagr. {0}/{1} {2}", w.ListaPlac.Okres.To.Year, w.ListaPlac.Okres.To.Month, nrListy);
            }
            catch (Exception ex) { }

            try
            {
                if (w.Elementy.Count == 1)
                {
                    WypElement e = (WypElement)w.Elementy.GetNext();
                    if (e is WypElementDodatek)
                    {
                        opis += " " + e.Nazwa;
                    }

                    //if (e is WypElementUmowa)
                    //{
                    //    opis += e.
                    //}
                }
            }
            catch (Exception ex) { }

            //if (w != null)
            //try
            //{
            //    WypElement e = (WypElement)w.Elementy.GetNext();
            //    if (w.Elementy.Count == 1)
            //    {
            //        if (e is WypElementDodatek)
            //        {
            //            string nazwa = e.Nazwa;

            //            if (e.Okres == FromTo.Year(e.Okres.To.Year))
            //                opis += string.Format(" {0}", nazwa);
            //            else if (e.Okres == FromTo.Month(e.Okres.To.Year, e.Okres.To.Month))
            //                opis += string.Format(" {0}", nazwa);
            //            else
            //                opis += string.Format(" {0}", nazwa);
            //        }
            //    }
            //}
            //catch (Exception ex) { }

            //try
            //{
            //    WypElement e = (WypElement)w.Elementy.GetNext();
            //    if (w.Elementy.Count == 1)
            //    {
            //        if (e is WypElementDodatek)
            //        {
            //            string nazwa = e.Definicja.RodzajZrodla.ToString();


            //            //if (e.Okres == FromTo.Year(e.Okres.To.Year))
            //            //    opis += string.Format(" {0}", nazwa);
            //            //else if (e.Okres == FromTo.Month(e.Okres.To.Year, e.Okres.To.Month))
            //            //    opis += string.Format(" {0}", nazwa);
            //            //else
            //            //    opis += string.Format(" {0}", nazwa);
            //        }
            //    }
            //}
            //catch (Exception ex) { }


            return opis;
        }
    }
}
