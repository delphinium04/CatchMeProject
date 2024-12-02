using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    StealManager _manager;
    NpcBehaviour _npcCollider;

    PlayerInput _playerInput;
    public float _moveSpeed = 5f;
    public bool _isEnd = false;

    public Vector2 _minPos = new Vector2(10, 10);
    public Vector2 _maxPos = new Vector2(10, 10);


    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _manager = FindObjectOfType<StealManager>();
    }

    void Start()
    {
        _playerInput.KeyAction += OnKeyboardClicked;
    }

    void OnKeyboardClicked()
    {
        if(_isEnd) return;
        InteractNpc();
        Move();
    }

    void InteractNpc()
    {
        if (_npcCollider != null && Input.GetKeyDown(KeyCode.F))
            _manager.TryPickItem(_npcCollider);
    }

    void Move()
    {
        Vector2 input = Vector2.right * Input.GetAxis("Horizontal") + Vector2.up * Input.GetAxis("Vertical");
        if (input == Vector2.zero || Input.GetKey(KeyCode.F)) return;

        Vector2 moveDir = input.normalized * (_moveSpeed * Time.deltaTime);
        transform.position += moveDir.x * Vector3.right + moveDir.y * Vector3.up;
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, _minPos.x, _maxPos.x), Mathf.Clamp(transform.position.y, _minPos.y, _maxPos.y));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
            _npcCollider = other.GetComponent<NpcBehaviour>();
        else if(other.CompareTag("End"))
        {
            _manager.StageEnd();
            _isEnd = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("NPC") || _npcCollider != other.GetComponent<NpcBehaviour>()) return;
        _npcCollider = null;
    }
}