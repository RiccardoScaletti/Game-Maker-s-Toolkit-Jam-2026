using UnityEngine;

namespace EnemyAI.StateMachine
{
    public class GoToSnitchState : BaseState
    {
        public override void Enter()
        {
            enemy.Agent.SetDestination(PlayerManager.Instance.transform.position);
        }

        public override void Exit()
        {

        }

        public override void Perform()
        {
            if (enemy.CanSeePlayer() && enemy.enemyType != eEnemyType.Customer)
                stateMachine.ChangeState(new AttackState());
        }
    }
}