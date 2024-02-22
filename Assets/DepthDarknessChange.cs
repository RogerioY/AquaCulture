using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepthDarknessChange : MonoBehaviour {
    public GameObject darkUnderlay;
    public GameObject player;
    float spriteAlpha;
    public float DepthThresholdShallow;
    public float DepthThresholdDeep;
    float lastDepth;
    float blendTime;
    public float blendMulti;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        Color currentColour = darkUnderlay.GetComponent<SpriteRenderer>().color;
        currentColour.a = spriteAlpha;
        darkUnderlay.GetComponent<SpriteRenderer>().color = currentColour;
        lastDepth = player.transform.position.y;


        //if ((player.transform.position.y == DepthThresholdShallow) && (lastDepth > player.transform.position.y))
        //{
        //    //colourTransShallowtoMid();
        //}
        if ((player.transform.position.y < DepthThresholdShallow) && (player.transform.position.y > DepthThresholdDeep))
        {
            float fBufferZoneDistance =  DepthThresholdShallow - DepthThresholdDeep;
            float fDifference = - (player.transform.position.y - DepthThresholdShallow);
            float fInterpolant = Mathf.Clamp(fDifference / fBufferZoneDistance, 0.0f, 1.0f);
            Debug.Log("Bufferzone" + fBufferZoneDistance.ToString());
            Debug.Log("fDifference" + fDifference.ToString());
            Debug.Log("fInterpolant" + fInterpolant.ToString());
            spriteAlpha = Mathf.Lerp(0.0f, 1f, fInterpolant);
        }
        //else if (player.transform.position.y < DepthThresholdDeep)
        //{
        //    spriteAlpha = 1;
        //}
      }

    }


