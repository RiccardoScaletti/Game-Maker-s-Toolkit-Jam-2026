using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CashierGameManager : MonoBehaviour
{

    public bool allClientsServed;

    private void Awake()
    {
        controls = new CashierControls();
        allClientsServed = false;
    }

    [Header("Cashier Images")]
    [SerializeField] private GameObject openDrawer;
    [SerializeField] private GameObject closedDrawer;
    [SerializeField] private GameObject[] clientsImages;

    [Header("Cashier Buttons")]
    [SerializeField] private Button[] ChangeButtons;
    [SerializeField] private int buttonsIndex = 0;

    [Header("TMP Texts")]
    [SerializeField] private TextMeshProUGUI Cashiertext;
    [SerializeField] private TextMeshProUGUI CurrentChangetext;

    [SerializeField] private int currentClient = 0;


    private int[] changeNeeded;
    private CashierControls controls;
    public int selectedChange = 0;


    //activate debugs only if on
    [SerializeField] private bool isDebug;

    #region Setup

    private void Start()
    {
        changeNeeded = new int[clientsImages.Length];
        DrawerStateCheck(false);

        for (int i = 0; i < changeNeeded.Length; i++)
        {
            changeNeeded[i] = Random.Range(1, 1001); // cents: 1–1000
        }
        ShowCurrentClient();
    }

    private void OnEnable()
    {
        controls.Cashier.SelectNext.started += GoToNextChoice;
        controls.Cashier.SelectPrevious.started += GoToPreviousChoice;
        controls.Cashier.Confirm.performed += ConfirmPressed;
        controls.Cashier.OpenCloseCashier.performed += OpenClosePressed;

        controls.Cashier.Enable();
    }

    private void OnDisable()
    {
        controls.Cashier.SelectNext.started -= GoToNextChoice;
        controls.Cashier.SelectPrevious.started -= GoToPreviousChoice;
        controls.Cashier.Confirm.performed -= ConfirmPressed;

        controls.Cashier.Disable();
    }

    #endregion

    private void ShowCurrentClient()
    {
        selectedChange = 0;

        if (currentClient >= clientsImages.Length)
        {
            CurrentChangetext.text = "All clients served!";
            allClientsServed = true;
            return;
        }

        Cashiertext.text = $"{changeNeeded[currentClient] / 100f:0.00}\n";
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

    //visuals
    public void DrawerStateCheck(bool open)
    {
        if (isDebug) Debug.Log("clicked!");
        if (open)
        {
            openDrawer.SetActive(true);
            closedDrawer.SetActive(false);
            buttonsIndex = 0;
            SelectCurrentButton();
        }
        else
        {
            openDrawer.SetActive(false);
            closedDrawer.SetActive(true);
        }
    }


    //controls
    private void GoToPreviousChoice(InputAction.CallbackContext context)
    {
        if (openDrawer.activeSelf)
        {
            buttonsIndex--;
            if (buttonsIndex < 0)
            {
                buttonsIndex = ChangeButtons.Length - 1;
            }
            SelectCurrentButton();
        }
    }

    private void GoToNextChoice(InputAction.CallbackContext context)
    {
        if (openDrawer.activeSelf)
        {
            buttonsIndex = ++buttonsIndex % ChangeButtons.Length;
            //if (buttonsIndex + 1 > ChangeButtons.Length)
            //{
            //    return;
            //}
            //buttonsIndex++;
            SelectCurrentButton();
        }
    }

    private void ConfirmPressed(InputAction.CallbackContext context)
    {
        ChangeButtons[buttonsIndex].onClick.Invoke();
        //if (buttonsIndex == ChangeButtons.Length) //if confirm button selected
        
    }
    private void OpenClosePressed(InputAction.CallbackContext context)
    {
        if (openDrawer.activeSelf) DrawerStateCheck(false);
        else DrawerStateCheck(true);
    }
    private void SelectCurrentButton()
    {
        Debug.Log(buttonsIndex);
        ChangeButtons[buttonsIndex].Select();
    }

}