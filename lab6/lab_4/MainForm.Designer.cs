
namespace laba2oop
{
    partial class MainForm
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

            // statusLabel
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(15, 680);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(400, 25);
            statusLabel.TabIndex = 0;
            statusLabel.Text = "Ready";
            statusLabel.Font = new Font("Segoe UI", 10);
            statusLabel.BackColor = Color.LightGray;
            statusLabel.Padding = new Padding(5);

            // panelPaint
            panelPaint.Location = new Point(15, 16);
            panelPaint.Name = "panelPaint";
            panelPaint.Size = new Size(750, 650);
            panelPaint.TabIndex = 1;
            panelPaint.BackColor = Color.White;
            panelPaint.BorderStyle = BorderStyle.FixedSingle;
            panelPaint.Paint += panelPaint_Paint;
            panelPaint.MouseClick += panel1_MouseClick;

            // panel_buttons
            panel_buttons.Location = new Point(780, 16);
            panel_buttons.Name = "panel_buttons";
            panel_buttons.Size = new Size(280, 650);
            panel_buttons.TabIndex = 2;
            panel_buttons.BackColor = Color.FromArgb(240, 240, 240);
            panel_buttons.BorderStyle = BorderStyle.FixedSingle;
            panel_buttons.AutoScroll = true;

            // MainForm
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1080, 720);
            Controls.Add(panel_buttons);
            Controls.Add(panelPaint);
            Controls.Add(statusLabel);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Shape Drawing App with Encryption";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label statusLabel;
        private Panel panelPaint;
        private Panel panel_buttons;
    }
}