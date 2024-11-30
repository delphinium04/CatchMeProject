using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleScript : MonoBehaviour
{
    public GameObject fkey;
    public ParticleSystem particle;
    public int item;  //�÷��̾ � �������� ������ �ִ��� ������ ��ȣ

    void Start()
    {
        fkey.SetActive(false); //fŰ �Ⱥ��̱�
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other) //�ε����� ��
    {
        if (other.CompareTag("Player"))
        {
            fkey.SetActive(true); // Ȱ��ȭ  
        }
    }

    void OnTriggerExit2D(Collider2D other) //�������� ��
    {
        if (other.CompareTag("Player"))
        {
            fkey.SetActive(false); //��Ȱ��ȭ
        }
    }

}
