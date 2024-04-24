using System;
using System.Collections.Generic;
using System.IO;
using Clase;

namespace StocareDate
{
    public class ManagerTranzactii
    {
        private string caleFisier;

        public ManagerTranzactii(string caleFisier)
        {
            this.caleFisier = caleFisier;
            Stream tranzactieFisierText = File.Open(caleFisier, FileMode.OpenOrCreate);
            tranzactieFisierText.Close();
        }

        public void AdaugaTranzactie(TranzactieAuto tranzactie)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(caleFisier, true))
                {
                    string tranzactieAsString = RaportAuto.TranzactieToString(tranzactie);
                    sw.WriteLine(tranzactieAsString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"A apărut o eroare la scrierea în fișier: {ex.Message}");
            }
        }

        public List<TranzactieAuto> CitesteTranzactii()
        {
            List<TranzactieAuto> tranzactii = new List<TranzactieAuto>();

            try
            {
                using (StreamReader sr = new StreamReader(caleFisier))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        TranzactieAuto tranzactie = ParseazaLinieTranzactie(line);
                        if (tranzactie != null)
                        {
                            tranzactii.Add(tranzactie);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"A apărut o eroare la citirea din fișier: {ex.Message}");
            }

            return tranzactii;
        }

        private TranzactieAuto ParseazaLinieTranzactie(string line)
        {
            try
            {
                string[] tokens = line.Split(';');
                if (tokens.Length >= 9)
                {
                    TranzactieAuto tranzactie = new TranzactieAuto
                    {
                        NumeVanzator = tokens[0],
                        NumeCumparator = tokens[1],
                        TipMasina = tokens[2],
                        ModelMasina = tokens[3],
                        AnFabricatie = int.Parse(tokens[4]),
                        Culoare = (Culoare)Enum.Parse(typeof(Culoare), tokens[5]),
                        Optiuni = ParseazaOptiuni(tokens[6]),
                        DataTranzactie = DateTime.Parse(tokens[7]),
                        Pret = decimal.Parse(tokens[8])
                    };
                    return tranzactie;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la parsarea liniei: {ex.Message}");
            }

            return null;
        }

        private List<Optiuni> ParseazaOptiuni(string optiuniAsString)
        {
            List<Optiuni> optiuni = new List<Optiuni>();
            string[] tokens = optiuniAsString.Split(',');
            foreach (string token in tokens)
            {
                if (Enum.TryParse(token.Trim(), out Optiuni optiune))
                {
                    optiuni.Add(optiune);
                }
            }
            return optiuni;
        }
    }
}
