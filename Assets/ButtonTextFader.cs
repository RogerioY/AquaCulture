using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTextFader : MonoBehaviour {
    Button m_button;

	// Use this for initialization
	void Start () {
        m_button = GetComponentInParent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Text>().color = (m_button.interactable == true) ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.2f);
    }
}
