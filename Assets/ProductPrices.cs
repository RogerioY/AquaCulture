using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductPrices : MonoBehaviour {
    Dictionary<string, int> priceCheck = new Dictionary<string, int>();
    int seaweedPrice;
    int cigarPrice;
    int oxygenUpgradePrice;
    int pressureUpgradePrice;
    int cigarMachinePrice;
    int cargoCapUpgradePrice;

    void Awake()
    {
        seaweedPrice = 2;
        cigarPrice = 10;
        oxygenUpgradePrice = 2;
        cargoCapUpgradePrice = 20;
        pressureUpgradePrice = 6;
        cigarMachinePrice = 15;
 

        priceCheck.Add("seaweed", seaweedPrice);
        priceCheck.Add("cigar", cigarPrice);
        priceCheck.Add("oxygenUpgrade", oxygenUpgradePrice);
        priceCheck.Add("pressureUpgrade", pressureUpgradePrice);
        priceCheck.Add("cigarMachine",cigarMachinePrice);
        priceCheck.Add("cargoCapUpgrade", cargoCapUpgradePrice);
    }
	
    public int lookUpPrice(string m_item = null)
    {
       int temp=0;
        if (m_item != null)
        {
            if (priceCheck.TryGetValue(m_item, out temp)) {return temp;}
            else {return 0;}
        }
        else {return 0;}
    }
}


