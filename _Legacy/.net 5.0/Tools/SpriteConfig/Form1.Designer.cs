using System.Drawing;
using System.Windows.Forms;

namespace SpriteConfig
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblEditing = new System.Windows.Forms.Label();
            this.pnlGfx = new System.Windows.Forms.Panel();
            this.gTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlAnimatedSprite = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lblFrame = new System.Windows.Forms.Label();
            this.animationsListBox = new System.Windows.Forms.ListBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnStepSingle = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.addAnimationButton = new System.Windows.Forms.Button();
            this.removeAnimation = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.newToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(1037, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.openToolStripMenuItem.Text = "&Open Texture";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(90, 22);
            this.newToolStripMenuItem.Text = "&New Window";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // lblEditing
            // 
            this.lblEditing.AutoSize = true;
            this.lblEditing.Location = new System.Drawing.Point(9, 24);
            this.lblEditing.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEditing.Name = "lblEditing";
            this.lblEditing.Size = new System.Drawing.Size(45, 13);
            this.lblEditing.TabIndex = 1;
            this.lblEditing.Text = "Editing: ";
            // 
            // pnlGfx
            // 
            this.pnlGfx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGfx.Location = new System.Drawing.Point(0, 483);
            this.pnlGfx.Margin = new System.Windows.Forms.Padding(2);
            this.pnlGfx.Name = "pnlGfx";
            this.pnlGfx.Size = new System.Drawing.Size(929, 341);
            this.pnlGfx.TabIndex = 2;
            this.toolTip1.SetToolTip(this.pnlGfx, "Sprite Sheet");
            // 
            // gTimer
            // 
            this.gTimer.Interval = 30;
            this.gTimer.Tick += new System.EventHandler(this.GTimer_Tick);
            // 
            // pnlAnimatedSprite
            // 
            this.pnlAnimatedSprite.Location = new System.Drawing.Point(10, 65);
            this.pnlAnimatedSprite.Margin = new System.Windows.Forms.Padding(2);
            this.pnlAnimatedSprite.Name = "pnlAnimatedSprite";
            this.pnlAnimatedSprite.Size = new System.Drawing.Size(271, 209);
            this.pnlAnimatedSprite.TabIndex = 3;
            this.toolTip1.SetToolTip(this.pnlAnimatedSprite, "Preview");
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(286, 39);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(380, 427);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGrid1_PropertyValueChanged);
            // 
            // lblFrame
            // 
            this.lblFrame.AutoSize = true;
            this.lblFrame.Location = new System.Drawing.Point(9, 468);
            this.lblFrame.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFrame.Name = "lblFrame";
            this.lblFrame.Size = new System.Drawing.Size(0, 13);
            this.lblFrame.TabIndex = 4;
            // 
            // animationsListBox
            // 
            this.animationsListBox.FormattingEnabled = true;
            this.animationsListBox.Location = new System.Drawing.Point(717, 65);
            this.animationsListBox.Name = "animationsListBox";
            this.animationsListBox.Size = new System.Drawing.Size(212, 407);
            this.animationsListBox.TabIndex = 5;
            this.animationsListBox.Click += new System.EventHandler(this.animationsListBox_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Font = new System.Drawing.Font("Wingdings 3", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPlay.Location = new System.Drawing.Point(12, 279);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(223, 23);
            this.btnPlay.TabIndex = 7;
            this.btnPlay.Text = "u ";
            this.toolTip1.SetToolTip(this.btnPlay, "Play");
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Visible = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnStepSingle
            // 
            this.btnStepSingle.Font = new System.Drawing.Font("Wingdings 3", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnStepSingle.Location = new System.Drawing.Point(241, 279);
            this.btnStepSingle.Name = "btnStepSingle";
            this.btnStepSingle.Size = new System.Drawing.Size(39, 23);
            this.btnStepSingle.TabIndex = 3;
            this.btnStepSingle.Text = "g";
            this.toolTip1.SetToolTip(this.btnStepSingle, "Step Single");
            this.btnStepSingle.UseVisualStyleBackColor = true;
            this.btnStepSingle.Visible = false;
            this.btnStepSingle.Click += new System.EventHandler(this.btnStepSingle_Click);
            // 
            // addAnimationButton
            // 
            this.addAnimationButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addAnimationButton.Location = new System.Drawing.Point(672, 152);
            this.addAnimationButton.Name = "addAnimationButton";
            this.addAnimationButton.Size = new System.Drawing.Size(39, 38);
            this.addAnimationButton.TabIndex = 8;
            this.addAnimationButton.Text = "+";
            this.toolTip1.SetToolTip(this.addAnimationButton, "Play");
            this.addAnimationButton.UseVisualStyleBackColor = true;
            this.addAnimationButton.Click += new System.EventHandler(this.addAnimationButton_Click);
            // 
            // removeAnimation
            // 
            this.removeAnimation.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeAnimation.Location = new System.Drawing.Point(672, 196);
            this.removeAnimation.Name = "removeAnimation";
            this.removeAnimation.Size = new System.Drawing.Size(39, 38);
            this.removeAnimation.TabIndex = 9;
            this.removeAnimation.Text = "-";
            this.toolTip1.SetToolTip(this.removeAnimation, "Play");
            this.removeAnimation.UseVisualStyleBackColor = true;
            this.removeAnimation.Click += new System.EventHandler(this.removeAnimation_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 891);
            this.Controls.Add(this.removeAnimation);
            this.Controls.Add(this.addAnimationButton);
            this.Controls.Add(this.btnStepSingle);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.animationsListBox);
            this.Controls.Add(this.lblFrame);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.pnlAnimatedSprite);
            this.Controls.Add(this.pnlGfx);
            this.Controls.Add(this.lblEditing);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "SpriteConfig";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label lblEditing;
        private System.Windows.Forms.Panel pnlGfx;
        private System.Windows.Forms.Timer gTimer;
        private System.Windows.Forms.Panel pnlAnimatedSprite;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private PropertyGrid propertyGrid1;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label lblFrame;
        private ListBox animationsListBox;
        private Button btnPlay;
        private Button btnStepSingle;
        private ToolTip toolTip1;
        private Button addAnimationButton;
        private Button removeAnimation;
    }
}

