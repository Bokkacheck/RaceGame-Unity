using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
    public WheelCollider WheelL;
    public WheelCollider WheelR;
    private double AntiRoll = 5000.0;


    void FixedUpdate()
    {
        WheelHit hit;
        double travelL = 1.0;
        double travelR = 1.0;

        bool groundedL = WheelL.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;

        bool groundedR = WheelR.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;

        double antiRollForce = (travelL - travelR) * AntiRoll;

        if (groundedL)
            GetComponent<Rigidbody>().AddForceAtPosition(WheelL.transform.up * (int)-antiRollForce,WheelL.transform.position);
        if (groundedR)
            GetComponent<Rigidbody>().AddForceAtPosition(WheelR.transform.up * (int)antiRollForce,WheelR.transform.position);
    }
   
}
