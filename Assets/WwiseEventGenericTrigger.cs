using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseEventGenericTrigger : MonoBehaviour
{
    public AK.Wwise.Event m_WwiseEvent; //main Wwise event to post
    public AK.Wwise.Event m_WwiseEventSecondary; //another Wwise event that also gets posted if this is filled
    public GameObject otherGameObject;
    bool triggerActive = true;
    public float bufferTime;
   public void TriggerWwiseEvent()
    {
        GameObject wwiseObjectToPostOn;
        wwiseObjectToPostOn = (otherGameObject != null) ? otherGameObject : gameObject; //We have the option to post the Wwise event at another game object instead of this one. If we assign another gameobject in the inspector
                                                                                        //it will post it to that, if we don't it'll post it on this one. 
        if (triggerActive == true)
        {
            m_WwiseEvent.Post(wwiseObjectToPostOn); //post the main Wwise event

            if (m_WwiseEventSecondary != null)
            {
                m_WwiseEventSecondary.Post(wwiseObjectToPostOn);  //post the 2nd Wwise event if there is one assigned 
            } 
        }
        if (bufferTime > 0) { StartCoroutine(Buffer(bufferTime)); } // to stop nasty audio behaviour and excessive retriggering let's create a buffer period where the event cannot be triggered for the specified amount of time, igmore if no value given.
        Debug.Log("Wwise event " + m_WwiseEvent + " posted on " + wwiseObjectToPostOn); // let's print what happens to the debug log for posterity. 
         

    }
    IEnumerator Buffer (float bufferTime)
    {
        triggerActive = false;
        yield return new WaitForSeconds(bufferTime);
        triggerActive = true;
    }
}
