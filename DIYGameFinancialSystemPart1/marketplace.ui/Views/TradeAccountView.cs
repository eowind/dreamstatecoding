using System.Windows.Forms;
using marketplace.ui.Finance.Market;

namespace marketplace.ui.Views
{
    public partial class TradeAccountView : UserControl
    {
        private ITradeAccount _account;
        private Marketplace _marketplace;
        public TradeAccountView()
        {
            InitializeComponent();
        }

        public void Bind(ITradeAccount account, Marketplace marketplace)
        {
            _account = account;
            _account.Updated += TradeAccountOnUpdated;
            _marketplace = marketplace;
            UpdateAll();
        }

        private void TradeAccountOnUpdated(object sender, TradeAccount e)
        {
            UpdateAll();
        }

        public void UpdateAll()
        {
            if (DesignMode)
                return;
            if (_account == null)
                return;
            lblWalletId.Text = _account.BalanceAccount.Id.ToString();
            lblBalance.Text = _account.BalanceAccount.Balance.ToString();
            lblBuyOrders.Text = _account.ValueOfActiveBuyOrders.ToString();
            lboxOwned.Items.Clear();
            lboxOrders.Items.Clear();
            foreach (var item in _account.Securities.Values)
            {
                lboxOwned.Items.Add(item);
            }
            foreach (var item in _account.BuyOrders)
            {
                lboxOrders.Items.Add(item);
            }
            foreach (var item in _account.SellOrders)
            {
                lboxOrders.Items.Add(item);
            }
        }

        private void btnSell_Click(object sender, System.EventArgs e)
        {
            if (lboxOwned.SelectedIndex == -1)
                return;
            var order = _account.CreateSellOrder(((Security)lboxOwned.Items[lboxOwned.SelectedIndex]).Name, ulong.Parse(txtQuantity.Text), ulong.Parse(txtPrice.Text));
            _marketplace.Sell(order);
        }

        private void btnBuy_Click(object sender, System.EventArgs e)
        {
            var order = _account.CreateBuyOrder(txtName.Text, ulong.Parse(txtQuantity.Text), ulong.Parse(txtPrice.Text));
            _marketplace.Buy(order);
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            if (lboxOrders.SelectedIndex == -1)
                return;
            var order = lboxOrders.Items[lboxOrders.SelectedIndex];
            var sellOrder = order as SellOrder;
            if (sellOrder != null)
                sellOrder.Cancel();
            var buyOrder = order as BuyOrder;
            if (buyOrder != null)
                buyOrder.Cancel();
        }
    }
}
