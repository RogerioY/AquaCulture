using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClickerScript : MonoBehaviour
//Sets referenced objects active when you click on the gameObject object this script it attached to. Can activate up to 2 objects, but could easily be extentded if 
{
    public GameObject objectToActivate1;
    public GameObject objectToActivate2;


    void OnMouseDown()
    {
        if ((Input.GetMouseButtonDown(0)) && (objectToActivate1.activeSelf == false) && (objectToActivate1 !=null))
        {
            objectToActivate1.SetActive(true);

        }

        if ((Input.GetMouseButtonDown(0)) && (objectToActivate2.activeSelf == false) && (objectToActivate2 != null))
        {
            objectToActivate2.SetActive(true);

        }

    }
}