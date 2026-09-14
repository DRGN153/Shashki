using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] Button startGameButton;
    [SerializeField] Text[] playerNameTexts = new Text[2];

    void ClientHandleInfoUpdated()
    {
        List<PlayerNetwork> players=((CheckersNetworkManager)NetworkManager.singleton).NetworkPlayers;
        for (int i = 0;i<players.Count;i++)
            playerNameTexts[i].text = players[i].DisplayName;
        for (int i = players.Count; i < playerNameTexts.Length; i++)
            playerNameTexts[i].text = "}|{geM urpoka...";
        startGameButton.interactable = players.Count >= 2;
    }
    void Start()
    {
        PlayerNetwork.ClientOnInfoUpdated += ClientHandleInfoUpdated;
        PlayerNetwork.AuthorityOnLobbyOwnerStateUpdated += AuthorityHandleLobbyOwnerStateUpdated;
    }
    void AuthorityHandleLobbyOwnerStateUpdated(bool state)
    {
        startGameButton.gameObject.SetActive(state);
    }
    void OnDestroy()
    {
        PlayerNetwork.AuthorityOnLobbyOwnerStateUpdated -= AuthorityHandleLobbyOwnerStateUpdated;
        PlayerNetwork.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
    }
    public void StartGame()
    {
        NetworkManager.singleton.ServerChangeScene("Game Scene");
    }
}
