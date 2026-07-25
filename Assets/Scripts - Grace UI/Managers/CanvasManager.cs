using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public bool isPaused;

    [SerializeField] private GameObject startingCanvas;
    [SerializeField] private GameObject pauseMenu;

    private readonly Stack<GameObject> canvasHistory = new();
    private GameObject currentCanvas;
    private bool isResumePressed;

    private void Start()
    {
        currentCanvas = startingCanvas;
        currentCanvas.SetActive(true);
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame || isResumePressed)
        {
            PauseGame();
            isResumePressed = false;
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
        if (SceneManager.GetActiveScene().name == "Title Scene")
        {
            return;
        }
        else if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
            currentCanvas.SetActive(false);
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            isPaused = false;
            currentCanvas.SetActive(true);
            pauseMenu.SetActive(false);
         }
    }

    //This is for spefically pressing the Resume button
    public void OnPressingResume()
    {
        isResumePressed = true;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Title Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Button Pressed");
    }
}
