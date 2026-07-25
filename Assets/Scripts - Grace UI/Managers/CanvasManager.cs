using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject startingCanvas;

    private readonly Stack<GameObject> canvasHistory = new();
    private GameObject currentCanvas;

    private void Start()
    {
        currentCanvas = startingCanvas;
        currentCanvas.SetActive(true);
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

    public void LoadGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the next scene index
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index exists in your Build Settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene found in Build Settings! Loop back to main menu or stop.");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Button Pressed");
    }
}
