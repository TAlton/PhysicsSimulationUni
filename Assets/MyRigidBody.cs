using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class MyRigidBody : MonoBehaviour
{

    public enum State { Slide, Stationary, Move }
    [SerializeField] public float Mass;
    [SerializeField] public float DragCoef = 0.47f;
    [SerializeField] public float AngularDrag;
    [SerializeField] public float RestitutionCoef;
    [SerializeField] public Vector3 Impulse;
    [SerializeField] public Vector3 Drag;
    [SerializeField] public Vector3 Force;
    [SerializeField] public Vector3 Acceleration;
    [SerializeField] public Vector3 Velocity;
    [SerializeField] public Vector3 AngularMomentum;
    [SerializeField] public Vector3 AngularVelocity;
    [SerializeField] public float MomentOfInertia;
    [SerializeField] public Vector3 Torque;
    [SerializeField] public Vector3 Gravity;
    [SerializeField] public float Radius;
    [SerializeField] public float Area;
    [SerializeField] private float k1 = 0f;
    [SerializeField] private float k2 = 0f;
    [SerializeField] private float k3 = 0f;
    [SerializeField] private float k4 = 0f;
    [SerializeField] public float FluidDensity = 1.229f;
    [SerializeField] public bool useGravity;
    [SerializeField] private State eState;

    float Xn;
    float Yn;
    float Zn;



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

        Xn = this.transform.position.x;
        Yn = this.transform.position.y;
        Zn = this.transform.position.z;
        MomentOfInertia = 0.4f * Mass * (Radius * Radius);
        AddForce(Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(this.transform.position, (this.transform.position + this.Velocity.normalized));

        if (eState == State.Stationary) return;

        #region Linear Velocity

        if (useGravity) AddForce((Gravity * Time.deltaTime));
        AddForce(Drag * Time.deltaTime);
        CalcDrag();
       // if (Mathf.Abs(Force.magnitude) > 0.05f)
        {
            Acceleration = Force / Mass;
            Velocity = new Vector3(RK4(Acceleration.x * Time.deltaTime),
                RK4(Acceleration.y * Time.deltaTime),
                RK4(Acceleration.z * Time.deltaTime));

            this.transform.position += Velocity;

            //Xn = this.transform.position.x + Acceleration.x * Time.deltaTime;
            //Yn = this.transform.position.y + Acceleration.y * Time.deltaTime;
            //Zn = this.transform.position.z + Acceleration.z * Time.deltaTime;
            //this.transform.position = new Vector3(RK4(Xn), RK4(Yn), RK4(Zn));
            //this.transform.SetPositionAndRotation(new Vector3(RK4(Xn), RK4(Yn), RK4(Zn)), Quaternion.identity);
        }



        #endregion
        #region Angular Velocity

        this.transform.Rotate(AngularMomentum);

        #endregion
    }
    float CalcArea()
    {
        return (Mathf.PI * (Radius * Radius));
    }
    float RK4(float argAxisForce)
    {

        k1 = Time.deltaTime * dydt(Time.time, argAxisForce); //eulers method
        k2 = Time.deltaTime * dydt(Time.time + 0.5f * Time.deltaTime, argAxisForce * 0.5f * k1); //mid foint of force and k1
        k3 = Time.deltaTime * dydt(Time.time + 0.5f * Time.deltaTime, argAxisForce * 0.5f * k2); //slope of y and k2
        k4 = Time.deltaTime * dydt(Time.time + Time.deltaTime, argAxisForce * k3); //slope of y and k3

        return (argAxisForce += ((1f / 6f) * Time.deltaTime) * (k1 + (2f * k2) + (2f * k3) + k4));
    }

    float dydt(float argT, float argY)
    {
        if (argT == 0) argT = 1; //fix for initial update tick where Time.time == 0
        return (argY * argY - argT * argT) / (argY * argY  + argT * argT);
    }
    float dydx(float x, float y)
    {
        if (x == 0) x = 1;
        return (x - y) / 2;
    }

    void CalcDrag()
    {
        Drag = -Force.normalized * (0.5f * DragCoef * (Area / 2) * FluidDensity * Force.sqrMagnitude) * Time.deltaTime;
    }
    void CalcAngularDrag()
    {
        
    }
    public void AddForce(Vector3 argForce)
    {
        eState = State.Move;
        Force += (argForce * Mass);
    }
    public void AddTorque(float argForce, Vector3 argDir)
    {
        eState = State.Move;
        AngularMomentum += MomentOfInertia * (argForce * argDir);
       // Vector3 Vperp = Vector3.Dot(this.Velocity, other.velocity); //find the tangeant of the 
       // Torque += argForce * Radius * Mathf.Sin(Vector3.Angle(this.Velocity, Vperp));
    }
    public State getState()
    {
        if (Mathf.Abs(this.Force.x) < 0.0075f &&
            Mathf.Abs(this.Force.y) < 0.0075f &&
            Mathf.Abs(this.Force.z) < 0.0075f)
        {
            eState = State.Stationary;
            return eState;
        }

        return eState;
    }

}
