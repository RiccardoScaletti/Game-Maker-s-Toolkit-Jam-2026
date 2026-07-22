using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class CashierGameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cashierButtons;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject[] clientsImages;

    private float[] numbersToRandomize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numbersToRandomize = new float[clientsImages.Length];
        for (int i = 0; i < numbersToRandomize.Length; i++)
        {
            numbersToRandomize[i] = Random.Range(1, 1000);
        }
    }

    private void StartGame()
    {
        text.text = numbersToRandomize.ToString();
    }

    public void SubmitChange(int number_chosen)
    {
        
    }
}
