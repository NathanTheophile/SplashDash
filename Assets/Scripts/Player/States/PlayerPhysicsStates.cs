using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPhysicsStates", menuName = "Scriptable Objects/PlayerPhysicsStates")]
public class PlayerPhysicsStates : ScriptableObject
{
    public PlayerStats[] states;

    public PlayerStats GetStats(PlayerStates state)
    {
        int index = (int)state;

        if (index < 0 || index >= states.Length)
            return null;

        return states[index];
    }
}
