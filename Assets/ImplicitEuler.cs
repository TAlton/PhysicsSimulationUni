using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImplicitEuler : MonoBehaviour
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
        Force = Impulse + Gravity;
    }

    // Update is called once per frame
    void Update()
    {
        Force += Gravity * Time.deltaTime;                      //An+1
        Acceleration = Force / Mass;
        Velocity += Acceleration * Time.deltaTime;              //Vn+1 = Vn + An+1 * dt
        this.transform.position += Velocity * Time.deltaTime;   //Sn+1 = Sn + Vn+1 * dt
    }
}
