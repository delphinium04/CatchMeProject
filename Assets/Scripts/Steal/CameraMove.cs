using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameObject player;
    public float smoothSpeed = 0.125f; // �ε巯�� ī�޶� �̵��� ����
    float maxMapX = 26.6f;
    float minMapX = 0f;

    void Start()
    {
        this.player = GameObject.Find("대학생 도둑");
    }

    void FixedUpdate() //Lerp �Լ� ������ 
    {
        //x�����θ� ī�޶� ������
        float playerPosX = Mathf.Clamp(player.transform.position.x, minMapX, maxMapX);
        
        Vector3 desiredPosition = new Vector3(playerPosX, transform.position.y, transform.position.z); 
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
