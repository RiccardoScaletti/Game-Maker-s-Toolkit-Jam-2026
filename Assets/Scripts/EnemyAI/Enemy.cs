using EnemyAI.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI
{
    public enum eEnemyType { Patrolling }
    /// <summary>
    /// Generic class for enemy logic
    /// </summary>
    [RequireComponent(typeof(EnemyStateMachine), typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        public string currentState;
        public NavMeshAgent Agent { get; private set; }
        public eEnemyType enemyType;

        [Header("See Player Logic")]
        public GameObject player;
        public float sightDistance = 20;
        public float fieldOfView = 40f;
        public float eyeHeight;
        public float maxTimeToLosePlayer = 8f;
        [HideInInspector]
        public Vector3 lastKnownPlayerPos;

        [Header("Search For Player Logic")]
        public float maxSearchTimer = 10;

        [Header("Path Logic")]
        public EnemyPath path;

        private EnemyStateMachine stateMachine;

        private void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            stateMachine = GetComponent<EnemyStateMachine>();
            stateMachine.InitializeStateMachine();
        }

        private void Update()
        {
        }

        public bool CanSeePlayer()
        {
            if (player == null)
                return false;

            // if player is within range to see
            if (Vector3.Distance(transform.position, player.transform.position) <= sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                // if player is within field of view of enemy
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new();
                    // if player is not behind a wall
                    if (Physics.Raycast(ray, out hitInfo, sightDistance) && hitInfo.transform.gameObject == player)
                    {
                        Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                        return true;
                    }
                }
            }

            return false;
        }

    }
}

