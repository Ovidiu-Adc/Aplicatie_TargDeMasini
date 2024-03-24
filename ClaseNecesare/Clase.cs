using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaseNecesare
{
    public class TranzactieAuto
    {
        public string NumeVanzator { get; set; }
        public string NumeCumparator { get; set; }
        public string TipMasina { get; set; }
        public int AnFabricatie { get; set; }
        public string Culoare { get; set; }
        public string Optiuni { get; set; }
        public DateTime DataTranzactie { get; set; }
        public decimal Pret { get; set; }
    }

    public class RaportAuto
    {
        public void CeaMaiCautataMasina(string firmaSauModel, DateTime dataDeLa, DateTime dataPanaLa)
        {
            // cod ..
        }

        public void GraficPretPentruModel(string model, DateTime dataDeLa, DateTime dataPanaLa)
        {
            // cod ..
        }

        public void TranzactiiPeZi(DateTime zi)
        {
            // cod ..
        }
    }
}
