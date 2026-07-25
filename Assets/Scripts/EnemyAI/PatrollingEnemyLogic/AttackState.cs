using UnityEngine;

namespace EnemyAI.StateMachine
{
    public class AttackState : BaseState
    {
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
                enemy.Agent.SetDestination(enemy.player.transform.position);
                //enemy.transform.LookAt(enemy.player.transform);

                enemy.lastKnownPlayerPos = enemy.player.transform.position;
                enemy.Agent.speed = 6;
            }
            else
            {
                losePlayerTimer += Time.deltaTime;
                if (losePlayerTimer >= enemy.maxTimeToLosePlayer)
                {
                    // change to search state
                    stateMachine.ChangeState(new SearchState());
                }
}
        }
    }

}
