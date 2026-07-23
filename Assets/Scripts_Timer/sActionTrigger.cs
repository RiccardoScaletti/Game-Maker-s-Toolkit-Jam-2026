using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class sActionTrigger : MonoBehaviour
{
    [SerializeField] sTiming timing;

    // for testing only
    [SerializeField] TMP_Text tTimer;
    [SerializeField] TMP_Text tMultiplier;
    [SerializeField] Button bAddMltplyr;
    [SerializeField] Button bResetMltplyr;
    

    private void Start()
    {
        bAddMltplyr.onClick.AddListener(OnAddTimeMltplyrClick);
        bResetMltplyr.onClick.AddListener(OnResetTimeMltplyrClick);
    }
    public void UpdateTimerText(float _totalTime, float _timePassed, float _curMultiplier)
    {
        tTimer.text = (_totalTime - _timePassed).ToString("F0");
        tMultiplier.text = _curMultiplier.ToString("F2");
    }

    void OnActionPerformed(int _actionIdx)
    {

    }

    void OnAddTimeMltplyrClick()
    {
        timing.AddMultiplier(0.5f);
    }

    void OnResetTimeMltplyrClick()
    {
        timing.ResetTimeMultiplier();
    }



}
