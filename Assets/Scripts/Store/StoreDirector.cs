using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Dijkstra;

public class StoreDirector : MonoBehaviour
{
    readonly int[] _prices = { 500, 500, 5000, 5000 }; // 아이템 가격 배열
    public Button[] buyButtons; // 아이템을 구매할 버튼들
    public TMP_Text moneyText; // 플레이어의 금액을 표시할 텍스트
    public GameObject[] soldOutImages; // 판매 완료 이미지
    public GameObject itemPrefab; // 아이템을 표시할 프리팹
    public Transform scrollContentRoot; // Scroll View의 Content 오브젝트

    List<StealItem> _loadedStealItems; // 불러온 Steal Item
    // public int playerMoney; // 플레이어의 금액 < GameDataManger로 이전
    List<StealItem> MergeSort(List<StealItem> list)
    {
        if (list.Count <= 1)
            return list;
        int middle = list.Count / 2;
        List<StealItem> left = list.GetRange(0, middle);
        List<StealItem> right = list.GetRange(middle, list.Count - middle);
        return Merge(MergeSort(left), MergeSort(right));
    }


    // 병합 함수
    List<StealItem> Merge(List<StealItem> left, List<StealItem> right)
    {
        List<StealItem> result = new List<StealItem>();
        int i = 0, j = 0;
        while (i < left.Count && j < right.Count)
        {
            if (left[i].ItemValue >= right[j].ItemValue) // 내림차순
            {
                result.Add(left[i]);
                i++;
            }
            else
            {
                result.Add(right[j]);
                j++;
            }
        }
        while (i < left.Count)
        {
            result.Add(left[i]);
            i++;
        }
        while (j < right.Count)
        {
            result.Add(right[j]);
            j++;
        }
        return result;
    }
    void Start()
    {
        // GameDataManager 초기화 및 아이템 데이터 가져오기
        _loadedStealItems = GameDataManager.Instance.LoadStealItems();
        if (_loadedStealItems.Count == 0)
        {
            Debug.LogWarning("저장된 아이템 데이터가 없습니다. 데이터를 초기화하세요.");
            return;
        }

        _loadedStealItems.ForEach(item => GameDataManager.Instance._money += item.ItemValue * 100);

        DisplaySavedItems();
        UpdateMoneyUI();
    }

    // 저장된 아이템들을 스크롤뷰에 표시하는 함수
    void DisplaySavedItems()
    {
        _loadedStealItems = MergeSort(_loadedStealItems); // 합병 정렬 호출

        // 저장된 아이템들을 하나씩 스크롤뷰에 추가
        foreach (var item in _loadedStealItems)
        {
            GameObject listPrefab = Instantiate(itemPrefab, scrollContentRoot); // 아이템 프리팹 인스턴스화
            var itemTransform = listPrefab.transform;
            itemTransform.localScale = Vector3.one;

            // 아이템 정보 처리
            int value = item.ItemValue * 100; // 가격 계산

            // 가격 텍스트 설정
            TMP_Text priceText = listPrefab.transform.Find("PriceText")?.GetComponent<TMP_Text>();
            if (priceText != null)
            {
                priceText.fontSize = 24;
                priceText.text = $"{value}원";
            }

            // 이미지 설정
            Image itemImage = listPrefab.transform.Find("ItemImage")?.GetComponent<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = item.ItemSprite; // 이미지 설정
            }
        }
    }

    // 플레이어의 금액 UI를 업데이트하는 함수
    void UpdateMoneyUI()
    {
        moneyText.text = $"{GameDataManager.Instance._money:N0} 원";
    }

    // 아이템 구매 함수 (BuyButton onclick method)
    public void BuyItem(int itemIndex)
    {
        Debug.Log("BuyItem 함수 호출됨");

        // 구매 가능한지 체크
        if (GameDataManager.Instance._money < _prices[itemIndex])
        {
            Debug.Log("돈 부족");
            return;
        }

        GameDataManager.Instance._money -= _prices[itemIndex]; // 금액 차감
        buyButtons[itemIndex].interactable = false;
        soldOutImages[itemIndex].SetActive(true);

        switch (itemIndex)
        {
            case 0:
                GameDataManager.Instance.HasBag = true;
                break;
            case 1:
                GameDataManager.Instance.HasSpeed = true;
                break;
            case 2:
                GameDataManager.Instance.HasValueSearch = true;
                break;
            case 3:
                GameDataManager.Instance.HasNavigation = true;
                break;
            default:
                break;
        }

        UpdateMoneyUI(); // 금액 UI 업데이트
    }
}