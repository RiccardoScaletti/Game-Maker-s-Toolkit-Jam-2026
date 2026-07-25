using UnityEngine;

namespace EnemyAI.StateMachine
{
    public class SearchState : BaseState
    {
        private float searchTimer;
        private float moveTimer;
        public override void Enter()
        {
            enemy.Agent.SetDestination(enemy.lastKnownPlayerPos);
        }

        public override void Exit()
        {
        }

        public override void Perform()
        {
            if (enemy.CanSeePlayer())
                stateMachine.ChangeState(new AttackState());

            enemy.Agent.speed = 3.5f;

            // if enemy is within stopping distance, start searching
            if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
            {
                searchTimer += Time.deltaTime;
                moveTimer += Time.deltaTime;

                if (searchTimer > enemy.maxSearchTimer)
                {
                    Debug.Log("No longer searching");
                    stateMachine.ChangeState(new PatrolState());
                }

                // randomly pick a position nearby to move to every 2 to 5 seconds
                if (moveTimer > Random.Range(2, 5))
                {
                    enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10));
                    moveTimer = 0;
                }
            }
        }
    }
}
