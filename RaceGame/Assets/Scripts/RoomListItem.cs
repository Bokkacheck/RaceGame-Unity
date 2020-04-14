using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    public delegate void JoinRoomDelegate(MatchInfoSnapshot match);
    private JoinRoomDelegate joinRoomDelegate;
    private MatchInfoSnapshot match;
    [SerializeField]
    private Text roomNameText;
    public void Setup(MatchInfoSnapshot m, JoinRoomDelegate jrd)
    {
        match = m;
        joinRoomDelegate = jrd;
        roomNameText.text = match.name + " (" + match.currentSize + " / " + match.maxSize + " )";
    }
    public void JoinGame()
    {
        Debug.Log("KLIKNUTO");
        joinRoomDelegate.Invoke(match);
    }
}
