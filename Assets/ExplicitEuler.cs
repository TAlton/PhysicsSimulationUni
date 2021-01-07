using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplicitEuler : MonoBehaviour
{
    [SerializeField] private Vector3 Impulse;
    [SerializeField] private Vector3 Gravity;
    [SerializeField] private Vector3 Acceleration;
    [SerializeField] private Vector3 Force;
    [SerializeField] private Vector3 Velocity;
    [SerializeField] private float Mass;
    // Start is called before the first frame update
    void Start()
    {
        Gravity = new Vector3(0f, -9.18f, 0f);
        Force = Impulse + Gravity; //An
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lastVelocity = Velocity;
        Acceleration = Force / Mass;                            //a=f/m
        Velocity += Acceleration * Time.deltaTime;              //Vn+1 = Vn + An * st
        this.transform.position += lastVelocity * Time.deltaTime;   //Sn+1 = Sn + Vn * st
        Force += Gravity * Time.deltaTime;                      //An+1
    }
}
