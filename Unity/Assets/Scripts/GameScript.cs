﻿using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject[] TeamOnePieces;
    public GameObject[] TeamTwoPieces;
    public GameObject Ball;
    public Scoreboard Scoreboard;
    public SpriteRenderer TeamOneGoal;
    public SpriteRenderer TeamTwoGoal;
    public Confetti Confetti;
    public LevelTransition LevelTransition;

    public enum Team { TeamOne = -1, TeamTwo = 1 };

    private Player PlayerOne;
    private Player PlayerTwo;

    private GameObject[] allPieces;
    private Vector3[] startingPositions;
    private Team currentTurn;
    private bool piecesMoving = false;
    private bool piecesWereMoving = false;

    private int scoreToWin = 3;
    private int teamOneScore = 0;
    private int teamTwoScore = 0;
    private bool wasPaused = false;
    private bool goalScored = false;
    private bool endGame = false;

    private SignalRLib signalRLib;

    private const string SignalRHubURL = "https://localhost:44346/game-hub";

    [DllImport("__Internal")]
    private static extern void Ready();

    private void SetTeamNames(string teamOneName, string teamTwoName)
    {
        GameData.SetTeamNames(teamOneName, teamTwoName);
    }

    private void SetTeamColors(string teamOneHexColor, string teamTwoHexColor)
    {
        if (!ColorUtility.TryParseHtmlString(teamOneHexColor, out Color teamOneColor))
        {
            teamOneColor = PieceColors.Red;
        }

        if (!ColorUtility.TryParseHtmlString(teamTwoHexColor, out Color teamTwoColor))
        {
            teamOneColor = PieceColors.Blue;
        }

        GameData.SetTeamColors(teamOneColor, teamTwoColor);
    }

    private void SetTeamPiecesColor(GameObject[] pieces, Color color)
    {
        foreach (GameObject piece in pieces)
        {
            MeshRenderer renderer = piece.GetComponent<MeshRenderer>();
            renderer.material.color = color;
        }
    }

    public void InitializeGame(string gameDataJson)
    {
        var gameData = JsonUtility.FromJson<GameDataDTO>(gameDataJson);
        PlayerOne = gameData.PlayerOne;
        PlayerTwo = gameData.PlayerTwo;

        SetTeamNames(PlayerOne.Name, PlayerTwo.Name);
        SetTeamColors(PlayerOne.Color, PlayerTwo.Color);

        SetTeamPiecesColor(TeamOnePieces, GameData.TeamOneColor);
        SetTeamPiecesColor(TeamTwoPieces, GameData.TeamTwoColor);
        TeamOneGoal.color = new Color(
            GameData.TeamOneColor.r,
            GameData.TeamOneColor.g,
            GameData.TeamOneColor.b,
            0.79f);
        TeamTwoGoal.color = new Color(
            GameData.TeamTwoColor.r,
            GameData.TeamTwoColor.g,
            GameData.TeamTwoColor.b,
            0.79f);
    }

    public void SetStartingPositions()
    {
        for (int i = 0; i < allPieces.Length; i++)
        {
            startingPositions[i] = allPieces[i].transform.position;
        }
    }

    public void SetupSignalR()
    {
        signalRLib = new SignalRLib();
        signalRLib.Init(SignalRHubURL, "JoinedGame");

        signalRLib.ConnectionStarted += (object sender, MessageEventArgs e) =>
        {
            Debug.Log(e.Message);
            signalRLib.SendMessage("JoinGame", "12345");
        };

        signalRLib.MessageReceived += (object sender, MessageEventArgs e) =>
        {
            Debug.Log($"received {e.Message}");
        };
    }

    void Awake()
    {
        Ready();
        SetupSignalR();
        scoreToWin = GameData.ScoreToWin;

        int numberOfAllPieces = TeamOnePieces.Length + TeamTwoPieces.Length + 1;
        allPieces = new GameObject[numberOfAllPieces];

        int iterator = 0;
        foreach (GameObject piece in TeamOnePieces)
        {
            allPieces[iterator] = piece;
            iterator = iterator + 1;
        }

        foreach (GameObject piece in TeamTwoPieces)
        {
            allPieces[iterator] = piece;
            iterator = iterator + 1;
        }

        allPieces[iterator] = Ball;

        startingPositions = new Vector3[allPieces.Length];

        SetStartingPositions();
    }

    public void GetRandomTurn()
    {
        Array values = Enum.GetValues(typeof(Team));
        int randomIndex = Mathf.FloorToInt(UnityEngine.Random.Range(0f, values.Length));
        currentTurn = (Team)values.GetValue(randomIndex);
        OnNextTurn();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetRandomTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Pause.Paused)
        {
            wasPaused = true;
        }

        if (!wasPaused)
        {
            piecesMoving = !PiecesStoppedMoving(allPieces);
            if (piecesMoving)
            {
                piecesWereMoving = true;
                DisablePieceInteraction(TeamOnePieces);
                DisablePieceInteraction(TeamTwoPieces);
            }

            if (goalScored)
            {
                piecesWereMoving = false;
            }

            if (!piecesMoving && piecesWereMoving)
            {
                piecesWereMoving = false;
                ChangeTurn();
                StopAllPieces();
                OnNextTurn();
            }
        }
    }

    private void ChangeTurn()
    {
        int nextTurn = (int)currentTurn * -1;
        currentTurn = (Team)nextTurn;
    }

    private bool PiecesStoppedMoving(GameObject[] pieces)
    {
        foreach (GameObject piece in pieces)
        {
            PieceMovement pieceMovement = piece.GetComponent<PieceMovement>();
            if (pieceMovement.IsMoving)
            {
                return false;
            }
        }
        return true;
    }

    public void StopAllPieces()
    {
        foreach (GameObject piece in allPieces)
        {
            Rigidbody rb = piece.GetComponent<Rigidbody>();
            rb.Sleep();
        }
    }

    public void EnablePieceInteraction(GameObject[] pieces, bool isLocal)
    {
        if (!isLocal)
        {
            return;
        }

        foreach (GameObject piece in pieces)
        {
            PieceInteraction pieceInteraction = piece.GetComponent<PieceInteraction>();
            pieceInteraction.enabled = true;
        }
    }

    public void DisablePieceInteraction(GameObject[] pieces)
    {
        foreach (GameObject piece in pieces)
        {
            PieceInteraction pieceInteraction = piece.GetComponent<PieceInteraction>();
            pieceInteraction.enabled = false;
        }
    }

    public void ReenablePieceInteraction()
    {
        if (currentTurn.Equals(Team.TeamOne))
        {
            EnablePieceInteraction(TeamOnePieces, PlayerOne.IsLocal);
        }
        else
        {
            EnablePieceInteraction(TeamTwoPieces, PlayerTwo.IsLocal);
        }
    }

    public void IlluminatePieces(GameObject[] pieces)
    {
        foreach (GameObject piece in pieces)
        {
            PieceAnimation pieceAnimation = piece.GetComponent<PieceAnimation>();
            pieceAnimation.IlluminatePieceLight();
        }
    }

    public void DarkenPieces(GameObject[] pieces)
    {
        foreach (GameObject piece in pieces)
        {
            PieceAnimation pieceAnimation = piece.GetComponent<PieceAnimation>();
            pieceAnimation.DarkenPieceLight();
        }
    }

    public void OnNextTurn()
    {
        if (currentTurn.Equals(Team.TeamOne))
        {
            DarkenPieces(TeamTwoPieces);
            DisablePieceInteraction(TeamTwoPieces);
            IlluminatePieces(TeamOnePieces);
            EnablePieceInteraction(TeamOnePieces, PlayerOne.IsLocal);
        }
        else
        {
            DarkenPieces(TeamOnePieces);
            DisablePieceInteraction(TeamOnePieces);
            IlluminatePieces(TeamTwoPieces);
            EnablePieceInteraction(TeamTwoPieces, PlayerTwo.IsLocal);
        }
    }

    public void ResetAllPiecesToStart()
    {
        for (int i = 0; i < allPieces.Length; i++)
        {
            allPieces[i].transform.position = startingPositions[i];
        }
    }

    private Color GetTeamColor(Team team)
    {
        return team.Equals(Team.TeamOne) ? GameData.TeamOneColor : GameData.TeamTwoColor;
    }

    public void OnGoalScored(Team scoringTeam)
    {
        goalScored = true;
        Color color = GetTeamColor(scoringTeam);
        Confetti.Play(color);
        Scoreboard.DisplayMessage("Goal!");
        StopAllPieces();
        AddToScore(scoringTeam);
        Scoreboard.UpdateScoreboard(teamOneScore, teamTwoScore);
        Time.timeScale = 0f;
    }

    public void OnPostGoal()
    {
        goalScored = false;
        if (endGame)
        {
            LevelTransition.FadeToNextLevel();
        }

        if (teamOneScore >= scoreToWin)
        {
            OnWin(GameData.TeamOneName);
        }
        else if (teamTwoScore >= scoreToWin)
        {
            OnWin(GameData.TeamTwoName);
        }
        else
        {
            Confetti.Stop();
            Scoreboard.HideMessage();
            ResetAllPiecesToStart();
            Time.timeScale = 1f;
        }
    }

    public void OnWin(string winningTeam)
    {
        endGame = true;
        StartCoroutine(Scoreboard.ShuffleMessage($"{winningTeam} wins!"));
    }

    private void AddToScore(Team scoringTeam)
    {
        if (scoringTeam.Equals(Team.TeamOne))
        {
            teamOneScore = teamOneScore + 1;
        }
        else
        {
            teamTwoScore = teamTwoScore + 1;
        }
    }

    public void ResetScores()
    {
        teamOneScore = 0;
        teamTwoScore = 0;
        Scoreboard.UpdateScoreboard(teamOneScore, teamTwoScore);
    }

    public void SetUnpaused()
    {
        wasPaused = false;
    }
}
