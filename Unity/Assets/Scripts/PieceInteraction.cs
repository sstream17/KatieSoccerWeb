﻿using Mirror;
using UnityEngine;

public class PieceInteraction : NetworkBehaviour
{
    public Rigidbody rb;
    public float Speed = 200f;
    public PieceAnimation PieceAnimation;

    public bool interactionsEnabled = false;

    private bool isSelected = false;
    private readonly float triggerOffset = 0.3f;
    private readonly float speedClamp = 5f;
    private readonly float speedAdjust = 3f;
    private Vector3 arrow;
    private bool launchable = false;
    private Vector3 targetVector;

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
                CmdPlayTurn(launchForce);
            }
            else
            {
                PieceAnimation.PieceDeselected();
            }
        }
    }

    [Command]
    public void CmdPlayTurn(Vector3 force)
    {
        rb.AddForce(force);
    }
}
