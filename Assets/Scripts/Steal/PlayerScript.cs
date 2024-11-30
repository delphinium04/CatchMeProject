using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public StealUiManager StealUiManager;
    public float speed = 5f; //�÷��̾��� �̵� �ӵ�
    private IStealManager gm;
    //�÷��̾ �� �� �ִ� �� ������ ũ��
    public float maxX;
    public float maxY;
    public float minX;
    public float minY;
    public float next;
    private bool isColliding = false; //����� ������ �ִ��� Ȯ���ϴ� ����
    private bool canMove = true;

    private GameObject whocoll; //�ε��� ����� ����
    private SpriteRenderer sRen; //�÷��̾��� ���⿡ ���� �ٶ󺸴� ������ ���ϱ� ����

    public int stage; //���߿� ���� ������ �ٲ� ����

    void Start()
    {
        GameObject gameManager;
        stage = 1;
        if (stage == 1)
        {
            maxX = 39f;
            maxY = -0.8f;
            minX = -13f;
            minY = -5f;
            next = 38f;
            gameManager = GameObject.Find("Steal1Manager");
            gm = gameManager.GetComponent<Steal1ManagerScript>(); //���� �Ŵ����� ��ũ��Ʈ ��������
        }
        else if (stage == 2)
        {
            maxX = 69f;
            maxY = -1.7f;
            minX = -13f;
            minY = -5f;
            next = 68f;
            gameManager = GameObject.Find("Steal2Manager");
            gm = gameManager.GetComponent<Steal2ManagerScript>(); //���� �Ŵ����� ��ũ��Ʈ ��������
        }
        else if(stage == 3){
            maxX = 75f;
            maxY = -2.58f;
            minX = -13f;
            minY = -5f;
            next = 74f;
            gameManager = GameObject.Find("Steal3Manager");
            gm = gameManager.GetComponent<Steal3ManagerScript>(); //���� �Ŵ����� ��ũ��Ʈ ��������
        }
        sRen = GetComponent<SpriteRenderer>(); //�¿� ������ ����
    }

    void Update()
    {
        if (isColliding && Input.GetKeyDown(KeyCode.F)) //�ε��� ���¿��� fŰ�� ��������
        {
            PeopleScript pS = whocoll.GetComponent<PeopleScript>(); //�ε��� ����� ��ũ��Ʈ ��������
            canMove = false; // �÷��̾� ������ ���߱�
            if (pS.item >= 0 && pS.item <= 10) // �˸��� �������� ������ �ְ�
            {
                if (gm.currheavy + gm.heavy[pS.item] <= gm.maxbagsize) //���濡 ������ �ִٸ�
                {
                    gm.showitem(pS.item); //����� ������ �ִ� ������ �����ֱ�
                    StartCoroutine(WaitForYesOrNoButtonClick(pS.item)); //�ڷ�ƾ ����
                }
                else
                {
                    gm.ShowWhyCantSteal("���濡 �� ������ �����ϴ�");
                    canMove = true;
                }
            }
            else {
                gm.ShowWhyCantSteal("��ĥ �� �ִ� ������ �����ϴ�!");
                canMove = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            if (transform.position.x > next) {
                canMove = !canMove;
                StealUiManager.StageWin();
            }
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
        else if (other.CompareTag("woman4"))
        {
            isColliding = true; //�ε��� ���� Ȱ��ȭ
            whocoll = GameObject.Find("woman4");
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
        else if (other.CompareTag("man4"))
        {
            isColliding = true; //�ε��� ���� Ȱ��ȭ
            whocoll = GameObject.Find("man4");
        }
        else if (other.CompareTag("man5"))
        {
            isColliding = true; //�ε��� ���� Ȱ��ȭ
            whocoll = GameObject.Find("man5");
        }
        else if (other.CompareTag("man6"))
        {
            isColliding = true; //�ε��� ���� Ȱ��ȭ
            whocoll = GameObject.Find("man6");
        }
        else if (other.CompareTag("rich1"))
        {
            isColliding = true; //�ε��� ���� Ȱ��ȭ
            whocoll = GameObject.Find("rich1");
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
        else if (other.CompareTag("woman4"))
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
        else if (other.CompareTag("man3"))
        {
            isColliding = false;
        }
        else if (other.CompareTag("man4"))
        {
            isColliding = false;
        }
        else if (other.CompareTag("man5"))
        {
            isColliding = false;
        }
        else if (other.CompareTag("man6"))
        {
            isColliding = false;
        }
        else if (other.CompareTag("rich1"))
        {
            isColliding = false;
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
        gm.hideitem(item); //�����
        canMove = true;
        gm.yesorno = null; //�ڷ�ƾ �� �ʱ�ȭ
    }
}
