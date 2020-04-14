using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using TMPro;

public class PlayerNetworking : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componnets;
    private Camera tmpCamera;
    [SerializeField]
    private GameObject sound;

    private GameObject lights;
    private Light sun;
    private Light blue;
    [SerializeField]
    private Material night;

    static PlayerController plc;
    public static int numberOfPlayers = 0;
    public TextMeshProUGUI winLoseText;
    public Canvas remotePlayerCanvas;
    void Start()
    {
        numberOfPlayers++;
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componnets.Length; i++)
            {
                componnets[i].enabled = false;
            }
            sound.SetActive(false);
            remotePlayerCanvas = transform.Find("PlayerCanvas").GetComponent<Canvas>();
        }
        else
        {
            plc = GetComponent<PlayerController>();
            plc.enabled = false;
            GameObject.Find("Semaphore").GetComponent<Semaphore>().plc = plc;
            tmpCamera = Camera.main;
            tmpCamera.gameObject.SetActive(false);
            lights = GameObject.Find("Lights");
            sun = GameObject.Find("Sun").GetComponent<Light>();
            blue = GameObject.Find("BlueLight").GetComponent<Light>();
            CmdCheckForMap();
            transform.name = PlayerPrefs.GetString("name");
        }
        if (numberOfPlayers == 2)
        {
            GameObject.Find("Semaphore").GetComponent<Semaphore>().StartSemaphore();
        }
    }
    void OnDisable()
    {
        if (isLocalPlayer && tmpCamera!=null)
        {
            tmpCamera.gameObject.SetActive(true);
        }
    }
    [Command]
    public void CmdCheckForMap()
    {
        RpcSetMapType(HostGame.mapType);
    }
    [ClientRpc]
    public void RpcSetMapType(string mapType)
    {
        if (isLocalPlayer)
        {
            if (mapType == "day")
            {
                lights.SetActive(false);
            }
            else
            {
                RenderSettings.skybox = night;
                sun.enabled = false;
                blue.intensity = blue.intensity / 2.0f;
            }
        }
    }
    void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            if (plc != null)
            {
                remotePlayerCanvas.transform.rotation = plc.gameObject.transform.rotation;
            }
        }
    }
}
