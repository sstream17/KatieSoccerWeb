using Mirror;
using UnityEngine;

/*
	Documentation: https://mirror-networking.com/docs/Components/NetworkManager.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class KatieSoccerNetworkManager : NetworkManager
{
    public Transform[] TeamOneSpawns;
    public Transform[] TeamTwoSpawns;

    private int? playerOneConnectionId;
    private int? playerTwoConnectionId;

    #region Server System Callbacks

    /// <summary>
    /// Called on the server when a new client connects.
    /// <para>Unity calls this on the Server when a Client connects to the Server. Use an override to tell the NetworkManager what to do when a client connects to the server.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
    }

    /// <summary>
    /// Called on the server when a client is ready.
    /// <para>The default implementation of this function calls NetworkServer.SetClientReady() to continue the network setup process.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
    }

    /// <summary>
    /// Called on the server when a client adds a new player with ClientScene.AddPlayer.
    /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (numPlayers == 0)
        {
            // Spawn player one
            playerOneConnectionId = conn.connectionId;
            var player = Instantiate(playerPrefab);

            var transforms = player.GetComponentsInChildren<Transform>();

            for (var i = 0; i < transforms.Length; i++)
            {
                transform.position = TeamOneSpawns[i].position;
            }
        }
        else if (numPlayers == 1 && playerOneConnectionId.HasValue)
        {
            // Spawn player two
            playerTwoConnectionId = conn.connectionId;

            var player = Instantiate(playerPrefab);

            var transforms = player.GetComponentsInChildren<Transform>();

            for (var i = 0; i < transforms.Length; i++)
            {
                transform.position = TeamTwoSpawns[i].position;
            }
        }
        else if (numPlayers == 1 && playerTwoConnectionId.HasValue)
        {
            // Spawn player one
            playerOneConnectionId = conn.connectionId;

            var player = Instantiate(playerPrefab);

            var transforms = player.GetComponentsInChildren<Transform>();

            for (var i = 0; i < transforms.Length; i++)
            {
                transform.position = TeamOneSpawns[i].position;
            }
        }
    }

    /// <summary>
    /// Called on the server when a client disconnects.
    /// <para>This is called on the Server when a Client disconnects from the Server. Use an override to decide what should happen when a disconnection is detected.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (playerOneConnectionId.HasValue && playerOneConnectionId.Equals(conn.connectionId))
        {
            // Remove player one
            playerOneConnectionId = null;
        }
        else if (playerTwoConnectionId.HasValue && playerTwoConnectionId.Equals(conn.connectionId))
        {
            // Remove player two
            playerTwoConnectionId = null;
        }
    }

    #endregion

    #region Client System Callbacks

    /// <summary>
    /// Called on the client when connected to a server.
    /// <para>The default implementation of this function sets the client as ready and adds a player. Override the function to dictate what happens when the client connects.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }

    /// <summary>
    /// Called on clients when disconnected from a server.
    /// <para>This is called on the client when it disconnects from the server. Override this function to decide what happens when the client disconnects.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
    }

    /// <summary>
    /// Called on clients when a servers tells the client it is no longer ready.
    /// <para>This is commonly used when switching scenes.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public override void OnClientNotReady(NetworkConnection conn) { }

    #endregion

    #region Start & Stop Callbacks

    // Since there are multiple versions of StartServer, StartClient and StartHost, to reliably customize
    // their functionality, users would need override all the versions. Instead these callbacks are invoked
    // from all versions, so users only need to implement this one case.

    /// <summary>
    /// This is invoked when a host is started.
    /// <para>StartHost has multiple signatures, but they all cause this hook to be called.</para>
    /// </summary>
    public override void OnStartHost() { }

    /// <summary>
    /// This is invoked when a server is started - including when a host is started.
    /// <para>StartServer has multiple signatures, but they all cause this hook to be called.</para>
    /// </summary>
    public override void OnStartServer() { }

    /// <summary>
    /// This is invoked when the client is started.
    /// </summary>
    public override void OnStartClient() { }

    /// <summary>
    /// This is called when a host is stopped.
    /// </summary>
    public override void OnStopHost() { }

    /// <summary>
    /// This is called when a server is stopped - including when a host is stopped.
    /// </summary>
    public override void OnStopServer() { }

    /// <summary>
    /// This is called when a client is stopped.
    /// </summary>
    public override void OnStopClient() { }

    #endregion
}
