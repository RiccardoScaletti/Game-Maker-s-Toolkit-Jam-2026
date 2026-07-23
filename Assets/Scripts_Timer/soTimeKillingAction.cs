using UnityEngine;

[CreateAssetMenu(fileName = "New Time Killing Action", menuName = "Create Timer Action")]

public class soTimeKillingAction : ScriptableObject
{
    [Header("Action Properties")]
    public int actionIdx;
    public string actionName;
    public float timeMultiplier;

}
