using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace AquaCulture
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PrewarmDirector : MonoBehaviour
    {

        void OnEnable()
        {
            GetComponent<PlayableDirector>().RebuildGraph();

        }

    }
}