using UnityEngine;
using Dreamteck.Splines;
using System.Collections.Generic;

public class SplineControl : MonoBehaviour
{
    [HideInInspector] public bool isMove = false;
    [HideInInspector] public Rigidbody rb;
    public float speed;
    public Vector3[] WheelMotorTorque;
    public bool[] _hit;

    [SerializeField] private SplineComputer spline;
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private float radius = .35f;
    [SerializeField] private Wheel[] wheels;
    [SerializeField] private WheelCollider[] wheelCol;
    [SerializeField] private Transform front, back;

    private SplinePoint[] points = new SplinePoint[3];
    private List<GameObject> sCol = new List<GameObject>();
    private Vector3 center;
    private WheelHit hit;


    #region Singleton
    public static SplineControl instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new SplinePoint();
            points[i].position = Vector3.forward * i * 1.5f;
            points[i].normal = Vector3.up;
            points[i].size = 1f;
            points[i].color = Color.white;

            center += points[i].position;
            //AddSpherCollider(i);
        }
        center /= points.Length;
        points[1].position.y = 1.5f;
        spline.SetPoints(points);
        centerOfMass.position = center;
        rb.centerOfMass = centerOfMass.localPosition;
    }

    private void AddSpherCollider(int index)
    {
        sCol.Add(new GameObject());
        sCol[index].transform.SetParent(transform);
        sCol[index].layer = gameObject.layer;
        sCol[index].transform.position = points[index].position;
        SphereCollider s = sCol[index].AddComponent<SphereCollider>();
        s.radius = radius;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0) && !isMove)
        {
            isMove = true;
            foreach (Wheel wheel in wheels)
            {
                wheel.WheelRotate();
            }

            AddForceSpeed(false);
        }
        if (!isMove) return;

        Move();
    }

    private void Move()
    {
        _hit[0] = wheelCol[0].GetGroundHit(out hit);
        _hit[1] = wheelCol[1].GetGroundHit(out hit);

        if (rb.velocity.z > 20.0f)
            foreach (WheelCollider wheel in wheelCol)
            {
                wheel.motorTorque = speed * 10;
                wheel.steerAngle = 0;
            }
        else AddForceSpeed(false);

    }

    public void AddForceSpeed(bool isPower)
    {
        if (_hit[0] && _hit[1] && !isPower)
        {
            rb.AddForce(transform.forward * speed * .5f, ForceMode.Impulse);
        }
        else if (isPower)
            rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    public void SplinePointUpdate(LineDrawerUI line)
    {
        //speed = _speed;
        center = Vector3.zero;
        int count = sCol.Count;
        for(int i = 0; i < count; i++)
        {
            Destroy(sCol[0]);
            sCol.RemoveAt(0);

        }
        points = new SplinePoint[line.points.Count];
        for (int i = 0; i < line.points.Count; i++)
        {
            points[i] = new SplinePoint();
            points[i].position = new Vector3(0, line.points[i].y, line.points[i].x) * 2;
            points[i].normal = Vector3.up;
            points[i].size = 1f;
            points[i].color = Color.white;

        }

        Vector3 vector = Vector3.zero, playerPos;
        for (int i = 0; i < points.Length; i++)
        {
            if (vector.y > points[i].position.y)// En dip noktayı buluyor
                vector.y = points[i].position.y;
        }
        vector.y *= -1;

        if (points[1].position.z != 0)// ilk noktanın z ekseni hep sıfırıncı noktada oluyor.
            vector.z = points[1].position.z * -1;

        playerPos = transform.position;
        //playerPos.y += 3f;
        vector = vector + playerPos;

        for (int i = 0; i < points.Length; i++)
        {
            /// Dip noktaya göre pointleri düzenliyor
            points[i].position += vector;
            center += points[i].position;
            AddSpherCollider(i);
        }
        spline.SetPoints(points);

        center /= points.Length;
        centerOfMass.position = center;
        rb.centerOfMass = centerOfMass.localPosition;

        transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
    }
}
