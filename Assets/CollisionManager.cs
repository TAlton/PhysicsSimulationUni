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

            MyRigidBody rb1 = listCollideables[1].GetComponent<MyRigidBody>();
            MyRigidBody rb2 = listCollideables[2].GetComponent<MyRigidBody>();

            float totalSystemForce = listCollideables[1].GetComponent<MyRigidBody>().Force.magnitude + listCollideables[2].GetComponent<MyRigidBody>().Force.magnitude;
            float totalSystemMoment = listCollideables[1].GetComponent<MyRigidBody>().MomentOfInertia + listCollideables[2].GetComponent<MyRigidBody>().MomentOfInertia;
            Vector3 collisionNormal = Vector3.Cross(listCollideables[1].GetComponent<MyRigidBody>().transform.position,
                                                    listCollideables[2].GetComponent<MyRigidBody>().transform.position).normalized;

            rb1.AddTorque(rb1.Radius * rb1.Velocity.magnitude * Mathf.Sin(Vector3.Angle(rb2.transform.position, rb1.Velocity) * (Mathf.PI / 180f)), collisionNormal);
            rb2.AddTorque(rb2.Radius * rb2.Velocity.magnitude * Mathf.Sin(Vector3.Angle(rb2.transform.position, rb1.Velocity) * (Mathf.PI / 180f)), collisionNormal);


            listCollideables[1].GetComponent<MyRigidBody>().Force = ((Vector3.Reflect(listCollideables[1].GetComponent<MyRigidBody>().Velocity,
                (listCollideables[1].transform.position - listCollideables[2].transform.position).normalized)).normalized * listCollideables[1].GetComponent<MyRigidBody>().Force.magnitude) *
                listCollideables[1].GetComponent<MyRigidBody>().RestitutionCoef;

            listCollideables[2].GetComponent<MyRigidBody>().Force = ((Vector3.Reflect(listCollideables[2].GetComponent<MyRigidBody>().Velocity,
                (listCollideables[2].transform.position - listCollideables[1].transform.position).normalized)).normalized * listCollideables[2].GetComponent<MyRigidBody>().Force.magnitude) * listCollideables[2].GetComponent<MyRigidBody>().RestitutionCoef;

        }

        if (listCollideables[0].GetComponent<MyPlaneCollider>().hasCollided(listCollideables[2]))
        {
            //Debugging
            //listCollideables[1].GetComponent<MyRigidBody>().Force = Vector3.zero;
            //listCollideables[1].GetComponent<MyRigidBody>().useGravity = false;

            //Debug.DrawLine(Vector3.Reflect(listCollideables[1].GetComponent<MyRigidBody>().Velocity, (listCollideables[1].transform.position - listCollideables[0].transform.position)), listCollideables[1].transform.position);
            listCollideables[2].GetComponent<MyRigidBody>().Force = ((Vector3.Reflect(listCollideables[2].GetComponent<MyRigidBody>().Velocity,
                (listCollideables[2].transform.position - listCollideables[0].GetComponent<MyPlaneCollider>().PointOfContact).normalized)).normalized *
                listCollideables[2].GetComponent<MyRigidBody>().Force.magnitude) * listCollideables[2].GetComponent<MyRigidBody>().RestitutionCoef;

        }

        if (listCollideables[0].GetComponent<MyPlaneCollider>().hasCollided(listCollideables[1]))
        {
            //Debugging
            //listCollideables[1].GetComponent<MyRigidBody>().Force = Vector3.zero;
            //listCollideables[1].GetComponent<MyRigidBody>().useGravity = false;



            MyRigidBody rb1 = listCollideables[1].GetComponent<MyRigidBody>();

            Vector3 collisionNormal = Vector3.Cross(listCollideables[0].transform.position,
                                                    listCollideables[1].transform.position).normalized;

            rb1.AddTorque(rb1.Radius * rb1.Velocity.magnitude * Mathf.Sin(Vector3.Angle(listCollideables[0].transform.position, rb1.Velocity) * (Mathf.PI / 180f)), collisionNormal);

            //Debug.DrawLine(Vector3.Reflect(listCollideables[1].GetComponent<MyRigidBody>().Velocity, (listCollideables[1].transform.position - listCollideables[0].transform.position)), listCollideables[1].transform.position);
            listCollideables[1].GetComponent<MyRigidBody>().Force = ((Vector3.Reflect(listCollideables[1].GetComponent<MyRigidBody>().Velocity,
                (listCollideables[1].transform.position - listCollideables[0].GetComponent<MyPlaneCollider>().PointOfContact).normalized)).normalized *
                listCollideables[1].GetComponent<MyRigidBody>().Force.magnitude) * listCollideables[1].GetComponent<MyRigidBody>().RestitutionCoef;

        }

    }
}
