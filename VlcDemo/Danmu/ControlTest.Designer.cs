namespace VlcDemo
{
    partial class ControlTest
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
            this.button1 = new System.Windows.Forms.Button();
            this.danmuBox1 = new VlcDemo.DanmuBox();
            this.timerRoll = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(172, 197);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // danmuBox1
            // 
            this.danmuBox1.BackColor = System.Drawing.Color.Transparent;
            this.danmuBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.danmuBox1.Location = new System.Drawing.Point(0, 0);
            this.danmuBox1.Name = "danmuBox1";
            this.danmuBox1.Size = new System.Drawing.Size(284, 260);
            this.danmuBox1.TabIndex = 0;
            // 
            // timerRoll
            // 
            this.timerRoll.Tick += new System.EventHandler(this.timerRoll_Tick);
            // 
            // ControlTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 260);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.danmuBox1);
            this.DoubleBuffered = true;
            this.Name = "ControlTest";
            this.Text = "ControlTest";
            this.Shown += new System.EventHandler(this.ControlTest_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private DanmuBox danmuBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timerRoll;
    }
}