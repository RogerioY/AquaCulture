using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AquaCulture
{
    public class Spawner : MonoBehaviour
    {
        public DataSettings dataSettings;

        public Vector2 SpawnOffset = new Vector2( 0.0f, 0.0f );

        public UnityEvent OnSpawn;

        public void SpawnObject( GameObject other )
        {
            GameObject obj = Instantiate( other );
            obj.transform.SetPositionAndRotation( transform.position + new Vector3( SpawnOffset.x, SpawnOffset.y ), transform.rotation );
            obj.transform.parent = transform.parent;
            if ( OnSpawn.GetPersistentEventCount() > 0 )
            {
                OnSpawn.Invoke();
            }
        }
    }
}
