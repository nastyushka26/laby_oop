namespace lab_3
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listBox1 = new ListBox();
            panel1 = new Panel();
            btnLoad = new Button();
            btnSave = new Button();
            btnChange = new Button();
            btnRemove = new Button();
            btnAdd = new Button();
            richTextBox1 = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(69, 49);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(569, 164);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnLoad);
            panel1.Controls.Add(btnSave);
            panel1.Controls.Add(btnChange);
            panel1.Controls.Add(btnRemove);
            panel1.Controls.Add(btnAdd);
            panel1.Location = new Point(741, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(258, 494);
            panel1.TabIndex = 1;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(19, 389);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(218, 66);
            btnLoad.TabIndex = 4;
            btnLoad.Text = "Загрузить";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(19, 297);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(218, 67);
            btnSave.TabIndex = 3;
            btnSave.Text = "Сохранить";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnChange
            // 
            btnChange.Location = new Point(19, 207);
            btnChange.Name = "btnChange";
            btnChange.Size = new Size(218, 65);
            btnChange.TabIndex = 2;
            btnChange.Text = "Изменить";
            btnChange.UseVisualStyleBackColor = true;
            btnChange.Click += btnChange_Click;
            // 
            // btnRemove
            // 
            btnRemove.Location = new Point(19, 117);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(218, 66);
            btnRemove.TabIndex = 1;
            btnRemove.Text = "Удалить";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(19, 37);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(218, 59);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Добавить";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(69, 263);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(569, 243);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(69, 9);
            label1.Name = "label1";
            label1.Size = new Size(153, 32);
            label1.TabIndex = 3;
            label1.Text = "List Elements";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(65, 224);
            label2.Name = "label2";
            label2.Size = new Size(209, 32);
            label2.TabIndex = 4;
            label2.Text = "Element attributes";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LavenderBlush;
            ClientSize = new Size(1011, 529);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(richTextBox1);
            Controls.Add(panel1);
            Controls.Add(listBox1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private Panel panel1;
        private Button btnLoad;
        private Button btnSave;
        private Button btnChange;
        private Button btnRemove;
        private Button btnAdd;
        private RichTextBox richTextBox1;
        private Label label1;
        private Label label2;
    }
}
