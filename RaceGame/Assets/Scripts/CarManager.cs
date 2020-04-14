using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarManager : MonoBehaviour
{
    public int carNum;
    public string playerName;
    // Start is called before the first frame update
    void Start()
    {
        carNum = 1;
        PlayerPrefs.SetInt("car", 1);
        PlayerPrefs.Save();
    }
    public void setIme(Text txt)
    {
        playerName = txt.text;
        if (playerName.Trim() == "")
        {
            playerName = "Player" + Random.Range(1000, 10000);
        }
        PlayerPrefs.SetString("name", playerName);
        PlayerPrefs.Save();
    }
    public void carNext()
    {
        carNum++;
        PlayerPrefs.SetInt("car", carNum);
        PlayerPrefs.Save();
    }
    public void carPrev()
    {
        carNum--;
        PlayerPrefs.SetInt("car", carNum);
        PlayerPrefs.Save();
    }
}
