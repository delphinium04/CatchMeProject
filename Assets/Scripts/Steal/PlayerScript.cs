using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public StealUiManager StealUiManager;
    public float speed = 5f; //플레이어의 이동 속도
    private IStealManager gm;
    //플레이어가 갈 수 있는 맵 사이즈 크기
    public float maxX;
    public float maxY;
    public float minX;
    public float minY;
    public float next;
    private bool isColliding = false; //사람과 접촉해 있는지 확인하는 변수
    private bool canMove = true;

    private GameObject whocoll; //부딪힌 사람의 정보
    private SpriteRenderer sRen; //플레이어의 방향에 따라 바라보는 방향을 정하기 위해

    public int stage; //나중에 전역 변수로 바꿀 예정

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
            gm = gameManager.GetComponent<Steal1ManagerScript>(); //게임 매니저의 스크립트 가져오기
        }
        else if (stage == 2)
        {
            maxX = 69f;
            maxY = -1.7f;
            minX = -13f;
            minY = -5f;
            next = 68f;
            gameManager = GameObject.Find("Steal2Manager");
            gm = gameManager.GetComponent<Steal2ManagerScript>(); //게임 매니저의 스크립트 가져오기
        }
        else if(stage == 3){
            maxX = 75f;
            maxY = -2.58f;
            minX = -13f;
            minY = -5f;
            next = 74f;
            gameManager = GameObject.Find("Steal3Manager");
            gm = gameManager.GetComponent<Steal3ManagerScript>(); //게임 매니저의 스크립트 가져오기
        }
        sRen = GetComponent<SpriteRenderer>(); //좌우 반전을 위해
    }

    void Update()
    {
        if (isColliding && Input.GetKeyDown(KeyCode.F)) //부딪힌 상태에서 f키를 눌렀을때
        {
            PeopleScript pS = whocoll.GetComponent<PeopleScript>(); //부딪힌 사람의 스크립트 가져오기
            canMove = false; // 플레이어 움직임 멈추기
            if (pS.item >= 0 && pS.item <= 10) // 알맞은 아이템을 가지고 있고
            {
                if (gm.currheavy + gm.heavy[pS.item] <= gm.maxbagsize) //가방에 공간이 있다면
                {
                    gm.showitem(pS.item); //사람이 가지고 있는 아이템 보여주기
                    StartCoroutine(WaitForYesOrNoButtonClick(pS.item)); //코루틴 들어가기
                }
                else
                {
                    gm.ShowWhyCantSteal("가방에 들어갈 공간이 없습니다");
                    canMove = true;
                }
            }
            else {
                gm.ShowWhyCantSteal("훔칠 수 있는 물건이 없습니다!");
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
            // 플레이어의 이동 스크립트
            float h = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
            if (h < 0) //왼쪽 키를 눌렀을때
            {
                sRen.flipX = true; //대칭 활성화
            }
            else if (h > 0)
            { //오른쪽 키를 눌렀을때
                sRen.flipX = false; //대칭 비활성화
            }

            float v = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;
            Vector3 movePos = transform.position + new Vector3(h, v, 0f);
            movePos.x = Mathf.Clamp(movePos.x, minX, maxX);
            movePos.y = Mathf.Clamp(movePos.y, minY, maxY);
            transform.position = movePos;
        }

    }

    void OnTriggerEnter2D(Collider2D other) //부딪혔을 때
    {
        if (other.CompareTag("woman1"))
        {
            isColliding = true; //부딪힌 상태 활성화
            whocoll = GameObject.Find("woman1");
        }
        else if (other.CompareTag("woman2"))
        {
            isColliding = true; //부딪힌 상태 활성화
            whocoll = GameObject.Find("woman2");
        }
        else if (other.CompareTag("woman3"))
        {
            isColliding = true; //부딪힌 상태 활성화
            whocoll = GameObject.Find("woman3");
        }
        else if (other.CompareTag("woman4"))
        {
            isColliding = true; //부딪힌 상태 활성화
            whocoll = GameObject.Find("woman4");
        }
        else if (other.CompareTag("man1"))
        {
            isColliding = true; //부딪힌 상태 활성화
            whocoll = GameObject.Find("man1");
        }
        else if (other.CompareTag("man2"))
        {
            isColliding = true; //부딪힌 상태 활성화
            whocoll = GameObject.Find("man2");
        }
        else if (other.CompareTag("man3"))
        {
            isColliding = true; //부딪힌 상태 활성화
            whocoll = GameObject.Find("man3");
        }
        else if (other.CompareTag("man4"))
        {
            isColliding = true; //부딪힌 상태 활성화
            whocoll = GameObject.Find("man4");
        }
        else if (other.CompareTag("man5"))
        {
            isColliding = true; //부딪힌 상태 활성화
            whocoll = GameObject.Find("man5");
        }
        else if (other.CompareTag("man6"))
        {
            isColliding = true; //부딪힌 상태 활성화
            whocoll = GameObject.Find("man6");
        }
        else if (other.CompareTag("rich1"))
        {
            isColliding = true; //부딪힌 상태 활성화
            whocoll = GameObject.Find("rich1");
        }
    }

    void OnTriggerExit2D(Collider2D other) //떨어졌을 때
    {
        if (other.CompareTag("woman1"))
        {
            isColliding = false; //부딪힌 상태 비활성화
        }
        else if (other.CompareTag("woman2"))
        {
            isColliding = false; //부딪힌 상태 비활성화
        }
        else if (other.CompareTag("woman3"))
        {
            isColliding = false; //부딪힌 상태 비활성화
        }
        else if (other.CompareTag("woman4"))
        {
            isColliding = false; //부딪힌 상태 비활성화
        }
        else if (other.CompareTag("man1"))
        {
            isColliding = false; //부딪힌 상태 비활성화
        }
        else if (other.CompareTag("man2"))
        {
            isColliding = false; //부딪힌 상태 비활성화
        }
        else if (other.CompareTag("man2"))
        {
            isColliding = false; //부딪힌 상태 비활성화
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
    IEnumerator WaitForYesOrNoButtonClick(int item) //버튼을 눌러값이 설정 될때까지 기다리는 함수
    {
        // 버튼이 클릭될 때까지 기다림
        while (!gm.yesorno.HasValue)
        {
            yield return null;
        }

        if (gm.yesorno.Value)
        {
            gm.getitem(item); // 아이템 훔치기
            PeopleScript pS = whocoll.GetComponent<PeopleScript>();
            pS.item = -1; // 부딪힌 사람은 아이템 없음
        }
        gm.hideitem(item); //숨기기
        canMove = true;
        gm.yesorno = null; //코루틴 값 초기화
    }
}
