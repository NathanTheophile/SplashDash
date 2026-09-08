using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPhysicsStates", menuName = "Scriptable Objects/PlayerPhysicsStates")]
public class PlayerPhysicsStates : ScriptableObject
{
    public PlayerStats[] states;
}
