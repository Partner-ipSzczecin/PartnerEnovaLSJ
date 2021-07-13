using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartnerEnovaNormaPraca
{
    public class classes
    {
        public partial class Info
        {
            public static string Version = "v. 21.07.1";

        }

        /// <summary>
        /// Klasa przechowująca pojedynczy dzień planu
        /// </summary>
        public class DzienPlanu
        {
            public string Pracownik { get; set; }
            public string Data { get; set; }
            public string Definicja { get; set; }
            public string OdGodziny { get; set; }
            public string Czas { get; set; }
        }

        /// <summary>
        /// Klasa strefy pracy
        /// </summary>
        public class StrefaPracyW
        {
            public string Definicja { get; set; }
            public string OdGodziny { get; set; }
            public string Czas { get; set; }
            public string work { get; set; }
        }

        /// <summary>
        /// Klasa przechowująca pojedynczy dzień pracy
        /// </summary>
        public class DzienPracyW
        {
            public string Pracownik { get; set; }
            public string Data { get; set; }
            public string OdGodziny { get; set; }
            public string Czas { get; set; }
            public int LicznikStref { get; set; }
            public string Work { get; set; }

            public List<StrefaPracyW> Strefy = new List<StrefaPracyW>();
        }

        /// <summary>
        /// Klasa przechowująca pojedynczy dzień pracy dla importu
        /// </summary>
        public class DzienPracyImport
        {
            public DateTime Data { get; set; }
            public Time OdGodziny { get; set; }
            public Time DoGodziny { get; set; }
            public Time Czas { get; set; }
            public Time Plan { get; set; }

        }

        public class Nieobecnosc
        {
            public string Pracownik { get; set; }
            public string Okres { get; set; }
            public DateTime OkresOd { get; set; }
            public DateTime OkresDo { get; set; }
            public string Definicja { get; set; }
            public string PrzyczynaUrlopu { get; set; }
            public Time OdGodziny { get; set; }
            public Time DoGodziny { get; set; }
            public Time Czas { get; set; }
        }

        public class PracownikZestawienie
        {
            public string NazwiskoImie { get; set; }
            public string Nazwisko { get; set; }
            public string Imię { get; set; }
            public string KodUKontrahenta { get; set; }
            public string KodEnova { get; set; }

            public List<Dzien> ZestawienieDni = new List<Dzien>();
            public List<DzienPlanu> DniPlanu = new List<DzienPlanu>();// Lista dni planu pracownika
            public List<DzienPracyW> CzasPracy = new List<DzienPracyW>();// Lista dni pracy pracownika
            public List<Nieobecnosc> Nieobecności = new List<Nieobecnosc>();// Lista nieobecności pracownika
            public List<DzienPracyImport> CzasPracyImport = new List<DzienPracyImport>();// Lista dni pracy pracownika dla importu
        }

        public class Dzien
        {
            public DateTime Data { get; set; }
            public string DzienTygodnia { get; set; }
            public string DzienRodzaj { get; set; }
            public string DataS { get; set; }
            public string Plan { get; set; }
            public string Praca { get; set; }
            public TimeSpan PracaOd { get; set; }
            public TimeSpan PracaCzas { get; set; }
            public string NieobecnoscKod { get; set; }
            public string NieobecnoscEnova { get; set; }
            public string NieobecnoscEnovaPrzyczyna { get; set; }
            public TimeSpan NieobecnoscEnovaOd { get; set; }
            public TimeSpan NieobecnoscEnovaCzas { get; set; }
        }
    }
}
