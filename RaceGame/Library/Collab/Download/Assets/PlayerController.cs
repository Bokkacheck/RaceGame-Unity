using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    GameObject[] Tires;

    void Start()
    {
        tRL.enabled = false;
        tRR.enabled = false;
        Tires = GameObject.FindGameObjectsWithTag("Tires");
    }
    void FixedUpdate()
    {
        float forward = Input.GetAxis("Vertical");
        float steering = Input.GetAxis("Horizontal");

        wcLB.motorTorque = forward * 1000;
        wcRB.motorTorque = forward * 1000;
        if (steering != 0) {
            wcLF.steerAngle = steering * 30;
            wcRF.steerAngle = steering * 30;
        }

        Quaternion quat;
        Quaternion quat2;
        Vector3 position;
        Vector3 position2;
        wcLF.GetWorldPose(out position, out quat);
        wcLB.GetWorldPose(out position2, out quat2);
        Tires[0].transform.rotation = quat;
        Tires[1].transform.rotation = quat2;
        Tires[2].transform.rotation = quat;
        Tires[3].transform.rotation = quat2;



        if (Input.GetKey(KeyCode.Space))
        {
            wcRB.brakeTorque = 3000;
            wcLB.brakeTorque = 3000;
            RaycastHit hit;
            if (wcRB.attachedRigidbody.SweepTest(-wcRB.transform.up, out hit, 1.0f))
            {
                tRL.enabled = true;
            }
            else
            {
                tRL.enabled = false;
            }
            if(wcRB.attachedRigidbody.SweepTest(-wcLB.transform.up,out hit,1.0f))
            {
                tRR.enabled = true;
            }
            else
            {
                tRR.enabled = false;
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

}
