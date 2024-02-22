using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AquaCulture;

public class PlantGrowthControllerVer2 : MonoBehaviour {
    bool isFullyGrown = false;
    Animator plantGrowthAnim;
    Damageable damageableComponant;
	// Use this for initialization
	void Start () {
        plantGrowthAnim = GetComponent<Animator>();
        plantGrowthAnim.enabled = true;
        damageableComponant = GetComponent<Damageable>();
    }
	
	// Update is called once per frame
	void Update () {

        damageableComponant.enabled = isFullyGrown;
    }

    public void fullyGrown()
    {
        isFullyGrown = true;
        plantGrowthAnim.enabled=false;
        Debug.Log("plant fully grown");
    }

    public void harvestPlant()
    {
        isFullyGrown = false;
        plantGrowthAnim.enabled = true;
    }
}
