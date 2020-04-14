using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutomobilManager : MonoBehaviour
{
    public GameObject[] cars;
    public GameObject platforma;
    public Button next;
    public Button prev;
    public Text Od;
    public Text Do;
    public Text motor;
    public Text steering;
    public Text brake;
    public Image motorImgFrnt;
    public Image steeringImgFrnt;
    public Image brakeImgFrnt;

    public int carSelected;

    void Start()
    {
        prev.enabled = false;
        carSelected = 0;
        Od.text = carSelected + 1 + "";
        Do.text = cars.Length + "";
        aktiviraj(carSelected);
    }
    public void carNext()
    {
        prev.enabled = true;
        carSelected++;
        if(carSelected==cars.Length-1)
            next.enabled = false;
        aktiviraj(carSelected);
    }
    public void carPrev()
    {
        next.enabled = true;
        carSelected--;
        if (carSelected == 0)
            prev.enabled = false;
        aktiviraj(carSelected);
    }
    private void aktiviraj(int n)
    {
        PlayerController pc=null;
        for (int i = 0; i < cars.Length; i++)
        {
            if (i == n)
            {
                cars[i].SetActive(true);
                pc = cars[i].GetComponent<PlayerController>();
            }
            else
                cars[i].SetActive(false);
        }
        Od.text = n + 1 + "";
        upisiPodatke(pc);
    }
    private void upisiPodatke(PlayerController pc)
    {
        if (pc != null) {
            motor.text = (int)pc.Torque + "";
            steering.text = (int)pc.Steer + "";
            brake.text = (int)pc.BrakeTrq + "";
            motorImgFrnt.fillAmount = pc.Torque / 1500.0f;
            steeringImgFrnt.fillAmount = pc.Steer / 50.0f;
            brakeImgFrnt.fillAmount = pc.BrakeTrq / 3500.0f;
        }
        else
        {
            motor.text = "NaN";
            steering.text = "NaN";
            brake.text = "NaN";
        }
        
    }
    private void FixedUpdate()
    {
        platforma.transform.Rotate(Vector3.up,10*Time.deltaTime);
    }
}
