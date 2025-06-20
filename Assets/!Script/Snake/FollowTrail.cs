using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class TailFollow : MonoBehaviour
{
    private TrailRecorder targetTrail;
    public int followIndex = 10;
    public int trailNumber = 0; // ‚Ç‚ÌƒgƒŒƒCƒ‹‚ð’Ç‚¤‚©Žw’è

    float followSpeed = 50f;
    float rotationSpeed = 50f;
    bool atkDelay;

    Player player;

    void Start()
    {
        targetTrail = transform.parent.Find("body").GetComponent<TrailRecorder>();
        player = targetTrail.GetComponent<Player>();
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

    private Dictionary<Enemy, bool> enemyAtkDelays = new Dictionary<Enemy, bool>();

    private async void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (!enemyAtkDelays.ContainsKey(enemy))
                enemyAtkDelays[enemy] = false;

            if (!enemyAtkDelays[enemy])
            {
                enemy.SetDamage(player.damage);
                float poisonDamage = (player.poison * 0.01f) * enemy.HP;
                if (poisonDamage < 0) poisonDamage = 0;
                enemy.SetAttributeDamage(poisonDamage, player.poison, new Color32(25, 210, 0, 255));
                float fireDamage = Random.Range(player.damage * player.fire, player.damage * player.fire * 1.5f);
                enemy.SetAttributeDamage(fireDamage, player.fire, new Color32(255, 80, 0, 255));

                enemyAtkDelays[enemy] = true;
                await UniTask.Delay(1000);
                enemyAtkDelays[enemy] = false;
            }
        }
    }


    float Decimal(float damage)
    {
        return Mathf.Floor(damage * Mathf.Pow(10, 1)) / Mathf.Pow(10, 1);
    }
}
