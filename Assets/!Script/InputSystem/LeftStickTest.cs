using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Mathematics;

public class LeftStickTest : MonoBehaviour
{
    public Camera cam;
    private float zoomSpeed = 0.1f;
    private float minZoom = 60;
    private float maxZoom = 100;

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // 直前のタッチ位置
            Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

            // 前と現在の2本指の距離
            float prevDistance = Vector2.Distance(prevTouch0, prevTouch1);
            float currentDistance = Vector2.Distance(touch0.position, touch1.position);

            float delta = prevDistance - currentDistance;

            cam.fieldOfView += delta * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
        }

        //if (Input.touchCount == 1)
        //{
        //    Touch touch0 = Input.GetTouch(0);

        //    // 直前のタッチ位置
        //    Stick.rectTransform.position = touch0.position;
        //}
    }
}