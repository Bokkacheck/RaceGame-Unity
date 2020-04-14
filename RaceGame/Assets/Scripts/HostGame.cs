using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HostGame : MonoBehaviour
{
    public static string mapType;
    private uint roomSize = 2;
    private string roomName;
    private NetworkManager networkManager;
    [SerializeField]
    Canvas gameUI;
    [SerializeField]
    Canvas meniUI;
    [SerializeField]
    Light mainLight;
    [SerializeField]
    GameObject[] lights;
    public string RoomName { get => roomName; set => roomName = value; }
    void Start()
    {
        lights = GameObject.FindGameObjectsWithTag("Lights");
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }

    public void CreateDayGame()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
        mapType = "day";
        CreateRoom();
    }
    public void CreatNightGame()
    {
        mapType = "night";
        CreateRoom();
    }
    public void CreateRoom()
    {
        roomName = GameObject.Find("RoomNameCreate").GetComponent<InputField>().text;
        if(roomName != null && roomName != "")
        {
            Debug.Log("Creating room" + roomName + " room size:" + roomSize);
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "","","",0,0, networkManager.OnMatchCreate);
            gameUI.gameObject.SetActive(true);
            meniUI.gameObject.SetActive(false);
        }
    }
}
