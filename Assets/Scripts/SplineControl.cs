using UnityEngine;
using Dreamteck.Splines;
using System.Collections.Generic;

public class SplineControl : MonoBehaviour
{
    [HideInInspector] public bool isMove = false;
    [HideInInspector] public Rigidbody rb;
    public float speed;

    [SerializeField] private SplineComputer spline;
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private float radius = .35f;
    [SerializeField] private List<GameObject> sCol = new List<GameObject>();
    [SerializeField] private Wheel[] wheels;

    private SplinePoint[] points = new SplinePoint[3];
    private float _speed;
    private Vector3 center;


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
        _speed = speed;
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new SplinePoint();
            points[i].position = Vector3.forward * i * 1.5f;
            points[i].normal = Vector3.up;
            points[i].size = 1f;
            points[i].color = Color.white;

            center += points[i].position;
            AddSpherCollider(i);
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
                wheel.WheelRotate();
        }
        if (!isMove) return;

        Move();
    }

    private void Move()
    {
        rb.AddRelativeForce(new Vector3(0, 0, Vector3.forward.z) * speed);
        //Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        //localVelocity.x = 0;
        //rb.velocity = transform.TransformDirection(localVelocity);
    }

    public void SplinePointUpdate(LineDrawerUI line)
    {
        speed = _speed;
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

        Vector3 vektor = Vector3.zero, playerPos;
        for (int i = 0; i < points.Length; i++)
        {
            if (vektor.y > points[i].position.y)// En dip noktayı buluyor
                vektor.y = points[i].position.y;
        }
        vektor.y *= -1;

        if (points[1].position.z != 0)// ilk noktanın z ekseni hep sıfırıncı noktada oluyor.
            vektor.z = points[1].position.z * -1;

        playerPos = transform.position;
        //playerPos.y += 3f;
        vektor = vektor + playerPos;

        for (int i = 0; i < points.Length; i++)
        {
            /// Dip noktaya göre pointleri düzenliyor
            points[i].position += vektor;
            center += points[i].position;
            AddSpherCollider(i);
        }
        spline.SetPoints(points);

        center /= points.Length;
        centerOfMass.position = center;
        rb.centerOfMass = centerOfMass.localPosition;

        transform.rotation = Quaternion.Euler(0, 0, 0);
        //transform.position = new Vector3(0, transform.position.y + 1.2f, transform.position.z);

    }
}
