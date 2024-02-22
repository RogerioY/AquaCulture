using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AquaCulture
{
    public class Money : MonoBehaviour
    {
        public int Jules;
        public InventoryController PlayerInventory;
        // Use this for initialization
        //    void Start () {
        //        PlayerInventory = PlayerCharacter.PlayerInstance.GetComponent<InventoryController>();
        //    }

        //	// Update is called once per frame
        //	void Update () {
        //        Jules = PlayerInventory.GetNumberOfItem("Jules");
        //        GetComponent<Text>().text = "Jules: " + Jules;
        //	}
    }
}