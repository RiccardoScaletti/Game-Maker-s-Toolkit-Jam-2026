using UnityEngine;

namespace EnemyAI.StateMachine
{

    public class CapturePlayerState : BaseState
    {
        public override void Enter()
        {
            //PlayerManager.Instance.inputAllowed = false;

            GameObject curWaypoint = (enemy as Enemy_Supervisor).playerCapturePoint;
            enemy.Agent.SetDestination(curWaypoint.transform.position);
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override void Perform()
        {
            throw new System.NotImplementedException();
        }
    }
}
