using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AquaCulture
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovingPlatform : MonoBehaviour
    {
        public enum MovingPlatformType
        {
            BACK_FORTH,
            LOOP,
            ONCE
        }

        public PlatformCatcher platformCatcher;
        public float speed = 1.0f;
        [HideInInspector]
        public float m_Speed = 1.0f;
        public MovingPlatformType platformType;

        public bool startMovingOnlyWhenVisible;
        public bool isMovingAtStart = true;

        [HideInInspector]
        public Vector3[] localNodes = new Vector3[1];

        public float[] waitTimes = new float[1];

        public Vector3[] worldNode {  get { return m_WorldNode; } }

        protected Vector3[] m_WorldNode;
        protected Transform m_ParentAtGeneration;

        protected int m_Current = 0;
        protected int m_Next = 0;
        protected int m_Dir = 1;

        protected float m_WaitTime = -1.0f;

        protected Rigidbody2D m_Rigidbody2D;
        protected Vector2 m_Velocity;

        protected bool m_Started = false;
        protected bool m_VeryFirstStart = false;

        public Vector2 Velocity
        {
            get { return m_Velocity; }
        }

        public void SetSpeed( float NewSpeed )
        {
            m_Speed = NewSpeed;

            bool bChangingDirection = false;
            if( m_Dir > 0 )
            {
                if( NewSpeed < 0.0f )
                {
                    m_Dir = -1;
                    bChangingDirection = true;
                }
            }
            else
            {
                if ( NewSpeed > 0.0f )
                {
                    m_Dir = 1;
                    bChangingDirection = true;
                }
            }

            if ( bChangingDirection )
            {
                    // Swap current and next
                    int Temp = m_Current;
                    m_Current = m_Next;
                    m_Next = Temp;
            }

            if( !m_Started && Mathf.Abs( NewSpeed ) > 0.0f )
            {
                StartMoving();
            }
        }

        private void Reset()
        {
            //we always have at least a node which is the local position
            localNodes[0] = Vector3.zero;
            waitTimes[0] = 0;

            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Rigidbody2D.isKinematic = true;

            if (platformCatcher == null)
                platformCatcher = GetComponent<PlatformCatcher> ();
        }

        private void Start()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Rigidbody2D.isKinematic = true;

            if (platformCatcher == null)
                platformCatcher = GetComponent<PlatformCatcher>();

            //Allow to make platform only move when they became visible
            if ( startMovingOnlyWhenVisible )
            {
                Renderer[] renderers = GetComponentsInChildren<Renderer>();
                for ( int i = 0; i < renderers.Length; ++i )
                {
                    var b = renderers[i].gameObject.AddComponent<VisibleBubbleUp>();
                    b.objectBecameVisible = BecameVisible;
                }
            }

            //we make point in the path being defined in local space so game designer can move the platform & path together
            //but as the platform will move during gameplay, that would also move the node. So we convert the local nodes
            // (only used at edit time) to world position (only use at runtime)
            m_WorldNode = new Vector3[localNodes.Length];
            if( transform.parent != null )
            {
                // Set these up as parent-relative.
                // This will then handle runtime movements of
                // the ancestors this platform might have.
                // Note: More work is needed if we want to
                // have these continue to work if we have
                // dynamically changed parent.
                for (int i = 0; i < m_WorldNode.Length; ++i)
                {
                    m_WorldNode[i] = transform.parent.transform.InverseTransformPoint( transform.TransformPoint( localNodes[i] ) );
                }
                m_ParentAtGeneration = transform.parent;
            }
            else
            {
                for ( int i = 0; i < m_WorldNode.Length; ++i )
                    m_WorldNode[i] = transform.TransformPoint( localNodes[i] );
            }

            Init();
        }

        protected void Init()
        {
            m_Current = 0;
            m_Dir = 1;
            m_Next = localNodes.Length > 1 ? 1 : 0;

            m_WaitTime = waitTimes[0];

            m_VeryFirstStart = false;
            if (isMovingAtStart)
            {
                m_Started = !startMovingOnlyWhenVisible;
                SetSpeed( speed );
                m_VeryFirstStart = true;
            }
            else
                m_Started = false;

        }

        private void FixedUpdate()
        {
            if (!m_Started)
                return;

            //no need to update we have a single node in the path
            if (m_Current == m_Next)
                return;

            if(m_WaitTime > 0)
            {
                m_WaitTime -= Time.deltaTime;
                return;
            }

            float distanceToGo = Mathf.Abs( m_Speed ) * Time.deltaTime;

            while(distanceToGo > 0)
            {
                Vector3 PlatformPosition;
                if ( transform.parent != null )
                {
                    // Transform the position from world space into the local space of the parent
                    PlatformPosition = transform.parent.InverseTransformPoint( transform.position );
                }
                else
                {
                    // Alert the user if the wrong m_WorldNode values are about to be used.
                    // To trivially support this case we need to generate two sets of coordinates,
                    // or write this logic to be more robust.
                    Assert.IsTrue( m_ParentAtGeneration == null, "Class doesn't currently support dynamically changing platform parent. See comments." );
                    PlatformPosition = transform.position;
                }

                Vector2 direction = m_WorldNode[m_Next] - PlatformPosition;

                float dist = distanceToGo;
                if(direction.sqrMagnitude < dist * dist)
                {   //we have to go farther than our current goal point, so we set the distance to the remaining distance
                    //then we change the current & next indexes
                    dist = direction.magnitude;

                    m_Current = m_Next;

                    m_WaitTime = waitTimes[m_Current];

                    if (m_Dir > 0)
                    {
                        m_Next += 1;
                        if (m_Next >= m_WorldNode.Length)
                        { //we reach the end

                            switch(platformType)
                            {
                                case MovingPlatformType.BACK_FORTH:
                                    SetSpeed( -m_Speed );
                                    break;
                                case MovingPlatformType.LOOP:
                                    m_Next = 0;
                                    break;
                                case MovingPlatformType.ONCE:
                                    SetSpeed( -m_Speed );
                                    StopMoving();
                                    break;
                            }
                        }
                    }
                    else
                    {
                        m_Next -= 1;
                        if(m_Next < 0)
                        { //reached the beginning again

                            switch (platformType)
                            {
                                case MovingPlatformType.BACK_FORTH:
                                    SetSpeed( -m_Speed );
                                    break;
                                case MovingPlatformType.LOOP:
                                    m_Next = m_WorldNode.Length - 1;
                                    break;
                                case MovingPlatformType.ONCE:
                                    SetSpeed( -m_Speed );
                                    StopMoving();
                                    break;
                            }
                        }
                    }
                }

                if ( transform.parent != null )
                {
                    m_Velocity = transform.parent.TransformVector( direction.normalized ) * dist;
                }
                else
                {
                    m_Velocity = direction.normalized * dist;
                }

                //transform.position +=  direction.normalized * dist;
                m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + m_Velocity);
                platformCatcher.MoveCaughtObjects (m_Velocity);
                //We remove the distance we moved. That way if we didn't had enough distance to the next goal, we will do a new loop to finish
                //the remaining distance we have to cover this frame toward the new goal
                distanceToGo -= dist;

                // we have some wait time set, that mean we reach a point where we have to wait. So no need to continue to move the platform, early exit.
                if (m_WaitTime > 0.001f) 
                    break;
            }
        }

        public void StartMoving()
        {
            m_Started = true;

            StartCoroutine(sharkCageMoveSound(m_WaitTime));
        }

        IEnumerator sharkCageMoveSound(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            AkSoundEngine.PostEvent("Shark_Cage_Move", gameObject);
        }

        public void StopMoving()
        {
            m_Started = false;

            AkSoundEngine.PostEvent("Shark_Cage_Stop", gameObject);
        }

        public void ResetPlatform()
        {
            transform.position = m_WorldNode[0];
            Init();
        }

        private void BecameVisible(VisibleBubbleUp obj)
        {
            if (m_VeryFirstStart)
            {
                m_Started = true;
                m_VeryFirstStart = false;
            }
        }
    }
}