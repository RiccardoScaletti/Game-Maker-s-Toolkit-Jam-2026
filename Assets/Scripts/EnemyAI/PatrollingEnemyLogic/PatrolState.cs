using UnityEngine;

namespace EnemyAI.StateMachine
{
    public class PatrolState : BaseState
    {
        public int curWaypoint; // current waypoint enemy is on
        public override void Enter()
        {
            enemy.Agent.SetDestination(enemy.path.waypoints[curWaypoint].position);
        }

        public override void Exit()
        {
            
        }

        public override void Perform()
        {
            PatrolCycle();
        }

        public void PatrolCycle()
        {
            if (enemy.Agent.remainingDistance < .2f)
            {
                curWaypoint = (++curWaypoint) % enemy.path.waypoints.Count; // go to next waypoint
                enemy.Agent.SetDestination(enemy.path.waypoints[curWaypoint].position);
            }
        }
    }
}
