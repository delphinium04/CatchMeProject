using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Steal1ManagerScript : MonoBehaviour, IStealManager
{

    public GameObject[] people; //�����


    //0: ����, 1: �ڵ��, 2: ��������, 3: ���, 4: ���޶���, 5: ���̾�
    public List<int> heavy { get; set; } //���ǵ��� ���԰� ����ִ� ����Ʈ
    public List<int> price; //���ǵ��� ������ ����ִ� ����Ʈ

    public List<int> curritem; //�÷��̾ ���� ���� �ִ� ������ ����Ʈ
    public int currheavy { get; set; } //���� �÷��̾ ������ �ִ� ������ �ѹ���
    public int maxbagsize { get; set; } //������ �ִ� ũ��

    public Canvas itemCanvas;
    public Canvas UiCanvas;
    private Transform[] UiTransform;
    private Button[] itembuttons;
    private Image[] itemimages;

    public TMP_Text timerText;
    public TMP_Text bagSizeText;
    public TMP_Text whycantsteal;

    private float time;
    private int invokeCount = 0;
    private int maxInvokeCount = 5;

    public bool? yesorno { get; set; } = null;  //bool? Ÿ���� true false NULL�� ���� ���� �� ����

    public bool bag_item = false;

    void Awake()
    {
        send_item(); // ����鿡�� ������ ������
        set_item_ph(); //�������� ���ݰ� ���� �����ؼ� ����Ʈ�� �ֱ�
        HideCantSteal(); //��ġ�� ���Ҷ� ������ �ؽ�Ʈ �����

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
            if (index >= 0 && index <= 6) //�̹��� �������� ���
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

        if (bag_item) { //�賶 �������� ���� �ִٸ� (bool��)
            List<int> num = bag_Algorithm(heavy, price, maxbagsize); //�賶 �˰��������� �÷��̾ ������ �Ǵ� ������ ��ȣ���� ������
            foreach (int n in num)
            {
               Debug.Log(n);
            }
            foreach (GameObject human in people)  //��� ����� ã�ƺ��鼭
            {
                ParticleSystem particle = human.GetComponentInChildren<ParticleSystem>();
                if (particle != null)//��ƼŬ �ý����� �ְ�
                {
                    if (num.Contains(human.GetComponent<PeopleScript>().item)) //�ش� ��ȣ�� �ִٸ�
                    {
                        particle.Play(); //��ƼŬ ����
                    }
                }
            }
            bag_item = false; //�������� �������
        }
    }
 
    void Update()
    {
        time -= Time.deltaTime;
        int seconds = Mathf.FloorToInt(time);
        timerText.text = "남은 시간 : " + seconds;
        bagSizeText.text = "가방 : " + currheavy + "/" + maxbagsize;

        if (time <= 0) {
            SceneManager.LoadScene("StealFailed");
        }
    }

    void set_item_ph() { //�����۵��� ���ݰ� ���Ը� �����ϴ� �Լ�
        heavy = new List<int>();
        price = new List<int>();
        int[] heavyArray = { 1, 3, 4, 5, 5, 8 };
        int[] priceArray = { Random.Range(1, 4), Random.Range(2, 6), Random.Range(3, 9), 5, 7, 10 };
        for (int i = 0; i < heavyArray.Length; i++)
        {
            heavy.Add(heavyArray[i]);
            price.Add(priceArray[i]);
        }
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

        for (int i = 0; i < people.Length; i++)
        {
            PeopleScript p = people[i].GetComponent<PeopleScript>();
            if (p != null)
            {
                p.item = numbers[i];
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
    public void hideitem(int item)
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
    public void ShowWhyCantSteal(string why) {
        if (invokeCount >= maxInvokeCount) {
            CancelInvoke("HideCantSteal");
            invokeCount = 0;
            return;
        }
        whycantsteal.text = why;
        whycantsteal.gameObject.SetActive(true);
        Invoke("HideCantSteal", 1); //1�� �ڿ� ����Ǵ� �ɷι̷��
        invokeCount++;
    }
    void HideCantSteal() {
        whycantsteal.gameObject.SetActive(false);
        invokeCount--;
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

    List<int> bag_Algorithm(List<int> heavy, List<int> price, int maxbagsize)
    {
        int n = heavy.Count;
        int[,] K = new int[n + 1, maxbagsize + 1]; //���� �ڵ� ������ ���� ��ȣ�� ��ĭ �÷��� ����(C#������ �ڵ����� 0�ʱ�ȭ)
        List<int> items = new List<int>(); // ���õ� �����۵��� �� ����Ʈ

        for (int i = 1; i <= n; i++) //������ ä���
        { // �����ۿ� ���� �ݺ�(�ε����� 1�� ���� ����)
            for (int w = 1; w <= maxbagsize; w++)
            { // ���Կ� ���� �ݺ�(���԰� 0�϶��� ������ ��ġ0)
                if (heavy[i - 1] <= w) //�������� �� �� ������
                {
                    //������ ������ �� ������ �������� ���� �Ͱ� ���� �������� ���� ���� ��ġ�� ���� ��ġ�� �� ū���� ��
                    K[i, w] = Mathf.Max(K[i-1, w], K[i-1, w - heavy[i-1]] + price[i-1]); //�ε��� ��ȣ�� 0���� �����ϱ� ������ i - 1
                }
                else //�� �� ������ ���� �� �״��
                {
                    K[i, w] = K[i - 1, w];
                }
            }
        }

        // �ִ� ��ġ�� ���� �ϴ� �������� ã�� ���� ������
        int currw = maxbagsize;
        for (int i = n; i > 0; i--)
        {
            if (K[i, currw] != K[i-1, currw]) //������ �����϶�, ���̺����� �� �Ʒ� ���̰� ���� ��� �������� ���ٰ� �Ǵ�
            {
                items.Add(i-1); // ������ �ε����� ����Ʈ�� �߰�
                currw -= heavy[i-1]; //�� ������ �������� ����
            }
        }
        return items; //�� �����۵� ����
    }
}
