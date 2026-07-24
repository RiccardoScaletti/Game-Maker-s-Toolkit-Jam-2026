using UnityEngine;

public enum actionName { smoke, chat, pickpocket, playOnPhone, none }
[CreateAssetMenu(fileName = "New Time Killing Action", menuName = "Create Timer Action")]

public class soTimeKillingAction : ScriptableObject
{
    [Header("Action Properties")]

    //public int idx;
    public actionName actionName;
    public float actionDuration;
    public float timeMultiplier;

}
