using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public Animator MessageAnimator;
    public TextMeshProUGUI MessageText;

    [DllImport("__Internal")]
    private static extern void UpdateScore(string id, int score);

    private const string TeamOneId = "player-one-score";
    private const string TeamTwoId = "player-two-score";

    public void UpdateScoreboard(int teamOneScore, int teamTwoScore)
    {
        UpdateScore(TeamOneId, teamOneScore);
        UpdateScore(TeamTwoId, teamTwoScore);
    }

    public void DisplayMessage(string message)
    {
        MessageText.text = message;
        MessageAnimator.ResetTrigger("SlideOut");
        MessageAnimator.SetTrigger("SlideIn");
    }

    public bool HideMessage()
    {
        MessageAnimator.ResetTrigger("SlideIn");
        MessageAnimator.SetTrigger("SlideOut");
        return true;
    }

    public IEnumerator ShuffleMessage(string message)
    {
        HideMessage();
        yield return new WaitForSecondsRealtime(1f);
        DisplayMessage(message);
    }
}
