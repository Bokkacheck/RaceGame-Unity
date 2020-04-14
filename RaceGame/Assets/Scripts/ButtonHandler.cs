using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class ButtonHandler : MonoBehaviour
{
    private NetworkManager networkManager;
    public bool stop = false;
    public void BackTomenuFromCreation()
    {
        SceneManager.LoadScene("menu");
    }
    public void BackToMenu()
    {
        if (stop)
        {
            return;
        }
        PlayerNetworking.numberOfPlayers = 0;
        networkManager = NetworkManager.singleton;
        MatchInfo match = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(match.networkId, match.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopClient();
        networkManager.StopHost();
        stop = true;
    }
}
