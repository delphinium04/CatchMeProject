using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    PlayerInput _playerInput;
    public float _moveSpeed = 5f;
    //플레이어가 갈 수 있는 맵 사이즈 크기
    public float maxX;
    public float maxY;
    public float minX;
    public float minY;
    public float next;
    private bool hasTarget = false;

    private NpcBehaviour _npcCollider;

    void Awake()
    {
         _playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        _playerInput.KeyAction += OnKeyboardClicked;
    }

    void OnKeyboardClicked()
    {
        if (hasTarget && Input.GetKeyDown(KeyCode.F))
            InteractNPC();

        Move();
    }

    void InteractNPC()
    {
        // GameManager.Request(~, ~, ..)
    }

    void Move()
    {
        Vector2 input = Vector2.right * Input.GetAxis("Horizontal") + Vector2.up * Input.GetAxis("Vertical");
        if(input == Vector2.zero) return;
        
        Vector2 moveDir = input.normalized * (_moveSpeed * Time.deltaTime);
        transform.position += moveDir.x * Vector3.right + moveDir.y * Vector3.up;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("NPC")) return;
        _npcCollider = other.GetComponent<NpcBehaviour>();
        hasTarget = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("NPC") || _npcCollider != other.GetComponent<NpcBehaviour>()) return;
        _npcCollider = null;
        hasTarget = true;
    }
}
