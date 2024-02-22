using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AquaCulture;

public class PlayerHealthDisplay : MonoBehaviour {
    protected Damageable playerDamagable;
    public Text healthDisplayText;

	// Use this for initialization
	void Start () {
        playerDamagable = PlayerCharacter.PlayerInstance.GetComponent<Damageable>();
	}
	
	// Update is called once per frame
	void Update () {
        healthDisplayText.text = playerDamagable.CurrentHealth.ToString();
    }
}
