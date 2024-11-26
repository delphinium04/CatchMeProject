using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleScript : MonoBehaviour
{
    public GameObject fkey;
    public int item;  //플레이어가 어떤 아이템을 가지고 있는지 아이템 번호

    void Start()
    {
        fkey.SetActive(false); //f키 안보이기

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other) //부딪혔을 때
    {
        if (other.CompareTag("Player"))
        {
            fkey.SetActive(true); // 활성화
        }
    }

    void OnTriggerExit2D(Collider2D other) //떨어졌을 때
    {
        if (other.CompareTag("Player"))
        {
            fkey.SetActive(false); //비활성화
        }
    }

}
