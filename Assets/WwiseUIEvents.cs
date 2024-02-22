using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class WwiseUIEvents : MonoBehaviour, IPointerEnterHandler
{
EventSystem eventTrigger;
public AK.Wwise.Event hoverEvent;
public AK.Wwise.Event clickEvent;
Button button;

void Start()
    {
        Button m_button = GetComponent<Button>();
        m_button.onClick.AddListener(playClickSound);
    }

//public void playHoverSound()
    
//    {
//        if ((GetComponent<Button>().interactable == true) && (hoverEvent != null)) { hoverEvent.Post(gameObject); }

//    }

    public void playClickSound()

    {
        if ((GetComponent<Button>().interactable == true) && (clickEvent != null)) { clickEvent.Post(gameObject); }

    }

    public void OnPointerEnter(PointerEventData data)
    {
        if ((GetComponent<Button>().interactable == true) && (hoverEvent != null)) { hoverEvent.Post(gameObject); }
    }
}

