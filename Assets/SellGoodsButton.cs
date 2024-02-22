using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AquaCulture
{
    public class SellGoodsButton : MonoBehaviour
    {
        protected InventoryController PlayerInventory;
        protected InventoryController TraderInventory;
        public GameObject traderShip;
        Button m_button;
        public string goodName;
        int m_price;


        // Use this for initialization
        void Start()
        {
            PlayerInventory = PlayerCharacter.PlayerInstance.GetComponent<InventoryController>();
            TraderInventory = traderShip.GetComponent<InventoryController>();
            m_button = GetComponent<Button>();
            m_price = GetComponentInParent<ProductPrices>().lookUpPrice( goodName );
        }

        // Update is called once per frame
        void Update()
        {
            int traderJules = TraderInventory.GetNumberOfItem( "Jules" );
            int goodSupply = PlayerInventory.GetNumberOfItem( goodName );
            m_button.interactable = ( ( traderJules >= m_price ) && ( goodSupply > 0 ) ); //you can't sell if the trader doesn't have enough money, or if you don't have anything to sell.
        }
    }
}