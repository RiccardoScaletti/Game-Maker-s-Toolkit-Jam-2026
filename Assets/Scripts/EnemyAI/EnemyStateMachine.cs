using UnityEngine;

namespace EnemyAI.StateMachine
{
    public class EnemyStateMachine : MonoBehaviour
    {
        Enemy curEnemy;
        public BaseState CurState { get; private set; }

        public void InitializeStateMachine()
        {
            curEnemy = GetComponent<Enemy>();

            switch (curEnemy.enemyType)
            {
                case eEnemyType.Enemy:
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
            CurState?.Perform();
        }
        public void ChangeState(BaseState newState)
        {
            // finish previous state
            CurState?.Exit();

            CurState = newState;

            // start new state
            if (CurState != null)
            {
                curEnemy.debugCurState = CurState.GetType().Name;
                CurState.stateMachine = this;
                CurState.enemy = curEnemy;
                CurState?.Enter();
            }
            else
                curEnemy.debugCurState = "NULL";
        }
    }
}
