using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject startingCanvas;
    [SerializeField] private GameObject pauseMenu;

    private readonly Stack<GameObject> canvasHistory = new();
    private GameObject currentCanvas;
    private bool isPaused;
    private bool isclicked;

    private void Start()
    {
        currentCanvas = startingCanvas;
        currentCanvas.SetActive(true);
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame || isclicked)
        {
            isclicked = false;
            PauseGame();
        }
    }

    public void OpenCanvas(GameObject newCanvas)
    {
        if (newCanvas == null || newCanvas == currentCanvas)
            return;

        canvasHistory.Push(currentCanvas);

        currentCanvas.SetActive(false);
        newCanvas.SetActive(true);

        currentCanvas = newCanvas;
    }

    public void GoBack()
    {
        if (canvasHistory.Count == 0)
            return;

        currentCanvas.SetActive(false);
        currentCanvas = canvasHistory.Pop();
        currentCanvas.SetActive(true);
    }

    public void PauseGame()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            return;
        }
        else if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            isPaused = false;
            pauseMenu.SetActive(false);
        }
    }

    public void ResumeButtonWasClicked()
    {
        isclicked = true;
    }

    public void LoadGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 1f; //This is here since the game pauses when you win

        // Calculate the next scene index
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index exists in your Build Settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            nextSceneIndex = 0;

            SceneManager.LoadScene(nextSceneIndex);
            Debug.LogWarning("No next scene found in Build Settings! Loop back to main menu or stop.");
        }
    }

    //The opposite of the LoadGame where it goes to the main menu
    public void LoadBack()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 1f; //This is here since the game pauses when you win

        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Button Pressed");
    }
}
