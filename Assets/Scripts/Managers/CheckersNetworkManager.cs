using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckersNetworkManager : NetworkManager
{
    public static event Action ClientOnConnected;
    [SerializeField] GameObject gameOverHandlerPrefab, boardPrefab, 
        turnsHandlerPrefab;
    public List<PlayerNetwork>NetworkPlayers{ get;}= new List<PlayerNetwork>();
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientOnConnected?.Invoke();
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
        {
        GameObject playerInstance = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, playerInstance);
        var player = playerInstance.GetComponent<PlayerNetwork>();
        NetworkPlayers.Add(player);
        player.LobbyOwner=player.IsWhite = numPlayers == 1;
        player.DisplayName = player.IsWhite ? "CBETëb||/|" : "TEMHb||/|";
        }
    public override void OnStartServer()
    {
        var boardInstance = Instantiate(boardPrefab);
        NetworkServer.Spawn(boardInstance);
        var turnsHandlerInstance=Instantiate(turnsHandlerPrefab);
        NetworkServer.Spawn(turnsHandlerInstance);
    }
    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Game"))
        {
            var gameOverHandlerInstance= Instantiate(gameOverHandlerPrefab);
            NetworkServer.Spawn(gameOverHandlerInstance);
        }
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        var player=conn.identity.GetComponent<PlayerNetwork>();
        NetworkPlayers.Remove(player);
        base.OnServerDisconnect(conn);
    }
    public override void OnStopServer()
    {
        NetworkPlayers.Clear();
    }

    public override void OnClientDisconnect()
    {
        base .OnClientDisconnect();
        SceneManager.LoadScene("Lobby Scene");
        Destroy(gameObject);
    }
}
