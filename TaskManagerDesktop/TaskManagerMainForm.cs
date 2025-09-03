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
        private Button btnToggleComplete;
        private Button btnTestSelection;
        private Label lblTitle;
        private Label lblDescription;
        private Label lblCategory;
        private Label lblPriority;

        // Manager zadań
        private TaskManager taskManager;
        private Task selectedTask;

        public TaskManagerMainForm()
        {
            // Inicjalizacja task managera
            taskManager = new TaskManager();
            taskManager.TasksChanged += TaskManager_TasksChanged;
            selectedTask = null;

            SetupForm();
            SetupEventHandlers();
        }

        private void SetupForm()
        {
            // Ustawienia głównego formularza
            this.Text = "Task Manager Desktop";
            this.Size = new Size(800, 650);
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
            inputPanel.Size = new Size(230, 420);
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

            // Przycisk Toggle Complete
            btnToggleComplete = new Button();
            btnToggleComplete.Text = "Oznacz jako ukończone";
            btnToggleComplete.Location = new Point(10, 340);
            btnToggleComplete.Size = new Size(200, 25);
            btnToggleComplete.BackColor = Color.LightYellow;
            inputPanel.Controls.Add(btnToggleComplete);

            // PRZYCISK TESTOWY
            btnTestSelection = new Button();
            btnTestSelection.Text = "TEST - Wybierz pierwszy";
            btnTestSelection.Location = new Point(10, 370);
            btnTestSelection.Size = new Size(200, 30);
            btnTestSelection.BackColor = Color.Orange;
            inputPanel.Controls.Add(btnTestSelection);

            this.Controls.Add(inputPanel);

            // Status bar na dole
            StatusStrip statusStrip = new StatusStrip();
            ToolStripStatusLabel statusLabel = new ToolStripStatusLabel();
            statusLabel.Text = "Gotowy";
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);
        }

        private void SetupEventHandlers()
        {
            // Event handlery dla przycisków
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnToggleComplete.Click += BtnToggleComplete_Click;
            btnTestSelection.Click += BtnTestSelection_Click;

            // Event handlery dla DataGridView
            dgvTasks.SelectionChanged += DgvTasks_SelectionChanged;
            dgvTasks.CellClick += DgvTasks_CellClick;
        }

        // Dodatkowy event handler dla kliknięcia komórki
        private void DgvTasks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvTasks.Rows.Count)
            {
                dgvTasks.Rows[e.RowIndex].Selected = true;
                DgvTasks_SelectionChanged(sender, EventArgs.Empty);
            }
        }

        // Event handler dla zmian w task managerze
        private void TaskManager_TasksChanged(object sender, EventArgs e)
        {
            RefreshTaskList();
        }

        // Odświeżanie listy zadań w DataGridView
        private void RefreshTaskList()
        {
            dgvTasks.Rows.Clear();

            foreach (var task in taskManager.Tasks)
            {
                string[] row = new string[]
                {
                    task.Title,
                    task.Description,
                    task.Category.ToString(),
                    task.Priority.ToString(),
                    task.IsCompleted ? "Tak" : "Nie"
                };

                int rowIndex = dgvTasks.Rows.Add(row);
                dgvTasks.Rows[rowIndex].Tag = task; // Przechowujemy referencję do zadania

                // Kolorowanie ukończonych zadań
                if (task.IsCompleted)
                {
                    dgvTasks.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    dgvTasks.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.DarkGray;
                }
            }

            // WYMUŚ SELEKCJĘ PIERWSZEGO WIERSZA JEŚLI ISTNIEJE
            if (dgvTasks.Rows.Count > 0)
            {
                dgvTasks.ClearSelection();
                dgvTasks.Rows[0].Selected = true;
                dgvTasks.CurrentCell = dgvTasks.Rows[0].Cells[0];
                // Ręcznie wywołaj event selection
                DgvTasks_SelectionChanged(dgvTasks, EventArgs.Empty);
            }
            else
            {
                // Jeśli nie ma zadań, wyczyść formularz
                selectedTask = null;
                ClearForm();
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnToggleComplete.Enabled = false;
            }

            UpdateStatusBar();
        }

        // Aktualizacja paska statusu
        private void UpdateStatusBar()
        {
            var statusStrip = this.Controls.OfType<StatusStrip>().FirstOrDefault();
            if (statusStrip != null && statusStrip.Items.Count > 0)
            {
                var statusLabel = statusStrip.Items[0] as ToolStripStatusLabel;
                if (statusLabel != null)
                {
                    statusLabel.Text = $"Zadań: {taskManager.TaskCount} | " +
                                     $"Ukończonych: {taskManager.CompletedTaskCount} | " +
                                     $"Do zrobienia: {taskManager.PendingTaskCount}";
                }
            }
        }

        // Event handler dla wyboru zadania
        private void DgvTasks_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedRows.Count > 0)
            {
                selectedTask = dgvTasks.SelectedRows[0].Tag as Task;
                if (selectedTask != null)
                {
                    LoadTaskToForm(selectedTask);
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnToggleComplete.Enabled = true;

                    // Aktualizuj tekst przycisku w zależności od stanu zadania
                    btnToggleComplete.Text = selectedTask.IsCompleted ? "Oznacz jako nieukończone" : "Oznacz jako ukończone";
                }
            }
            else
            {
                selectedTask = null;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnToggleComplete.Enabled = false;
                btnToggleComplete.Text = "Oznacz jako ukończone";
            }
        }

        // Ładowanie zadania do formularza
        private void LoadTaskToForm(Task task)
        {
            if (task != null)
            {
                txtTitle.Text = task.Title;
                txtDescription.Text = task.Description;
                cmbCategory.SelectedIndex = (int)task.Category;
                cmbPriority.SelectedIndex = (int)task.Priority;
            }
        }

        // Czyszczenie formularza
        private void ClearForm()
        {
            txtTitle.Clear();
            txtDescription.Clear();
            cmbCategory.SelectedIndex = 0;
            cmbPriority.SelectedIndex = 1;
        }

        // Walidacja danych z formularza
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Tytuł zadania nie może być pusty!", "Błąd walidacji",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return false;
            }
            return true;
        }

        // Event handler dla przycisku Dodaj
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                taskManager.AddTask(
                    txtTitle.Text,
                    txtDescription.Text,
                    (TaskCategory)cmbCategory.SelectedIndex,
                    (TaskPriority)cmbPriority.SelectedIndex
                );

                ClearForm();
                MessageBox.Show("Zadanie zostało dodane!", "Sukces",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas dodawania zadania: {ex.Message}", "Błąd",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event handler dla przycisku Edytuj
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Wybierz zadanie do edycji!", "Informacja",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateForm()) return;

            try
            {
                taskManager.EditTask(
                    selectedTask.Id,
                    txtTitle.Text,
                    txtDescription.Text,
                    (TaskCategory)cmbCategory.SelectedIndex,
                    (TaskPriority)cmbPriority.SelectedIndex
                );

                MessageBox.Show("Zadanie zostało zaktualizowane!", "Sukces",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas edycji zadania: {ex.Message}", "Błąd",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event handler dla przycisku Usuń
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Wybierz zadanie do usunięcia!", "Informacja",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Czy na pewno chcesz usunąć zadanie '{selectedTask.Title}'?",
                "Potwierdzenie",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                taskManager.RemoveTask(selectedTask.Id);
                selectedTask = null; // Wyczyść wybranego taska
                ClearForm();
                MessageBox.Show("Zadanie zostało usunięte!", "Sukces",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Event handler dla przycisku Toggle Complete
        private void BtnToggleComplete_Click(object sender, EventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Wybierz zadanie!", "Informacja",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            taskManager.ToggleTaskCompletion(selectedTask.Id);

            string status = selectedTask.IsCompleted ? "ukończone" : "nieukończone";
            MessageBox.Show($"Zadanie oznaczone jako {status}!", "Sukces",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // METODA TESTOWA
        private void BtnTestSelection_Click(object sender, EventArgs e)
        {
            if (dgvTasks.Rows.Count > 0)
            {
                // Wybierz pierwszy wiersz
                dgvTasks.ClearSelection();
                dgvTasks.Rows[0].Selected = true;
                dgvTasks.CurrentCell = dgvTasks.Rows[0].Cells[0];

                // Pobierz Task z Tag
                Task testTask = dgvTasks.Rows[0].Cells[0].OwningRow.Tag as Task;

                if (testTask != null)
                {
                    MessageBox.Show($"Znaleziony task: '{testTask.Title}'");

                    // Ładuj ręcznie do formularza
                    txtTitle.Text = testTask.Title;
                    txtDescription.Text = testTask.Description;
                    cmbCategory.SelectedIndex = (int)testTask.Category;
                    cmbPriority.SelectedIndex = (int)testTask.Priority;

                    // Włącz przyciski
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnToggleComplete.Enabled = true;
                    selectedTask = testTask;

                    MessageBox.Show("Task załadowany ręcznie! Teraz spróbuj Edytuj/Usuń");
                }
                else
                {
                    MessageBox.Show("Tag wiersza jest NULL!");
                }
            }
            else
            {
                MessageBox.Show("Brak zadań w liście!");
            }
        }
    }
}