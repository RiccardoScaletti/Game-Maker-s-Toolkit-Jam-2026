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
        SceneManager.LoadScene("MainGame");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Button Pressed");
    }
}
