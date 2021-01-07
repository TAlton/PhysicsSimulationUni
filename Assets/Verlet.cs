using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Verlet : MonoBehaviour
{
    [SerializeField] private Vector3 Impulse;
    [SerializeField] private Vector3 Gravity;
    [SerializeField] private Vector3 Acceleration;
    [SerializeField] private Vector3 Force;
    [SerializeField] private Vector3 Velocity;
    [SerializeField] private Vector3 LastPosition;
    [SerializeField] private float Mass;
    // Start is called before the first frame update
    void Start()
    {
        Gravity = new Vector3(0f, -9.18f, 0f);
        Force = Impulse + Gravity;                              //An
        LastPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        LastPosition = this.transform.position;
        Acceleration = Force / Mass;
        this.transform.position += Velocity * Time.deltaTime + 0.5f * Acceleration * Time.deltaTime * Time.deltaTime;
        Velocity += Acceleration * Time.deltaTime;
        Force += Gravity * Time.deltaTime;                      //An+1
    }
}
