using UnityEngine;

public class TailFollow : MonoBehaviour
{
    private TrailRecorder targetTrail;
    public int followIndex = 10;
    float followSpeed = 50f;
    float rotationSpeed = 50f;

    void Update()
    {
        targetTrail = transform.parent.Find("body").GetComponent<TrailRecorder>();
        if (targetTrail.trail.Count > followIndex)
        {
            var point = targetTrail.trail[followIndex];
            transform.position = Vector3.Lerp(transform.position, point.position, Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, point.rotation, Time.deltaTime * rotationSpeed);
        }
    }
}
