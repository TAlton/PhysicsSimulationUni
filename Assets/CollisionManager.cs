using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private bool delayedCheck = true;
    [SerializeField] private List<GameObject> listCollideables;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (delayedCheck) listCollideables = GameObject.FindGameObjectsWithTag("CollisionDynamic").ToList<GameObject>();

        if (listCollideables[1].GetComponent<MySphereCollider>().hasCollided(listCollideables[2]))
        {

            listCollideables[1].GetComponent<MyRigidBody>().Force = ((Vector3.Reflect(listCollideables[1].GetComponent<MyRigidBody>().Velocity,
                (listCollideables[1].transform.position - listCollideables[2].transform.position).normalized)).normalized * listCollideables[1].GetComponent<MyRigidBody>().Force.magnitude) *
                listCollideables[1].GetComponent<MyRigidBody>().RestitutionCoef;
            //listCollideables[1].GetComponent<MyRigidBody>().AddForce()
            //    (Vector3.Reflect(listCollideables[1].GetComponent<MyRigidBody>().Velocity,
            //    (listCollideables[1].transform.position - listCollideables[2].transform.position).normalized)); 

        }

        if (listCollideables[0].GetComponent<MyPlaneCollider>().hasCollided(listCollideables[1]))
        {
            //Debugging
            //listCollideables[1].GetComponent<MyRigidBody>().Force = Vector3.zero;
            //listCollideables[1].GetComponent<MyRigidBody>().useGravity = false;

            //Debug.DrawLine(Vector3.Reflect(listCollideables[1].GetComponent<MyRigidBody>().Velocity, (listCollideables[1].transform.position - listCollideables[0].transform.position)), listCollideables[1].transform.position);
            listCollideables[1].GetComponent<MyRigidBody>().Force = ((Vector3.Reflect(listCollideables[1].GetComponent<MyRigidBody>().Velocity,
                (listCollideables[1].transform.position - listCollideables[0].GetComponent<MyPlaneCollider>().PointOfContact).normalized)).normalized *
                listCollideables[1].GetComponent<MyRigidBody>().Force.magnitude) * listCollideables[1].GetComponent<MyRigidBody>().RestitutionCoef;

        }

    }
}
