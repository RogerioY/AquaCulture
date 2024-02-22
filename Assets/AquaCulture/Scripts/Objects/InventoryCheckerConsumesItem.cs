using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AquaCulture
{
    public class InventoryCheckerConsumesItem : MonoBehaviour, IDataPersister
    {
        public bool m_CheckInventoryOnUpdate;
        public string[] requiredInventoryItemKeys;
        public Sprite[] unlockStateSprites;


        public DirectorTrigger keyDirectorTrigger;
        public UnityEvent onUnlocked;
        public DataSettings dataSettings;

        protected InventoryController PlayerInventory;

        SpriteRenderer m_SpriteRenderer;

        [ContextMenu( "Update State" )]
        public void CheckInventory()
        {
            if ( PlayerInventory.HasAllItemsUniquely( requiredInventoryItemKeys ) )
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
            PlayerInventory = PlayerCharacter.PlayerInstance.GetComponent<InventoryController>();
            PlayerInventory.OnInventoryLoaded += CheckInventory;
        }

        void Update()
        {
            if( m_CheckInventoryOnUpdate )
            {
                CheckInventory();
            }
        }

        public void ConsumePlayerItems()
        {
            foreach ( var i in requiredInventoryItemKeys )
            {
                if ( PlayerInventory.HasItem( i ) )
                {
                    PlayerInventory.RemoveItem( i );
                }
            }
        }

        public DataSettings GetDataSettings()
        {
            return dataSettings;
        }

        public void LoadData( Data data )
        {
            Data<Sprite, bool> d = (Data<Sprite, bool>)data;
            m_SpriteRenderer.sprite = d.value0;
            m_CheckInventoryOnUpdate = d.value1;
        }

        public Data SaveData()
        {
            return new Data<Sprite, bool>( m_SpriteRenderer.sprite, m_CheckInventoryOnUpdate );
        }

        public void SetDataSettings( string dataTag, DataSettings.PersistenceType persistenceType )
        {
            dataSettings.dataTag = dataTag;
            dataSettings.persistenceType = persistenceType;
        }
    }
}
