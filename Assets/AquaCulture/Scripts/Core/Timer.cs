using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AquaCulture
{
    public class Timer : MonoBehaviour, IDataPersister
    {
        public float InitialTime;
        public float InterpolationMax = 1.0f;
        public float InterpolationMin = 0.0f;
        [HideInInspector]
        public float TimerTime;

        public DataSettings dataSettings;

        [Serializable]
        public class UpdatingOtherEvent : UnityEvent<float>
        { }

        public UpdatingOtherEvent UpdateableEvent;
        public UpdatingOtherEvent UpdateableEventInvertedTime;
        public UpdatingOtherEvent UpdateableEventInterpolatedValue;
        public UnityEvent OnTimeReached;

        // Awake is called when the script instance is being loaded.
        void Awake()
        {
            PersistentDataManager.RegisterPersister(this);
            TimerTime = Mathf.Infinity;
        }

        void Reset()
        {
            dataSettings = new DataSettings();
            TimerTime = Mathf.Infinity;
        }

        public void RestartTimer()
        {
            TimerTime = InitialTime;
        }

        void Update()
        {
            if ( TimerTime <= InitialTime )
            {
                if( TimerTime < 0.0f )
                {
                    OnTimeReached.Invoke();
                    TimerTime = Mathf.Infinity;
                }
                else
                {
                    TimerTime -= Time.deltaTime;

                    // Internally we intend to have TimerTime become negative by a small amount.
                    // This shouldn't be the case for the external facing time.
                    float ExternalTime = Mathf.Max( TimerTime, 0.0f );
                    UpdateableEvent.Invoke( ExternalTime );
                    UpdateableEventInvertedTime.Invoke( InitialTime - ExternalTime );
                    UpdateableEventInterpolatedValue.Invoke( Mathf.Lerp( InterpolationMin, InterpolationMax, ExternalTime / InitialTime ) );
                }
            }
        }

        public void Save()
        {
            PersistentDataManager.SetDirty(this);
        }

        public DataSettings GetDataSettings()
        {
            return dataSettings;
        }

        public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
        {
            dataSettings.dataTag = dataTag;
            dataSettings.persistenceType = persistenceType;
        }

        public Data SaveData()
        {
            return new Data<float>( TimerTime );
        }

        public void LoadData( Data data )
        {
            Data<float> Data = (Data<float>)data;
            TimerTime = Data.value;
        }
    }
}
