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
    public int playerMoney; // 플레이어의 금액

    void Start()
    {
        // GameDataManager 초기화 및 아이템 데이터 가져오기
        _loadedStealItems = GameDataManager.Instance.LoadStealItems();
        if (_loadedStealItems.Count == 0)
        {
            Debug.LogWarning("저장된 아이템 데이터가 없습니다. 데이터를 초기화하세요.");
            return;
        }

        _loadedStealItems.ForEach(item => playerMoney += item.ItemValue * 100);

        DisplaySavedItems();
        UpdateMoneyUI();
    }

    // 저장된 아이템들을 스크롤뷰에 표시하는 함수
    void DisplaySavedItems()
    {
        _loadedStealItems.Sort((a, b) => b.ItemValue.CompareTo(a.ItemValue)); // 가치 기준 내림차순

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
        moneyText.text = $"{playerMoney:N0} 원";
    }

    // 아이템 구매 함수 (BuyButton onclick method)
    public void BuyItem(int itemIndex)
    {
        Debug.Log("BuyItem 함수 호출됨");

        // 구매 가능한지 체크
        if (playerMoney < _prices[itemIndex])
        {
            Debug.Log("돈 부족");
            return;
        }

        playerMoney -= _prices[itemIndex]; // 금액 차감
        buyButtons[itemIndex].interactable = false;
        soldOutImages[itemIndex].SetActive(true);

        UpdateMoneyUI(); // 금액 UI 업데이트
    }
}