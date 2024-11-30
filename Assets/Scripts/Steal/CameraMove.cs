using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameObject player;
    public float smoothSpeed = 0.125f; // 부드러운 카메라 이동을 위해
    float maxMapX;
    float minMapX = 0f;

    public int stage;

    void Start()
    {
        stage = 1;
        if (stage == 1)
        {
            maxMapX = 24.6f;
        }
        else if (stage == 2)
        {
            maxMapX = 53f;
        }
        else if (stage == 3)
        {
            maxMapX = 60f;
        }
        this.player = GameObject.Find("대학생 도둑");
    }

    void FixedUpdate() //Lerp 함수 때문에 
    {
        //x축으로만 카메라 움직임
        float playerPosX = Mathf.Clamp(player.transform.position.x, minMapX, maxMapX);
        
        Vector3 desiredPosition = new Vector3(playerPosX, transform.position.y, transform.position.z); 
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
