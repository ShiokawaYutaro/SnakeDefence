using System.Collections.Generic;
using UnityEngine;

public class TrailRecorder : MonoBehaviour
{
    public struct TrailPoint
    {
        public Vector3 position;
        public Quaternion rotation;

        public TrailPoint(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    public List<TrailPoint> trail = new List<TrailPoint>();
    public float recordSpacing = 0.5f;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
        trail.Add(new TrailPoint(transform.position, transform.rotation));
    }

    void Update()
    {
        if (Vector3.Distance(lastPosition, transform.position) >= recordSpacing)
        {
            trail.Insert(0, new TrailPoint(transform.position, transform.rotation));
            lastPosition = transform.position;

            if (trail.Count > 500)
                trail.RemoveAt(trail.Count - 1);
        }
    }
}
