using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
public class JoinGame : MonoBehaviour
{
    private NetworkManager networkManager;
    List<GameObject> roomList = new List<GameObject>();
    [SerializeField]
    private Text status;
    [SerializeField]
    private GameObject roomLisItemPrefab;
    [SerializeField]
    private Transform roomListParent;
    [SerializeField]
    private GameObject meniUI;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
        RefreshRoomList();
    }
    public void RefreshRoomList()
    {
        ClearRoomList();
        networkManager.matchMaker.ListMatches(0, 20, "",false,0,0, OnMatchList);
        status.text = "Loading...";
    }
    public void OnMatchList(bool success,string extendedInfo,List<MatchInfoSnapshot> matches)
    {
        status.text = "";
        if(matches == null)
        {
            status.text = "Couldn't get room list";
        }
        foreach(MatchInfoSnapshot match in matches)
        {
            GameObject roomListItemGO = Instantiate(roomLisItemPrefab);
            roomListItemGO.transform.SetParent(roomListParent);
            RoomListItem roomListItem = roomListItemGO.GetComponent<RoomListItem>();
            if (roomListItem != null)
            {
                roomListItem.Setup(match,JoinRoom);
            }
            roomList.Add(roomListItemGO);
        }
        if (roomList.Count == 0)
        {
            status.text = "No rooms at the moment";
        }
        else
        {
            status.text = "List of available rooms";
        }

    }
    private void ClearRoomList()
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }

        roomList.Clear();
    }
    public void JoinRoom(MatchInfoSnapshot match)
    {
        networkManager.matchMaker.JoinMatch(match.networkId, "","","",0,0, networkManager.OnMatchJoined);
        ClearRoomList();
        status.text = "Joining...";
        meniUI.SetActive(false);
    }

}
