using System.Collections.Generic;
using System.Windows.Forms;
using marketplace.ui.Finance;
using marketplace.ui.Finance.Market;

namespace marketplace.ui
{
    public partial class Form1 : Form
    {
        private Marketplace _marketplace;
        private IAccount _accountOne;
        private IAccount _accountTwo;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            if (DesignMode)
                return;
            InitializeMarketplaceDemo();
        }

        private void InitializeMarketplaceDemo()
        {
            _marketplace = new Marketplace("Dreamstate Market");
            _accountOne = new Account(100);
            accountViewOne.Bind(new TradeAccount(_accountOne), _marketplace);
            _accountTwo = new Account(100);
            var tradeTwo = new TradeAccount(_accountTwo, new List<Security> { new Security("Iron Ore", 10ul)});
            accountViewTwo.Bind(tradeTwo, _marketplace);

            marketplaceView1.Bind(_marketplace);
        }
    }
}
