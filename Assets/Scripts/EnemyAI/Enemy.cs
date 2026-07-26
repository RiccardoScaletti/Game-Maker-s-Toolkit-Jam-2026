using EnemyAI.StateMachine;
using System;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

namespace EnemyAI
{
    public enum eEnemyType { Enemy, Customer }
    /// <summary>
    /// Generic class for enemy logic
    /// </summary>
    [RequireComponent(typeof(EnemyStateMachine), typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        public static event Action OnSnitchOnPlayer;

        public string debugCurState;
        public NavMeshAgent Agent { get; private set; }
        public eEnemyType enemyType;

        [Header("Movement")]
        public float defaultSpeed = 3.5f;
        public float chaseSpeed = 10;

        [Header("See Player Logic")]
        public float sightDistance = 20;
        public float fieldOfView = 40f;
        public float eyeHeight;
        public float maxTimeToLosePlayer = 8f;
        [Space(.5f)]
        public bool showRay;
        [HideInInspector]
        public Vector3 lastKnownPlayerPos;

        [Header("Search For Player Logic")]
        public float maxSearchTimer = 10;

        [Header("Path Logic")]
        public EnemyPath mainPath;

        [Header("Animation")]
        public Animator anim;
        public float curVelocity;

        protected EnemyStateMachine stateMachine;

        private void OnEnable()
        {
            Enemy_Supervisor.onPlayerCaptured += PlayerGotCaptured;
        }

        private void OnDisable()
        {
            Enemy_Supervisor.onPlayerCaptured -= PlayerGotCaptured;
        }

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.updateRotation = false;
            stateMachine = GetComponent<EnemyStateMachine>();
            stateMachine.InitializeStateMachine();

            if (!TryGetComponent<Animator>(out anim) && transform.childCount > 0)
            {
                transform.GetChild(0).TryGetComponent<Animator>(out anim);
            }

            if (anim == null)
            {
                Debug.LogError($"No animator found on Enemy: {name}");
            }
        }

        private void Update()
        {
            curVelocity = Agent.velocity.magnitude;

            if (enemyType != eEnemyType.Customer)
            {
                if (CanSeePlayer())
                    anim.SetBool("canSeePlayer", true);
                else
                    anim.SetBool("canSeePlayer", false);
            }

            if (Agent.velocity.magnitude > 3.1f)
                anim.SetFloat("velocity", 3.5f);
            else if (Agent.velocity.magnitude > 3.5f)
                anim.SetFloat("velocity", 6f);
            else
                anim.SetFloat("velocity", 0);
        }

        public bool CanSeePlayer()
        {
            if (PlayerManager.Instance == null)
                return false;

            // if player is within range to see
            if (Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= sightDistance)
            {
                Vector3 targetDirection = PlayerManager.Instance.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                // if player is within field of view of enemy
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new();
                    // if player is not behind a wall
                    if (Physics.Raycast(ray, out hitInfo, sightDistance) && hitInfo.transform.gameObject.CompareTag("Player"))
                    {
                        if (showRay)
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                        return true;
                    }
                }
            }

            return false;
        }

        public void SnitchOnPlayer()
        {
            // if supervisor already has player
            if (stateMachine.CurState is CapturePlayerState)
                return;
            // ignore snitches if patrolling office
            if (stateMachine.CurState is PatrolOfficeState)
                return;

            if (this is not Enemy_Supervisor && enemyType == eEnemyType.Enemy)
                OnSnitchOnPlayer?.Invoke();
        }

        private void PlayerGotCaptured()
        {
            // if supervisor already has player
            if (stateMachine.CurState is CapturePlayerState)
                return;
            // ignore snitches if patrolling office
            if (stateMachine.CurState is PatrolOfficeState)
                return;

            stateMachine.ChangeState(new PatrolState());
        }
    }
}

