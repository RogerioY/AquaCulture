using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AquaCulture
{
    public class GateInvoker : MonoBehaviour
    {

        [System.Serializable]
        public class UnityFloatEvent : UnityEvent<float>
        { }

        public float InterpolationMax = 1.0f;
        public float InterpolationMin = 0.0f;

        public UnityFloatEvent AnyEventTakingFloat;
        public UnityEvent HasOpened;
        public UnityEvent HasClosed;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ProgressInvoke( float InputFloat )
        {
            AnyEventTakingFloat.Invoke( Mathf.Lerp( InterpolationMin, InterpolationMax, InputFloat ) );
        }

        public void HasOpenedInvoke()
        {
            HasOpened.Invoke();
        }

        public void HasClosedInvoke()
        {
            HasClosed.Invoke();
        }
    }
}