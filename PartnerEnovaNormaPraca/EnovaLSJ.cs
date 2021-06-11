using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Soneta.Tools;
using Soneta.Types;
using Soneta.Business;
using Soneta.Business.Licence;
using Soneta.Business.Forms;
using Soneta.Business.App;
using Soneta.Kadry;

[assembly: Worker(
    typeof(PartnerEnovaNormaPraca.EnovaLSJ),
    typeof(Soneta.Kadry.Pracownicy))]

[assembly: Worker(
    typeof(PartnerEnovaNormaPraca.frmPrzeliczenieHistorii),
    typeof(Soneta.Kadry.Pracownicy))]

namespace PartnerEnovaNormaPraca
{
    public class EnovaLSJ
    {
        [Action(
            "Norma i praca dla analizy",
            Description = "Norma i praca dla analizy",
            Priority = 1001,
            Mode = ActionMode.SingleSession,
            Target = ActionTarget.ToolbarWithText
            )]

        public void PrzeliczCzasy(Context cx)
        {
        //    ArrayList arr = new ArrayList();
        //    int i = 0;
        //    Soneta.Business.INavigatorContext cx1 = cx[typeof(Soneta.Business.INavigatorContext)] as Soneta.Business.INavigatorContext;
        //        foreach (object obj in cx1.SelectedRows)
        //        {

        //            Pracownik prac = (Pracownik)obj;
        //        //arr.Add(obj == null ? "null" : obj);
        //        arr.Add(prac);
        //            i = i + 1;
        //        }
        //    frmNormaPraca control = new frmNormaPraca();
        //    control.InitContext(cx);
        //    control.Arr = arr;
        //    control.Show();
        //}

        //[Action(
        //    "Przeliczenie historii",
        //    Description = "Przeliczenie historii",
        //    Priority = 1001,
        //    Mode = ActionMode.SingleSession,
        //    Target = ActionTarget.ToolbarWithText
        //    )]


        //public void PrzeliczNorme(Context cx)
        //{
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
    }
}
