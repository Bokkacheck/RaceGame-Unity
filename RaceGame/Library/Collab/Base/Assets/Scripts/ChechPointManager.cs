using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class ChechPointManager : NetworkBehaviour
{
    private Color checkedCP = new Color(0.5764706f, 1, 0.4941176f, 1);
    private Color nextCP = new Color(1, 0.5764706f, 0.4941176f, 1);

    private Text LapTimer;
    private Text CPTimes;

    public Rigidbody rb;

    public float time;
    public float lapTime;
    private TimeSpan vreme;
    private bool startstop = false;
    private GameObject[] checkpoints = new GameObject[6];
    private int cpCount = 0;

    private  Vector3 savedPos;
    private Quaternion savedRot;
    private Vector3 spawnPos;
    private Quaternion spawnRot;

    public Animator animator;

    private GameObject WinnerSpawn;
    private GameObject LoserSpawn;

    
    private GameObject game_Canvas;
    private static GameObject endUI;

    public TextMeshProUGUI winLoseTxt;

    private static Dictionary<String, String> playerTime;

    public bool Startstop { get => startstop; set => startstop = value; }
    public static ChechPointManager player;

    private static string winnerText;
    private static string loserText;

    void Start()
    {
        game_Canvas = GameObject.Find("GameUI");
        LapTimer = GameObject.Find("LapTimer").GetComponent<Text>();
        CPTimes = GameObject.Find("CPTimes").GetComponent<Text>();
        WinnerSpawn = GameObject.Find("WinnerSpawn");
        LoserSpawn = GameObject.Find("LoserSpawn");
        for (int i = 0; i < 6; i++)
        {
            checkpoints[i] = GameObject.Find("CP" + (i + 1));
        }
        checkpoints[cpCount].GetComponent<SpriteRenderer>().color = nextCP;
        Debug.Log(GetComponent<Rigidbody>());
        savedPos = spawnPos = transform.position;
        savedRot = spawnRot = transform.rotation;
        if (isLocalPlayer)
        {
            endUI = GameObject.Find("EndUI");
            endUI.GetComponent<Canvas>().enabled = true;
            endUI.SetActive(false);
            playerTime = new Dictionary<string, string>();
            player = this;
        }
    }
    private void FixedUpdate()
    {
        CountTime();
    }
    private void CountTime()
    {
        if (startstop)
        {
            time = Time.time - lapTime;
            vreme = TimeSpan.FromSeconds(time);
            LapTimer.text = vreme.ToString("mm\\:ss\\:ff");
        }
    }
    private void LateUpdate()
    {
        ResetPositon();
    }
    void ResetPositon()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            transform.SetPositionAndRotation(savedPos + new Vector3(0, 1.5f, 0), savedRot);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer || !startstop)
        {
            return;
        }
        if (other.CompareTag("CheckPoint"))
        {
            if (checkpoints[cpCount].GetComponent<BoxCollider>().Equals(other))
            {
                savedPos = other.transform.position;
                savedRot = other.transform.rotation;
                other.GetComponent<AudioSource>().Play();
                other.GetComponent<SpriteRenderer>().color = checkedCP;
                other.enabled = false;
                CPTimes.text += other.name + ": " + vreme.ToString("mm\\:ss\\:ff") + Environment.NewLine;
                cpCount++;
                if (cpCount < 6)
                {
                    checkpoints[cpCount].GetComponent<SpriteRenderer>().color = nextCP;
                }
            }
        }
        if (other.CompareTag("NewLap") && checkpoints.Length == cpCount)
        {
            CPTimes.text += "Final: " + vreme.ToString("mm\\:ss\\:ff") + Environment.NewLine;
            startstop = false;
            savedRot = spawnRot;
            savedPos = spawnPos;
            other.enabled = false;
            
            CmdFinished(transform.name, vreme.ToString("mm\\:ss\\:ff"));
        }

    }
    [Command]//Poziva original
    void CmdFinished(string name,string vreme)
    {
        RpcSomeoneFinished(name, vreme);
    }
    [ClientRpc]//Moze se izvrsiti i na kopiji 
    void RpcSomeoneFinished(string name,string vreme)
    {
        Debug.Log(name + " " + vreme);
        playerTime.Add(name, vreme);
        if (playerTime.Count == 1 && transform.name == name) //Ja sam pobedio
        {
            if (isLocalPlayer)
            {
                animator.SetTrigger("FadeOut");
            }
            winnerText = "WINNER: " + name + " " + vreme;
            RaceEnd(1);
        }
        else if(playerTime.Count == 1 && transform.name!= name)//Drugi igrac je pobedio, imam jos 15 sec pa dolazim ovde kao gubitnik
        {
            RaceEnd(1);
            winnerText = "WINNER: " + name + " " + vreme;
            StartCoroutine(player.WaitForEnd());
        }
        else if(playerTime.Count==2)//Neko je izgubio
        {
            if (isLocalPlayer)
            {
                animator.SetTrigger("FadeOut");
            }
            RaceEnd(2);
            loserText = "LOSER: " + name + " " + vreme;
            StartCoroutine(player.ShowResult());
        }
    }
    IEnumerator WaitForEnd()    //15 sek ako se ne zavrsi nasilno se javlja serveru da je igrac zavrsio i ocitava mu se to vreme
    {
        if (!isLocalPlayer)
        {
            yield return new WaitForSeconds(1);
        }
        int counter = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            counter++;
            if(playerTime.Count == 2)
            {
                break;
            }
            if (counter == 15)
            {
                Debug.Log("Korutina 15sec");
                startstop = false;
                CmdFinished(transform.name, vreme.ToString("mm\\:ss\\:ff"));
                break;
            }
        }
    }
    public void RaceEnd(int place)
    {
        if (isLocalPlayer)
        {
            animator.SetTrigger("FadeIn");
            game_Canvas.gameObject.SetActive(false);
        }
        //   if (rb != null)
        //   {
        Debug.Log(rb);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (place == 1)
            {
                rb.position = WinnerSpawn.transform.position;
                rb.rotation = WinnerSpawn.transform.rotation;
                winLoseTxt.text = "Winner";
            }
            else if (place == 2)
            {
                rb.position = LoserSpawn.transform.position;
                rb.rotation = LoserSpawn.transform.rotation;
                winLoseTxt.text = "Loser";
            }
 //       }
    }
    public IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(2);
        if (isLocalPlayer)
        {
            winLoseTxt.text = "";
        }
        endUI.SetActive(true);
        GameObject panel = endUI.transform.Find("Panel").gameObject;
        Text winner = panel.transform.Find("winnerText").GetComponent<Text>();
        Text loser = panel.transform.Find("loserText").GetComponent<Text>();
        winner.text = winnerText;
        loser.text = loserText;
    }
}
