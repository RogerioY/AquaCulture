using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AquaCulture
{
    public class ReplenishOxygenSupply : MonoBehaviour
    {
        public Text buttonText;
        // Use this for initialization
        void Start()
        {
            Button replenishOxygen = gameObject.GetComponent<Button>();
            replenishOxygen.onClick.AddListener( advanceDay );
            gameObject.GetComponent<Button>().interactable = false;
            buttonText.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void advanceDay()
        {
            StartCoroutine( fadeOutFadeIn( 1.0f ) ); //fade screen in and out

        }

        IEnumerator fadeOutFadeIn( float seconds )
        {
            StartCoroutine( ScreenFader.FadeSceneOut( ScreenFader.FadeType.Black ) );
            AkSoundEngine.PostEvent( "ScreenFade", null );
            yield return new WaitForSeconds( 1.0f );
            StartCoroutine( ScreenFader.FadeSceneIn() );
        }

        public void HasReachedSurface()
        {
            gameObject.GetComponent<Button>().interactable = true;
            buttonText.enabled = true;

        }

        public void HasDived()
        {
            gameObject.GetComponent<Button>().interactable = false;
            buttonText.enabled = false;
        }

    }
}