namespace laba2oop
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            panelPaint = new Panel();
            panel_buttons = new Panel();
            statusLabel = new Label();
            SuspendLayout();
            // 
            // panelPaint
            // 
            resources.ApplyResources(panelPaint, "panelPaint");
            panelPaint.Name = "panelPaint";
            panelPaint.MouseClick += panel1_MouseClick;
            // 
            // panel_buttons
            // 
            resources.ApplyResources(panel_buttons, "panel_buttons");
            panel_buttons.Name = "panel_buttons";
            // 
            // statusLabel
            // 
            resources.ApplyResources(statusLabel, "statusLabel");
            statusLabel.Name = "statusLabel";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(statusLabel);
            Controls.Add(panel_buttons);
            Controls.Add(panelPaint);
            Name = "MainForm";
            Paint += MainForm_Paint;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelPaint;
        private Panel panel_buttons;
        private Label statusLabel;
    }
}
