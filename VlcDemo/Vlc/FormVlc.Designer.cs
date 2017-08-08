namespace VlcDemo
{
    partial class FormVlc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVlc));
            this.panelControls = new System.Windows.Forms.Panel();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.panelDisplay = new System.Windows.Forms.Panel();
            this.trackBarSeek = new System.Windows.Forms.TrackBar();
            this.listBoxList = new System.Windows.Forms.ListBox();
            this.panelList = new System.Windows.Forms.Panel();
            this.openFileDialogOpen = new System.Windows.Forms.OpenFileDialog();
            this.panelControls.SuspendLayout();
            this.panelDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSeek)).BeginInit();
            this.panelList.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControls
            // 
            this.panelControls.Controls.Add(this.buttonStop);
            this.panelControls.Controls.Add(this.buttonPause);
            this.panelControls.Controls.Add(this.buttonNext);
            this.panelControls.Controls.Add(this.buttonPlay);
            this.panelControls.Controls.Add(this.buttonOpen);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControls.Location = new System.Drawing.Point(0, 515);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(641, 43);
            this.panelControls.TabIndex = 0;
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(234, 6);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(60, 30);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.Location = new System.Drawing.Point(168, 6);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(60, 30);
            this.buttonPause.TabIndex = 3;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(113, 6);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(49, 30);
            this.buttonNext.TabIndex = 3;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(58, 6);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(49, 30);
            this.buttonPlay.TabIndex = 2;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(3, 6);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(49, 30);
            this.buttonOpen.TabIndex = 1;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // panelDisplay
            // 
            this.panelDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelDisplay.Controls.Add(this.trackBarSeek);
            this.panelDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDisplay.Location = new System.Drawing.Point(90, 0);
            this.panelDisplay.Name = "panelDisplay";
            this.panelDisplay.Size = new System.Drawing.Size(551, 515);
            this.panelDisplay.TabIndex = 1;
            // 
            // trackBarSeek
            // 
            this.trackBarSeek.AutoSize = false;
            this.trackBarSeek.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.trackBarSeek.Location = new System.Drawing.Point(0, 471);
            this.trackBarSeek.Margin = new System.Windows.Forms.Padding(0);
            this.trackBarSeek.Maximum = 100;
            this.trackBarSeek.Name = "trackBarSeek";
            this.trackBarSeek.Size = new System.Drawing.Size(547, 40);
            this.trackBarSeek.TabIndex = 4;
            this.trackBarSeek.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarSeek.Scroll += new System.EventHandler(this.trackBarSeek_Scroll);
            // 
            // listBoxList
            // 
            this.listBoxList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxList.FormattingEnabled = true;
            this.listBoxList.HorizontalScrollbar = true;
            this.listBoxList.Location = new System.Drawing.Point(0, 0);
            this.listBoxList.Name = "listBoxList";
            this.listBoxList.ScrollAlwaysVisible = true;
            this.listBoxList.Size = new System.Drawing.Size(90, 515);
            this.listBoxList.TabIndex = 2;
            this.listBoxList.TabStop = false;
            this.listBoxList.SelectedIndexChanged += new System.EventHandler(this.listBoxList_SelectedIndexChanged);
            // 
            // panelList
            // 
            this.panelList.Controls.Add(this.listBoxList);
            this.panelList.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelList.Location = new System.Drawing.Point(0, 0);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(90, 515);
            this.panelList.TabIndex = 1;
            // 
            // openFileDialogOpen
            // 
            this.openFileDialogOpen.Multiselect = true;
            this.openFileDialogOpen.RestoreDirectory = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 558);
            this.Controls.Add(this.panelDisplay);
            this.Controls.Add(this.panelList);
            this.Controls.Add(this.panelControls);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "P";
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.LocationChanged += new System.EventHandler(this.FormMain_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.panelControls.ResumeLayout(false);
            this.panelDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSeek)).EndInit();
            this.panelList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Panel panelDisplay;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.ListBox listBoxList;
        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.TrackBar trackBarSeek;
        private System.Windows.Forms.OpenFileDialog openFileDialogOpen;
        private System.Windows.Forms.Button buttonStop;
    }
}

