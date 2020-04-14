using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaphore : MonoBehaviour
{
    [SerializeField]
    private Light[] zeleno;
    [SerializeField]
    private Light[] zuto;
    [SerializeField]
    private Light[] crveno;
    private int brojac = 0;
    public PlayerController plc;
    [SerializeField]
    private AudioClip Countdown;
    [SerializeField]
    private AudioClip Go;
    private AudioSource audiosource;


    void Start()
    {
        Debug.Log("SEMAFOR POCETAK");
        audiosource = gameObject.GetComponent<AudioSource>();
        for (int i = 0; i < zeleno.Length; i++)
        {
            zeleno[i].enabled = crveno[i].enabled = zuto[i].enabled;
        }
    }
    public void StartSemaphore()
    {
        Debug.Log("START");
        StartCoroutine(Semafor());
    }
    IEnumerator Semafor()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            brojac++;
            if (brojac == 3)
            {
                for (int i = 0; i < zeleno.Length; i++)
                {
                    crveno[i].enabled = true;
                    audiosource.clip = Countdown;
                    audiosource.Play();
                }
            }
            else if(brojac == 5){
                for (int i = 0; i < zeleno.Length; i++)
                {
                    crveno[i].enabled = false;
                    zuto[i].enabled = true;
                    audiosource.clip = Countdown;
                    audiosource.Play();
                }
            }
            else if (brojac == 7)
            {
                for (int i = 0; i < zeleno.Length; i++)
                {
                    zuto[i].enabled = false;
                    zeleno[i].enabled = true;
                    audiosource.clip = Go;
                    audiosource.Play();
                }
                StartRace();
                break;
            }
        }
    }
    public void StartRace()
    {
        plc.enabled = true;
        plc.gameObject.GetComponent<ChechPointManager>().lapTime = Time.time;
        plc.gameObject.GetComponent<ChechPointManager>().Startstop = true;
        return;
    }
}
