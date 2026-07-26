using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject goblinArm;
    [SerializeField] private GameObject handClock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowClockHand()
    {
        if (SceneManager.GetActiveScene().name == "TutorialScene" || SceneManager.GetActiveScene().name == "Level-Keniel")
        {

        }
    }
}
