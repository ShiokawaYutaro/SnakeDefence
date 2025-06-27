
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;

public class camera : MonoBehaviour
{
    [SerializeField] Player player;
    Transform playerPos;

    [SerializeField] Transform ultCameraPos;
    Camera cameraCompo;

    private void Start()
    {
        playerPos = player.transform;
        cameraCompo = GetComponent<Camera>();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (!player.ult)
        {
            transform.position = new Vector3(playerPos.position.x, playerPos.position.y + 10, playerPos.position.z - 6);
        }

        else
        {
            transform.position = new Vector3(ultCameraPos.position.x, ultCameraPos.position.y, ultCameraPos.position.z);
            transform.rotation = Quaternion.Euler(ultCameraPos.eulerAngles);
            cameraCompo.cullingMask &= ~(1 << LayerMask.NameToLayer("UI")); ;
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    isAnimating = true;
        //    timer = 0f;

        //    startPos = Camera.main.transform.position;

        //    // プレイヤーの正面にゴール
        //    endPos = playerPos.position + playerPos.forward * 3f + Vector3.up + Vector3.right;

        //    // 中間点（上に持ち上げて放物線に）
        //    controlPoint = (startPos + endPos) / 2f + Vector3.right * 5f;
        //}

        //if (isAnimating)
        //{
        //    timer += Time.deltaTime;
        //    float t = Mathf.Clamp01(timer / duration);

        //    t = 1f - Mathf.Pow(1f - t, 5f);
        //    // 二次ベジェ曲線で放物線軌道を描く
        //    Vector3 a = Vector3.Lerp(startPos, controlPoint, t);
        //    Vector3 b = Vector3.Lerp(controlPoint, endPos, t);
        //    Vector3 camPos = Vector3.Lerp(a, b, t);

        //    Camera.main.transform.position = camPos;
        //    Vector3 targtPos = new Vector3(playerPos.position.x, playerPos.position.y + 1, playerPos.position.z);
        //    Camera.main.transform.LookAt(targtPos);

        //    if (t >= 1f)
        //    {
        //        isAnimating = false;
        //    }
        //}

        //else
        //{

        //}


    }

}
