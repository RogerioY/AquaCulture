using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using AquaCulture;

public class ItemPlacementController : MonoBehaviour
{
    LayerMask mask;
    Collider2D placableObjectCollider;
    bool allowedToPlace = false;
    // Use this for initialization
    void Start()
    {
        mask = LayerMask.GetMask("Usable");
        placableObjectCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    //    Debug.Log("Touching useable object = " + placableObjectCollider.IsTouchingLayers(mask).ToString());
    }

  void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("collided with " + col);
    }
}
