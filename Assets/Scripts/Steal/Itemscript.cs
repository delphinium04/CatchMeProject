using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Camera mainCamera;
    public Vector2 offset;

    void Start()
    {
        GameObject player = GameObject.Find("대학생 도둑");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPosition = new Vector3(0, Screen.height, mainCamera.nearClipPlane);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition) + (Vector3)offset;
        transform.position = worldPosition;
    }
}
