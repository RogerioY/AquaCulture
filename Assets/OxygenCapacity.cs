using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquaCulture
{
    public class OxygenCapacity : MonoBehaviour
    {
        public float[] OxygenTimeForNumberOfUpgrades = new float[5];
        protected InventoryController PlayerInventory;
        public Timer oxygenTimer;

        float oxygenCapacity;
        // Use this for initialization
        void Awake()
        {
            PlayerInventory = PlayerCharacter.PlayerInstance.GetComponent<InventoryController>();
            updateOxygenCapacity();
        }

        // Update is called once per frame
       public void updateOxygenCapacity()
        {
            switch ( PlayerInventory.GetNumberOfItem( "oxygenUpgrade" ) )
            {
                case 0: oxygenCapacity = OxygenTimeForNumberOfUpgrades[0]; break;
                case 1: oxygenCapacity = OxygenTimeForNumberOfUpgrades[1]; break;
                case 2: oxygenCapacity = OxygenTimeForNumberOfUpgrades[2]; break;
                case 3: oxygenCapacity = OxygenTimeForNumberOfUpgrades[3]; break;
                case 4: oxygenCapacity = OxygenTimeForNumberOfUpgrades[4]; break;
            }
            oxygenTimer.InitialTime = oxygenCapacity;

        }

    }
}