using UnityEngine;

public class PieceInteraction : MonoBehaviour
{
    public int PieceIndex;
    public float Speed = 200f;
    public PieceAnimation PieceAnimation;
    public KatieSoccerPlayer Player;

    public bool interactionsEnabled = false;

    private bool isSelected = false;
    private float triggerOffset = 0.3f;
    private float speedClamp = 5f;
    private float speedAdjust = 3f;
    private Vector3 arrow;
    private bool launchable = false;
    private Vector3 targetVector;

    private Vector3 cachedForce = Vector3.zero;

    // Update is called once per frame
    private void Update()
    {
        if (interactionsEnabled)
        {
            if (isSelected)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetVector = mousePosition - transform.position;
                targetVector.z = 0f;
                arrow = new Vector3(targetVector.x, 0f, targetVector.y);
                PieceAnimation.ArrowDirection = Vector3.ClampMagnitude(-arrow, PieceAnimation.MaxArrowLength);
                if (targetVector.magnitude >= triggerOffset)
                {
                    PieceAnimation.PieceSelected();
                    PieceAnimation.DrawArrow();
                    launchable = true;
                }
                else
                {
                    PieceAnimation.PieceDeselected();
                    PieceAnimation.HideArrow();
                    launchable = false;
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (interactionsEnabled)
        {
            isSelected = true;
        }
    }

    private void OnMouseUp()
    {
        if (interactionsEnabled)
        {
            isSelected = false;
            PieceAnimation.HideArrow();
            if (launchable)
            {
                launchable = false;
                PieceAnimation.PieceLaunched();
                var launchForce = Vector3.ClampMagnitude(targetVector * speedAdjust, speedClamp) * -Speed;
                Player.CmdPlayTurn(PieceIndex, launchForce);
            }
            else
            {
                PieceAnimation.PieceDeselected();
            }
        }
    }
}
