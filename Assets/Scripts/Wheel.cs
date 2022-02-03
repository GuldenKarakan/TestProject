using UnityEngine;
using DG.Tweening;

public class Wheel : MonoBehaviour
{
    [SerializeField] private Vector3 destinationRotation;
    [SerializeField] private float duration;

    public void WheelRotate()
    {
        transform.DOLocalRotate(destinationRotation, duration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }
}
