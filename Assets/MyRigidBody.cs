using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRigidBody : MonoBehaviour
{
    [SerializeField] public float Mass;
    [SerializeField] public float DragCoef = 0.47f;
    [SerializeField] public float AngularDrag;
    [SerializeField] public float RestitutionCoef;
    [SerializeField] public Vector3 Impulse;
    [SerializeField] public Vector3 Drag;
    [SerializeField] public Vector3 Force;
    [SerializeField] public Vector3 Acceleration;
    [SerializeField] public Vector3 Velocity;
    [SerializeField] public Vector3 AngularVelocity;
    [SerializeField] public Vector3 AngularImpulse;
    [SerializeField] public Vector3 InertiaTensor;
    [SerializeField] public Vector3 Torque;
    [SerializeField] public Vector3 Gravity;
    [SerializeField] public float Radius;
    [SerializeField] public float Area;
    [SerializeField] private float k1;
    [SerializeField] private float k2;
    [SerializeField] private float k3;
    [SerializeField] private float k4;
    [SerializeField] public float FluidDensity = 1.229f;
    [SerializeField] public bool useGravity;

    // Start is called before the first frame update
    void Start()
    {
        Radius = this.transform.localScale.x / 2;
        Gravity = new Vector3(0f, -9.81f, 0f);

        if (Radius < 1)
        {
            Radius *= 10;
            Area = CalcArea() / 10;
            Radius /= 10;
        }
        else
        {
            Area = CalcArea();
        }

        AddForce(Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(this.transform.position, (this.transform.position + this.Velocity.normalized));

        #region Linear Velocity

        if (useGravity) AddForce(Gravity * Time.deltaTime);
        AddForce(Drag * Time.deltaTime);
        CalcDrag();
        if (Mathf.Abs(Force.magnitude) > 0.05f)
        {
            Acceleration = Force / Mass;
            Velocity = new Vector3(RK4(Acceleration.x * Time.deltaTime),
                RK4(Acceleration.y * Time.deltaTime),
                RK4(Acceleration.z * Time.deltaTime));

            this.transform.position += Velocity;
        }

        #endregion
        #region Angular Velocity



        #endregion
    }
    float CalcArea()
    {
        return (Mathf.PI * (Radius * Radius));
    }
    float RK4(float argAxisForce)
    {

        k1 = Time.deltaTime * dydt(Time.time, argAxisForce);
        k2 = Time.deltaTime * dydt(Time.time + (0.5f * Time.deltaTime), argAxisForce + (Time.deltaTime * (0.5f * k1)));
        k3 = Time.deltaTime * dydt(Time.time + (0.5f * Time.deltaTime), argAxisForce + (Time.deltaTime * (0.5f * k2)));
        k4 = Time.deltaTime * dydt(Time.time + Time.deltaTime, argAxisForce + (Time.deltaTime * k3));

        return (argAxisForce += ((1f / 6f) * Time.deltaTime) * (k1 + (2f * k2) + (2f * k3) + k4));
    }

    float dydt(float argT, float argY)
    {
        if (argT == 0) argT = 1; //fix for initial update tick where Time.time == 0
        return (argY / argT);
    }

    void CalcDrag()
    {
        Drag = -Force.normalized * (0.5f * DragCoef * (Area / 2) * FluidDensity * Force.sqrMagnitude) * Time.deltaTime;
    }
    public void AddForce(Vector3 argForce)
    {
        Force += (argForce);
    }
    public void AddAngularForce(Vector3 argForce)
    {
       // Vector3 Vperp = Vector3.Dot(this.Velocity, other.velocity); //find the tangeant of the 
       // Torque += argForce * Radius * Mathf.Sin(Vector3.Angle(this.Velocity, Vperp));
    }

}
