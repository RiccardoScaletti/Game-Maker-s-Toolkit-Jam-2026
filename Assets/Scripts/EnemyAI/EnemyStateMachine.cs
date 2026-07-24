using UnityEngine;

namespace EnemyAI.StateMachine
{
    public class EnemyStateMachine : MonoBehaviour
    {
        Enemy curEnemy;
        BaseState curState;

        public void InitializeStateMachine()
        {
            curEnemy = GetComponent<Enemy>();

            switch (curEnemy.enemyType)
            {
                case eEnemyType.Minion:
                    ChangeState(new PatrolState());
                    break;
                default:
                    ChangeState(new PatrolState());
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
                curEnemy.debugCurState = curState.GetType().Name;
                curState.stateMachine = this;
                curState.enemy = curEnemy;
                curState?.Enter();
            }
            else
                curEnemy.debugCurState = "NULL";
        }
    }
}
