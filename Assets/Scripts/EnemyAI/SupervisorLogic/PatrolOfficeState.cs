using UnityEngine;


namespace EnemyAI.StateMachine
{
    public class PatrolOfficeState : BaseState
    {
        private int curWaypoint;
        private Enemy_Supervisor supEnemy;

        private float curTimeInOffice = 0;
        private bool isInOffice = false;
        public override void Enter()
        {
            Debug.Log("Returning to office");
            supEnemy = enemy as Enemy_Supervisor;

            if (supEnemy.officePath.waypoints.Count == 0 || supEnemy.officePath == null)
            {
                Debug.LogWarning($"Enemy {enemy.name} has no waypoints to patrol");
                stateMachine.ChangeState(null);
                return;
            }
            curWaypoint = supEnemy.officePath.GetClosestWaypoint();
            enemy.Agent.SetDestination(supEnemy.officePath.waypoints[curWaypoint].position);
        }

        public override void Exit()
        {

        }

        public override void Perform()
        {
            if (isInOffice)
                curTimeInOffice += Time.deltaTime;

            if (curTimeInOffice > supEnemy.maxTimeInOffice)
                stateMachine.ChangeState(new PatrolState());
            else
                PatrolCycle();
        }


        public void PatrolCycle()
        {
            if (enemy.Agent.remainingDistance < .2f)
            {
                isInOffice = true;
                curWaypoint = (++curWaypoint) % enemy.mainPath.waypoints.Count; // go to next waypoint
                enemy.Agent.SetDestination(enemy.mainPath.waypoints[curWaypoint].position);
            }
        }
    }
}

