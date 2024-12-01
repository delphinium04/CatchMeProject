using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{

    public GameObject[] people; //사람들
    //0: woman1, 1: woman2, 2: woman3, 3: man1, 4: man2, 5: man3 
    private List<int> whoHave; //사람의 인덱스 번호에따라 어떤 아이템(번호)가 담겨있는 리스트 

    //0: 지갑, 1: 핸드백, 2: 서류가방, 3: 루비, 4: 에메랄드, 5: 다이아
    public List<int> heavy; //물건들의 무게가 담겨있는 리스트
    public List<int> price; //물건들의 가격이 담겨있는 리스트

    public List<int> curritem; //플레이어가 현재 갖고 있는 아이템 리스트
    public int currheavy; //현재 플레이어가 가지고 있는 물건의 총무게
    public int maxbagsize; //가방의 최대 크기

    public Canvas itemCanvas;
    public Canvas UiCanvas;
    private Transform[] UiTransform;
    private Button[] itembuttons;
    private Image[] itemimages;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bagSizeText;
    private float time;

    public bool? yesorno = null; //bool? 타입은 true false NULL의 값을 가질 수 있음

    void Awake()
    {
        send_item(); // 사람들에게 아이템 보내기
        set_item_ph(); //아이템의 가격과 무게 설정해서 리스트에 넣기

        curritem = new List<int>();
        maxbagsize = 10;
        currheavy = 0;
        time = 60;

        itembuttons = itemCanvas.GetComponentsInChildren<Button>();
        itemimages = itemCanvas.GetComponentsInChildren<Image>();
        UiTransform = new Transform[UiCanvas.transform.childCount];

        for (int i = 0; i < UiCanvas.transform.childCount; i++)
        {
            UiTransform[i] = UiCanvas.transform.GetChild(i);
        }
        Debug.Log("UiTransform.Length :" + UiTransform.Length);

        for (int i = 0; i < people.Length; i++)
        {
            Button button = itembuttons[i];
            Image image = itemimages[i];
            int index = -1;

            switch (button.name)
            {
                case "wallet":
                    index = 0;
                    break;
                case "handbag":
                    index = 1;
                    break;
                case "briefcase":
                    index = 2;
                    break;
                case "ruby":
                    index = 3;
                    break;
                case "emerald":
                    index = 4;
                    break;
                case "diamond":
                    index = 5;
                    break;
            }
            if (index != -1)
            {
                button.onClick.AddListener(() => OnButtonClick(index)); //인덱스에 해당하는 버튼 클릭 넣기
                Color color = image.color;
                color.a = 0.5f; // 투명도를 50%로 설정
                image.color = color;
            }
        }
        Transform[] setUiTransform = new Transform[UiCanvas.transform.childCount];
        for (int i = 0; i < UiTransform.Length; i++)//원하는 순서대로 넣기 위해
        {
            Transform ui = UiTransform[i];
            int index = -1;

            switch (ui.name)
            {
                case "wallet":
                    index = 0;
                    setUiTransform[index] = ui;
                    break;
                case "handbag":
                    index = 1;
                    setUiTransform[index] = ui;
                    break;
                case "briefcase":
                    index = 2;
                    setUiTransform[index] = ui;
                    break;
                case "ruby":
                    index = 3;
                    setUiTransform[index] = ui;
                    break;
                case "emerald":
                    index = 4;
                    setUiTransform[index] = ui;
                    break;
                case "diamond":
                    index = 5;
                    setUiTransform[index] = ui;
                    break;
                case "box":
                    index = 6;
                    setUiTransform[index] = ui;
                    break;
                case "stillornot":
                    index = 7;
                    setUiTransform[index] = ui;
                    break;
                case "yesbutton":
                    index = 8;
                    setUiTransform[index] = ui;
                    break;
                case "nobutton":
                    index = 9;
                    setUiTransform[index] = ui;
                    break;
            }
            if(index >= 0 && index <= 6)  //이미지 아이템일 경우
            {
                Image Uiitemimage = ui.GetComponent<Image>();
                Uiitemimage.enabled = false;
            }
            else if (index == 7) //텍스트 일때
            {
                TextMeshProUGUI stillornot = ui.GetComponent<TextMeshProUGUI>();
                stillornot.enabled = false;
            }
            else if (index == 8)//스틸 버튼
            {
                TextMeshProUGUI yestext = ui.GetComponentInChildren<TextMeshProUGUI>();
                yestext.enabled = false;

                Image yesImage = ui.GetComponent<Image>(); // 버튼 이미지 숨기기
                yesImage.enabled = false;

                Button yesButton = ui.GetComponent<Button>();
                yesButton.onClick.AddListener(() => OnButtonClickYesOrNo(true));
                yesButton.interactable = false;
            }
            else if (index == 9) //아니오 버튼
            {
                TextMeshProUGUI notext = ui.GetComponentInChildren<TextMeshProUGUI>();
                notext.enabled = false;

                Image noImage = ui.GetComponent<Image>(); // 버튼 이미지 숨기기
                noImage.enabled = false;

                Button noButton = ui.GetComponent<Button>(); //버튼 숨기기
                noButton.onClick.AddListener(() => OnButtonClickYesOrNo(false));
                noButton.interactable = false;
            }
        }
        UiTransform = setUiTransform;
    }


        //if (bag_item) { 배낭 아이템을 갖고 있다면 (bool형)
        //int num = bag_Algorithm(heavy, price, maxbagsize); 배낭 알고리즘으로 플레이어가 가져야 되는 아이템 번호들을 가져옴
        //if(num == whoHave[i]);에 해당하는 사람 빛나기
        //bag_item = false;

    void Update()
    {
        time -= Time.deltaTime;
        int seconds = Mathf.FloorToInt(time);
        timerText.text = "남은 시간 : " + seconds.ToString();
        bagSizeText.text = "가방 : " + currheavy.ToString() + "/" + maxbagsize.ToString();
    }

    void set_item_ph()
    { //아이템들의 가격과 무게를 세팅하는 함수
        heavy = new List<int>();
        price = new List<int>();
        heavy.Add(1);
        heavy.Add(3);
        heavy.Add(4);
        heavy.Add(5);
        heavy.Add(5);
        heavy.Add(7);
        price.Add(Random.Range(1, 4));
        price.Add(Random.Range(2, 6));
        price.Add(Random.Range(3, 9));
        price.Add(5);
        price.Add(7);
        price.Add(10);
    }
    void Shuffle(int[] array) //배열을 랜덤으로 섞는 함수 
    {
        for (int i = 0; i < array.Length; i++)
        {
            int temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
    void send_item()
    { //게임 시작시 랜덤으로 사람들에게 아이템을 주는 변수
        int[] numbers = new int[people.Length];
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = i;
        }

        Shuffle(numbers);

        whoHave = new List<int>(numbers);

        for (int i = 0; i < people.Length; i++)
        {
            PeopleScript p = people[i].GetComponent<PeopleScript>();
            if (p != null)
            {
                whoHave.Add(numbers[i]);
                p.item = numbers[i];
            }
            else
            {
                Debug.LogError("People을 찾을 수 없습니다.");
            }
        }
    }
    public void showitem(int item) 
    {
        Image box = UiTransform[6].GetComponent<Image>();
        TextMeshProUGUI stillornot = UiTransform[7].GetComponent<TextMeshProUGUI>();

        Button yesButton = UiTransform[8].GetComponent<Button>();
        Button noButton = UiTransform[9].GetComponent<Button>();
        Image yesImage = UiTransform[8].GetComponent<Image>();
        Image noImage = UiTransform[9].GetComponent<Image>();
        TextMeshProUGUI yestext = UiTransform[8].GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI notext = UiTransform[9].GetComponentInChildren<TextMeshProUGUI>();

        Image Uiitemimage = UiTransform[item].GetComponent<Image>();

        box.enabled = true;
        stillornot.enabled = true;
        yesButton.interactable = true;
        yesImage.enabled = true;
        yestext.enabled = true;
        noButton.interactable = true;
        noImage.enabled = true;
        notext.enabled = true;
        Uiitemimage.enabled = true;
        //보여주기
    }
    public void hiddeitem(int item)
    {
        Image box = UiTransform[6].GetComponent<Image>();
        TextMeshProUGUI stillornot = UiTransform[7].GetComponent<TextMeshProUGUI>();

        Button yesButton = UiTransform[8].GetComponent<Button>();
        Button noButton = UiTransform[9].GetComponent<Button>();
        Image yesImage = UiTransform[8].GetComponent<Image>();
        Image noImage = UiTransform[9].GetComponent<Image>();

        TextMeshProUGUI yestext = UiTransform[8].GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI notext = UiTransform[9].GetComponentInChildren<TextMeshProUGUI>();

        Image Uiitemimage = UiTransform[item].GetComponent<Image>();

        box.enabled = false;
        stillornot.enabled = false;
        yesButton.interactable = false;
        yesImage.enabled = false;
        yestext.enabled = false;
        noButton.interactable = false;
        noImage.enabled = false;
        notext.enabled = false;
        Uiitemimage.enabled = false;
        //숨기기
    }
    public void getitem(int item) //아이템 얻기
    {
        curritem.Add(item); //현재 가지고 있는 아이템 리스트에 추가
        currheavy += heavy[item]; //무게 추가
        itembuttons[item].interactable = true; //해당 아이템 버튼 활성화
        Color color = itemimages[item].color;
        color.a = 1f; // 투명도를 100%로 설정
        itemimages[item].color = color;
    }
    void OnButtonClick(int item)  // 아이템에 해당하는 버튼을 클릭했을때 버리기
    {
        for (int i = 0; i < curritem.Count; i++)
        {
            if (curritem[i] == item) //현재 가지고 있는 아이템 중에 클릭한 아이템이 있다면
            {
                curritem.Remove(item); //해당 아이템 삭제
                itembuttons[item].interactable = false; // 해당 아이템 버튼 비활성화
                currheavy -= heavy[item];
                Color color = itemimages[item].color;
                color.a = 0.5f; // 투명도를 50%로 설정
                itemimages[item].color = color;
            }
        }
    }
    void OnButtonClickYesOrNo(bool result) {
        yesorno = result;

    }
    IEnumerator WaitForYesOrNoButtonClick() //버튼을 눌러값이 설정 될때까지 기다리는 함수
    {
        // 버튼이 클릭될 때까지 기다림
        while (yesorno == null)
        {
            yield return null;
        }
    }
}
