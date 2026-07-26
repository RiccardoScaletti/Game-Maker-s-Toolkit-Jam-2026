using UnityEngine;

namespace EnemyAI.StateMachine
{

    public class CapturePlayerState : BaseState
    {
        Enemy_Supervisor supEnemy;
        public override void Enter()
        {
            Debug.Log("capturing player");
            supEnemy = enemy as Enemy_Supervisor;

            PlayerManager.Instance.CapturePlayer(supEnemy.playerHolder);

            // Send enemy to capture point
            GameObject curWaypoint = supEnemy.playerCapturePoint.gameObject;
            enemy.Agent.SetDestination(curWaypoint.transform.position);

            enemy.Agent.speed = supEnemy.toOfficeSpeed;
        }

        public override void Exit()
        {
            enemy.Agent.speed = enemy.defaultSpeed;
        }

        public override void Perform()
        {
            if (enemy.Agent.remainingDistance < .1f)
            {
                // place player on capture point
                PlayerManager.Instance.CapturePlayer(supEnemy.playerCapturePoint.gameObject);
                // activate capture point
                supEnemy.playerCapturePoint.SetActive(true);
                // set destination to "office"
                stateMachine.ChangeState(new PatrolOfficeState());
            }
        }
    }
}
