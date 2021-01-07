using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RK4 : MonoBehaviour
{
    [SerializeField] private Vector3 Impulse;
    [SerializeField] private Vector3 Gravity;
    [SerializeField] private Vector3 Acceleration;
    [SerializeField] private Vector3 Force;
    [SerializeField] private Vector3 Velocity;
    [SerializeField] private Vector3 LastPosition;
    [SerializeField] private float Mass;
    [SerializeField] private float k1 = 0f;
    [SerializeField] private float k2 = 0f;
    [SerializeField] private float k3 = 0f;
    [SerializeField] private float k4 = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Gravity = new Vector3(0f, -9.81f, 0f);
        Force = Impulse;
    }

    // Update is called once per frame
    void Update()
    {
        Force += Gravity * Time.deltaTime;                      //An+1
        Acceleration = Force / Mass;
        Velocity = new Vector3(RKOrder4(Acceleration.x * Time.deltaTime),
            RKOrder4(Acceleration.y * Time.deltaTime),
            RKOrder4(Acceleration.z * Time.deltaTime));

        this.transform.position += Velocity;
    }

    float RKOrder4(float argAxisForce)
    {

        k1 = Time.deltaTime * dydt(Time.time, argAxisForce); //eulers method
        k2 = Time.deltaTime * dydt(Time.time + 0.5f * Time.deltaTime, argAxisForce + 0.5f * k1); //mid foint of force and k1
        k3 = Time.deltaTime * dydt(Time.time + 0.5f * Time.deltaTime, argAxisForce + 0.5f * k2); //slope of y and k2
        k4 = Time.deltaTime * dydt(Time.time + Time.deltaTime, argAxisForce + k3); //slope of y and k3

        return (argAxisForce += ((1f / 6f) * Time.deltaTime) * (k1 + (2f * k2) + (2f * k3) + k4));
    }

    float dydt(float argT, float argY)
    {
        if (argT == 0) argT = 1; //fix for initial update tick where Time.time == 0
        return (argY * argY - argT * argT) / (argY * argY + argT * argT);
    }
}


