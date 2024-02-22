using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AquaCulture
{
    // We want this class enable us to drive the controller from animations, so we can use animation controllers
    // to write all our dialogue and space out how long it's on screen for..
    public class DialogueSMB : SceneLinkedSMB<DialogueCanvasController>
    {
        [TextArea]
        public string DialogueText;

        public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.ActivateCanvasWithText( DialogueText );
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_MonoBehaviour.DeactivateCanvasWithDelay( 0.0f );
        }
    }
}