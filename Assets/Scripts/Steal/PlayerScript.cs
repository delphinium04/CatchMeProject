using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public float speed = 5f; //�÷��̾��� �̵� �ӵ�
    private GameObject gameManager;
    private GameManagerScript gm;
    //�÷��̾ �� �� �ִ� �� ������ ũ��
    public float maxX = 40f;
    public float maxY = -0.8f;
    public float minX = -13f;
    public float minY = -5f;
    private bool isColliding = false; //����� ������ �ִ��� Ȯ���ϴ� ����
    private bool canMove = true;

    private GameObject whocoll; //�ε��� ����� ����
    private SpriteRenderer sRen; //�÷��̾��� ���⿡ ���� �ٶ󺸴� ������ ���ϱ� ����

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gm = gameManager.GetComponent<GameManagerScript>(); //���� �Ŵ����� ��ũ��Ʈ ��������
        sRen = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isColliding && Input.GetKeyDown(KeyCode.F)) //�ε��� ���¿��� fŰ�� ��������
        {
            PeopleScript pS = whocoll.GetComponent<PeopleScript>(); //�ε��� ����� ��ũ��Ʈ ��������
            canMove = false; // �÷��̾� ������ ���߱�
            if (pS.item >= 0 && pS.item <= 5) // �˸��� �������� ������ �ְ�
            {
                if (gm.currheavy + gm.heavy[pS.item] <= gm.maxbagsize) //���濡 ������ �ִٸ�
                {
                    gm.showitem(pS.item); //����� ������ �ִ� ������ �����ֱ�
                    StartCoroutine(WaitForYesOrNoButtonClick(pS.item)); //�ڷ�ƾ ����
                }
                else
                {
                    Debug.Log("���濡 �� ������ �����ϴ�!");
                    canMove = true;
                }
            }
            else {
                Debug.Log("��ĥ �� �ִ� ������ �����ϴ�!");
                canMove = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            // �÷��̾��� �̵� ��ũ��Ʈ
            float h = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
            if (h < 0) //���� Ű�� ��������
            {
                sRen.flipX = true; //��Ī Ȱ��ȭ
            }
            else if (h > 0)
            { //������ Ű�� ��������
                sRen.flipX = false; //��Ī ��Ȱ��ȭ
            }

            float v = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;
            Vector3 movePos = transform.position + new Vector3(h, v, 0f);
            movePos.x = Mathf.Clamp(movePos.x, minX, maxX);
            movePos.y = Mathf.Clamp(movePos.y, minY, maxY);
            transform.position = movePos;
        }

    }

    void OnTriggerEnter2D(Collider2D other) //�ε����� ��
    {
        if (other.CompareTag("woman1"))
        {
            isColliding = true; //�ε��� ���� Ȱ��ȭ
            whocoll = GameObject.Find("woman1");
        }
        else if (other.CompareTag("woman2"))
        {
            isColliding = true; //�ε��� ���� Ȱ��ȭ
            whocoll = GameObject.Find("woman2");
        }
        else if (other.CompareTag("woman3"))
        {
            isColliding = true; //�ε��� ���� Ȱ��ȭ
            whocoll = GameObject.Find("woman3");
        }
        else if (other.CompareTag("man1"))
        {
            isColliding = true; //�ε��� ���� Ȱ��ȭ
            whocoll = GameObject.Find("man1");
        }
        else if (other.CompareTag("man2"))
        {
            isColliding = true; //�ε��� ���� Ȱ��ȭ
            whocoll = GameObject.Find("man2");
        }
        else if (other.CompareTag("man3"))
        {
            isColliding = true; //�ε��� ���� Ȱ��ȭ
            whocoll = GameObject.Find("man3");
        }
    }

    void OnTriggerExit2D(Collider2D other) //�������� ��
    {
        if (other.CompareTag("woman1"))
        {
            isColliding = false; //�ε��� ���� ��Ȱ��ȭ
        }
        else if (other.CompareTag("woman2"))
        {
            isColliding = false; //�ε��� ���� ��Ȱ��ȭ
        }
        else if (other.CompareTag("woman3"))
        {
            isColliding = false; //�ε��� ���� ��Ȱ��ȭ
        }
        else if (other.CompareTag("man1"))
        {
            isColliding = false; //�ε��� ���� ��Ȱ��ȭ
        }
        else if (other.CompareTag("man2"))
        {
            isColliding = false; //�ε��� ���� ��Ȱ��ȭ
        }
        else if (other.CompareTag("man2"))
        {
            isColliding = false; //�ε��� ���� ��Ȱ��ȭ
        }
    }
    IEnumerator WaitForYesOrNoButtonClick(int item) //��ư�� �������� ���� �ɶ����� ��ٸ��� �Լ�
    {
        // ��ư�� Ŭ���� ������ ��ٸ�
        while (!gm.yesorno.HasValue)
        {
            yield return null;
        }

        if (gm.yesorno.Value)
        {
            gm.getitem(item); // ������ ��ġ��
            PeopleScript pS = whocoll.GetComponent<PeopleScript>();
            pS.item = -1; // �ε��� ����� ������ ����
        }
        gm.hiddeitem(item); //�����
        canMove = true;
        gm.yesorno = null; //�ڷ�ƾ �� �ʱ�ȭ
    }
}
