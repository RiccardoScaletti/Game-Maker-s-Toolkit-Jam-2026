using System;
using UnityEngine;

namespace EnemyAI
{
    public class PlayerCapturePoint : MonoBehaviour
    {
        [Tooltip("How long the player stays captured")]
        public float maxCaptureTime;

        private float captureTime;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            captureTime = 0;
            if (enabled)
                enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            captureTime += Time.deltaTime;

            if (captureTime > maxCaptureTime)
            {
                Debug.Log("Releasing player");
                // release player
                PlayerManager.Instance.ReleasePlayerFromCapture();
                captureTime = 0;
                enabled = false;
            }
        }
    }
}
