using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquaCulture
{
    public class CargoCaps : MonoBehaviour
    {
        public GameObject dialogueGraph;
     //   DialogueCanvasController dialogueCanvas;
       // GameObject[] taggedGameObjects;
       
        int[] cargoCaps;
        int numGoods;
        int playerCargoCap;

        // Use this for initialization

        void Start()
        {
            cargoCaps = new int[5] { 5, 10, 15, 20, 500 };
            updateCargoCap();

     //       taggedGameObjects = GameObject.FindGameObjectsWithTag( "DialogueCanvasParent" );
           // dialogueCanvas = taggedGameObjects[0].GetComponentInChildren<DialogueCanvasController>();
        }

        public void capCargo(string m_item)
        {
           InventoryController PlayerInventory = PlayerCharacter.PlayerInstance.GetComponent<InventoryController>();
            numGoods = ( PlayerInventory.GetNumberOfItem( "seaweed" ) + PlayerInventory.GetNumberOfItem( "cigar" ) );
            updateCargoCap();
            Debug.Log( "playerCargoCap = " + playerCargoCap );
            Debug.Log( "numGoods = " + numGoods );

            if ( numGoods > playerCargoCap )
            {
                PlayerInventory.RemoveItem( m_item );
                dialogueGraph.GetComponent<Animator>().Play("Cargo bay full");
           //     dialogueCanvas.ActivateCanvasWithText( "The cargo bay is full. I can't take anything else with me." );
             //   dialogueCanvas.DeactivateCanvasWithDelay( 2.0f );
            }

        }

        void updateCargoCap()
        {
            InventoryController PlayerInventory = PlayerCharacter.PlayerInstance.GetComponent<InventoryController>();
            switch ( PlayerInventory.GetNumberOfItem( "cargoCapUpgrade" ) )
            {
                case 0: playerCargoCap = 5; break;
                case 1: playerCargoCap = 10; break;
                case 2: playerCargoCap = 15; break;
                case 3: playerCargoCap = 20; break;
                case 4: playerCargoCap = 500; break;
            }
        }
    }
}