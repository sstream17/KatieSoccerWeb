using UnityEngine;
using Mirror;
using System.Collections.Generic;

/*
	Documentation: https://mirror-networking.com/docs/Guides/NetworkBehaviour.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class KatieSoccerPlayer : NetworkBehaviour
{
	private GameObject[] pieces;

    public void SetPieces()
    {
        pieces = new GameObject[3];

        int i = 0;
        foreach (Transform child in transform)
        {
            pieces[i] = child.gameObject;
            i = i + 1;
        }
    }

	[Command]
	public void CmdPlayTurn(int pieceIndex, Vector3 force)
    {
		var rb = pieces[pieceIndex].GetComponent<Rigidbody>();

		if (rb != null)
        {
			rb.AddForce(force);
        }
    }

    [ClientRpc]
    public void RpcEnablePieceInteraction(GameObject[] pieces, bool isLocal)
    {
        Debug.Log($"Can enable pieces: {isLocal}");

        if (!isLocal)
        {
            return;
        }

        foreach (GameObject piece in pieces)
        {
            PieceInteraction pieceInteraction = piece.GetComponent<PieceInteraction>();
            pieceInteraction.interactionsEnabled = true;
        }
    }
}
