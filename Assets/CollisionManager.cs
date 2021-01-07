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

            //rb1.Sleep = false;
            //rb2.Sleep = false;

            //perfectly elastic collision
            //Vf1 = [(M1 - M2) * V1  + 2 * M2  * V2] / (M1 - M2)
            //Vf2 =  [2 * M1 * V1 - (M1 - M2) * V2] / (M1 + M2)
            float f1 = rb1.Force.magnitude;
            float f2 = rb2.Force.magnitude;

            if(rb1.Mass == rb2.Mass)
            {
                f1 = rb1.Force.magnitude;
                f2 = rb2.Force.magnitude;
            } else
            {
                f1 = ((rb1.Mass - rb2.Mass) * rb1.Force.magnitude + 2 * rb2.Mass * rb2.Force.magnitude) / (rb1.Mass + rb2.Mass);
                f2 = (2 * rb1.Mass * rb1.Force.magnitude - (rb1.Mass - rb2.Mass) * rb2.Force.magnitude) / (rb1.Mass + rb2.Mass);
            }


            float totalSystemForce = rb1.Force.magnitude + rb2.Force.magnitude;
            float totalSystemMoment = rb1.MomentOfInertia + rb2.MomentOfInertia;
            Vector3 collisionNormal = Vector3.Cross(rb1.transform.position,
                                                    rb2.transform.position).normalized;

            rb1.AddTorque(rb1.Radius * rb1.Velocity.magnitude * Mathf.Sin(Vector3.Angle(rb2.transform.position, rb1.Velocity) * (Mathf.PI / 180f)), collisionNormal);
            rb2.AddTorque(rb2.Radius * rb2.Velocity.magnitude * Mathf.Sin(Vector3.Angle(rb2.transform.position, rb1.Velocity) * (Mathf.PI / 180f)), collisionNormal);

            Vector3 rbForce1, rbForce2;

            rbForce1 = ((Vector3.Reflect(rb1.Velocity.normalized,
                (rb1.transform.position - rb2.transform.position).normalized)).normalized * f1) *
                (rb1.RestitutionCoef * rb2.RestitutionCoef);

            rbForce2 = ((Vector3.Reflect(rb2.Velocity.normalized,
                (rb2.transform.position - rb1.transform.position).normalized)).normalized * f2) *
                (rb2.RestitutionCoef * rb1.RestitutionCoef);

            rb1.Force = rbForce1;
            rb2.Force = rbForce2;

        }

        if (listCollideables[0].GetComponent<MyPlaneCollider>().hasCollided(listCollideables[2]) && listCollideables[2].GetComponent<MyRigidBody>().getState() != MyRigidBody.State.Stationary)
        {
            //Debugging
            //listCollideables[1].GetComponent<MyRigidBody>().Force = Vector3.zero;
            //listCollideables[1].GetComponent<MyRigidBody>().useGravity = false;

            MyRigidBody rb1 = listCollideables[2].GetComponent<MyRigidBody>();

            Vector3 collisionNormal = Vector3.Cross(listCollideables[0].transform.position,
                                                listCollideables[2].transform.position).normalized;

            // rb1.AddTorque(rb1.Radius * rb1.Velocity.magnitude * Mathf.Sin(Vector3.Angle(listCollideables[0].transform.position, rb1.Velocity) * (Mathf.PI / 180f)), collisionNormal);

            //Debug.DrawLine(Vector3.Reflect(listCollideables[1].GetComponent<MyRigidBody>().Velocity, (listCollideables[1].transform.position - listCollideables[0].transform.position)), listCollideables[1].transform.position);

            listCollideables[2].GetComponent<MyRigidBody>().Force = ((Vector3.Reflect(listCollideables[2].GetComponent<MyRigidBody>().Velocity,
                listCollideables[0].GetComponent<MyPlaneCollider>().PlaneNormal)).normalized *
                listCollideables[2].GetComponent<MyRigidBody>().Force.magnitude) * listCollideables[2].GetComponent<MyRigidBody>().RestitutionCoef;

        }

        if (listCollideables[0].GetComponent<MyPlaneCollider>().hasCollided(listCollideables[1]) && listCollideables[1].GetComponent<MyRigidBody>().getState() != MyRigidBody.State.Stationary)
        {
            //Debugging
            //listCollideables[1].GetComponent<MyRigidBody>().Force = Vector3.zero;
            //listCollideables[1].GetComponent<MyRigidBody>().useGravity = false;

                Vector3 collisionNormal = Vector3.Cross(listCollideables[0].transform.up,
                                                        listCollideables[1].transform.position).normalized;

                //rb1.AddTorque(rb1.Radius * rb1.Velocity.magnitude * Mathf.Sin(Vector3.Angle(listCollideables[0].transform.position, rb1.Velocity) * (Mathf.PI / 180f)), collisionNormal);

                //Debug.DrawLine(Vector3.Reflect(listCollideables[1].GetComponent<MyRigidBody>().Velocity, (listCollideables[1].transform.position - listCollideables[0].transform.position)), listCollideables[1].transform.position);
                listCollideables[1].GetComponent<MyRigidBody>().Force = ((Vector3.Reflect(listCollideables[1].GetComponent<MyRigidBody>().Velocity,
                    listCollideables[0].GetComponent<MyPlaneCollider>().PlaneNormal)).normalized *
                    listCollideables[1].GetComponent<MyRigidBody>().Force.magnitude) * listCollideables[1].GetComponent<MyRigidBody>().RestitutionCoef;

        }

    }
}
