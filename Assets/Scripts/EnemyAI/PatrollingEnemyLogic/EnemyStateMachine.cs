using UnityEngine;

namespace EnemyAI.StateMachine
{
    public enum EnemyType { Patrolling }
    public class EnemyStateMachine : MonoBehaviour
    {
        public EnemyType enemyType;
        BaseState curState;

        private void Start()
        {
            switch (enemyType)
            {
                case EnemyType.Patrolling:
                    // Initialize curState with Patrolling State
                    break;
                default:
                    // initialize with a default state
                    break;
            }
        }

        private void Update()
        {
            // perform current state
            curState?.Perform();
        }
        public void ChangeState(BaseState newState)
        {
            // finish previous state
            curState?.Exit();

            curState = newState;

            // start new state
            if (curState != null)
            {
                curState.stateMachine = this;
                curState?.Enter();
            }
        }

        // Patrol State
        // Chase State
        // Search State
    }

}
