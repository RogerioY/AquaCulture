using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AquaCulture
{
    public class UpgradePriceDisplay : MonoBehaviour
    {
        ProductPrices priceList;
        string upgradeType;
        int price;


        // Use this for initialization
        void Start()
        {
            priceList = GetComponentInParent<ProductPrices>();
            upgradeType = GetComponentInParent<UpgradeButtons>().upgradeType;
            price = priceList.lookUpPrice( upgradeType );
            GetComponent<Text>().text = ( "" + price );

        }
    }
}