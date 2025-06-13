using Cysharp.Threading.Tasks;
using UnityEngine;

public class TailFollow : MonoBehaviour
{
    private TrailRecorder targetTrail;
    public int followIndex = 10;
    public int trailNumber = 0; // ‚Ç‚ÌƒgƒŒƒCƒ‹‚ð’Ç‚¤‚©Žw’è

    float followSpeed = 50f;
    float rotationSpeed = 50f;
    bool atkDelay;

    void Start()
    {
        targetTrail = transform.parent.Find("body").GetComponent<TrailRecorder>();
    }

    void Update()
    {
        if (targetTrail.trails.Count > trailNumber && targetTrail.trails[trailNumber].Count > followIndex)
        {
            var point = targetTrail.trails[trailNumber][followIndex];
            transform.position = Vector3.Lerp(transform.position, point.position, Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, point.rotation, Time.deltaTime * rotationSpeed);
        }
    }

    private async void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "‚Ú‚¤‚¯‚ñ‚µ‚á" && !atkDelay)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.SetDamage(1);
            atkDelay = true;
            await UniTask.Delay(700);
            atkDelay = false;
        }
    }
}
