using UnityEngine;

namespace EnemyAI.StateMachine
{
    public class AttackState : BaseState
    {
        private float moveTimer;
        private float losePlayerTimer;
        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Perform()
        {
            if (enemy.CanSeePlayer())
            {
                losePlayerTimer = 0;
                enemy.Agent.SetDestination(enemy.Player.transform.position);
            }
            else
            {
                moveTimer += Time.deltaTime;
                if (moveTimer > Random.Range(2, 5))
                {
                    enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                    moveTimer = 0;
                }
                losePlayerTimer += Time.deltaTime;
                if (losePlayerTimer >= enemy.timeToLosePlayer)
                {
                    // change to search state
                    stateMachine.ChangeState(new PatrolState());
                }
}
        }
    }

}
