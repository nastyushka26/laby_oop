namespace lab_3
{
    partial class VehicleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            CmbType = new ComboBox();
            txtBrand = new TextBox();
            txtColor = new TextBox();
            txtYear = new TextBox();
            txtMaxSpeed = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            panelSpecific = new Panel();
            lblBrand = new Label();
            lblColor = new Label();
            label1 = new Label();
            lblMaxSpeed = new Label();
            lblType = new Label();
            SuspendLayout();
            // 
            // CmbType
            // 
            CmbType.FormattingEnabled = true;
            CmbType.Location = new Point(167, 252);
            CmbType.Name = "CmbType";
            CmbType.Size = new Size(200, 40);
            CmbType.TabIndex = 0;
            CmbType.SelectedIndexChanged += CmbType_SelectedIndexChanged;
            // 
            // txtBrand
            // 
            txtBrand.Location = new Point(167, 17);
            txtBrand.Name = "txtBrand";
            txtBrand.Size = new Size(200, 39);
            txtBrand.TabIndex = 1;
            // 
            // txtColor
            // 
            txtColor.Location = new Point(167, 74);
            txtColor.Name = "txtColor";
            txtColor.Size = new Size(200, 39);
            txtColor.TabIndex = 2;
            // 
            // txtYear
            // 
            txtYear.Location = new Point(167, 132);
            txtYear.Name = "txtYear";
            txtYear.Size = new Size(200, 39);
            txtYear.TabIndex = 3;
            // 
            // txtMaxSpeed
            // 
            txtMaxSpeed.Location = new Point(167, 191);
            txtMaxSpeed.Name = "txtMaxSpeed";
            txtMaxSpeed.Size = new Size(200, 39);
            txtMaxSpeed.TabIndex = 4;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(381, 343);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(171, 46);
            btnSave.TabIndex = 13;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(620, 343);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(168, 46);
            btnCancel.TabIndex = 14;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // panelSpecific
            // 
            panelSpecific.BorderStyle = BorderStyle.FixedSingle;
            panelSpecific.Location = new Point(381, 17);
            panelSpecific.Name = "panelSpecific";
            panelSpecific.Size = new Size(407, 300);
            panelSpecific.TabIndex = 15;
            // 
            // lblBrand
            // 
            lblBrand.AutoSize = true;
            lblBrand.Location = new Point(20, 20);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new Size(76, 32);
            lblBrand.TabIndex = 16;
            lblBrand.Text = "Brand";
            // 
            // lblColor
            // 
            lblColor.AutoSize = true;
            lblColor.Location = new Point(20, 74);
            lblColor.Name = "lblColor";
            lblColor.Size = new Size(71, 32);
            lblColor.TabIndex = 17;
            lblColor.Text = "Color";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 139);
            label1.Name = "label1";
            label1.Size = new Size(58, 32);
            label1.TabIndex = 18;
            label1.Text = "Year";
            // 
            // lblMaxSpeed
            // 
            lblMaxSpeed.AutoSize = true;
            lblMaxSpeed.Location = new Point(20, 194);
            lblMaxSpeed.Name = "lblMaxSpeed";
            lblMaxSpeed.Size = new Size(126, 32);
            lblMaxSpeed.TabIndex = 19;
            lblMaxSpeed.Text = "MaxSpeed";
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Location = new Point(26, 250);
            lblType.Name = "lblType";
            lblType.Size = new Size(65, 32);
            lblType.TabIndex = 20;
            lblType.Text = "Type";
            // 
            // VehicleForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 409);
            Controls.Add(lblType);
            Controls.Add(lblMaxSpeed);
            Controls.Add(label1);
            Controls.Add(lblColor);
            Controls.Add(lblBrand);
            Controls.Add(CmbType);
            Controls.Add(panelSpecific);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtMaxSpeed);
            Controls.Add(txtYear);
            Controls.Add(txtColor);
            Controls.Add(txtBrand);
            Name = "VehicleForm";
            Text = "VehicleForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox CmbType;
        private TextBox txtBrand;
        private TextBox txtColor;
        private TextBox txtYear;
        private TextBox txtMaxSpeed;
        private Button btnSave;
        private Button btnCancel;
        private Panel panelSpecific;
        private Label lblBrand;
        private Label lblColor;
        private Label label1;
        private Label lblMaxSpeed;
        private Label lblType;
    }
}