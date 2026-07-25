using UnityEngine;

namespace EnemyAI.StateMachine
{
    public class PatrolState : BaseState
    {
        public int curWaypoint; // current waypoint enemy is on
        public override void Enter()
        {
            if (enemy.mainPath.waypoints.Count == 0 || enemy.mainPath == null)
            {
                Debug.LogWarning($"Enemy {enemy.name} has no waypoints to patrol");
                stateMachine.ChangeState(null);
                return;
            }
            curWaypoint = enemy.mainPath.GetClosestWaypoint();
            enemy.Agent.SetDestination(enemy.mainPath.waypoints[curWaypoint].position);
        }

        public override void Exit()
        {
            
        }

        public override void Perform()
        {
            PatrolCycle();
            if (enemy.CanSeePlayer() && enemy.enemyType != eEnemyType.Customer)
                stateMachine.ChangeState(new AttackState());
        }

        public void PatrolCycle()
        {
            if (enemy.Agent.remainingDistance < .2f)
            {
                curWaypoint = (++curWaypoint) % enemy.mainPath.waypoints.Count; // go to next waypoint
                enemy.Agent.SetDestination(enemy.mainPath.waypoints[curWaypoint].position);
            }
        }
    }
}
