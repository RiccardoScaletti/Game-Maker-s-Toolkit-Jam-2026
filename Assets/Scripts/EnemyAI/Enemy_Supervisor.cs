using UnityEngine;
using EnemyAI.StateMachine;

namespace EnemyAI
{ 
    public class Enemy_Supervisor : Enemy
    {
        [Header("Supervisor Logic")]
        [Tooltip("The path this enemy will return after capturing the player")]
        public EnemyPath officePath;
        [Tooltip("The place the player will be taken to after being hit.")]
        public GameObject playerCapturePoint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                stateMachine.ChangeState(new CapturePlayerState());
        }


    }
}

