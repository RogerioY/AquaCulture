using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wwise_Anim_Event : MonoBehaviour {

public void PostWwiseEventFromAnim(string wwiseEvent)
    {
        if (wwiseEvent != null) { AkSoundEngine.PostEvent(wwiseEvent, gameObject); }
    }
}
