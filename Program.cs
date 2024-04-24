using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Clase;
using StocareDate;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace Aplicatie_TargDeMasini
{
    class Program
    {
        private static string numeFisier = "tranzactii.txt";
        private static string caleFisier = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, numeFisier);

        private static ManagerTranzactii adminTranzactii = new ManagerTranzactii(caleFisier);

        static void Main()
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
            int anFabricatie;
            while (!int.TryParse(Console.ReadLine(), out anFabricatie))
            {
                Console.WriteLine("Introduceti un an de fabricatie valid.");
            }

            Console.WriteLine("Culoare:");
            foreach (Culoare culoare in Enum.GetValues(typeof(Culoare)))
            {
                Console.WriteLine($"{(int)culoare}. {culoare}");
            }
            Console.Write("Selectati o culoare: ");
            int culoareIndex;
            while (!int.TryParse(Console.ReadLine(), out culoareIndex) || !Enum.IsDefined(typeof(Culoare), culoareIndex))
            {
                Console.WriteLine("Selectati o culoare valida.");
            }
            Culoare culoareSelectata = (Culoare)culoareIndex;

            // Selectare optiuni
            Console.WriteLine("Optiuni:");
            foreach (Optiuni optiune in Enum.GetValues(typeof(Optiuni)))
            {
                Console.WriteLine($"{(int)optiune}. {optiune}");
            }
            Console.Write("Selectati optiunile: ");
            string optiuniInput = Console.ReadLine();
            string[] optiuniArray = optiuniInput.Split(' ');
            List<Optiuni> optiuniSelectate = new List<Optiuni>();
            foreach (string optiuneString in optiuniArray)
            {
                if (Enum.TryParse(optiuneString.Trim(), out Optiuni optiune))
                {
                    optiuniSelectate.Add(optiune);
                }
            }


            Console.Write("Data tranzactie (YYYY-MM-DD): ");
            DateTime dataTranzactie = DateTime.Parse(Console.ReadLine());

            Console.Write("Pret: ");
            decimal pret;
            while (!decimal.TryParse(Console.ReadLine(), out pret))
            {
                Console.WriteLine("Introduceti un pret valid.");
            }

            TranzactieAuto tranzactie = new TranzactieAuto
            {
                NumeVanzator = numeVanzator,
                NumeCumparator = numeCumparator,
                TipMasina = tipMasina,
                ModelMasina = modelMasina,
                AnFabricatie = anFabricatie,
                Culoare = culoareSelectata,
                Optiuni = optiuniSelectate,
                DataTranzactie = dataTranzactie,
                Pret = pret
            };

            // Adăugare tranzacție în lista locală
            tranzactii.Add(tranzactie);

            // Scriere in fisier
            adminTranzactii.AdaugaTranzactie(tranzactie);

            Console.WriteLine("Tranzactia a fost adaugata cu succes.");
        }

        static void AfisareTranzactii(List<TranzactieAuto> tranzactii)
        {
            Console.WriteLine("Tranzactii inregistrate:");
            foreach (var tranzactie in tranzactii)
            {
                string optiuniAsString = RaportAuto.OptiuniToString(tranzactie.Optiuni); 
                Console.WriteLine($"\nVanzator: {tranzactie.NumeVanzator}\nCumparator: {tranzactie.NumeCumparator}\nTip masina: {tranzactie.TipMasina}\nModel masina: {tranzactie.ModelMasina}\nAn fabricatie: {tranzactie.AnFabricatie}\nCuloare: {tranzactie.Culoare}\nOptiuni: {optiuniAsString}\nData tranzactie: {tranzactie.DataTranzactie.ToString("yyyy-MM-dd")}\nPret: {tranzactie.Pret}$\n");
            }
        }

        static void CeaMaiCautataMasina(List<TranzactieAuto> tranzactii)
        {
            Console.WriteLine("Introduceti firma sau modelul masinii: ");
            string firmasauModel = Console.ReadLine();

            Console.Write("Introduceti data de la (YYYY-MM-DD): ");
            DateTime dataDeLa = DateTime.Parse(Console.ReadLine());

            Console.Write("Introduceti data pana la (YYYY-MM-DD): ");
            DateTime dataPanaLa = DateTime.Parse(Console.ReadLine());

            RaportAuto raportAuto = new RaportAuto();
            raportAuto.CeaMaiCautataMasina(tranzactii, firmasauModel, dataDeLa, dataPanaLa);
        }
    }
}
