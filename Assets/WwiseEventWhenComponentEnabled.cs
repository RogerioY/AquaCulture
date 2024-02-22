using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseEventWhenComponentEnabled : MonoBehaviour {
    bool allowToTrigger;
    // Use this for initialization
    void start()
    {
        allowToTrigger = true;

    }
	// Update is called once per frame
	void Update () {
		if ((allowToTrigger == true) && (GetComponent<SpriteRenderer>().enabled == true))
        {
            playMyWwiseEvent();
        }
        if ((GetComponent<SpriteRenderer>().enabled == false))
        {
            allowToTrigger = true;
        }
	}

void playMyWwiseEvent()
{
        Debug.Log("Wwise Evgent should Trigger");
        allowToTrigger = false;
       // AkSoundEngine.PostEvent("Sub_Launch",gameObject);

    }


}
