namespace marketplace.ui.Views
{
    partial class MarketplaceView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lboxActiveSell = new System.Windows.Forms.ListBox();
            this.lblMarketplace = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lboxBuy = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lboxActiveSell
            // 
            this.lboxActiveSell.FormattingEnabled = true;
            this.lboxActiveSell.Location = new System.Drawing.Point(6, 49);
            this.lboxActiveSell.Name = "lboxActiveSell";
            this.lboxActiveSell.Size = new System.Drawing.Size(451, 147);
            this.lboxActiveSell.TabIndex = 0;
            // 
            // lblMarketplace
            // 
            this.lblMarketplace.AutoSize = true;
            this.lblMarketplace.Location = new System.Drawing.Point(3, 0);
            this.lblMarketplace.Name = "lblMarketplace";
            this.lblMarketplace.Size = new System.Drawing.Size(10, 13);
            this.lblMarketplace.TabIndex = 1;
            this.lblMarketplace.Text = "-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Active Sell";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 199);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Active Buy";
            // 
            // lboxBuy
            // 
            this.lboxBuy.FormattingEnabled = true;
            this.lboxBuy.Location = new System.Drawing.Point(6, 218);
            this.lboxBuy.Name = "lboxBuy";
            this.lboxBuy.Size = new System.Drawing.Size(451, 147);
            this.lboxBuy.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 333;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MarketplaceView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lboxBuy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMarketplace);
            this.Controls.Add(this.lboxActiveSell);
            this.Name = "MarketplaceView";
            this.Size = new System.Drawing.Size(463, 377);
            this.Load += new System.EventHandler(this.MarketplaceView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lboxActiveSell;
        private System.Windows.Forms.Label lblMarketplace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lboxBuy;
        private System.Windows.Forms.Timer timer1;
    }
}
