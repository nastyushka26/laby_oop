// Replace the content of PluginSettingsForm.cs with this full implementation
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PluginInterface;

namespace laba2oop
{
    public partial class PluginSettingsForm : Form
    {
        private List<IDataProcessorPlugin> _availableProcessors;
        private List<IDataProcessorPlugin> _selectedProcessors;
        private CheckedListBox clbProcessors;
        private Button btnConfigure;
        private Button btnMoveUp;
        private Button btnMoveDown;
        private Button btnOk;
        private Button btnCancel;
        private Label lbl;
        private Label lblInfo;

        public List<IDataProcessorPlugin> SelectedProcessors => _selectedProcessors;

        public PluginSettingsForm(List<IDataProcessorPlugin> availableProcessors, List<IDataProcessorPlugin> currentSelection)
        {
            _availableProcessors = availableProcessors ?? new List<IDataProcessorPlugin>();
            _selectedProcessors = currentSelection?.Select(p => p.Clone()).ToList() ?? new List<IDataProcessorPlugin>();

            InitializeComponents();
            LoadProcessors();
        }

        private void InitializeComponents()
        {
            // Form settings
            this.Text = "Data Processor Plugins Settings";
            this.Size = new Size(750, 550);
            this.MinimumSize = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = SystemColors.Control;

            // Info label
            lblInfo = new Label()
            {
                Text = "Processors are applied in order from top to bottom when saving,\nand in reverse order when loading.",
                Location = new Point(15, 15),
                Size = new Size(700, 40),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.DarkBlue,
                AutoSize = false
            };

            // Main label
            lbl = new Label()
            {
                Text = "Select processors to apply (in order):",
                Location = new Point(15, 60),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = false
            };

            // CheckedListBox
            clbProcessors = new CheckedListBox()
            {
                Location = new Point(15, 90),
                Size = new Size(450, 350),
                CheckOnClick = true,
                Font = new Font("Segoe UI", 10),
                IntegralHeight = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom
            };
            clbProcessors.ItemCheck += ClbProcessors_ItemCheck;

            // Configure button
            btnConfigure = new Button()
            {
                Text = "Configure Selected",
                Location = new Point(480, 90),
                Size = new Size(230, 35),
                Font = new Font("Segoe UI", 10),
                FlatStyle = FlatStyle.Standard,
                BackColor = Color.LightBlue,
                UseVisualStyleBackColor = false
            };
            btnConfigure.Click += BtnConfigure_Click;

            // Move Up button
            btnMoveUp = new Button()
            {
                Text = "▲ Move Up",
                Location = new Point(480, 140),
                Size = new Size(110, 35),
                Font = new Font("Segoe UI", 9),
                FlatStyle = FlatStyle.Standard
            };
            btnMoveUp.Click += BtnMoveUp_Click;

            // Move Down button
            btnMoveDown = new Button()
            {
                Text = "▼ Move Down",
                Location = new Point(600, 140),
                Size = new Size(110, 35),
                Font = new Font("Segoe UI", 9),
                FlatStyle = FlatStyle.Standard
            };
            btnMoveDown.Click += BtnMoveDown_Click;

            // OK button
            btnOk = new Button()
            {
                Text = "OK",
                Location = new Point(480, 450),
                Size = new Size(110, 45),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                DialogResult = DialogResult.OK,
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Standard
            };

            // Cancel button
            btnCancel = new Button()
            {
                Text = "Cancel",
                Location = new Point(600, 450),
                Size = new Size(110, 45),
                Font = new Font("Segoe UI", 10),
                DialogResult = DialogResult.Cancel,
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Standard
            };

            // Add all controls
            this.Controls.Add(lblInfo);
            this.Controls.Add(lbl);
            this.Controls.Add(clbProcessors);
            this.Controls.Add(btnConfigure);
            this.Controls.Add(btnMoveUp);
            this.Controls.Add(btnMoveDown);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
        }

        private void ClbProcessors_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Rebuild selected list after check change
            this.BeginInvoke(new Action(RebuildSelectedList));
        }

        private void LoadProcessors()
        {
            clbProcessors.Items.Clear();

            // Add currently selected processors (checked)
            foreach (var proc in _selectedProcessors)
            {
                int index = clbProcessors.Items.Add(proc.ProcessorName);
                clbProcessors.SetItemChecked(index, true);
            }

            // Add available but not selected processors (unchecked)
            foreach (var proc in _availableProcessors)
            {
                if (!_selectedProcessors.Any(p => p.ProcessorId == proc.ProcessorId))
                {
                    int index = clbProcessors.Items.Add(proc.ProcessorName);
                    clbProcessors.SetItemChecked(index, false);
                }
            }

            if (clbProcessors.Items.Count == 0)
            {
                clbProcessors.Items.Add("No processors available");
                clbProcessors.Enabled = false;
            }
        }

        private void BtnConfigure_Click(object? sender, EventArgs e)
        {
            if (clbProcessors.SelectedIndex >= 0)
            {
                string selectedName = clbProcessors.SelectedItem?.ToString() ?? "";
                var processor = GetAllProcessors().FirstOrDefault(p => p.ProcessorName == selectedName);
                if (processor != null)
                {
                    try
                    {
                        processor.Configure();
                        MessageBox.Show($"Configured {processor.ProcessorName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Configuration failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnMoveUp_Click(object? sender, EventArgs e)
        {
            int idx = clbProcessors.SelectedIndex;
            if (idx > 0)
            {
                var item = clbProcessors.Items[idx];
                bool isChecked = clbProcessors.GetItemChecked(idx);
                clbProcessors.Items.RemoveAt(idx);
                clbProcessors.Items.Insert(idx - 1, item);
                clbProcessors.SetItemChecked(idx - 1, isChecked);
                clbProcessors.SelectedIndex = idx - 1;
                RebuildSelectedList();
            }
        }

        private void BtnMoveDown_Click(object? sender, EventArgs e)
        {
            int idx = clbProcessors.SelectedIndex;
            if (idx >= 0 && idx < clbProcessors.Items.Count - 1)
            {
                var item = clbProcessors.Items[idx];
                bool isChecked = clbProcessors.GetItemChecked(idx);
                clbProcessors.Items.RemoveAt(idx);
                clbProcessors.Items.Insert(idx + 1, item);
                clbProcessors.SetItemChecked(idx + 1, isChecked);
                clbProcessors.SelectedIndex = idx + 1;
                RebuildSelectedList();
            }
        }

        private void RebuildSelectedList()
        {
            _selectedProcessors.Clear();
            for (int i = 0; i < clbProcessors.Items.Count; i++)
            {
                if (clbProcessors.GetItemChecked(i))
                {
                    string name = clbProcessors.Items[i]?.ToString() ?? "";
                    var proc = GetAllProcessors().FirstOrDefault(p => p.ProcessorName == name);
                    if (proc != null)
                    {
                        _selectedProcessors.Add(proc.Clone());
                    }
                }
            }
        }

        private List<IDataProcessorPlugin> GetAllProcessors()
        {
            var all = new List<IDataProcessorPlugin>(_availableProcessors);
            all.AddRange(_selectedProcessors);
            return all.GroupBy(p => p.ProcessorId).Select(g => g.First()).ToList();
        }
    }
}