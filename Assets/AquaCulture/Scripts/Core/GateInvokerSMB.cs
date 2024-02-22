using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AquaCulture
{
    [RequireComponent( typeof( GateInvoker ) )]
    public class GateInvokerSMB : StateMachineBehaviour
    {
        readonly int m_HashOpening = Animator.StringToHash( "Gate Opening" );
        readonly int m_HashClosing = Animator.StringToHash( "Gate Closing" );
        GateInvoker m_MonoInvoker;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
        {
            if( m_MonoInvoker == null )
            {
                m_MonoInvoker = animator.transform.GetComponent<GateInvoker>();
            }

            if ( stateInfo.speed * stateInfo.speedMultiplier > 0.0f )
            {
                m_MonoInvoker.ProgressInvoke( stateInfo.normalizedTime );
            }
            else
            {
                m_MonoInvoker.ProgressInvoke( 1.0f - stateInfo.normalizedTime );
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
        {
            if ( m_MonoInvoker == null )
            {
                m_MonoInvoker = animator.transform.GetComponent<GateInvoker>();
            }

            if ( stateInfo.shortNameHash == m_HashOpening )
            {
                m_MonoInvoker.HasOpenedInvoke();
            }
            else if ( stateInfo.shortNameHash == m_HashClosing )
            {
                m_MonoInvoker.HasClosedInvoke();
            }
            else
            {
                Assert.IsTrue( false, "We ought to have been able to determine our current state here. Check the hashes." );
            }
        }
    }
}