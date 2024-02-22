using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AquaCulture
{
    public class UpgradeButtons : MonoBehaviour
    {
        public string upgradeType;
        Button m_button;
        protected InventoryController PlayerInventory;
        protected InventoryController TraderInventory;
        public GameObject tradership;
        int m_price;
        // Use this for initialization
        void Start()
        {
            PlayerInventory = PlayerCharacter.PlayerInstance.GetComponent<InventoryController>();
            TraderInventory = tradership.GetComponent<InventoryController>();
            m_button = GetComponent<Button>();
            //   m_button.onClick.AddListener(() => dispenseUpgrade(upgradeType));
            m_price = GetComponentInParent<ProductPrices>().lookUpPrice( upgradeType );
        }
        void Update()
        {
            int playerJules = PlayerInventory.GetNumberOfItem( "Jules" );
            int upgradeSupply = TraderInventory.GetNumberOfItem( upgradeType );

            m_button.interactable = ( ( playerJules >= m_price ) && ( upgradeSupply > 0 ) ) ? true : false;

        }
    }

}