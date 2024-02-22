using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AquaCulture
{
    public class PlantGrowth : MonoBehaviour
    {
        public Sprite phase1;
        public Sprite phase2;
        public Sprite phase3;
        public Sprite phase4;
        public int growthPhase;
        public Button surfaceButton;

        void Start()
        {
            growthPhase = 1;
            GetComponent<SpriteRenderer>().sprite = phase1;
            Button advanceGrowthButton = surfaceButton.GetComponent<Button>();
            advanceGrowthButton.onClick.AddListener(advancePlantGrowth);

        }

        // Identical to the function on the player character
        //MAKE SURE TO CHANGE THIS ASHTON BECAUSE THE WAY YOU DID THIS BEFORE IS STUPID. You don't want to call the advancePlantGrowth when harvesting. 

        //public void OnHurt( Damager damager, Damageable damageable )
        //{
        //    if ( growthPhase == 4 )
        //    {
        //        advancePlantGrowth( true );
        //    }
        //}

        public void advancePlantGrowth()
        {
            Debug.Log( "growth advanced" );
            if (growthPhase < 5)
            {
                growthPhase++;
            }
            switch ( growthPhase )
            {

                case 1:
                    GetComponent<SpriteRenderer>().sprite = phase1;
                    break;
                case 2:
                    GetComponent<SpriteRenderer>().sprite = phase2;
                    break;
                case 3:
                    GetComponent<SpriteRenderer>().sprite = phase3;
                    break;
                case 4:
                    GetComponent<SpriteRenderer>().sprite = phase4;
                    break;

                default:
                    GetComponent<SpriteRenderer>().sprite = phase4;
                    break;
            }
            //if ( harvested == true )
            //{
            //    GetComponent<SpriteRenderer>().sprite = phase1;
            //    growthPhase = 1;
            //}



        }
    }
}