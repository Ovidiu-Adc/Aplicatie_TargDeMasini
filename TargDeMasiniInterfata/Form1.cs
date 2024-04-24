using Clase;
using StocareDate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TargDeMasiniInterfata
{
    public partial class Form1 : Form
    {
        private Label lblVanzator;
        private Label lblCumparator;
        private Label lblTipMasina;
        private Label lblModelMasina;
        private Label lblAnFabrica;
        private Label lblCuloare;
        private Label lblOptiuni;
        private Label lblData;
        private Label lblPret;

        //private ManagerTranzactii managerTranzactii;
        private List<TranzactieAuto> tranzactii;

        private const int LATIME_CONTROL = 100;
        private const int DIMENSIUNE_PAS_Y = 80;
        private const int DIMENSIUNE_PAS_X = 100;
        private int indexRanduri = 0;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();

            this.Size = new Size(1200, 500);

            lblVanzator = new Label();
            lblVanzator.Width = LATIME_CONTROL;
            lblVanzator.Text = "Vanzator";
            lblVanzator.Left = DIMENSIUNE_PAS_X;
            lblVanzator.ForeColor = Color.DarkBlue;
            lblVanzator.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.Controls.Add(lblVanzator);

            lblCumparator = new Label();
            lblCumparator.Width = LATIME_CONTROL;
            lblCumparator.Text = "Cumparator";
            lblCumparator.Left = 2 * DIMENSIUNE_PAS_X;
            lblCumparator.ForeColor = Color.DarkBlue;
            lblCumparator.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.Controls.Add(lblCumparator);

            lblTipMasina = new Label();
            lblTipMasina.Width = LATIME_CONTROL;
            lblTipMasina.Text = "Tip Masina";
            lblTipMasina.Left = 3 * DIMENSIUNE_PAS_X;
            lblTipMasina.ForeColor = Color.DarkBlue;
            lblTipMasina.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.Controls.Add(lblTipMasina);

            lblModelMasina = new Label();
            lblModelMasina.Width = LATIME_CONTROL;
            lblModelMasina.Text = "Model Masina";
            lblModelMasina.Left = 4 * DIMENSIUNE_PAS_X;
            lblModelMasina.ForeColor = Color.DarkBlue;
            lblModelMasina.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.Controls.Add(lblModelMasina);

            lblAnFabrica = new Label();
            lblAnFabrica.Width = LATIME_CONTROL;
            lblAnFabrica.Text = "Anul fabricarii";
            lblAnFabrica.Left = 5 * DIMENSIUNE_PAS_X;
            lblAnFabrica.ForeColor = Color.DarkBlue;
            lblAnFabrica.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.Controls.Add(lblAnFabrica);

            lblCuloare = new Label();
            lblCuloare.Width = LATIME_CONTROL;
            lblCuloare.Text = "Culoare";
            lblCuloare.Left = 6 * DIMENSIUNE_PAS_X;
            lblCuloare.ForeColor = Color.DarkBlue;
            lblCuloare.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.Controls.Add(lblCuloare);

            lblData = new Label();
            lblData.Width = LATIME_CONTROL;
            lblData.Text = "Data tranzactiei";
            lblData.Left = 7 * DIMENSIUNE_PAS_X;
            lblData.ForeColor = Color.DarkBlue;
            lblData.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.Controls.Add(lblData);

            lblPret = new Label();
            lblPret.Width = LATIME_CONTROL;
            lblPret.Text = "Pret";
            lblPret.Left = 8 * DIMENSIUNE_PAS_X;
            lblPret.ForeColor = Color.DarkBlue;
            lblPret.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.Controls.Add(lblPret);

            lblOptiuni = new Label();
            lblOptiuni.Width = LATIME_CONTROL * 6;
            lblOptiuni.Text = "Optiuni";
            lblOptiuni.Left = 10 * DIMENSIUNE_PAS_X;
            lblOptiuni.ForeColor = Color.DarkBlue;
            lblOptiuni.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.Controls.Add(lblOptiuni);


        }

        private void InitializeCustomComponents()
        {

        }

        private void LoadTransactions()
        {
            string numeFisier = "tranzactii.txt";
            string locatieFisierSolutie = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleFisier = locatieFisierSolutie + "\\" + numeFisier;



            tranzactii = CitesteTranzactii(caleFisier);

            if (tranzactii != null)
            {
                AfiseazaTranzactii();
            }
            else
            {
                MessageBox.Show("Eroare la citirea tranzacțiilor din fișier.");
            }
        }

        private List<TranzactieAuto> CitesteTranzactii(string caleFisier)
        {
            List<TranzactieAuto> tranzactiiCitite = new List<TranzactieAuto>();

            try
            {
                using (StreamReader sr = new StreamReader(caleFisier))
                {
                    string linie;
                    while ((linie = sr.ReadLine()) != null)
                    {
                        string[] informatii = linie.Split(';');

                        TranzactieAuto tranzactie = new TranzactieAuto
                        {
                            NumeVanzator = informatii[0],
                            NumeCumparator = informatii[1],
                            TipMasina = informatii[2],
                            ModelMasina = informatii[3],
                            AnFabricatie = int.Parse(informatii[4]),
                            Culoare = (Culoare)Enum.Parse(typeof(Culoare), informatii[5]),
                            Optiuni = new List<Optiuni>(Array.ConvertAll(informatii[6].Split(','), s => (Optiuni)Enum.Parse(typeof(Optiuni), s))),
                            DataTranzactie = DateTime.Parse(informatii[7]),
                            Pret = decimal.Parse(informatii[8])
                        };

                        tranzactiiCitite.Add(tranzactie);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la citirea tranzacțiilor din fișier: {ex.Message}");
            }

            return tranzactiiCitite;
        }

        private void AfiseazaTranzactii()
        {
            foreach (TranzactieAuto tranzactie in tranzactii)
            {
                AdaugaTranzactiePeFormular(tranzactie);
            }
        }

        private void AdaugaTranzactiePeFormular(TranzactieAuto tranzactie)
        {
            // Creare label-uri pentru detalii tranzacție
            Label[] labels = new Label[8];
            string[] detaliiTranzactie = new string[]
            {
                tranzactie.NumeVanzator,
                tranzactie.NumeCumparator,
                tranzactie.TipMasina,
                tranzactie.ModelMasina,
                tranzactie.AnFabricatie.ToString(),
                tranzactie.Culoare.ToString(),
                tranzactie.DataTranzactie.ToString("yyyy-MM-dd"),
                tranzactie.Pret.ToString()
            };

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = new Label();
                labels[i].Width = LATIME_CONTROL;
                labels[i].Text = detaliiTranzactie[i];
                labels[i].Left = (i + 1) * DIMENSIUNE_PAS_X;
                labels[i].Top = DIMENSIUNE_PAS_Y * indexRanduri + 40;
                labels[i].ForeColor = Color.Blue;
                this.Controls.Add(labels[i]);
            }

            // Creare label pentru Opțiuni
            Label lblsOptiuni = new Label();
            lblsOptiuni.Width = LATIME_CONTROL * 4; // Lățime mai mare pentru afișare pe mai multe linii
            lblsOptiuni.Text = string.Join(", ", tranzactie.Optiuni.Select(optiune => optiune.ToString()));
            lblsOptiuni.Left = 9 * DIMENSIUNE_PAS_X;
            lblsOptiuni.Top = DIMENSIUNE_PAS_Y * indexRanduri + 40;
            lblsOptiuni.ForeColor = Color.Blue;
            this.Controls.Add(lblsOptiuni);

            indexRanduri++;
        }



        // Implementarea metodei Form1_Load pentru a gestiona evenimentul de încărcare a formularului
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadTransactions();
        }
    }
}

