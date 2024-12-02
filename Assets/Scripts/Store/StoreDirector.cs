using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Dijkstra;

public class StoreDirector : MonoBehaviour
{
    public Canvas itemCanvas;               // 아이템들이 표시될 캔버스
    public Image[] itemimages;              // 아이템 이미지 배열
    public GameObject money;                // 플레이어의 금액을 표시할 텍스트
    public GameObject[] soldOutImages;      // 판매 완료된 아이템에 대한 SoldOut 이미지 배열
    public int playerMoney;                 // 플레이어의 금액
    public GameObject[] buttons;            // 아이템을 구매할 버튼들
    private List<int[]> loadedItemsAndValues; // 저장된 아이템과 그 값들을 담을 리스트
    int[] price = new int[] { 500, 500, 5000, 5000 }; // 아이템 가격 배열
    private GameDataManager gameDataManager; // 게임 데이터를 관리하는 객체
    public GameManagerScript GameManagerScript; // 게임 상태를 관리하는 객체
    public DijkstraManager DijkstraManager;
    public GameObject scrollContent;        // Scroll View의 Content 오브젝트
    public GameObject itemPrefab;           // 아이템을 표시할 프리팹

    // ScrollView 내용의 크기를 업데이트하는 함수
    void UpdateContentSize()
    {
        RectTransform contentRect = scrollContent.GetComponent<RectTransform>();

        if (contentRect != null)
        {
            // 아이템 높이와 자식 개수를 기반으로 Content 크기 업데이트
            float itemHeight = itemPrefab.GetComponent<RectTransform>().rect.height;
            int itemCount = scrollContent.transform.childCount;
            contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, itemHeight * itemCount);
        }
    }

    // 저장된 아이템들을 스크롤뷰에 표시하는 함수
    public void DisplaySavedItems()
    {
        // GameDataManager 객체가 초기화되지 않으면 에러 메시지 출력
        if (gameDataManager == null)
        {
            Debug.LogError("GameDataManager가 초기화되지 않았습니다.");
            return;
        }

        // 아이템 데이터 가져오기
        List<int[]> itemAndValue = gameDataManager.GetItemAndValue();
        if (itemAndValue == null || itemAndValue.Count == 0)
        {
            Debug.LogError("GameDataManager에서 가져온 itemAndValue가 비어 있습니다.");
            return;
        }

        itemAndValue.Sort((a, b) => b[1].CompareTo(a[1])); // 값(가치)를 기준으로 내림차순

        // 기존에 추가된 아이템들 제거
        foreach (Transform child in scrollContent.transform)
        {
            Destroy(child.gameObject);
        }

        // 저장된 아이템들을 하나씩 스크롤뷰에 추가
        foreach (int[] itemData in itemAndValue)
        {
            GameObject itemUI = Instantiate(itemPrefab, scrollContent.transform); // 아이템 프리팹 인스턴스화

            RectTransform rectTransform = itemUI.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(300, 100); // 아이템 크기 설정
                rectTransform.localScale = Vector3.one;
            }

            // 아이템 정보 처리
            int index = itemData[0];
            int value = itemData[1] * 100; // 가격 계산

            // 가격 텍스트 설정
            TextMeshProUGUI priceText = itemUI.transform.Find("PriceText")?.GetComponent<TextMeshProUGUI>();
            if (priceText != null)
            {
                priceText.fontSize = 24; // 폰트 크기 설정
                priceText.text = $"{value}원"; // 가격 표시
            }

            // 이미지 설정
            Image itemImage = itemUI.transform.Find("ItemImage")?.GetComponent<Image>();
            if (itemImage != null)
            {
                RectTransform imageRect = itemImage.GetComponent<RectTransform>();
                if (imageRect != null)
                {
                    imageRect.sizeDelta = new Vector2(80, 80); // 이미지 크기 설정
                }

                if (index >= 0 && index < itemimages.Length)
                {
                    itemImage.sprite = itemimages[index].sprite; // 이미지 설정
                }
            }
        }

        // 스크롤뷰의 내용 크기 업데이트
        UpdateContentSize();
    }

    

    // 플레이어의 금액 UI를 업데이트하는 함수
    void UpdateMoneyUI()
    {
        this.money.GetComponent<TextMeshProUGUI>().text = playerMoney.ToString("N0") + "원"; // 금액 표시
    }

    // 아이템 구매 함수
    public void BuyItem(int itemIndex)
    {
        Debug.Log("BuyItem 함수 호출됨");

        // 구매 가능한지 체크
        if (playerMoney >= price[itemIndex])
        {
            playerMoney -= price[itemIndex]; // 금액 차감

            // 각 아이템에 대해 다른 효과 처리
            switch (itemIndex)
            {
                case 0:
                    // 가방사이즈 증가
                    break;
                case 1:
                    // 속도 증가
                    break;
                case 2:
                    // 가치 확인 아이템 처리 예정
                    break;
                case 3:
                    // 네비게이션 아이템 처리 예정
                    break;
            }

            UpdateMoneyUI(); // 금액 UI 업데이트

            // 아이템 구매 버튼 비활성화
            Button itemButton = buttons[itemIndex].GetComponent<Button>();
            if (itemButton != null)
            {
                itemButton.interactable = false;
                Debug.Log("버튼 비활성화 성공");
            }
            else
            {
                Debug.Log("버튼 컴포넌트를 찾을 수 없음");
            }

            // SoldOut 이미지 활성화
            if (soldOutImages[itemIndex] != null)
            {
                soldOutImages[itemIndex].SetActive(true);
                Debug.Log("SoldOut 이미지 활성화 성공");
            }
            else
            {
                Debug.Log("SoldOut 이미지를 찾을 수 없음");
            }
        }
        else
        {
            Debug.Log("잔액 부족");
        }
    }

    void Start()
    {
        // GameDataManager 초기화 및 아이템 데이터 가져오기
        gameDataManager = FindObjectOfType<GameDataManager>();
        List<int[]> itemAndValue = gameDataManager.GetItemAndValue();
        UpdateContentSize(); // 스크롤뷰 크기 업데이트

        // 훔친 물건 정산
        foreach (int[] itemData in itemAndValue)
        {
            playerMoney += itemData[1] * 100;
        }

        // GameDataManager이 초기화되지 않으면 에러 출력
        if (gameDataManager == null)
        {
            Debug.LogError("GameDataManager를 찾을 수 없습니다.");
            return;
        }

        itemCanvas = GameObject.Find("itemCanvas").GetComponent<Canvas>(); // itemCanvas 가져오기
        if (itemCanvas == null)
        {
            Debug.LogError("itemCanvas를 찾을 수 없습니다. 이름을 확인하세요.");
            return;
        }

        // itemCanvas에서 Image 컴포넌트 가져오기
        itemimages = itemCanvas.GetComponentsInChildren<Image>();
        if (itemimages == null || itemimages.Length == 0)
        {
            Debug.LogError("itemCanvas에서 Image 컴포넌트를 가져오지 못했습니다.");
        }
        else
        {
            Debug.Log($"itemimages 초기화 완료. Count: {itemimages.Length}");
            foreach (var image in itemimages)
            {
                Debug.Log($"itemimages 요소: {image.name}");
            }
        }

        // 저장된 아이템 데이터가 없으면 에러 출력
        if (itemAndValue == null || itemAndValue.Count == 0)
        {
            Debug.LogError("저장된 아이템 데이터가 없습니다. 데이터를 초기화하세요.");
            return;
        }


        // 저장된 아이템들 표시
        DisplaySavedItems();

        // SoldOut 이미지 초기화
        foreach (GameObject image in soldOutImages)
        {
            if (image != null)
            {
                image.SetActive(false);
            }
        }

        // 금액 UI 업데이트
        UpdateMoneyUI();
    }

    void Update()
    {

    }
}
