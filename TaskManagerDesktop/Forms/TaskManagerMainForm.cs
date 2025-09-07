using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TaskManagerDesktop
{
    public class TaskManagerMainForm : Form
    {
        // UI Controls
        private DataGridView dgvTasks;
        private TextBox txtTitle;
        private TextBox txtDescription;
        private ComboBox cmbCategory;
        private ComboBox cmbPriority;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnToggleComplete;
        private Label lblTitle;
        private Label lblDescription;
        private Label lblCategory;
        private Label lblPriority;

        // Manager
        private TaskManager taskManager;
        private Task selectedTask;
        private string dataFilePath;

        public TaskManagerMainForm()
        {
            // Path
            dataFilePath = Path.Combine(Application.StartupPath, "tasks.dat");

            taskManager = new TaskManager();
            taskManager.TasksChanged += TaskManager_TasksChanged;
            selectedTask = null;

            SetupForm();
            SetupEventHandlers();
            LoadTasksFromFile();
        }

        private void SetupForm()
        {
            // Main Form
            this.Text = "Task Manager Desktop";
            this.Size = new Size(843, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            try
            {
                this.Icon = new Icon("taskIcon.ico");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Can't load incon file: {ex.Message}\nPath: {Environment.CurrentDirectory}");
            }

            // DataGridView for tasks
            dgvTasks = new DataGridView();
            dgvTasks.Location = new Point(20, 20);
            dgvTasks.Size = new Size(543, 350);
            dgvTasks.AllowUserToAddRows = false;
            dgvTasks.AllowUserToDeleteRows = false;
            dgvTasks.ReadOnly = true;
            dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTasks.MultiSelect = false;

            // DataGridView Columns
            dgvTasks.Columns.Add("Title", "Title");
            dgvTasks.Columns.Add("Description", "Description");
            dgvTasks.Columns.Add("Category", "Category");
            dgvTasks.Columns.Add("Priority", "Priority");
            dgvTasks.Columns.Add("IsCompleted", "IsCompleted");

            dgvTasks.Columns[0].Width = 120;
            dgvTasks.Columns[1].Width = 150;
            dgvTasks.Columns[2].Width = 80;
            dgvTasks.Columns[3].Width = 80;
            dgvTasks.Columns[4].Width = 70;

            this.Controls.Add(dgvTasks);

            // Input Panel
            Panel inputPanel = new Panel();
            inputPanel.Location = new Point(570, 20);
            inputPanel.Size = new Size(230, 380);
            inputPanel.BorderStyle = BorderStyle.FixedSingle;

            // Label & TextBox for title
            lblTitle = new Label();
            lblTitle.Text = "Title:";
            lblTitle.Location = new Point(10, 15);
            lblTitle.Size = new Size(60, 20);
            inputPanel.Controls.Add(lblTitle);

            txtTitle = new TextBox();
            txtTitle.Location = new Point(10, 35);
            txtTitle.Size = new Size(200, 25);
            inputPanel.Controls.Add(txtTitle);

            // Label & TextBox for description
            lblDescription = new Label();
            lblDescription.Text = "Description:";
            lblDescription.Location = new Point(10, 70);
            lblDescription.Size = new Size(60, 20);
            inputPanel.Controls.Add(lblDescription);

            txtDescription = new TextBox();
            txtDescription.Location = new Point(10, 90);
            txtDescription.Size = new Size(200, 80);
            txtDescription.Multiline = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;
            inputPanel.Controls.Add(txtDescription);

            // Label & ComboBox for category
            lblCategory = new Label();
            lblCategory.Text = "Category:";
            lblCategory.Location = new Point(10, 185);
            lblCategory.Size = new Size(70, 20);
            inputPanel.Controls.Add(lblCategory);

            cmbCategory = new ComboBox();
            cmbCategory.Location = new Point(10, 205);
            cmbCategory.Size = new Size(200, 25);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Items.AddRange(new string[] { "Work", "Home", "Shopping", "Other" });
            cmbCategory.SelectedIndex = 0;
            inputPanel.Controls.Add(cmbCategory);

            // Label & ComboBox for priority
            lblPriority = new Label();
            lblPriority.Text = "Priority:";
            lblPriority.Location = new Point(10, 240);
            lblPriority.Size = new Size(70, 20);
            inputPanel.Controls.Add(lblPriority);

            cmbPriority = new ComboBox();
            cmbPriority.Location = new Point(10, 260);
            cmbPriority.Size = new Size(200, 25);
            cmbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPriority.Items.AddRange(new string[] { "Low", "Medium", "High", "Critical" });
            cmbPriority.SelectedIndex = 1;
            inputPanel.Controls.Add(cmbPriority);

            // Buttons: Add, Edit, Delete
            btnAdd = new Button();
            btnAdd.Text = "Add";
            btnAdd.Location = new Point(10, 300);
            btnAdd.Size = new Size(60, 30);
            btnAdd.BackColor = Color.LightGreen;
            inputPanel.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Text = "Edit";
            btnEdit.Location = new Point(80, 300);
            btnEdit.Size = new Size(60, 30);
            btnEdit.BackColor = Color.LightBlue;
            btnEdit.Enabled = false;
            inputPanel.Controls.Add(btnEdit);

            btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.Location = new Point(150, 300);
            btnDelete.Size = new Size(60, 30);
            btnDelete.BackColor = Color.LightCoral;
            btnDelete.Enabled = false;
            inputPanel.Controls.Add(btnDelete);

            // Button Toggle Complete
            btnToggleComplete = new Button();
            btnToggleComplete.Text = "Finished";
            btnToggleComplete.Location = new Point(10, 340);
            btnToggleComplete.Size = new Size(200, 25);
            btnToggleComplete.BackColor = Color.LightYellow;
            btnToggleComplete.Enabled = false;
            inputPanel.Controls.Add(btnToggleComplete);

            this.Controls.Add(inputPanel);

            // Status bar
            StatusStrip statusStrip = new StatusStrip();
            ToolStripStatusLabel statusLabel = new ToolStripStatusLabel();
            statusLabel.Text = "Ready";
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);
        }

        private void SetupEventHandlers()
        {
            // Event handler for buttons
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnToggleComplete.Click += BtnToggleComplete_Click;

            // Event handler for DataGridView
            dgvTasks.SelectionChanged += DgvTasks_SelectionChanged;
            dgvTasks.CellClick += DgvTasks_CellClick;

            // Event handler for closing app
            this.FormClosing += TaskManagerMainForm_FormClosing;
        }

        // Extra event handler for cell click to ensure selection
        private void DgvTasks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvTasks.Rows.Count)
            {
                dgvTasks.Rows[e.RowIndex].Selected = true;
                DgvTasks_SelectionChanged(sender, EventArgs.Empty);
            }
        }

        // Event handler for task change
        private void TaskManager_TasksChanged(object sender, EventArgs e)
        {
            RefreshTaskList();
            SaveTasksToFile(); // Automatyczny zapis przy każdej zmianie
        }

        // refresh the task list in DataGridView
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
                    task.IsCompleted ? "Yes" : "No"
                };

                int rowIndex = dgvTasks.Rows.Add(row);
                dgvTasks.Rows[rowIndex].Tag = task;

                if (task.IsCompleted)
                {
                    dgvTasks.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    dgvTasks.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.DarkGray;
                }
            }

            // Force selection of the first item if available
            if (dgvTasks.Rows.Count > 0)
            {
                dgvTasks.ClearSelection();
                dgvTasks.Rows[0].Selected = true;
                dgvTasks.CurrentCell = dgvTasks.Rows[0].Cells[0];
                DgvTasks_SelectionChanged(dgvTasks, EventArgs.Empty);
            }
            else
            {
                selectedTask = null;
                ClearForm();
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnToggleComplete.Enabled = false;
            }

            UpdateStatusBar();
        }

        // update status bar information
        private void UpdateStatusBar()
        {
            var statusStrip = this.Controls.OfType<StatusStrip>().FirstOrDefault();
            if (statusStrip != null && statusStrip.Items.Count > 0)
            {
                var statusLabel = statusStrip.Items[0] as ToolStripStatusLabel;
                if (statusLabel != null)
                {
                    statusLabel.Text = $"Tasks: {taskManager.TaskCount} | " +
                                     $"Finished: {taskManager.CompletedTaskCount} | " +
                                     $"To do: {taskManager.PendingTaskCount}";
                }
            }
        }

        // Event handler for tasks selection change
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

                    btnToggleComplete.Text = selectedTask.IsCompleted ? "Not finished" : "Finished";
                }
            }
            else
            {
                selectedTask = null;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnToggleComplete.Enabled = false;
                btnToggleComplete.Text = "Finished";
            }
        }

        // load task details into the form
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

        // clear the input form
        private void ClearForm()
        {
            txtTitle.Clear();
            txtDescription.Clear();
            cmbCategory.SelectedIndex = 0;
            cmbPriority.SelectedIndex = 1;
        }

        // validate form inputs
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Title cannot be empty!", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return false;
            }
            return true;
        }

        // event handler for Add button
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
                MessageBox.Show("The task has been added", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while adding task: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // event handler for Edit button
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Select a task to edit!", "Information",
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

                MessageBox.Show("Task is updated", "Succes",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while updating task: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event handler for delete button
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Select task for delete!", "Information",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure to delete this task '{selectedTask.Title}'?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                taskManager.RemoveTask(selectedTask.Id);
                selectedTask = null;
                ClearForm();
                MessageBox.Show("Task is deleted!", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Event handler for toggle complete button
        private void BtnToggleComplete_Click(object sender, EventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Sleect Task", "Information",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            taskManager.ToggleTaskCompletion(selectedTask.Id);

            string status = selectedTask.IsCompleted ? "finished" : "not finished";
            MessageBox.Show($"Task marked as {status}!", "Success",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===== files =====

        // save tasks to file
        private void SaveTasksToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(dataFilePath))
                {
                    writer.WriteLine($"# Task Manager Data - {DateTime.Now}");

                    foreach (var task in taskManager.Tasks)
                    {
                        // Format: ID|Title|Description|Category|Priority|IsCompleted|CreatedDate|CompletedDate
                        string line = $"{task.Id}|{EscapeString(task.Title)}|{EscapeString(task.Description)}|" +
                                    $"{(int)task.Category}|{(int)task.Priority}|{task.IsCompleted}|" +
                                    $"{task.CreatedDate.ToBinary()}|{(task.CompletedDate?.ToBinary().ToString() ?? "null")}";
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error with saving files: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // read tasks from file
        private void LoadTasksFromFile()
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    var tasksToLoad = new List<Task>();
                    string[] lines = File.ReadAllLines(dataFilePath);

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                            continue;

                        try
                        {
                            string[] parts = line.Split('|');
                            if (parts.Length >= 8)
                            {
                                var task = new Task();
                                task.Id = int.Parse(parts[0]);
                                task.Title = UnescapeString(parts[1]);
                                task.Description = UnescapeString(parts[2]);
                                task.Category = (TaskCategory)int.Parse(parts[3]);
                                task.Priority = (TaskPriority)int.Parse(parts[4]);
                                task.IsCompleted = bool.Parse(parts[5]);
                                task.CreatedDate = DateTime.FromBinary(long.Parse(parts[6]));

                                if (parts[7] != "null")
                                    task.CompletedDate = DateTime.FromBinary(long.Parse(parts[7]));

                                tasksToLoad.Add(task);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error with parsing: {line} - {ex.Message}");
                        }
                    }

                    if (tasksToLoad.Count > 0)
                    {
                        taskManager.LoadTasksFromFile(tasksToLoad);

                        RefreshTaskList();

                        UpdateStatusInformation($"Read {tasksToLoad.Count} tasks from file");
                    }
                    else
                    {
                        UpdateStatusInformation("Valid data in file");
                    }
                }
                else
                {
                    UpdateStatusInformation("No saved tasks - start over");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file: {ex.Message}\n\nTry deleting the tasks.dat file and restarting.", "Loading error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UpdateStatusInformation("Error loading file");
            }
        }

        // Event handler for closing app
        private void TaskManagerMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveTasksToFile();
        }

        // helper methods for escaping/unescaping strings
        private string EscapeString(string input)
        {
            return input?.Replace("|", "&#124;").Replace("\r\n", "\\n").Replace("\n", "\\n") ?? "";
        }

        private string UnescapeString(string input)
        {
            return input?.Replace("&#124;", "|").Replace("\\n", "\n") ?? "";
        }

        // helper method to update status information temporarily
        private void UpdateStatusInformation(string message)
        {
            var statusStrip = this.Controls.OfType<StatusStrip>().FirstOrDefault();
            if (statusStrip != null && statusStrip.Items.Count > 0)
            {
                var statusLabel = statusStrip.Items[0] as ToolStripStatusLabel;
                if (statusLabel != null)
                {
                    statusLabel.Text = message;

                    var timer = new Timer();
                    timer.Interval = 3000;
                    timer.Tick += (s, args) => {
                        UpdateStatusBar();
                        timer.Stop();
                        timer.Dispose();
                    };
                    timer.Start();
                }
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskManagerMainForm));
            this.SuspendLayout();
            // 
            // TaskManagerMainForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TaskManagerMainForm";
            this.ResumeLayout(false);

        }
    }
}