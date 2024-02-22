using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AquaCulture
{
    public class PlayerInventoryUI : MonoBehaviour
    {
        public Text numPlayerCigarDisplay;
        public Text numPlayerSeaweedDisplay;
        public Text julesDisplay;
        public Text oxyUpgradesDisplay;
        public Text presUpgradesDisplay;
        public Button playerCigarButton;
        public Button playerSeaweedButton;
        public bool depositingCargo = false;
        bool isActive = true;
        int jules;
        int numOxyUpgrades;
        int numPresUpgrades;

        protected InventoryController PlayerInventory;

        // Use this for initialization
        void Start()
        {
            PlayerInventory = PlayerCharacter.PlayerInstance.GetComponent<InventoryController>();

        }

        // Update is called once per frame
        void Update()
        {
            int numPlayerSeaweed;
            int numPlayerCigar;

            if ( PlayerInventory != null )
            {
                numPlayerSeaweed = PlayerInventory.GetNumberOfItem( "seaweed" );
                numPlayerCigar = PlayerInventory.GetNumberOfItem( "cigar" );
                numOxyUpgrades = PlayerInventory.GetNumberOfItem("oxygenUpgrade");
                numPresUpgrades = PlayerInventory.GetNumberOfItem("pressureUpgrade");
                jules = PlayerInventory.GetNumberOfItem("Jules");

                numPlayerSeaweedDisplay.text = numPlayerSeaweed.ToString();
                numPlayerCigarDisplay.text = numPlayerCigar.ToString();
                julesDisplay.text = jules.ToString();
                oxyUpgradesDisplay.text = numOxyUpgrades.ToString();
                presUpgradesDisplay.text = numPresUpgrades.ToString();

                //playerCigarButton.GetComponent<Button>().interactable = ( numPlayerCigar > 0 ); 
                //playerSeaweedButton.GetComponent<Button>().interactable = ( numPlayerSeaweed > 0 ); 
            }

            if ( PlayerInput.Instance.Inventory.Down )
            {
                // isActive = !isActive;
                Debug.Log( "I pressed" );
            }
        }
    }
}