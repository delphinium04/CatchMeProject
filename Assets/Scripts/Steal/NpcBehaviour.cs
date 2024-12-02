using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBehaviour : MonoBehaviour
{
    GameObject _interactableSprite;
    ParticleSystem _particle;
    public int _item;

    void Awake()
    {
        _interactableSprite = transform.GetChild(0).gameObject;
        _particle = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        _interactableSprite.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _interactableSprite.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _interactableSprite.SetActive(false);
        }
    }

}
