using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseSwitchListener : MonoBehaviour {
    public string switchGroup;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetWwiseSwitch(string WwiseSwitch)
    {
    if (WwiseSwitch != null)
        {
            AkSoundEngine.SetSwitch(switchGroup, WwiseSwitch, gameObject);
        }
    }
}
