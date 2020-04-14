using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public int carNum;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        carNum = 1; 
    }
    public void izborAutomobila(int x)
    {
        carNum = x;
    }
}
