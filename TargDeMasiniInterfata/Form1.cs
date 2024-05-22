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
        private const int SPATIU_INTRE_COLOANE = 0; 
        private const int INALTIME_RAND = 40;

        private TableLayoutPanel tableLayoutPanel;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
            InitializeDataGrid();
            dataGridTranzactii.CellContentClick += dataGridTranzactii_CellContentClick;
            dataGridTranzactii.CellEndEdit += dataGridTranzactii_CellEndEdit;
            string caleFisier = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "tranzactii.txt");
            managerTranzactii = new ManagerTranzactii(caleFisier);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadTransactions();
            AfiseazaTranzactiiInDataGrid();
        }

        private void InitializeCustomComponents()
        {
            this.Size = new Size(1850, 700);

            Panel panelCentral = new Panel();
            panelCentral.Dock = DockStyle.Fill;
            this.Controls.Add(panelCentral);

            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Anchor = AnchorStyles.None;
            tableLayoutPanel.Size = new Size(1350, 450);
            tableLayoutPanel.Location = new Point(460, 115);
            tableLayoutPanel.AutoScroll = true;
            tableLayoutPanel.ColumnCount = 9;
            tableLayoutPanel.RowCount = 1;

            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            for (int i = 0; i < 8; i++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 8));
            }
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            panelCentral.Controls.Add(tableLayoutPanel);


            string[] titluri = { "Vanzator", "Cumparator", "Tip Masina", "Model Masina", "Anul fabricarii", "Culoare", "Data tranzactiei", "Pret(€)", "Optiuni" };
            for (int i = 0; i < titluri.Length; i++)
            {
                Label label = new Label();
                label.Text = titluri[i];
                label.ForeColor = Color.Black;
                label.Font = new Font("Segoe UI", 13f, FontStyle.Bold); 
                label.Margin = new Padding(SPATIU_INTRE_COLOANE); 
                label.Size = new Size(120, INALTIME_RAND);
                tableLayoutPanel.Controls.Add(label, i, 0);
            }
        }
        private void LoadTransactions()
        {
            string numeFisier = "tranzactii.txt";
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleFisier = Path.Combine(locatieFisierSolutie, numeFisier);
            tranzactii = CitesteTranzactii(caleFisier);

            RefreshTableLayoutPanel();
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

        /*private void AfiseazaTranzactii()
        {
            foreach (TranzactieAuto tranzactie in tranzactii)
            {
                AdaugaTranzactiePeFormular(tranzactie);
            }
        }*/

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
                label.Font = new Font("Segoe UI", 10f);
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.AutoSize = true;
                label.Margin = new Padding(SPATIU_INTRE_COLOANE);

                if (i == detaliiTranzactie.Length - 1)
                {
                    label.AutoEllipsis = true;
                    label.MaximumSize = new Size(250, 0);
                }

                tableLayoutPanel.Controls.Add(label, i, tableLayoutPanel.RowCount - 1);
            }
        }

        private void RefreshTableLayoutPanel()
        {
            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.RowCount = 1;

            string[] titluri = { "Vanzator", "Cumparator", "Tip Masina", "Model Masina", "Anul fabricarii", "Culoare", "Data tranzactiei", "Pret(€)", "Optiuni" };
            for (int i = 0; i < titluri.Length; i++)
            {
                Label label = new Label();
                label.Text = titluri[i];
                label.ForeColor = Color.Black;
                label.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
                label.Margin = new Padding(SPATIU_INTRE_COLOANE);
                label.Size = new Size(120, INALTIME_RAND);
                tableLayoutPanel.Controls.Add(label, i, 0);
            }

            foreach (var tranzactie in tranzactii)
            {
                AdaugaTranzactiePeFormular(tranzactie);
            }
        }

        private void btnAdauga_Click(object sender, EventArgs e)
        {
            bool valid = true;

            lblVanzator.ForeColor = Color.White;
            lblCump.ForeColor = Color.White;
            lblTip.ForeColor = Color.White;
            lblModel.ForeColor = Color.White;
            lblFabricare.ForeColor = Color.White;
            lblCuloare.ForeColor = Color.White;
            lblPret.ForeColor = Color.White;
            lblOptiuni.ForeColor = Color.White;

            if (string.IsNullOrWhiteSpace(txtVanzator.Text) || txtVanzator.Text.All(char.IsDigit))
            {
                valid = false;
                lblVanzator.ForeColor = Color.Red;
            }
            if (string.IsNullOrWhiteSpace(txtCump.Text) || txtCump.Text.All(char.IsDigit))
            {
                valid = false;
                lblCump.ForeColor = Color.Red;
            }
            if (string.IsNullOrWhiteSpace(txtTip.Text) || txtTip.Text.All(char.IsDigit))
            {
                valid = false;
                lblTip.ForeColor = Color.Red;
            }
            if (string.IsNullOrWhiteSpace(txtModel.Text))
            {
                valid = false;
                lblModel.ForeColor = Color.Red;
            }
            if (string.IsNullOrWhiteSpace(txtPret.Text) || !decimal.TryParse(txtPret.Text, out _))
            {
                valid = false;
                lblPret.ForeColor = Color.Red;
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
            if (ckbCtAuto.Checked && ckbCtManual.Checked)
            {
                lblOptiuni.ForeColor = Color.Red;
                MessageBox.Show("Se poate selecta numai o cutie de viteze (automată sau manuală).");
                valid = false;
            }

            if (valid)
            {
                Culoare culoareSelectata = Culoare.Rosu;
                if (rdbAlb.Checked) culoareSelectata = Culoare.Alb;
                else if (rdbNegru.Checked) culoareSelectata = Culoare.Negru;
                else if (rdbAlbastru.Checked) culoareSelectata = Culoare.Albastru;
                else if (rdbGri.Checked) culoareSelectata = Culoare.Gri;
                else if (rdbVerde.Checked) culoareSelectata = Culoare.Verde;

                List<Optiuni> optiuniSelectate = new List<Optiuni>();
                if (ckbCtAuto.Checked) optiuniSelectate.Add(Optiuni.CutieAutomata);
                if (ckbCtManual.Checked) optiuniSelectate.Add(Optiuni.CutieManuala);
                if (ckbAC.Checked) optiuniSelectate.Add(Optiuni.AerConditionat);
                if (ckbGPS.Checked) optiuniSelectate.Add(Optiuni.NavigatieGPS);
                if (ckbIncalzire.Checked) optiuniSelectate.Add(Optiuni.ScauneIncalzite);
                if (ckbSenzori.Checked) optiuniSelectate.Add(Optiuni.SenzoriParcare);

                TranzactieAuto tranzactie = new TranzactieAuto
                {
                    NumeVanzator = txtVanzator.Text,
                    NumeCumparator = txtCump.Text,
                    TipMasina = txtTip.Text,
                    ModelMasina = txtModel.Text,
                    AnFabricatie = anFabricatie,
                    Culoare = culoareSelectata,
                    Optiuni = optiuniSelectate,
                    DataTranzactie = dtpData.Value,
                    Pret = decimal.Parse(txtPret.Text)
                };

                tranzactii.Add(tranzactie);
                AdaugaTranzactiePeFormular(tranzactie);
                AfiseazaTranzactiiInDataGrid();
                MessageBox.Show("Tranzacția a fost adăugată cu succes.");
                managerTranzactii.AdaugaTranzactie(tranzactie);
                ReseteazaControale();
            }
            else
            {
                MessageBox.Show("Vă rugăm să completați câmpurile corespunzătoare și corect.");
            }
        }

        private void ReseteazaControale()
        {
            txtVanzator.Text = string.Empty;
            txtCump.Text = string.Empty;
            txtTip.Text = string.Empty;
            txtModel.Text = string.Empty;
            txtFabricare.Text = string.Empty;
            txtPret.Text = string.Empty;

            rdbRosu.Checked = false;
            rdbAlb.Checked = false;
            rdbNegru.Checked = false;
            rdbAlbastru.Checked = false;
            rdbGri.Checked = false;
            rdbVerde.Checked = false;

            ckbCtAuto.Checked = false;
            ckbCtManual.Checked = false;
            ckbAC.Checked = false;
            ckbGPS.Checked = false;
            ckbIncalzire.Checked = false;
            ckbSenzori.Checked = false;
        }

        private void btnAdaugareTr_Click(object sender, EventArgs e)
        {
            flMeniuAdaugare.Visible = true;
            flMeniuCautare.Visible = false;
            flMeniuEditare.Visible = false;
        }

        private void btnCautareTr_Click(object sender, EventArgs e)
        {
            flMeniuAdaugare.Visible = false;
            flMeniuCautare.Visible = true;
            flMeniuEditare.Visible=false;
        }
        private void btnEditareTr_Click(object sender, EventArgs e)
        {
            flMeniuAdaugare.Visible = false;
            flMeniuCautare.Visible = false;
            flMeniuEditare.Visible = true;
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

        private void InitializeDataGrid()
        {
            dataGridTranzactii.Columns.Clear();
            dataGridTranzactii.Columns.Add("NumeVanzator", "Nume Vânzător");
            dataGridTranzactii.Columns.Add("NumeCumparator", "Nume Cumpărător");
            dataGridTranzactii.Columns.Add("TipMasina", "Tip Mașină");
            dataGridTranzactii.Columns.Add("ModelMasina", "Model Mașină");
            dataGridTranzactii.Columns.Add("AnFabricatie", "Anul Fabricării");
            dataGridTranzactii.Columns.Add("Culoare", "Culoare");
            dataGridTranzactii.Columns.Add("DataTranzactie", "Data Tranzacției");
            dataGridTranzactii.Columns.Add("Pret", "Preț");
            dataGridTranzactii.Columns.Add("Optiuni", "Opțiuni");

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.HeaderText = "Ștergere";
            btnDelete.Name = "btnDelete";
            btnDelete.Text = "Șterge";
            btnDelete.UseColumnTextForButtonValue = true;
            dataGridTranzactii.Columns.Add(btnDelete);
        }

        private void StergereTranzactie(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < tranzactii.Count)
            {
                tranzactii.RemoveAt(rowIndex);

                managerTranzactii.SalveazaTranzactii(tranzactii);


                AfiseazaTranzactiiInDataGrid();
            }
        }

        private void dataGridTranzactii_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridTranzactii.Rows[e.RowIndex];
                TranzactieAuto tranzactie = tranzactii[e.RowIndex];

                tranzactie.NumeVanzator = row.Cells["NumeVanzator"].Value.ToString();
                tranzactie.NumeCumparator = row.Cells["NumeCumparator"].Value.ToString();
                tranzactie.TipMasina = row.Cells["TipMasina"].Value.ToString();
                tranzactie.ModelMasina = row.Cells["ModelMasina"].Value.ToString();
                tranzactie.AnFabricatie = int.Parse(row.Cells["AnFabricatie"].Value.ToString());
                tranzactie.Culoare = (Culoare)Enum.Parse(typeof(Culoare), row.Cells["Culoare"].Value.ToString());
                tranzactie.DataTranzactie = DateTime.Parse(row.Cells["DataTranzactie"].Value.ToString()); 
                tranzactie.Pret = decimal.Parse(row.Cells["Pret"].Value.ToString());
                tranzactie.Optiuni = ParseazaOptiuni(row.Cells["Optiuni"].Value.ToString());

                managerTranzactii.SalveazaTranzactii(tranzactii);

                row.Cells["NumeVanzator"].Value = tranzactie.NumeVanzator;
                row.Cells["NumeCumparator"].Value = tranzactie.NumeCumparator;
                row.Cells["TipMasina"].Value = tranzactie.TipMasina;
                row.Cells["ModelMasina"].Value = tranzactie.ModelMasina;
                row.Cells["AnFabricatie"].Value = tranzactie.AnFabricatie;
                row.Cells["Culoare"].Value = tranzactie.Culoare.ToString();
                row.Cells["DataTranzactie"].Value = tranzactie.DataTranzactie.ToString("yyyy-MM-dd");
                row.Cells["Pret"].Value = tranzactie.Pret;
                row.Cells["Optiuni"].Value = string.Join(", ", tranzactie.Optiuni.Select(optiune => optiune.ToString()));
            }
        }

        private void dataGridTranzactii_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var deleteColumn = dataGridTranzactii.Columns["btnDelete"];
            if (deleteColumn != null && e.RowIndex >= 0 && e.ColumnIndex == deleteColumn.Index)
            {
                StergereTranzactie(e.RowIndex);
            }
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
        private void AfiseazaTranzactiiInDataGrid()
        {
            dataGridTranzactii.Rows.Clear();
            foreach (var tranzactie in tranzactii)
            {
                int rowIndex = dataGridTranzactii.Rows.Add();
                DataGridViewRow row = dataGridTranzactii.Rows[rowIndex];
                row.Cells["NumeVanzator"].Value = tranzactie.NumeVanzator;
                row.Cells["NumeCumparator"].Value = tranzactie.NumeCumparator;
                row.Cells["TipMasina"].Value = tranzactie.TipMasina;
                row.Cells["ModelMasina"].Value = tranzactie.ModelMasina;
                row.Cells["AnFabricatie"].Value = tranzactie.AnFabricatie;
                row.Cells["Culoare"].Value = tranzactie.Culoare.ToString();
                row.Cells["DataTranzactie"].Value = tranzactie.DataTranzactie.ToString("yyyy-MM-dd");
                row.Cells["Pret"].Value = tranzactie.Pret;
                row.Cells["Optiuni"].Value = string.Join(", ", tranzactie.Optiuni.Select(optiune => optiune.ToString()));
            }
        }

        private void btnRefreh_Click(object sender, EventArgs e)
        {
            RefreshTableLayoutPanel();
        }
    }
}
