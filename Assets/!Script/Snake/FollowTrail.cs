using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class TailFollow : MonoBehaviour
{
    private TrailRecorder targetTrail;
    public int followIndex = 10;
    float followSpeed = 50f;
    float rotationSpeed = 50f;

    bool atkDelay;

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

    private async void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "‚Ú‚¤‚¯‚ñ‚µ‚á" && !atkDelay)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.SetDamage(1);
            atkDelay = true;
            await UniTask.Delay(700);
            atkDelay = false;
        }
    }
}
