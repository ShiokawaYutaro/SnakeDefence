
using Unity.VisualScripting;
using UnityEngine;

public class camera : MonoBehaviour
{
    Vector3 playerPos;

    public float speed = 1;

    //Vector3 lastTarget;

    float mouseX;
    float mouseY;
    Quaternion rotation;
    Vector3 playerPos2;

    Vector3 origin;
    Vector3 direction;
    [SerializeField]GameObject neck;
    public GameObject pickImage;  // PickImageを表示するUI
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(playerPos.x, playerPos.y + 1.5f, playerPos.z - 7f);
        //RockON = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        mouseY = Mathf.Clamp(mouseY, -90, 90);
        //float mouseScroll = Input.GetAxis("Mause ScrollWheel");

        //float cameraY = Mathf.Clamp(mouseY, mouseY - 0.2f, mouseY + 0.5f);
        //playerPos = transform.parent.position;
        //playerPos2 = new Vector3(playerPos.x, playerPos.y + 1.5f, playerPos.z);

        var dir = new Vector3(0, 2f, 0);
        if (mouseY <= 80 && mouseY >= -90)
        {
            mouseY += Input.GetAxis("Mouse Y") * speed;
        }
        if(mouseY < -80)
        {
            mouseY = -80;
        }
        if (mouseY > 80)
        {
            mouseY = 80;
        }


        mouseX += Input.GetAxis("Mouse X") * speed;
        rotation = Quaternion.Euler(-mouseY, mouseX, 0);

        //transform.rotation = rotation;
        //transform.position = new Vector3(transform.position.x, playerPos.y + 1.3f,transform.position.z) ;
        //transform.LookAt(playerPos2);


        neck.transform.rotation = rotation;
    }

    public void ShowPickImage()
    {
        pickImage.SetActive(true);  // アイテムやドアノブが視界に入った場合
    }

    public void HidePickImage()
    {
        pickImage.SetActive(false);  // 視界から外れた場合
    }

}
