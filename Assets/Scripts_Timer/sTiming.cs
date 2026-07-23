using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sTiming : MonoBehaviour
{
    [SerializeField] float totalTime;
    [SerializeField] float timePassed;
    [SerializeField] float curMultiplier;
    [SerializeField] List<soTimeKillingAction> actionList;
    float originalMultiplier = 1f;

    // for testing use only
    [SerializeField] sActionTrigger action;
    bool gameStarted = true;

    private void Start()
    {
        InitTiming();
    }

    public void InitTiming()
    {
        ResetTimeMultiplier();
        action.UpdateTimerText(totalTime, timePassed, curMultiplier);

        StartCoroutine(Countdown(totalTime));
    }

    public void ResetTimeMultiplier()
    {
        curMultiplier = originalMultiplier;
        Debug.Log("current multiplier is: " + curMultiplier);
    }

    public void AddMultiplier(float _addedMultiplier)
    {
        curMultiplier += _addedMultiplier;
        Debug.Log("current multiplier is: " + curMultiplier);
    }

    IEnumerator Countdown(float _totalTime)
    {
        yield return new WaitUntil(() => gameStarted);
        // check how much time has passed
        while (timePassed < _totalTime)
        {
            yield return new WaitForSeconds(1 / curMultiplier);
            timePassed += 1;
            // update UI
            // temp UI for testing function
            action.UpdateTimerText(totalTime, timePassed, curMultiplier);
        }
        // time ends, end level
        Debug.Log("Level ended");
    }

    // for testing use only



}
