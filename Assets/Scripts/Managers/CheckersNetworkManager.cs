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
        player.IsWhite = numPlayers == 1;
        player.DisplayName = player.IsWhite ? "CBET/\b||/|" : "TEMHb||/|";
        }
}
