using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySphereCollider : MonoBehaviour
{
    [SerializeField] public float Radius;
    [SerializeField] private float Area;
    [SerializeField] public Vector3 PointOfContact;
    [SerializeField] private float ThetaContact;


    // Start is called before the first frame update
    void Start()
    {
        Radius = this.transform.localScale.x * 0.5f;
        if(Radius < 1)
        {
            Radius *= 10;
            Area = CalcArea() / 10;
            Radius /= 10;
        } else
        {
            Area = CalcArea();
        }
        this.tag = "CollisionDynamic";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float CalcArea()
    {
        return (Mathf.PI * (Radius * Radius));
    }

    public bool hasCollided(GameObject other) //the biggest error in this is the resolution of the default unity sphere
    {
        float DistanceOfCenters = (this.transform.position - other.transform.position).magnitude;
        float SumOfRadii = (other.GetComponent<MySphereCollider>().Radius) + Radius; //if they both have a radius var we can access other.radius instead
        float DistanceOfEdges = DistanceOfCenters - SumOfRadii;
        MyRigidBody rb = gameObject.GetComponent<MyRigidBody>();
        MyRigidBody OtherRB = other.GetComponent<MyRigidBody>();

        if (rb.Velocity.magnitude - OtherRB.Velocity.magnitude < DistanceOfEdges) //if the distance of the two closest edges of the sphere is more than the magnitude of the velocity they will not collide
        {
            return false;
        }

        Vector3 NormVec = (rb.Velocity - OtherRB.Velocity).normalized;

        Vector3 DistCenter = other.transform.position - this.transform.position;
        float D = Vector3.Dot(NormVec, DistCenter);

        if (D <= 0) return false; //making sure that one sphere is moving towards the other

        float DistCenterMagnitude = DistCenter.magnitude;
        float OppositeSide = (DistCenterMagnitude * DistCenterMagnitude) - (D * D); //pythagoras
        float SumOfRadiiSq = Mathf.Pow(SumOfRadii, 2); //Mathf.pow is slower than this

        if (OppositeSide >= SumOfRadiiSq) return false; //if the height of the triangle is >= the combined length of the radii the sphers will not touch

        float t = SumOfRadiiSq - OppositeSide;

        if (t <= 0) return false; //prevent sqrt of negatives

        float Dist = D - Mathf.Sqrt(t);
        float Magn = rb.Velocity.magnitude * Time.deltaTime; //the magnitude of the vector in this frame

        if (Magn < Dist) return false; //check if the magnitude is less than the distance so we are sure they collide in this update frame
        //rb.Velocity = rb.Velocity.normalized;
        this.transform.position += Dist * rb.Velocity.normalized;
        float dx = 0.5f * (this.transform.position.x + other.transform.position.x);
        float dy = 0.5f * (this.transform.position.y + other.transform.position.y);
        float dz = 0.5f * (this.transform.position.z + other.transform.position.z);
        PointOfContact = new Vector3(dx, dy, dz);
        ThetaContact = 90f - Vector3.Angle(this.transform.position, PointOfContact);

        return true;
    }
}
