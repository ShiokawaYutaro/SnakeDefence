
using Unity.VisualScripting;
using UnityEngine;

public class camera : MonoBehaviour
{
    [SerializeField] Player player;
    Transform playerPos;

    private void Start()
    {
        playerPos = player.transform;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (player.attack)
        {
            float moveSin = Mathf.Sin(Time.time);
        }
        transform.position = new Vector3(playerPos.position.x,playerPos.position.y + 10, playerPos.position.z - 7);

    }

}
