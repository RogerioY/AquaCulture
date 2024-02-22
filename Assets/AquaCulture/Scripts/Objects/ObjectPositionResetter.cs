using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquaCulture
{
    public class ObjectPositionResetter : MonoBehaviour
    {
        public GameObject resettingGameObject;

        public void ResetPosition ()
        {
            GameObjectTeleporter.Teleport (resettingGameObject, transform);
        }
    }
}