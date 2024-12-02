using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{

    public GameObject[] people; //�����
    //0: woman1, 1: woman2, 2: woman3, 3: man1, 4: man2, 5: man3 
    private List<int> whoHave; //����� �ε��� ��ȣ������ � ������(��ȣ)�� ����ִ� ����Ʈ 

    //0: ����, 1: �ڵ��, 2: ��������, 3: ���, 4: ���޶���, 5: ���̾�
    public List<int> heavy; //���ǵ��� ���԰� ����ִ� ����Ʈ
    public List<int> price; //���ǵ��� ������ ����ִ� ����Ʈ

    public List<int> curritem; //�÷��̾ ���� ���� �ִ� ������ ����Ʈ
    public int currheavy; //���� �÷��̾ ������ �ִ� ������ �ѹ���
    public int maxbagsize; //������ �ִ� ũ��

    public Canvas itemCanvas;
    public Canvas UiCanvas;
    private Transform[] UiTransform;
    private Button[] itembuttons;
    private Image[] itemimages;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bagSizeText;
    private float time;

    public bool? yesorno = null; //bool? Ÿ���� true false NULL�� ���� ���� �� ����

    void Awake()
    {
        send_item(); // ����鿡�� ������ ������
        set_item_ph(); //�������� ���ݰ� ���� �����ؼ� ����Ʈ�� �ֱ�

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
                button.onClick.AddListener(() => OnButtonClick(index)); //�ε����� �ش��ϴ� ��ư Ŭ�� �ֱ�
                Color color = image.color;
                color.a = 0.5f; // �������� 50%�� ����
                image.color = color;
            }
        }
        Transform[] setUiTransform = new Transform[UiCanvas.transform.childCount];
        for (int i = 0; i < UiTransform.Length; i++)//���ϴ� ������� �ֱ� ����
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
            if(index >= 0 && index <= 6) //�̹��� �������� ���
            {
                Image Uiitemimage = ui.GetComponent<Image>();
                Uiitemimage.enabled = false;
            }
            else if (index == 7) //�ؽ�Ʈ �϶�
            {
                TextMeshProUGUI stillornot = ui.GetComponent<TextMeshProUGUI>();
                stillornot.enabled = false;
            }
            else if (index == 8)//��ƿ ��ư
            {
                TextMeshProUGUI yestext = ui.GetComponentInChildren<TextMeshProUGUI>();
                yestext.enabled = false;

                Image yesImage = ui.GetComponent<Image>(); // ��ư �̹��� �����
                yesImage.enabled = false;

                Button yesButton = ui.GetComponent<Button>();
                yesButton.onClick.AddListener(() => OnButtonClickYesOrNo(true));
                yesButton.interactable = false;
            }
            else if (index == 9) //�ƴϿ� ��ư
            {
                TextMeshProUGUI notext = ui.GetComponentInChildren<TextMeshProUGUI>();
                notext.enabled = false;

                Image noImage = ui.GetComponent<Image>(); // ��ư �̹��� �����
                noImage.enabled = false;

                Button noButton = ui.GetComponent<Button>(); //��ư �����
                noButton.onClick.AddListener(() => OnButtonClickYesOrNo(false));
                noButton.interactable = false;
            }  
        }
        UiTransform = setUiTransform;
    }


        //if (bag_item) { �賶 �������� ���� �ִٸ� (bool��)
        //int num = bag_Algorithm(heavy, price, maxbagsize); �賶 �˰��������� �÷��̾ ������ �Ǵ� ������ ��ȣ���� ������
        //if(num == whoHave[i]);�� �ش��ϴ� ��� ������
        //bag_item = false;

    void Update()
    {
        time -= Time.deltaTime;
        int seconds = Mathf.FloorToInt(time);
        timerText.text = "���� �ð� : " + seconds.ToString();
        bagSizeText.text = "���� : " + currheavy.ToString() + "/" + maxbagsize.ToString();
    }

    void set_item_ph() { //�����۵��� ���ݰ� ���Ը� �����ϴ� �Լ�
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
    void Shuffle(int[] array) //�迭�� �������� ���� �Լ� 
    {
        for (int i = 0; i < array.Length; i++)
        {
            int temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
    void send_item() { //���� ���۽� �������� ����鿡�� �������� �ִ� ����
        int[] numbers = new int[people.Length];
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = i;
        }

        Shuffle(numbers);

        whoHave = new List<int>(numbers);

        for (int i = 0; i < people.Length; i++)
        {
            NpcBehaviour p = people[i].GetComponent<NpcBehaviour>();
            if (p != null)
            {
                whoHave.Add(numbers[i]);
                p._item = numbers[i];
            }
            else
            {
                Debug.LogError("People�� ã�� �� �����ϴ�.");
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
        //�����ֱ�
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
        //�����
    }
    public void getitem(int item) //������ ���
    {
        curritem.Add(item); //���� ������ �ִ� ������ ����Ʈ�� �߰�
        currheavy += heavy[item]; //���� �߰�
        itembuttons[item].interactable = true; //�ش� ������ ��ư Ȱ��ȭ
        Color color = itemimages[item].color;
        color.a = 1f; // �������� 100%�� ����
        itemimages[item].color = color;
    }
    void OnButtonClick(int item)  // �����ۿ� �ش��ϴ� ��ư�� Ŭ�������� ������
    {
        for (int i = 0; i < curritem.Count; i++) 
        {
            if (curritem[i] == item) //���� ������ �ִ� ������ �߿� Ŭ���� �������� �ִٸ�
            {
                curritem.Remove(item); //�ش� ������ ����
                itembuttons[item].interactable = false; // �ش� ������ ��ư ��Ȱ��ȭ
                currheavy -= heavy[item];
                Color color = itemimages[item].color;
                color.a = 0.5f; // �������� 50%�� ����
                itemimages[item].color = color;
            }
        }
    }
    void OnButtonClickYesOrNo(bool result) {
        yesorno = result;

    }
    IEnumerator WaitForYesOrNoButtonClick() //��ư�� �������� ���� �ɶ����� ��ٸ��� �Լ�
     {
        // ��ư�� Ŭ���� ������ ��ٸ�
        while (yesorno == null)
        {
            yield return null;
        }
    }
}
