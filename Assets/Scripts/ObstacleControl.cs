using UnityEngine;
using DG.Tweening;

public class ObstacleControl : MonoBehaviour
{
    [SerializeField] private bool move = false;
    [SerializeField] private Vector3 destinationMove;
    [SerializeField] private bool rot = false;
    [SerializeField] private Vector3 destinationRotation;
    [SerializeField] private float duration = 0;
    [SerializeField] private ParticleSystem particle;

    void Start()
    {
        if (move)
            transform.DOLocalMoveY(destinationMove.y, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        else if(rot)
            transform.DOLocalRotate(destinationRotation, duration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //SplineControl.instance.speed = 0;
        particle.Play();
        //GetComponent<Collider>().enabled = false;
    }
}
