using UnityEngine;

namespace EnemyAI.StateMachine
{
    public class PatrolState : BaseState
    {
        public int curWaypoint; // current waypoint enemy is on
        public override void Enter()
        {
            if (enemy.path.waypoints.Count == 0 || enemy.path == null)
            {
                Debug.LogWarning($"Enemy {enemy.name} has no waypoints to patrol");
                stateMachine.ChangeState(null);
                return;
            }
            curWaypoint = enemy.path.GetClosestWaypoint();
            enemy.Agent.SetDestination(enemy.path.waypoints[curWaypoint].position);
        }

        public override void Exit()
        {
            
        }

        public override void Perform()
        {
            PatrolCycle();
            if (enemy.CanSeePlayer())
                stateMachine.ChangeState(new AttackState());
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
