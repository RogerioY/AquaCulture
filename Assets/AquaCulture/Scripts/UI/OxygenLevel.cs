using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AquaCulture
{
    public class OxygenLevel : MonoBehaviour
    {
        public GameObject oxygenNeedle;
        public float oxyDialRotation;
        float m_oxygenDepletionMulti;
        public GameObject oxygenColourRing;
        bool blinking = false;
        protected InventoryController PlayerInventory;


        // Made this readonly as we don't expect it to change while this script executes.
        readonly int numBlinks = 8;

        // Use this for initialization
        void Start()
        {
            oxyDialRotation = 140;
            oxygenColourRing.SetActive( false );
            PlayerInventory = PlayerCharacter.PlayerInstance.GetComponent<InventoryController>();
        }

        // Allows us to externally set the rotation on the dial
        public void SetDialRotation( float Degrees )
        {
            oxyDialRotation = Degrees;
        }

        // Update is called once per frame
        void Update()
        {
            oxygenNeedle.GetComponent<RectTransform>().rotation = Quaternion.Euler( new Vector3( 0.0f, 0.0f, -oxyDialRotation ) );
            if ( oxyDialRotation > -140 )
            {
                oxyDialRotation -= Time.deltaTime * m_oxygenDepletionMulti;
            }
            if ( ( oxyDialRotation < -90 ) && ( oxyDialRotation > -100 ) && ( blinking == false ) )
            {
                StartCoroutine( Blink( 0.3f ) );
                blinking = true;
            }
        }

        IEnumerator Blink( float seconds )
        {
            for ( int i = 0; i < numBlinks; i++ )
            {
                // Toggle set active to be the opposite state of what it is now (so active = not active)
                oxygenColourRing.SetActive( !oxygenColourRing.activeSelf );
                yield return new WaitForSeconds( seconds );
            }
        }

    }
}