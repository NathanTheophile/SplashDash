using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPhysicsStates", menuName = "Scriptable Objects/PlayerPhysicsStates")]
public class PlayerPhysicsStates : ScriptableObject
{
    public PlayerStats[] states;

    public PlayerStats GetStats(PlayerStates state)
    {
        if (states == null)
            return null;

        int index = (int)state;

        if (index < 0 || index >= states.Length)
            return null;

        return states[index];
    }
}
