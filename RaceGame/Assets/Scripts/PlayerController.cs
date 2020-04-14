using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    WheelCollider wcLF;
    [SerializeField]
    WheelCollider wcLB;
    [SerializeField]
    WheelCollider wcRF;
    [SerializeField]
    WheelCollider wcRB;
    [SerializeField]
    TrailRenderer tRR;
    [SerializeField]
    TrailRenderer tRL;
    [SerializeField]
    GameObject[] tires;
    [SerializeField]
    ParticleSystem psFire;
    [SerializeField]
    float torque = 1000.0f;
    [SerializeField]
    float steer = 40.0f;
    [SerializeField]
    float brake = 3000.0f;

    [SerializeField]
    Camera prim;
    [SerializeField]
    Camera sec;

    Speedometer spdMtr;
    RpmMeter rpmMeter;
    Rigidbody rb;
    bool primCam = true;
    bool direction = true;
    float forward;
    public float rpm = 800;

    [SerializeField]
    SpriteRenderer posIcon;

    public float Torque { get => torque; }
    public float Steer { get => steer; }
    public float BrakeTrq { get => brake; }

    void Start()
    {
        tRL.enabled = false;
        tRR.enabled = false;
        if (!isLocalPlayer)
        {
            posIcon.color = Color.red;
            return;
        }
        else
        {
            posIcon.color = Color.green;
        }
        spdMtr = GameObject.Find("Speedometer").GetComponent<Speedometer>();
        rpmMeter = GameObject.Find("RpmMeter").GetComponent<RpmMeter>();
        rb = GetComponent<Rigidbody>();
        prim.gameObject.SetActive(true);
        sec.gameObject.SetActive(false);
      
    }
    void FixedUpdate()
    {
        Movement();
        Brake();
        AnimateWheel();
        RpmParticles();
        spdMtr.Speed = rb.velocity.magnitude*3.6f;  //"m/s" -> "km/h"
    }
    void LateUpdate()
    {
        CameraSwitch();
    }
    private void Movement()
    {
        forward = Input.GetAxis("Vertical");
        float steering = Input.GetAxis("Horizontal");
        if(forward<0 && (rb.velocity.magnitude*3.6) > 38)
        {
            forward = 0f;
        }
        wcLB.motorTorque = forward * torque;
        wcRB.motorTorque = forward * torque;
        if (steering != 0)
        {
            wcLF.steerAngle = steering * steer;
            wcRF.steerAngle = steering *steer;
        }
    }
    private void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey("joystick button 0"))
        {
            wcRB.brakeTorque = brake;
            wcLB.brakeTorque = brake;
            if (wcRB.isGrounded)
            {
                tRR.enabled = true;
            }
            else
            {
                tRR.enabled = false;
            }
            if (wcLB.isGrounded)
            {
                tRL.enabled = true;
            }
            else
            {
                tRL.enabled = false;
            }
        }
        else
        {
            wcRB.brakeTorque = 0;
            wcLB.brakeTorque = 0;
            tRL.enabled = false;
            tRR.enabled = false;
            tRL.Clear();
            tRR.Clear();
        }
    }
    private void AnimateWheel()
    {
        Quaternion quat;
        Quaternion quat2;
        Vector3 position;
        Vector3 position2;
        wcLF.GetWorldPose(out position, out quat);
        wcLB.GetWorldPose(out position2, out quat2);
        tires[0].transform.rotation = quat;
        tires[1].transform.rotation = quat2;
        tires[2].transform.rotation = quat;
        tires[3].transform.rotation = quat2;
    }
    private void RpmParticles()
    {
        rpm = 800;
        string gear = "";
        float speed = rb.velocity.magnitude * 3.6f;
        if (speed < 50)
        {
            gear = "1";
            rpm += speed * 6500 / 50.0f;
        }
        else if (speed < 90)
        {
            gear = "2";
            rpm += speed * 6500 / 90.0f;
        }
        else if (speed < 130)
        {
            gear = "3";
            rpm += speed * 6500 / 130.0f;
        }
        else if (speed < 170)
        {
            gear = "4";
            rpm += speed * 6500 / 170.0f;
        }
        else
        {
            gear = "5";
            rpm += speed * 7000 / 240.0f;
        }
        if (forward< 0 && speed < 5)
        {
            direction = false;

        }else if(forward > 0 &&  speed < 5)
        {
            direction = true;
        }
        if (!direction)
        {
            gear = "R";
        }
        if(speed<1)
        {
            rpm = 800;
            gear = "N";
        }
        rpmMeter.Speed = rpm;
        rpmMeter.Gear = gear;
        if (rpm > 7200)
        {
            if (!psFire.isPlaying)
            {
                psFire.Play();
            }
        }
    }

    private void CameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown("joystick button 1"))
        {
            if (primCam)
            {
                prim.gameObject.SetActive(false);
                sec.gameObject.SetActive(true);
                primCam = false;
            }
            else
            {
                prim.gameObject.SetActive(true);
                sec.gameObject.SetActive(false);
                primCam = true;
            }
        }
    }

}
