using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquaCulture
{
    public class SubmarineActivator : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter2D( Collision2D col )
        {
            Debug.Log( "press Y to launch submarine!" );
        }
    }
}