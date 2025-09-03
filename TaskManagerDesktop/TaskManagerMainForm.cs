using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TaskManagerDesktop
{
    public class TaskManagerMainForm : Form
    {
        // Kontrolki UI
        private DataGridView dgvTasks;
        private TextBox txtTitle;
        private TextBox txtDescription;
        private ComboBox cmbCategory;
        private ComboBox cmbPriority;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Label lblTitle;
        private Label lblDescription;
        private Label lblCategory;
        private Label lblPriority;

        public TaskManagerMainForm()
        {
            SetupForm();
        }

        private void SetupForm()
        {
            // Ustawienia głównego formularza
            this.Text = "Task Manager Desktop";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // DataGridView do wyświetlania zadań
            dgvTasks = new DataGridView();
            dgvTasks.Location = new Point(20, 20);
            dgvTasks.Size = new Size(500, 350);
            dgvTasks.AllowUserToAddRows = false;
            dgvTasks.AllowUserToDeleteRows = false;
            dgvTasks.ReadOnly = true;
            dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTasks.MultiSelect = false;

            // Kolumny DataGridView
            dgvTasks.Columns.Add("Title", "Tytuł");
            dgvTasks.Columns.Add("Description", "Opis");
            dgvTasks.Columns.Add("Category", "Kategoria");
            dgvTasks.Columns.Add("Priority", "Priorytet");
            dgvTasks.Columns.Add("IsCompleted", "Ukończone");

            // Ustawianie szerokości kolumn
            dgvTasks.Columns[0].Width = 120;
            dgvTasks.Columns[1].Width = 150;
            dgvTasks.Columns[2].Width = 80;
            dgvTasks.Columns[3].Width = 80;
            dgvTasks.Columns[4].Width = 70;

            this.Controls.Add(dgvTasks);

            // Panel do wprowadzania danych
            Panel inputPanel = new Panel();
            inputPanel.Location = new Point(540, 20);
            inputPanel.Size = new Size(230, 350);
            inputPanel.BorderStyle = BorderStyle.FixedSingle;

            // Label i TextBox dla tytułu
            lblTitle = new Label();
            lblTitle.Text = "Tytuł:";
            lblTitle.Location = new Point(10, 15);
            lblTitle.Size = new Size(60, 20);
            inputPanel.Controls.Add(lblTitle);

            txtTitle = new TextBox();
            txtTitle.Location = new Point(10, 35);
            txtTitle.Size = new Size(200, 25);
            inputPanel.Controls.Add(txtTitle);

            // Label i TextBox dla opisu
            lblDescription = new Label();
            lblDescription.Text = "Opis:";
            lblDescription.Location = new Point(10, 70);
            lblDescription.Size = new Size(60, 20);
            inputPanel.Controls.Add(lblDescription);

            txtDescription = new TextBox();
            txtDescription.Location = new Point(10, 90);
            txtDescription.Size = new Size(200, 80);
            txtDescription.Multiline = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;
            inputPanel.Controls.Add(txtDescription);

            // Label i ComboBox dla kategorii
            lblCategory = new Label();
            lblCategory.Text = "Kategoria:";
            lblCategory.Location = new Point(10, 185);
            lblCategory.Size = new Size(70, 20);
            inputPanel.Controls.Add(lblCategory);

            cmbCategory = new ComboBox();
            cmbCategory.Location = new Point(10, 205);
            cmbCategory.Size = new Size(200, 25);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Items.AddRange(new string[] { "Praca", "Dom", "Zakupy", "Inne" });
            cmbCategory.SelectedIndex = 0;
            inputPanel.Controls.Add(cmbCategory);

            // Label i ComboBox dla priorytetu
            lblPriority = new Label();
            lblPriority.Text = "Priorytet:";
            lblPriority.Location = new Point(10, 240);
            lblPriority.Size = new Size(70, 20);
            inputPanel.Controls.Add(lblPriority);

            cmbPriority = new ComboBox();
            cmbPriority.Location = new Point(10, 260);
            cmbPriority.Size = new Size(200, 25);
            cmbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPriority.Items.AddRange(new string[] { "Niski", "Średni", "Wysoki", "Krytyczny" });
            cmbPriority.SelectedIndex = 1;
            inputPanel.Controls.Add(cmbPriority);

            // Przyciski
            btnAdd = new Button();
            btnAdd.Text = "Dodaj";
            btnAdd.Location = new Point(10, 300);
            btnAdd.Size = new Size(60, 30);
            btnAdd.BackColor = Color.LightGreen;
            inputPanel.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Text = "Edytuj";
            btnEdit.Location = new Point(80, 300);
            btnEdit.Size = new Size(60, 30);
            btnEdit.BackColor = Color.LightBlue;
            inputPanel.Controls.Add(btnEdit);

            btnDelete = new Button();
            btnDelete.Text = "Usuń";
            btnDelete.Location = new Point(150, 300);
            btnDelete.Size = new Size(60, 30);
            btnDelete.BackColor = Color.LightCoral;
            inputPanel.Controls.Add(btnDelete);

            this.Controls.Add(inputPanel);

            // Status bar na dole
            StatusStrip statusStrip = new StatusStrip();
            ToolStripStatusLabel statusLabel = new ToolStripStatusLabel();
            statusLabel.Text = "Gotowy";
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);
        }
    }
}