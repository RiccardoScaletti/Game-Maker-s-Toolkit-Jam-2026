using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneManager : MonoBehaviour
{
   public void OnClick()
    {
        SceneManager.LoadScene(0);
    }
}
