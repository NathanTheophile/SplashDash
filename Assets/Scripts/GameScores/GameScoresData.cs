using UnityEngine;

public struct GameScoresData
{
    public PlayerScore[] orderedScores;
}

public struct PlayerScore
{
    public int id;
    public int time;
}
