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
        private ManagerTranzactii managerTranzactii;
        private List<TranzactieAuto> tranzactii;
        private const int LATIME_CONTROL = 100;
        private const int SPATIU_INTRE_COLOANE = 0; 
        private const int INALTIME_RAND = 40;

        private TableLayoutPanel tableLayoutPanel;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
            string caleFisier = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "tranzactii.txt");
            managerTranzactii = new ManagerTranzactii(caleFisier);
        }

        private void InitializeCustomComponents()
        {
            this.Size = new Size(1800, 700);

            // Cream un panel pentru a centra tableLayoutPanel in formular
            Panel panelCentral = new Panel();
            panelCentral.Dock = DockStyle.Fill;
            this.Controls.Add(panelCentral);

            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Anchor = AnchorStyles.Right;
            tableLayoutPanel.Size = new Size(1350, 350); // Ajustează dimensiunea după nevoie
            tableLayoutPanel.Location = new Point(this.ClientSize.Width - tableLayoutPanel.Width, 115); // Ajustează poziția după nevoie
            tableLayoutPanel.AutoScroll = true;
            tableLayoutPanel.ColumnCount = 9;
            tableLayoutPanel.RowCount = 1;

            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            // Set column styles
            for (int i = 0; i < 8; i++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 8));
            }
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            // Add table layout panel to the form
            panelCentral.Controls.Add(tableLayoutPanel);

            // Add titles to the table
            string[] titluri = { "Vanzator", "Cumparator", "Tip Masina", "Model Masina", "Anul fabricarii", "Culoare", "Data tranzactiei", "Pret(€)", "Optiuni" };
            for (int i = 0; i < titluri.Length; i++)
            {
                Label label = new Label();
                label.Text = titluri[i];
                label.ForeColor = Color.Black;
                label.Font = new Font("Segoe UI", 13f, FontStyle.Bold); // Dimensiunea textului micșorată
                //label.TextAlign = ContentAlignment.MiddleLeft;
                label.Margin = new Padding(SPATIU_INTRE_COLOANE); // Eliminăm complet distanța între coloane
                label.Size = new Size(120, INALTIME_RAND);
                tableLayoutPanel.Controls.Add(label, i, 0);
            }
        }
        
        private void btnAdauga_Click(object sender, EventArgs e)
        {
            bool valid = true;

            // Reset label colors
            lblVanzator.ForeColor = Color.White;
            lblCump.ForeColor = Color.White;
            lblTip.ForeColor = Color.White;
            lblModel.ForeColor = Color.White;
            lblFabricare.ForeColor = Color.White;
            lblCuloare.ForeColor = Color.White;
            lblPret.ForeColor = Color.White;
            lblOptiuni.ForeColor = Color.White;

            // Validate fields
            if (string.IsNullOrWhiteSpace(txtVanzator.Text))
            {
                lblVanzator.ForeColor = Color.Red;
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(txtCump.Text))
            {
                lblCump.ForeColor = Color.Red;
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(txtTip.Text))
            {
                lblTip.ForeColor = Color.Red;
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(txtModel.Text))
            {
                lblModel.ForeColor = Color.Red;
                valid = false;
            }
            if (!int.TryParse(txtFabricare.Text, out int anFabricatie))
            {
                lblFabricare.ForeColor = Color.Red;
                valid = false;
            }
            if (!rdbRosu.Checked && !rdbAlb.Checked && !rdbNegru.Checked && !rdbAlbastru.Checked && !rdbGri.Checked && !rdbVerde.Checked)
            {
                lblCuloare.ForeColor = Color.Red;
                valid = false;
            }
            if (!ckbCtAuto.Checked && !ckbCtManual.Checked && !ckbAC.Checked && !ckbGPS.Checked && !ckbIncalzire.Checked && !ckbSenzori.Checked)
            {
                lblOptiuni.ForeColor = Color.Red;
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(txtPret.Text))
            {
                lblPret.ForeColor = Color.Red;
                valid = false;
            }
            if (ckbCtAuto.Checked && ckbCtManual.Checked)
            {
                lblOptiuni.ForeColor = Color.Red;
                MessageBox.Show("Se poate selecta numai o cutie de viteze (automată sau manuală).");
                valid = false;
            }

            if (valid)
            {
                // Get selected color
                Culoare culoareSelectata = Culoare.Rosu;
                if (rdbAlb.Checked) culoareSelectata = Culoare.Alb;
                else if (rdbNegru.Checked) culoareSelectata = Culoare.Negru;
                else if (rdbAlbastru.Checked) culoareSelectata = Culoare.Albastru;
                else if (rdbGri.Checked) culoareSelectata = Culoare.Gri;
                else if (rdbVerde.Checked) culoareSelectata = Culoare.Verde;

                // Get selected options
                List<Optiuni> optiuniSelectate = new List<Optiuni>();
                if (ckbCtAuto.Checked) optiuniSelectate.Add(Optiuni.CutieAutomata);
                if (ckbCtManual.Checked) optiuniSelectate.Add(Optiuni.CutieManuala);
                if (ckbAC.Checked) optiuniSelectate.Add(Optiuni.AerConditionat);
                if (ckbGPS.Checked) optiuniSelectate.Add(Optiuni.NavigatieGPS);
                if (ckbIncalzire.Checked) optiuniSelectate.Add(Optiuni.ScauneIncalzite);
                if (ckbSenzori.Checked) optiuniSelectate.Add(Optiuni.SenzoriParcare);

                // Create new transaction
                TranzactieAuto tranzactie = new TranzactieAuto
                {
                    NumeVanzator = txtVanzator.Text,
                    NumeCumparator = txtCump.Text,
                    TipMasina = txtTip.Text,
                    ModelMasina = txtModel.Text,
                    AnFabricatie = anFabricatie,
                    Culoare = culoareSelectata,
                    Optiuni = optiuniSelectate,
                    DataTranzactie = DateTime.Now,
                    Pret = decimal.Parse(txtPret.Text)
                };

                // Add transaction to list and update UI
                tranzactii.Add(tranzactie);
                AdaugaTranzactiePeFormular(tranzactie);
                MessageBox.Show("Tranzacția a fost adăugată cu succes.");
                managerTranzactii.AdaugaTranzactie(tranzactie);
                ReseteazaControale();
            }
            else
            {
                MessageBox.Show("Vă rugăm să completați toate câmpurile obligatorii.");
            }
        }

        private void ReseteazaControale()
        {
            // Resetează toate câmpurile TextBox
            txtVanzator.Text = string.Empty;
            txtCump.Text = string.Empty;
            txtTip.Text = string.Empty;
            txtModel.Text = string.Empty;
            txtFabricare.Text = string.Empty;
            txtPret.Text = string.Empty;

            // Resetează toate RadioButton-urile
            rdbRosu.Checked = false;
            rdbAlb.Checked = false;
            rdbNegru.Checked = false;
            rdbAlbastru.Checked = false;
            rdbGri.Checked = false;
            rdbVerde.Checked = false;

            // Resetează toate CheckBox-urile
            ckbCtAuto.Checked = false;
            ckbCtManual.Checked = false;
            ckbAC.Checked = false;
            ckbGPS.Checked = false;
            ckbIncalzire.Checked = false;
            ckbSenzori.Checked = false;
        }

        private void LoadTransactions()
        {
            string numeFisier = "tranzactii.txt";
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleFisier = Path.Combine(locatieFisierSolutie, numeFisier);

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
            tableLayoutPanel.RowCount++;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            string[] detaliiTranzactie = new string[]
            {
                tranzactie.NumeVanzator,
                tranzactie.NumeCumparator,
                tranzactie.TipMasina,
                tranzactie.ModelMasina,
                tranzactie.AnFabricatie.ToString(),
                tranzactie.Culoare.ToString(),
                tranzactie.DataTranzactie.ToString("yyyy-MM-dd"),
                tranzactie.Pret.ToString(),
                string.Join(", ", tranzactie.Optiuni.Select(optiune => optiune.ToString()))
            };

            for (int i = 0; i < detaliiTranzactie.Length; i++)
            {
                Label label = new Label();
                label.Text = detaliiTranzactie[i];
                label.ForeColor = Color.Black;
                label.Font = new Font("Segoe UI", 10f); // Dimensiunea textului micșorată
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.AutoSize = true;
                label.Margin = new Padding(SPATIU_INTRE_COLOANE); // Eliminăm complet distanța între coloane

                if (i == detaliiTranzactie.Length - 1)
                {
                    // Specific for the last column (Optiuni)
                    label.Margin = new Padding(0, SPATIU_INTRE_COLOANE, SPATIU_INTRE_COLOANE, SPATIU_INTRE_COLOANE); // Adaugă o margine stânga
                }

                tableLayoutPanel.Controls.Add(label, i, tableLayoutPanel.RowCount - 1);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadTransactions();
        }

        private void btnAdaugareTr_Click(object sender, EventArgs e)
        {
            flMeniuAdaugare.Visible = true;
            flMeniuCautare.Visible = false;
        }

        private void btnCautareTr_Click(object sender, EventArgs e)
        {
            flMeniuAdaugare.Visible = false;
            flMeniuCautare.Visible = true;
        }

        private void btnCauta_Click(object sender, EventArgs e)
        {
            string tipModel = txtTipModel.Text.Trim();
            DateTime dataDeLa = dtpDeLa.Value;
            DateTime dataPanaLa = dtpPanaLa.Value;

            if (string.IsNullOrWhiteSpace(tipModel))
            {
                MessageBox.Show("Vă rugăm să introduceți tipul sau modelul mașinii.");
                return;
            }

            if (dataDeLa > dataPanaLa)
            {
                MessageBox.Show("Data de început nu poate fi mai mare decât data de sfârșit.");
                return;
            }

            var tranzactiiFiltrate = tranzactii
                .Where(t => (t.TipMasina.Contains(tipModel) || t.ModelMasina.Contains(tipModel)) && t.DataTranzactie >= dataDeLa && t.DataTranzactie <= dataPanaLa)
                .ToList();

            if (tranzactiiFiltrate.Count == 0)
            {
                MessageBox.Show("Nu există nicio mașină căutată în perioada specificată.");
                return;
            }

            var grupareTranzactii = tranzactiiFiltrate
                .GroupBy(t => new { t.TipMasina, t.ModelMasina })
                .Select(g => new { TipModel = $"{g.Key.TipMasina} {g.Key.ModelMasina}", Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefault();

            if (grupareTranzactii != null)
            {
                lstAfisare.Items.Clear();
                lstAfisare.Items.Add($"Cea mai căutată mașină: {grupareTranzactii.TipModel} cu {grupareTranzactii.Count} apariții.");
            }
            else
            {
                MessageBox.Show("Nu există nicio mașină căutată în perioada specificată.");
            }
        }
    }
}
