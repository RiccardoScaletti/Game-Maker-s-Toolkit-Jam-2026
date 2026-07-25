using UnityEngine;
using EnemyAI.StateMachine;

namespace EnemyAI
{ 
    public class Enemy_Supervisor : Enemy
    {
        [Header("Supervisor Logic")]
        [Tooltip("The path this enemy will return after capturing the player")]
        public EnemyPath officePath;
        //[Tooltip("The path the enemy will take ")]
        //public EnemyPath patrolPath;
        [Tooltip("The place the player will be taken to after being captured.")]
        public PlayerCapturePoint playerCapturePoint;

        [Tooltip("The gameobject the player will be parented to when captured.")]
        public GameObject playerHolder;
        public float toOfficeSpeed = 20;

        public float maxTimeInOffice = 5;

        private void OnEnable()
        {
            Enemy.OnSnitchOnPlayer += GoToSnitch;
        }

        private void OnDisable()
        {
            Enemy.OnSnitchOnPlayer -= GoToSnitch;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                stateMachine.ChangeState(new CapturePlayerState());
        }

        private void GoToSnitch()
        {
            stateMachine.ChangeState(new GoToSnitchState());
        }

    }
}

