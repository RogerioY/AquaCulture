using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseSubMovement : MonoBehaviour
{
    public GameObject subDriveable;
    bool moveEventSwitch = true;
   // bool stopEventSwitch = false;
    bool subActiveEventSwitch = true;
    bool subInactiveEventSwitch = false;
    public GameObject buzztrack;

    // Use this for initialization
    void Start()
    {
        AkSoundEngine.SetSwitch("Buzztracks", "In_Sub", buzztrack);
    }

    // Update is called once per frame
    void Update()
    {
        if (subDriveable.GetComponent<SpriteRenderer>().enabled == true)
        {
    
            if ((((Input.GetKeyDown("w")) || (Input.GetKeyDown("s"))) && (moveEventSwitch == true)))
            {
                AkSoundEngine.PostEvent("Move_vertical", gameObject);
                AkSoundEngine.PostEvent("Play_Ballast_Bubbles", gameObject);
                //AkSoundEngine.PostEvent("Play_sub_move_off_vert", gameObject);
                //AkSoundEngine.SetRTPCValue("PropellorSpeed", 100.0f);
                moveEventSwitch = false;
               // stopEventSwitch = true;
            }

            if ((((Input.GetKeyUp("w")) || (Input.GetKeyUp("s"))) && (moveEventSwitch == false)))
            {
                moveEventSwitch = true;
                AkSoundEngine.PostEvent("Stop_Ballast_Bubbles", gameObject);
                //  AkSoundEngine.PostEvent("Stop_sub_move_vert_loop", gameObject);
                // AkSoundEngine.PostEvent("Play_sub_stop_vert", gameObject);
                // AkSoundEngine.SetRTPCValue("PropellorSpeed", 20.0f);
                // stopEventSwitch = false;

            }

            if (subActiveEventSwitch == true)
            {
                AkSoundEngine.PostEvent("Activate_sub", gameObject);
                AkSoundEngine.SetSwitch("Buzztracks", "In_water", buzztrack);
                // AkSoundEngine.SetRTPCValue("PropellorSpeed", 20.0f);
                subActiveEventSwitch = false;
                subInactiveEventSwitch = true;
            }

        }

        if ((subDriveable.GetComponent<SpriteRenderer>().enabled == false) && (subInactiveEventSwitch == true))
        {
            AkSoundEngine.PostEvent("Deactivate_sub", gameObject);
            AkSoundEngine.SetSwitch("Buzztracks", "In_sub", buzztrack);
            subInactiveEventSwitch = false;
            subActiveEventSwitch = true;
        }

        AkSoundEngine.SetRTPCValue("Depth", gameObject.transform.position.y);
        //if ((gameObject.transform.position.y > 23) && (gameObject.transform.position.y <= 23.2))
        //{
        //    AkSoundEngine.SetSwitch("Buzztracks", "Surface", buzztrack);
        //}


    }
}