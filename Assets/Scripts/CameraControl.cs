using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [HideInInspector] public Camera cam;

    [SerializeField] private Transform camTarget;
    public float speed = 5;
    public Vector3 posOffset;
    private Vector3 pos;

    #region Singleton
    public static CameraControl instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    private void FixedUpdate()
    {
        pos = camTarget.position + posOffset;
        transform.localPosition = Vector3.Lerp(transform.localPosition, pos, Time.deltaTime * speed);
    }
}
