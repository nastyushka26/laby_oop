namespace laba2oop
{
    partial class NewMainForm
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
            statusLabel = new Label();
            panelPaint = new Panel();
            panel_buttons = new Panel();
            SuspendLayout();
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(765, 16);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(78, 32);
            statusLabel.TabIndex = 0;
            statusLabel.Text = "label1";
            // 
            // panelPaint
            // 
            panelPaint.Location = new Point(15, 16);
            panelPaint.Name = "panelPaint";
            panelPaint.Size = new Size(701, 628);
            panelPaint.TabIndex = 1;
            panelPaint.Paint += panelPaint_Paint;
            panelPaint.MouseClick += panelPaint_MouseClick;
            // 
            // panel_buttons
            // 
            panel_buttons.Location = new Point(750, 115);
            panel_buttons.Name = "panel_buttons";
            panel_buttons.Size = new Size(307, 529);
            panel_buttons.TabIndex = 2;
            // 
            // NewMainForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1084, 656);
            Controls.Add(panel_buttons);
            Controls.Add(panelPaint);
            Controls.Add(statusLabel);
            Name = "NewMainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NewMainForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label statusLabel;
        private Panel panelPaint;
        private Panel panel_buttons;
    }
}