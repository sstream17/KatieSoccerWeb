using System;
using UnityEngine;

[Serializable]
public class TurnDataDTO
{
    public string GameId;

    public int Player;

    public int PieceIndex;

    public Vector3 Force;
}
