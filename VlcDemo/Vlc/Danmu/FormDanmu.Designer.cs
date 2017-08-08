namespace VlcDemo.Danmu
{
    partial class FormDanmu
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
            this.components = new System.ComponentModel.Container();
            this.timerRoll = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerRoll
            // 
            this.timerRoll.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // FormDanmu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormDanmu";
            this.ShowInTaskbar = false;
            this.Text = "FormDanmu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerRoll;
    }
}