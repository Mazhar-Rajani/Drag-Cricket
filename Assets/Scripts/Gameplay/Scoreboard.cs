using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public static event System.Action<int> OnTargetScoreGenerated;
    public static event System.Action OnTargetReached;

    [SerializeField] private TextMeshProUGUI targetText = default;
    [SerializeField] private int minTargetScore = default;
    [SerializeField] private int maxTargetScore = default;

    public int TargetScore { get; private set; }

    private void OnEnable()
    {
        GameBootStrapper.OnGameBootStrap += ResetScoreboard;
        Button_Play.OnPlayMatch += GenerateTargetScore;
    }

    private void OnDisable()
    {
        GameBootStrapper.OnGameBootStrap -= ResetScoreboard;
        Button_Play.OnPlayMatch -= GenerateTargetScore;
    }

    private void ResetScoreboard()
    {
        ResetTargetText();
        ResetMessageText();
    }

    private void GenerateTargetScore()
    {
        TargetScore = Random.Range(minTargetScore, maxTargetScore);
        targetText.text = "TARGET - " + TargetScore;
        OnTargetScoreGenerated?.Invoke(TargetScore);

        if (TargetScore <= 0)
        {
            OnTargetReached?.Invoke();
        }
    }

    private void OnRunsScored(int count)
    {
        UpdateTargetText(count);
    }

    private void UpdateTargetText(int count)
    {
        TargetScore -= count;
        TargetScore = Mathf.Clamp(TargetScore, 0, maxTargetScore);
        targetText.text = "TARGET - " + TargetScore;
    }

    private void ResetTargetText()
    {
        targetText.text = string.Empty;
    }

    private void ResetMessageText()
    {
    }
}