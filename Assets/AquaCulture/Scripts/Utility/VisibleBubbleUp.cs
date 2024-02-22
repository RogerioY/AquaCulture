using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquaCulture
{
    //Allow to bubble up a OnBecameVisible call. usefull to have parent object (without renderer)
    //be notified when a child become visible

    // YJ: This is quite cool. A parent gameobject with a specific method on a script component on it 
    // originally traversed all the child renderers, found the gameobjects on those, then added this 
    // script as a component. It then set up this delegate (C# version of a callback) objectBecameVisible 
    // as that specific method. OnBecameVisible is called when a camera can see this gameobject, and
    // so this calls the delegate for the object higher in the heirarchy.
    public class VisibleBubbleUp : MonoBehaviour
    {
        public System.Action<VisibleBubbleUp> objectBecameVisible;

        private void OnBecameVisible()
        {
            objectBecameVisible(this);
        }
    }
}