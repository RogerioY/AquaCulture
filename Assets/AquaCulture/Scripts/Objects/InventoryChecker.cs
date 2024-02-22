using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AquaCulture
{
    public class InventoryChecker : MonoBehaviour, IDataPersister
    {
        public string[] requiredInventoryItemKeys;
        public Sprite[] unlockStateSprites;

        public DirectorTrigger keyDirectorTrigger;
        public InventoryController characterInventory;
        public UnityEvent onUnlocked;
        public DataSettings dataSettings;

        SpriteRenderer m_SpriteRenderer;

        [ContextMenu("Update State")]
        void CheckInventory()
        {
            if ( characterInventory.HasAllItemsUniquely( requiredInventoryItemKeys ) )
            {
                if ( keyDirectorTrigger != null )
                {
                    keyDirectorTrigger.OverrideAlreadyTriggered( true );
                }

                m_SpriteRenderer.sprite = unlockStateSprites[0];
                onUnlocked.Invoke();
            }
        }

        void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            characterInventory.OnInventoryLoaded += CheckInventory;
        }

        void Update ()
        {
            CheckInventory ();
        }

        public DataSettings GetDataSettings()
        {
            return dataSettings;
        }

        public void LoadData(Data data)
        {
            var d = data as Data<Sprite>;
            m_SpriteRenderer.sprite = d.value;
        }

        public Data SaveData()
        {
            return new Data<Sprite>(m_SpriteRenderer.sprite);
        }

        public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
        {
            dataSettings.dataTag = dataTag;
            dataSettings.persistenceType = persistenceType;
        }


    }
}