using System;
using UnityEngine;

namespace EnemyAI
{
    public class PlayerCapturePoint : MonoBehaviour
    {
        //[Tooltip("How long the player stays captured")]
        //public float maxCaptureTime;

        [SerializeField] private GameObject minigamePrefab;
        private GameObject spawnedMinigame;
        private CashierGameManager manager;

        private void OnEnable()
        {
            spawnedMinigame = Instantiate(minigamePrefab);
            manager = spawnedMinigame.GetComponentInChildren<CashierGameManager>();
        }

        private void Update()
        {
            Debug.Log("are clients server? " + manager.allClientsServed);
            if (manager.allClientsServed)
            {
                Destroy(spawnedMinigame);
                PlayerManager.Instance.ReleasePlayerFromCapture();
                gameObject.SetActive(false);
            }
        }
    }
}
