
using Unity.VisualScripting;
using UnityEngine;

public class camera : MonoBehaviour
{
    [SerializeField] Transform playerPos;

    public float speed = 1;

    //Vector3 lastTarget;

    float mouseX;
    float mouseY;
    Quaternion rotation;

    Vector3 origin;
    Vector3 direction;
    [SerializeField]GameObject neck;
    public GameObject pickImage;  // PickImage‚ð•\Ž¦‚·‚éUI
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(playerPos.x, playerPos.y + 1.5f, playerPos.z - 7f);
        //RockON = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        transform.position = new Vector3(playerPos.position.x,playerPos.position.y + 10, playerPos.position.z - 7);
    }

}
