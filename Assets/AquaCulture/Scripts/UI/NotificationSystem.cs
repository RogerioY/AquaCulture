using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AquaCulture
{
    [RequireComponent( typeof( RectTransform ) )]
    public class NotificationSystem : MonoBehaviour
    {
        public static NotificationSystem Instance { get; protected set; }

        public GameObject NotificationPrefab;
        
        protected Queue PendingNotifications;
        protected List<GameObject> CurrentlyDisplayedNotifications;

        protected float InternalPeriodTimer;

        static readonly int m_HashActivePara = Animator.StringToHash( "Active" );
        static readonly int m_HashScrollInPara = Animator.StringToHash( "ScrollIn" );
        static readonly int m_HashFadeAwayPara = Animator.StringToHash( "FadeAway" );
        protected const int m_NumAllowedNotifications = 5;
        protected const float k_NotificationAnchorHeight = 1.0f / m_NumAllowedNotifications;
        protected const float k_NotificationDisappearTime = 0.041f;
        protected const float k_NotificationScrollTime = 1.0f;

        private void Awake()
        {
            Instance = this;
            PendingNotifications = new Queue();
            CurrentlyDisplayedNotifications = new List<GameObject>();
            InternalPeriodTimer = k_NotificationScrollTime;
        }

        private void Start()
        {
        }

        private void Update()
        {
            InternalPeriodTimer -= Time.deltaTime;

            if ( InternalPeriodTimer < 0.0f )
            {
                InternalPeriodTimer = k_NotificationScrollTime;

                if( PendingNotifications.Count > 0 )
                {
                    AddNewTopmostNotification();
                    ShiftNotificationsDown();
                }

                if ( m_NumAllowedNotifications == CurrentlyDisplayedNotifications.Count )
                {
                    RemoveEldestNotification();
                }

                if ( m_NumAllowedNotifications == CurrentlyDisplayedNotifications.Count - 1 )
                {
                    FadeEldestNotification();
                }
            }
        }

        private void AddNewTopmostNotification()
        {
            if ( PendingNotifications.Count == 0 )
            {
                return;
            }

            GameObject Notification = Instantiate( NotificationPrefab );
            Notification.transform.SetParent( transform );
            RectTransform NotificationRect = Notification.transform as RectTransform;
            NotificationRect.anchoredPosition = new Vector2( 0.0f, 0.0f );
            NotificationRect.anchorMin = new Vector2( 0.5f, 1.0f );
            NotificationRect.anchorMax = new Vector2( 0.5f, 1.0f );
            NotificationRect.sizeDelta = new Vector2( NotificationRect.sizeDelta.x, k_NotificationAnchorHeight * ((RectTransform)transform).sizeDelta.y );

            TMPro.TextMeshProUGUI TextComponent = Notification.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            TextComponent.SetText( (string)PendingNotifications.Dequeue() );
            TextComponent.rectTransform.sizeDelta = NotificationRect.sizeDelta;


            CurrentlyDisplayedNotifications.Add( Notification );
        }

        public void ShiftNotificationsDown()
        {
            if( CurrentlyDisplayedNotifications.Count == 0 )
            {
                return;
            }

            for ( int i = 0; i < CurrentlyDisplayedNotifications.Count; i++ )
            {
                RectTransform NotificationRect = CurrentlyDisplayedNotifications[i].transform as RectTransform;
                NotificationRect.anchorMin -= new Vector2( 0.0f, k_NotificationAnchorHeight );
                NotificationRect.anchorMax -= new Vector2( 0.0f, k_NotificationAnchorHeight );
                NotificationRect.GetComponent<Animator>().SetBool( m_HashScrollInPara, true );
            }
        }

        public void FadeEldestNotification()
        {
            if ( CurrentlyDisplayedNotifications.Count == 0 )
            {
                return;
            }

            int FinalNotificationIndex = CurrentlyDisplayedNotifications.Count - 1;
            GameObject FinalNotification = CurrentlyDisplayedNotifications[FinalNotificationIndex];
            FinalNotification.GetComponent<Animator>().SetBool( m_HashFadeAwayPara, true );
        }

        public void RemoveEldestNotification()
        {
            if ( CurrentlyDisplayedNotifications.Count == 0 )
            {
                return;
            }

            int EldestNotificationIndex = 0;
            GameObject FinalNotification = CurrentlyDisplayedNotifications[EldestNotificationIndex];
            Destroy( FinalNotification );
            CurrentlyDisplayedNotifications.RemoveAt( EldestNotificationIndex );
        }

        private bool CurrentlyShownNotificationsIsFull() { return m_NumAllowedNotifications == CurrentlyDisplayedNotifications.Count; }

        public void AddUINotification( string Key )
        {
            PendingNotifications.Enqueue( "Picked up " + Key );

            if( m_NumAllowedNotifications > CurrentlyDisplayedNotifications.Count )
            {
                AddNewTopmostNotification();
                ShiftNotificationsDown();
            }
        }
    }
}