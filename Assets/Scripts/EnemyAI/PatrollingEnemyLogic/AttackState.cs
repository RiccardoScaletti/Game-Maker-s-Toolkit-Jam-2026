using System;
using Unity.VisualScripting;
using UnityEngine;

namespace EnemyAI.StateMachine
{
    public class AttackState : BaseState
    {

        private float losePlayerTimer;
        public override void Enter()
        {
            enemy.Agent.speed = enemy.chaseSpeed;
        }

        public override void Exit()
        {
        }

        public override void Perform()
        {
            if (enemy.CanSeePlayer())
            {
                losePlayerTimer = 0;
                enemy.Agent.SetDestination(PlayerManager.Instance.transform.position);
                //enemy.transform.LookAt(enemy.player.transform);

                enemy.lastKnownPlayerPos = PlayerManager.Instance.transform.position;
                enemy.SnitchOnPlayer();
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
