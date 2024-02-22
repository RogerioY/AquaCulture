using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AquaCulture
{
    public class InventoryController : MonoBehaviour, IDataPersister
    {
        [System.Serializable]
        public class PassStringEvent : UnityEvent<string>
        { }

        [System.Serializable]
        public class InventoryEvent
        {
            public string key;
            public PassStringEvent OnAdd, OnRemove;
        }


        [System.Serializable]
        public class InventoryChecker
        {
            public string[] inventoryItems;
            public UnityEvent OnHasItem, OnDoesNotHaveItem;

            public bool CheckInventory(InventoryController inventory)
            {
                if (inventory != null)
                {
                    for (var i = 0; i < inventoryItems.Length; i++)
                    {
                        if (!inventory.HasItem(inventoryItems[i]))
                        {
                            OnDoesNotHaveItem.Invoke();
                            return false;
                        }
                    }
                    OnHasItem.Invoke();
                    return true;
                }
                return false;
            }
        }


        public InventoryEvent[] inventoryEvents;
        public event System.Action OnInventoryLoaded;

        public DataSettings dataSettings;

        public Dictionary<string, int> m_InventoryItems = new Dictionary<string, int>();

        public string cheatItem;
        public int cheatAmount;


        //Debug function useful in editor during play mode to print in console all objects in that InventoyController
        [ContextMenu("Dump")]
        void Dump()
        {
            foreach (var item in m_InventoryItems)
            {
                Debug.Log("We have " + item.Value.ToString() + " of " + item.Key);
            }
        }

        [ContextMenu("Cheat")] // Enter the item you want as cheat item and then amoutn you want as cheat amount, right click the invetory controller that you want to add the things to and click "cheat".
        void giveMoney()
        {
            for (int i = 0; i < cheatAmount; i++) { AddItem(cheatItem); }
        }

        void OnEnable()
        {
            PersistentDataManager.RegisterPersister(this);
        }

        void OnDisable()
        {
            PersistentDataManager.UnregisterPersister(this);
        }

        public void AddItem(string key)
        {
            if (!m_InventoryItems.ContainsKey( key ))
            {
                m_InventoryItems.Add(key, 1);
            }
            else
            {
                m_InventoryItems[key] += 1;
            }

            var ev = GetInventoryEvent(key);
            if (ev != null) ev.OnAdd.Invoke(key);
        }

        public void RemoveItem(string key)
        {
            if ( m_InventoryItems.ContainsKey( key ) )
            {
                m_InventoryItems[key] -= 1;
            }

            var ev = GetInventoryEvent( key );
            if (ev != null) ev.OnRemove.Invoke( key );
        }

        public bool HasItem(string key)
        {
            return m_InventoryItems.ContainsKey( key );
        }

        public bool HasAllItemsUniquely( string[] keys )
        {
            int NumItemsFound = 0;
            Dictionary<string, int> UniqueInventoryItems = new Dictionary<string, int>( m_InventoryItems );

            foreach ( string key in keys )
            {
                if ( UniqueInventoryItems.ContainsKey( key ) && UniqueInventoryItems[key] > 0 )
                {
                    UniqueInventoryItems[key] -= 1;
                    NumItemsFound++;
                }
            }

            return NumItemsFound == keys.GetLength( 0 );
        }

        public int GetNumberOfItem( string key )
        {
            if ( HasItem( key ) )
            {
                return m_InventoryItems[key];
            }
            else
            {
                return 0;
            }
        }

        public void Clear()
        {
            m_InventoryItems.Clear();
        }

        InventoryEvent GetInventoryEvent(string key)
        {
            foreach (var iv in inventoryEvents)
            {
                if (iv.key == key) return iv;
            }
            return null;
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
            return new Data<Dictionary<string, int>>(m_InventoryItems);
        }

        public void LoadData(Data data)
        {
            Data<Dictionary<string, int>> inventoryData = (Data<Dictionary<string, int>>)data;
            m_InventoryItems = inventoryData.value;
            if (OnInventoryLoaded != null) OnInventoryLoaded();
        }
    }
}