namespace marketplace.ui
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
            this.marketplaceView1 = new marketplace.ui.Views.MarketplaceView();
            this.accountViewTwo = new marketplace.ui.Views.TradeAccountView();
            this.accountViewOne = new marketplace.ui.Views.TradeAccountView();
            this.SuspendLayout();
            // 
            // marketplaceView1
            // 
            this.marketplaceView1.Location = new System.Drawing.Point(338, 12);
            this.marketplaceView1.Name = "marketplaceView1";
            this.marketplaceView1.Size = new System.Drawing.Size(463, 379);
            this.marketplaceView1.TabIndex = 2;
            // 
            // accountViewTwo
            // 
            this.accountViewTwo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.accountViewTwo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accountViewTwo.Location = new System.Drawing.Point(806, 12);
            this.accountViewTwo.Name = "accountViewTwo";
            this.accountViewTwo.Size = new System.Drawing.Size(320, 501);
            this.accountViewTwo.TabIndex = 1;
            // 
            // accountViewOne
            // 
            this.accountViewOne.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.accountViewOne.Location = new System.Drawing.Point(12, 12);
            this.accountViewOne.Name = "accountViewOne";
            this.accountViewOne.Size = new System.Drawing.Size(320, 501);
            this.accountViewOne.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 542);
            this.Controls.Add(this.marketplaceView1);
            this.Controls.Add(this.accountViewTwo);
            this.Controls.Add(this.accountViewOne);
            this.Name = "Form1";
            this.Text = "dreamstatecoding.blogspot.com - marketplace";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Views.TradeAccountView accountViewOne;
        private Views.TradeAccountView accountViewTwo;
        private Views.MarketplaceView marketplaceView1;
    }
}

