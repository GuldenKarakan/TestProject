using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class LineDrawerUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform imageRect;

    [HideInInspector] public List<Vector2> points = new List<Vector2>();

    [SerializeField] private LineRenderer Line;
    [SerializeField] private float lineWidth;
    [SerializeField] private float minimumVertexDistance;
    private float inverseScaleFactor;
    //private bool isLineStarted;

    void Start()
    {
        Line.startWidth = lineWidth;
        Line.endWidth = lineWidth;

        //isLineStarted = false;
        Line.positionCount = 0;
        inverseScaleFactor = 1f / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnPointerDown(PointerEventData ped)
    {
        Vector2 pos = (ped.position - (Vector2)imageRect.position) * 2 * inverseScaleFactor;
        pos.x /= imageRect.sizeDelta.x;
        pos.y /= imageRect.sizeDelta.y;

        //LineRenderer işlemi
        Line.positionCount = 2;
        Line.SetPosition(0, pos);
        Line.SetPosition(1, pos);
        //isLineStarted = true;

    }

    public void OnPointerUp(PointerEventData ped)
    {
        //line noktalarını araca gönder
        for (int i = 0; i < Line.positionCount; i++)
            points.Add(Line.GetPosition(i));
        if (points.Count > 5)
            SplineControl.instance.SplinePointUpdate(this);
        points.Clear();
        Line.positionCount = 0;
        //isLineStarted = false;
    }

    public void OnDrag(PointerEventData ped)
    {
        Vector2 pos = (ped.position - (Vector2)imageRect.position) * 2 * inverseScaleFactor;
        pos.x /= imageRect.sizeDelta.x;
        pos.y /= imageRect.sizeDelta.y;

        pos.x = Mathf.Clamp(pos.x, -1, 1);
        pos.y = Mathf.Clamp(pos.y, -1, 1);

        //line renderer işlemi
        Vector3 currentPos = pos;
        float distance = Vector3.Distance(currentPos, Line.GetPosition(Line.positionCount - 1));
        if (distance > minimumVertexDistance)
        {
            Line.positionCount++;
            Line.SetPosition(Line.positionCount - 1, pos);
        }
    }
}
