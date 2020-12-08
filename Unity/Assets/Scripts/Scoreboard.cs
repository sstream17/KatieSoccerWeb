using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public Animator MessageAnimator;
    public TextMeshProUGUI MessageText;

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
