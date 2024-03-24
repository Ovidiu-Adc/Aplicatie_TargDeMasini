using System;
using System.Collections.Generic;
using ClaseNecesare;

namespace Aplicatie_TargDeMasini
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<TranzactieAuto> tranzactii = new List<TranzactieAuto>();

            bool continua = true;
            while (continua)
            {
                
                Console.WriteLine("1. Adaugare tranzactie");
                Console.WriteLine("2. Afisare tranzactii");
                Console.WriteLine("3. Cea mai cautata masina intr-o anumita perioada");
                Console.WriteLine("4. Iesire");
                Console.Write("Selectati o optiune: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        AdaugareTranzactie(tranzactii);
                        break;
                    case "2":
                        AfisareTranzactii(tranzactii);
                        break;
                    case "3":
                        CeaMaiCautataMasina(tranzactii);
                        break;
                    case "4":
                        continua = false;
                        break;
                    default:
                        Console.WriteLine("Optiune invalida. Selectati din nou.");
                        break;
                }
            }
        }

        static void AdaugareTranzactie(List<TranzactieAuto> tranzactii)
        {
            Console.WriteLine("Introduceti detalii despre tranzactia auto:");
            Console.Write("Nume vanzator: ");
            string numeVanzator = Console.ReadLine();
            Console.Write("Nume cumparator: ");
            string numeCumparator = Console.ReadLine();
            Console.Write("Tip masina: ");
            string tipMasina = Console.ReadLine();
            Console.Write("Model: ");
            string modelMasina = Console.ReadLine();
            Console.Write("An fabricatie: ");
            int anFabricatie = int.Parse(Console.ReadLine());
            Console.Write("Culoare: ");
            string culoare = Console.ReadLine();
            Console.Write("Optiune: ");
            string optiuni = Console.ReadLine();
            Console.Write("Data tranzactie (YYYY-MM-DD): ");
            DateTime dataTranzactie = DateTime.Parse(Console.ReadLine());
            Console.Write("Pret: ");
            decimal pret = decimal.Parse(Console.ReadLine());

            tranzactii.Add(new TranzactieAuto
            {
                NumeVanzator = numeVanzator,
                NumeCumparator = numeCumparator,
                TipMasina = tipMasina,
                ModelMasina = modelMasina,
                AnFabricatie = anFabricatie,
                Culoare = culoare,
                Optiuni = optiuni,
                DataTranzactie = dataTranzactie,
                Pret = pret
            });

            Console.WriteLine("Tranzactia a fost adaugata cu succes.");
        }

        static void AfisareTranzactii(List<TranzactieAuto> tranzactii)
        {
            Console.WriteLine("Tranzactii inregistrate:");
            foreach (var tranzactie in tranzactii)
            {
                Console.WriteLine($"\nVanzator: {tranzactie.NumeVanzator}\nCumparator: {tranzactie.NumeCumparator}\nTip masina: {tranzactie.TipMasina}\nModel masina: {tranzactie.ModelMasina}\nAn fabricatie: {tranzactie.AnFabricatie}\nCuloare: {tranzactie.Culoare}\nOptiuni: {tranzactie.Optiuni}\nData tranzactie: {tranzactie.DataTranzactie.ToString("yyyy-MM-dd")}\nPret: {tranzactie.Pret}$\n");
            }
        }

        static void CeaMaiCautataMasina(List<TranzactieAuto> tranzactii)
        {
            Console.WriteLine("Introduceti firma masinii: ");
            string firmasiModel = Console.ReadLine();

            Console.Write("Introduceti data de la (YYYY-MM-DD): ");
            DateTime dataDeLa = DateTime.Parse(Console.ReadLine());

            Console.Write("Introduceti data pana la (YYYY-MM-DD): ");
            DateTime dataPanaLa = DateTime.Parse(Console.ReadLine());

            RaportAuto raportAuto = new RaportAuto();
            raportAuto.CeaMaiCautataMasina(tranzactii, firmasiModel, dataDeLa, dataPanaLa);
        }

    }
}
