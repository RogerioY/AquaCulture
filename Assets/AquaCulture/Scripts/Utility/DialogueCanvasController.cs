using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AquaCulture
{
    public class DialogueCanvasController : MonoBehaviour
    {
        public Animator animator;
        public TextMeshProUGUI textMeshProUGUI;

        protected Coroutine m_DeactivationCoroutine;
    
        protected readonly int m_HashActivePara = Animator.StringToHash ("Active");

        private void Awake()
        {
            // Need to double check this as it seems very special case
            // Getting the dialogue animator on this object and linking to to the controller on this object
            // If there is no animator on this object, what happens?
            // If there are no scene linked SMBs on the animator, what happens?
            // Note: The member Animator is the one with the GUI canvas on it.
            Animator AnimatorOnThisObject = GetComponent<Animator>();
            SceneLinkedSMB<DialogueCanvasController>.Initialise( AnimatorOnThisObject, this );
        }

        IEnumerator SetAnimatorParameterWithDelay (float delay)
        {
            yield return new WaitForSeconds (delay);
            animator.SetBool(m_HashActivePara, false);
        }

        public void ActivateCanvasWithText (string text)
        {
            if (m_DeactivationCoroutine != null)
            {
                StopCoroutine (m_DeactivationCoroutine);
                m_DeactivationCoroutine = null;
            }

            gameObject.SetActive (true);
            animator.SetBool (m_HashActivePara, true);
            textMeshProUGUI.text = text;
        }

        public void ActivateCanvasWithTranslatedText (string phraseKey)
        {
            if (m_DeactivationCoroutine != null)
            {
                StopCoroutine(m_DeactivationCoroutine);
                m_DeactivationCoroutine = null;
            }

            gameObject.SetActive(true);
            animator.SetBool(m_HashActivePara, true);
            textMeshProUGUI.text = Translator.Instance[phraseKey];
        }

        public void DeactivateCanvasWithDelay (float delay)
        {
            m_DeactivationCoroutine = StartCoroutine (SetAnimatorParameterWithDelay (delay));
        }
    }
}
