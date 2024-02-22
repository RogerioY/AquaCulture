using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AquaCulture
{
    public class TradershipShop : MonoBehaviour
    {
        //public Button oxygenUpgradeButton;
        //public Button pressureUpgradeButton;
        //public Button cigarMachineButton;
         public Button confirmPurchaseButton;
        //public Button playerSellSeaweed;
        //public Button playerSellCigar;


        int transactionAmount;

        List<string> buyCart;
        List<string> sellCart;

        int ringfencedJulesPlayer;
        int ringfencedJulesTrader;

        protected InventoryController PlayerInventory;
        protected InventoryController TraderInventory;
        public GameObject oxygenSupply;
        public Pressure pressure;
        

        public GameObject traderShip;
        public GameObject transactionJulesIcon;
        public Text transactionAmountDisplay;
        public GameObject cigarMachine;

        public Text traderJulesDisplay;
        public Text traderNumOxyUpgradesDisplay;
        public Text traderNumPresUpgradesDisplay;
        public Text traderNumCigarMachineDisplay;
        public Text traderNumCigarsDisplay;
        public Text traderNumSeaweedDisplay;

        // Use this for initialization
        void Awake()
        {
            //initialise those inventories
            PlayerInventory = PlayerCharacter.PlayerInstance.GetComponent<InventoryController>();
            TraderInventory = traderShip.GetComponent<InventoryController>();

            //let's initialise our buttons and add listeners to them and point them to the right methods
            Button confirm = confirmPurchaseButton.GetComponent<Button>();
            GameObject[] buyButtons = GameObject.FindGameObjectsWithTag("buyButton");
            foreach (GameObject button in buyButtons)
            {
                button.GetComponent<Button>().onClick.AddListener(() => updateBuycart(button.GetComponent<UpgradeButtons>().upgradeType));
            }

            GameObject[] sellButtons = GameObject.FindGameObjectsWithTag("sellButton");
            foreach (GameObject button in sellButtons)
            {
                button.GetComponent<Button>().onClick.AddListener(() => updateSellcart(button.GetComponent<SellGoodsButton>().goodName));
            }

             confirm.onClick.AddListener( confirmTransaction );

            //let's stock up the shop with some things
            for ( int i = 4; i > 0; i-- ) { TraderInventory.AddItem( "oxygenUpgrade" ); }
            for ( int i = 4; i > 0; i-- ) { TraderInventory.AddItem( "pressureUpgrade" ); }
            for (int i = 4; i > 0; i--) { TraderInventory.AddItem("cargoCapUpgrade"); }
            for ( int i = 30; i > 0; i-- ) { TraderInventory.AddItem( "Jules" ); }
            TraderInventory.AddItem("cigarMachine");

        }

        void Start()
        {
            //initialise fresh variables when you open the shop
            transactionAmount = 0;
            buyCart = new List<string>();
            sellCart = new List<string>();
            ringfencedJulesPlayer = 0;
            ringfencedJulesTrader = 0;
        }

        void Update()
        {
            //display how much Jules you'll pay/recieve if you confirm the transaction
            transactionAmountDisplay.text = ( transactionAmount != 0 ) ? transactionAmount.ToString() : "";
            transactionJulesIcon.GetComponent<Image>().enabled = ( transactionAmount != 0 );

            //Keep the trader's inventory display updated. 
            traderJulesDisplay.text = TraderInventory.GetNumberOfItem( "Jules" ).ToString();
            traderNumOxyUpgradesDisplay.text = TraderInventory.GetNumberOfItem( "oxygenUpgrade" ).ToString();
            traderNumPresUpgradesDisplay.text = TraderInventory.GetNumberOfItem( "pressureUpgrade" ).ToString();
            traderNumCigarsDisplay.text = TraderInventory.GetNumberOfItem( "cigar" ).ToString();
            traderNumSeaweedDisplay.text = TraderInventory.GetNumberOfItem( "seaweed" ).ToString();
            traderNumCigarMachineDisplay.text = TraderInventory.GetNumberOfItem("cigarMachine").ToString();
            confirmPurchaseButton.interactable = ((buyCart.Count > 0) ||(sellCart.Count > 0));
        }

        //Debug cheat to check both carts contents
        [ContextMenu( "CheckCart" )]
        void CheckCart()
        {
            Debug.Log( "Buy cart contains: " );
            foreach ( string item in buyCart )
            {
                Debug.Log( item );
            }
            Debug.Log( "Sell cart contains: " );
            foreach ( string item in sellCart )
            {
                Debug.Log( item );
            }
        }

        //When you click something in the shop it'll go in the cart, and the Jules it would cost are ringfenced, so that you can't buy more than the shop has in supply. The transaction amount will be updated.
        void updateBuycart( string m_item )
        {
            buyCart.Add( m_item );
            transactionAmount -= GetComponent<ProductPrices>().lookUpPrice( m_item );  //note this is '-=' so it looks up the amount then subtracts it from the transaction amount, so it's negative it you're buying.  
            ringfence( GetComponent<ProductPrices>().lookUpPrice( m_item ), true, m_item );
        }

        //likewise with selling
        void updateSellcart( string m_item )
        {
            sellCart.Add( m_item );
            transactionAmount += GetComponent<ProductPrices>().lookUpPrice( m_item );
            ringfence( GetComponent<ProductPrices>().lookUpPrice( m_item ), false, m_item );
        }

        void ringfence( int rf_jules, bool isBuying, string m_item )  //take away these Jules and items from the player/trader temporaily to ringfence the funds (so that you can't buy things you can't afford, or sell things the trader can't afford)
        {                                                              //takes the price and the a bool for is it's a buy  (if it's false then it's a sell) and the item you're buying/selling.
            if ( isBuying == true )
            {
                for ( int i = 0; i < rf_jules; i++ )
                {
                    PlayerInventory.RemoveItem( "Jules" );
                    TraderInventory.AddItem( "Jules" );
                }
                TraderInventory.RemoveItem( m_item );
                ringfencedJulesPlayer += rf_jules;

            }
            else
            {
                for ( int i = 0; i < rf_jules; i++ )
                {
                    TraderInventory.RemoveItem( "Jules" );
                    PlayerInventory.AddItem( "Jules" );
                }
                PlayerInventory.RemoveItem( m_item );
                ringfencedJulesTrader += rf_jules;
            }

        }


        void unRingfence()  //give the player/trader back their money, they didn't buy/sell anything!
        {
            for ( int i = 0; i < ringfencedJulesPlayer; i++ )
            {
                PlayerInventory.AddItem( "Jules" );
                TraderInventory.RemoveItem( "Jules" );
            }
            ringfencedJulesPlayer = 0;

            for ( int i = 0; i < ringfencedJulesTrader; i++ )
            {
                TraderInventory.AddItem( "Jules" );
                PlayerInventory.RemoveItem( "Jules" );
            }
            ringfencedJulesTrader = 0;
        }


        void confirmTransaction()  //when you click confirm
        {


            foreach ( string item in buyCart ) // find every item in the buycart and give it to the player (it was ringfenced already so don't remove from trader's inventory again)
            {
                PlayerInventory.AddItem( item );
            }

            if (buyCart.Contains("oxygenUpgrade")) {oxygenSupply.GetComponent<OxygenCapacity>().updateOxygenCapacity();  }
            if (buyCart.Contains("pressureUpgrade")) { pressure.GetComponent<Pressure>().upgradePressureResistance();  }
            if (buyCart.Contains("cigarMachine")) { cigarMachine.SetActive(true); }

            foreach ( string item in sellCart ) // find every item in the sellcart and give it to the trader (it was ringfenced already so don't remove from player's inventory again)
            {
                TraderInventory.AddItem( item );
            }
            ResetAllTransactionData();
        }

        public void cancelTrade() //this gets called from the inspector when you click the close shop button. Empty carts back into inventories if you haven't confirmed.
        {
            unRingfence();
            foreach ( string item in buyCart )
            {
                TraderInventory.AddItem( item );
            }


            foreach ( string item in sellCart )
            {
                PlayerInventory.AddItem( item );
            }

            ResetAllTransactionData();
        }



        public void ResetAllTransactionData() //set everything back to nothing 
        {
            transactionAmount = 0;
            buyCart.Clear();
            sellCart.Clear();

        }



    }




}