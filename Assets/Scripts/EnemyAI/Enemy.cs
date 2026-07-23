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
        public NavMeshAgent Agent { get; private set; }
        public eEnemyType enemyType;

        public EnemyPath path;

        private EnemyStateMachine stateMachine;

        private void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            stateMachine = GetComponent<EnemyStateMachine>();
            stateMachine.InitializeStateMachine();
        }

    }
}

