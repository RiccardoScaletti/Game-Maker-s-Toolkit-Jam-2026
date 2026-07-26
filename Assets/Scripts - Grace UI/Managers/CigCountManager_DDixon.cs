using UnityEngine;
using UnityEngine.SceneManagement;

public class CigCountManager : MonoBehaviour
{
    public static int CigCount; //This is purely for the tutorial but gathering 6 cigarretes will end the tutorial

    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject jointUi;

    private void Awake()
    {
        CigCount = 0;
    }

    private void Update()
    {
        if (CigCount == 6)
        {
            winCanvas.SetActive(true);
        }
    }
}
