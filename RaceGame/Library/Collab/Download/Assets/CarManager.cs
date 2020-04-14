using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarManager : MonoBehaviour
{
    public int carNum;
    public string ime;
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        carNum = 1; 
    }
    public void setIme(Text txt)
    {
        ime = txt.text;
        if (ime.Trim() == "")
            ime = "Player" + Random.Range(1000, 10000);
    }
    public void carNext()
    {
        carNum++;
    }
    public void carPrev()
    {
        carNum--;
    }
}
