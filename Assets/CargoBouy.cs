using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AquaCulture
{
    public class CargoBouy : MonoBehaviour
    {
        public int numSeaweed = 0;
        public int numCigar = 0;
        public Text Seaweed;
        public Text Cigar;
        public Button playerCigar;
        public GameObject playerInv;
        AudioSource menuSound;
        public AudioClip clickSound;
        public AudioClip beepSound;
        public AudioClip releaseBouy;
        public Button releaseCargo;
        public Text cashSaleDisplay;
        public Text playerMessage;
        public Button playerSeaweed;
        int saleCash;

        protected InventoryController PlayerInventory;

        // Use this for initialization
        void Start()
        {
            PlayerInventory = PlayerCharacter.PlayerInstance.GetComponent<InventoryController>();
            menuSound = GetComponent<AudioSource>();
            Button depositSeaweed = playerSeaweed.GetComponent<Button>();
            Button depositCigar = playerCigar.GetComponent<Button>();
            Button release = releaseCargo.GetComponent<Button>();
            depositSeaweed.onClick.AddListener( () => DepositClick( 1 ) );
            depositCigar.onClick.AddListener( () => DepositClick( 2 ) );
            release.onClick.AddListener( ReleaseClick );
            cashSaleDisplay.text = "";
            playerMessage.text = "";
        }

        // Update is called once per frame
        void Update()
        {
            Seaweed.text = numSeaweed.ToString();
            Cigar.text = numCigar.ToString();
        }

        void DepositClick( int itemName )
        {


            switch ( itemName )
            {
                case 1: //Seaweed clicked
                    numSeaweed++;
                    PlayerInventory.RemoveItem( "Seaweed" );
                    menuSound.PlayOneShot( clickSound );
                    saleCash += GetComponent<ProductPrices>().lookUpPrice("seaweed");
                    break;

                case 2: //Cigar clicked

                    numCigar++;
                    PlayerInventory.RemoveItem( "Cigar" );
                    menuSound.PlayOneShot( clickSound );
                    saleCash += GetComponent<ProductPrices>().lookUpPrice("cigar");
                    break;
            }
        }


        void ReleaseClick()
        {
            if ( saleCash > 0 )
            {
                // moneyDisplay.GetComponent<Money>().Jules += saleCash;
                for (int x=0; x<saleCash; x++)
                {
                    PlayerInventory.AddItem("Jules");
                }
                numCigar = 0;
                numSeaweed = 0;
                menuSound.PlayOneShot( releaseBouy );
                StartCoroutine( DisplaySaleCash( 2.0f ) );
            }
            else
            {
                StartCoroutine( DisplayReleaseError( 2.0f ) );
            }

        }

        IEnumerator DisplaySaleCash( float seconds )
        {
            cashSaleDisplay.text = "+" + saleCash;
            yield return new WaitForSeconds( seconds );
            cashSaleDisplay.text = "";
        }

        IEnumerator DisplayReleaseError( float seconds )
        {
            menuSound.PlayOneShot( clickSound );
            playerMessage.text = "There's no cargo in the stash to sell!";
            yield return new WaitForSeconds( seconds );
            playerMessage.text = "";
        }

        void OnEnable()
        {
            playerInv.GetComponent<PlayerInventoryUI>().depositingCargo = true;
        }

        void OnDisable()
        {
            playerInv.GetComponent<PlayerInventoryUI>().depositingCargo = false;
        }



    }

}