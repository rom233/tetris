
namespace tetris
{
    partial class MainWindow
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
            this.panel = new System.Windows.Forms.Panel();
            this.lblStats = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbNextShape = new System.Windows.Forms.PictureBox();
            this.btnPause = new tetris.FlatButton();
            this.btnFillField = new tetris.FlatButton();
            this.btnStartGame = new tetris.FlatButton();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNextShape)).BeginInit();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.pbNextShape);
            this.panel.Controls.Add(this.btnPause);
            this.panel.Controls.Add(this.lblStats);
            this.panel.Controls.Add(this.btnFillField);
            this.panel.Controls.Add(this.btnStartGame);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(400, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(594, 745);
            this.panel.TabIndex = 0;
            // 
            // lblStats
            // 
            this.lblStats.AutoSize = true;
            this.lblStats.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStats.Location = new System.Drawing.Point(26, 227);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(271, 266);
            this.lblStats.TabIndex = 2;
            this.lblStats.Text = "Линий сломано:\r\n90 (★900)\r\n\r\nвыапывапвыап\r\nывапывапывап\r\n\r\nСледующая фигура:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 745);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pbNextShape
            // 
            this.pbNextShape.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbNextShape.Location = new System.Drawing.Point(33, 436);
            this.pbNextShape.Name = "pbNextShape";
            this.pbNextShape.Size = new System.Drawing.Size(222, 199);
            this.pbNextShape.TabIndex = 4;
            this.pbNextShape.TabStop = false;
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            this.btnPause.Label_text = "Пауза";
            this.btnPause.Location = new System.Drawing.Point(33, 117);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(222, 62);
            this.btnPause.TabIndex = 3;
            // 
            // btnFillField
            // 
            this.btnFillField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            this.btnFillField.Label_text = "Заполнить поле";
            this.btnFillField.Location = new System.Drawing.Point(33, 641);
            this.btnFillField.Name = "btnFillField";
            this.btnFillField.Size = new System.Drawing.Size(170, 39);
            this.btnFillField.TabIndex = 1;
            // 
            // btnStartGame
            // 
            this.btnStartGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            this.btnStartGame.Label_text = "Начать игру";
            this.btnStartGame.Location = new System.Drawing.Point(33, 35);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(222, 62);
            this.btnStartGame.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(994, 745);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNextShape)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private FlatButton btnStartGame;
        private System.Windows.Forms.PictureBox pictureBox1;
        private FlatButton btnFillField;
        private System.Windows.Forms.Label lblStats;
        private FlatButton btnPause;
        private System.Windows.Forms.PictureBox pbNextShape;
    }
}