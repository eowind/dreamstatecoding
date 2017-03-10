using System;
using System.Windows.Forms;
using marketplace.ui.Finance.Market;

namespace marketplace.ui.Views
{
    public partial class MarketplaceView : UserControl
    {
        private Marketplace _marketplace;

        public MarketplaceView()
        {
            InitializeComponent();
        }
        private void MarketplaceView_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            timer1.Enabled = true;
        }

        public void Bind(Marketplace marketplace)
        {
            _marketplace = marketplace;
            _marketplace.Updated += MarketplaceOnUpdated;
            UpdateAll();
        }

        private void MarketplaceOnUpdated(object sender, Marketplace marketplace1)
        {
            UpdateAll();
        }

        private void UpdateAll()
        {
			if(_marketplace == null)
				return;
            lboxBuy.Items.Clear();
            foreach (var item in _marketplace.BuyOrders)
            {
                lboxBuy.Items.Add(item);
            }
            lboxActiveSell.Items.Clear();
            foreach (var item in _marketplace.SellOrders)
            {
                lboxActiveSell.Items.Add(item);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            _marketplace.Update();
        }

    }
}
