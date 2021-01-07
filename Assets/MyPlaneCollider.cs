using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MyPlaneCollider : MonoBehaviour
{

    [SerializeField] public Vector3 PointOfContact;
    [SerializeField] public Vector3 PlaneNormal;
    [SerializeField] public Vector3 p1;
    [SerializeField] public Vector3 p2;
    [SerializeField] public Vector3 p3;
    [SerializeField] public Vector3 p4;

    // Start is called before the first frame update
    void Start()
    {
        this.tag = "CollisionDynamic";
        PlaneNormal = this.transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool hasCollided(GameObject other)
    {
        //MyRigidBody rb = other.gameObject.GetComponent<MyRigidBody>();
        //Vector3 vec = other.transform.position - this.transform.position;
        //float dist = Vector3.Dot(vec, this.transform.up);

        //float distEdges = dist - other.GetComponent<MySphereCollider>().Radius;

        //if (rb.Velocity.magnitude < distEdges) return false;

        //if (dist > other.GetComponent<MySphereCollider>().Radius) return false;

        //other.transform.position += distEdges * rb.Velocity.normalized;

        //float dx = other.transform.position.x;
        //float dy = other.transform.position.y - other.GetComponent<MySphereCollider>().Radius;
        //float dz = other.transform.position.z;
        //PointOfContact = new Vector3(dx, dy, dz);

        //return true;

        if (other.GetComponent<MyRigidBody>().getState() == MyRigidBody.State.Stationary) return false;

        MyRigidBody rb = other.gameObject.GetComponent<MyRigidBody>(); //distance

        Vector3 distArbitrary = rb.transform.position - this.transform.position;

        float dist = distArbitrary.magnitude * Mathf.Cos(Vector3.Angle(distArbitrary, this.transform.up) * (Mathf.PI / 180f)); //dot product 
        float distEdges = dist - other.GetComponent<MySphereCollider>().Radius;

        if (rb.Velocity.magnitude < distEdges) return false;

        if (dist > other.GetComponent<MySphereCollider>().Radius) return false;

        if (Vector3.Dot(rb.Velocity, PlaneNormal) > 0) return false;


        other.transform.position += distEdges * rb.Velocity.normalized;

        float dx = other.transform.position.x;
        float dy = other.transform.position.y - other.GetComponent<MySphereCollider>().Radius;
        float dz = other.transform.position.z;
        PointOfContact = new Vector3(dx, dy, dz);


        return true;

    }
    
}
