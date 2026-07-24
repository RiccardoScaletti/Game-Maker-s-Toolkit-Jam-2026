using System.Collections;
using UnityEngine;


public class sTiming : MonoBehaviour
{

    [NamedArray(typeof(actionName))][SerializeField] soTimeKillingAction[] actions;

    [SerializeField] float totalTime;
    [SerializeField] float timeLeft;
    [SerializeField] float curMultiplier;


    float originalMultiplier = 1f;
    bool gameStarted = true;



    private void Start()
    {
        InitTiming();
    }

    public void InitTiming()
    {
        ResetTimeMultiplier();
        if (gameStarted)
        {
            StartCoroutine(Countdown(totalTime));
        }
    }

    public void ResetTimeMultiplier()
    {
        curMultiplier = originalMultiplier;
        Debug.Log("current multiplier is: " + curMultiplier);
        // TODO: update display

    }

    public void AddMultiplier(float _addedMultiplier)
    {
        curMultiplier += _addedMultiplier;
        Debug.Log("current multiplier is: " + curMultiplier);
    }

    public IEnumerator Countdown(float _totalTime)
    {
        timeLeft = _totalTime;
        yield return new WaitUntil(() => gameStarted);
        // check how much time has passed
        while (timeLeft >= 0)
        {
            yield return new WaitForSecondsRealtime(1f / curMultiplier);
            timeLeft -= 1f;
            // TODO: update display

        }
        // time ends, end level
        Debug.Log("Level ended");
    }

    // call when within range of the action object
    public void OnActionPerformed(actionName _action)
    {
        AddMultiplier(actions[(int)_action].timeMultiplier);
    }

    public void OnCaught()
    {
        ResetTimeMultiplier();
    }



}
