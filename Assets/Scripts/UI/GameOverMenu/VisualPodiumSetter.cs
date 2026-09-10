using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VisualPodiumSetter : MonoBehaviour
{
    [SerializeField] private VisualPodiumPlayer[] _visualPlayers = new VisualPodiumPlayer[0];

    public void SetGameScores(GameScoresData data)
    {
        for(int i = 0; i< data.orderedScores.Length; i++)
        {
            if (i >= _visualPlayers.Length) return;
            var score = data.orderedScores[i];

            _visualPlayers[i].SetIdTime(score.id, score.time);
        }

        for(int i = data.orderedScores.Length; i < _visualPlayers.Length; i++)
        {
            _visualPlayers[i].gameObject.SetActive(false);
        }
    }
}
