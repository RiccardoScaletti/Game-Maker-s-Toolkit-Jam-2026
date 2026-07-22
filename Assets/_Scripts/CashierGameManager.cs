using TMPro;
using UnityEngine;

public class CashierGameManager : MonoBehaviour
{
    public static CashierGameManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private TextMeshProUGUI Cashiertext;
    [SerializeField] private TextMeshProUGUI CurrentChangetext;
    [SerializeField] private GameObject[] clientsImages;

    private int[] changeNeeded;
    private int currentClient = 0;

    public int selectedChange = 0;

    private void Start()
    {
        changeNeeded = new int[clientsImages.Length];

        for (int i = 0; i < changeNeeded.Length; i++)
        {
            changeNeeded[i] = Random.Range(1, 1001); // cents: 1–1000
            clientsImages[i].SetActive(i == 0);
        }

        ShowCurrentClient();
    }

    private void ShowCurrentClient()
    {
        selectedChange = 0;

        if (currentClient >= clientsImages.Length)
        {
            CurrentChangetext.text = "All clients served!";
            return;
        }

        Cashiertext.text = $"{changeNeeded[currentClient] / 100f:0.00}\n";
    }

    // Assign this from each UI button.
    // Example: penny = 1, nickel = 5, dime = 10, dollar = 100.
    //public void SubmitChange(int valueInCents)
    public void SubmitChange()
    {
        if (currentClient >= clientsImages.Length)
            return;

        if (selectedChange == changeNeeded[currentClient])
        {
            clientsImages[currentClient].SetActive(false);
            currentClient++;
            ShowCurrentClient();
        }
        else if (selectedChange > changeNeeded[currentClient])
        {
            CurrentChangetext.text = "Too much change! Try again.";
            selectedChange = 0;
        }
        else
        {
            Cashiertext.text = $"{changeNeeded[currentClient] / 100f:0.00}";
                        
        }
    }

    public void AddChange(int amount)
    {
        selectedChange += amount;
        CurrentChangetext.text = $"{selectedChange / 100f:0.00}";
    }

    public void ClearSelectedChange()
    {
        selectedChange = 0;
        ShowCurrentClient();
    }
}