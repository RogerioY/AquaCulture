using UnityEngine;

namespace AquaCulture
{
    public class KeyUI : MonoBehaviour
    {
        public static KeyUI Instance { get; protected set; }

        public GameObject keyIconPrefab;
        public string[] keyNames;

        protected GameObject[] m_KeyIcons;

        protected readonly int m_HashActivePara = Animator.StringToHash("Active");
        protected const float k_KeyIconAnchorWidth = 0.041f;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SetInitialKeyCount();
        }

        public void SetInitialKeyCount()
        {
            if (m_KeyIcons != null && m_KeyIcons.Length == keyNames.Length)
                return;

            m_KeyIcons = new GameObject[keyNames.Length];

            for (int i = 0; i < m_KeyIcons.Length; i++)
            {
                GameObject keyIcon = Instantiate(keyIconPrefab);
                keyIcon.transform.SetParent(transform);
                RectTransform keyIconRect = keyIcon.transform as RectTransform;
                keyIconRect.anchoredPosition = Vector2.zero;
                keyIconRect.sizeDelta = Vector2.zero;
                keyIconRect.anchorMin -= new Vector2(k_KeyIconAnchorWidth, 0f) * i;
                keyIconRect.anchorMax -= new Vector2(k_KeyIconAnchorWidth, 0f) * i;
                m_KeyIcons[i] = keyIcon;
            }

            
        }

        public void ChangeKeyUI(InventoryController controller)
        {
            // TODO: This should invoke a function on the key icon ideally.
            for (int i = 0; i < keyNames.Length; i++)
            {
                bool bHaveItem = controller.HasItem( keyNames[i] );
                // We expect that there is a child for the key, and that child is the possessed sprite. 
                m_KeyIcons[i].transform.GetChild( 0 ).gameObject.SetActive( bHaveItem );
                // Set the Active state in the animator to true/false;
                m_KeyIcons[i].GetComponent<Animator>().SetBool(m_HashActivePara, bHaveItem );
            }
        }
    }
}