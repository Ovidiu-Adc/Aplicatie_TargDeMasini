using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clase
{
    public class TranzactieAuto
    {
        public string NumeVanzator { get; set; }
        public string NumeCumparator { get; set; }
        public string TipMasina { get; set; }
        public string ModelMasina { get; set; }
        public int AnFabricatie { get; set; }
        public Culoare Culoare { get; set; }
        public List<Optiuni> Optiuni { get; set; }

        public DateTime DataTranzactie { get; set; }
        public decimal Pret { get; set; }
    }

    public enum Culoare
    {
        Rosu,
        Alb,
        Negru,
        Albastru,
        Gri
    }

    [Flags]
    public enum Optiuni
    {
        CutieAutomata = 1,
        CutieManuala=2,
        AerConditionat = 3,      
        NavigatieGPS=4,
        ScauneIncalzite=5,
        SenzoriParcare=6,
        SistemFranareAutomata=7
    }

    public class RaportAuto
    {
        public void CeaMaiCautataMasina(List<TranzactieAuto> tranzactii, string firmasauModel, DateTime dataDeLa, DateTime dataPanaLa)
        {
            int numarAparitii = 0;
            string ceaMaiCautataMasina = "";

            foreach (var tranzactie in tranzactii)
            {
                if ((tranzactie.TipMasina == firmasauModel || tranzactie.ModelMasina == firmasauModel) && tranzactie.DataTranzactie >= dataDeLa && tranzactie.DataTranzactie <= dataPanaLa)
                {
                    int aparitiiCurent = 0;
                    foreach (var tranzactieInner in tranzactii)
                    {
                        if ((tranzactieInner.TipMasina == tranzactie.TipMasina || tranzactieInner.ModelMasina == tranzactie.ModelMasina) && tranzactieInner.DataTranzactie >= dataDeLa && tranzactieInner.DataTranzactie <= dataPanaLa)
                            aparitiiCurent++;
                    }

                    if (aparitiiCurent > numarAparitii)
                    {
                        numarAparitii = aparitiiCurent;
                        ceaMaiCautataMasina = $"{tranzactie.TipMasina} {tranzactie.ModelMasina}";
                    }
                }
            }

            if (ceaMaiCautataMasina != "")
                Console.WriteLine($"Cea mai cautata masina intre {dataDeLa:yyyy-MM-dd} si {dataPanaLa:yyyy-MM-dd}: {ceaMaiCautataMasina}");
            else
                Console.WriteLine($"Nu s-a gasit nicio masina in perioada specificata.");
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
