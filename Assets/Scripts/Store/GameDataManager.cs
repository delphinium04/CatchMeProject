using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public bool _hasBag = false;
    public bool _hasSpeed = false;
    public bool _hasValueSearch = false;
    public bool _hasNavigation = false;

    public HouseType _houseType;

    public enum HouseType
    {
        SeoulStation,
        SmallRoom,
        GoodRoom,
        Building
    }

    public static GameDataManager Instance { get; private set; }

    // 아이템과 가격을 저장하는 2차원 배열 리스트
    List<StealItem> _itemList = new(); // [아이템, 가격] 형태로 저장

    public int _currentStage { get; set; } = 1;

    public int _money { get; set; } = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentStage(int s)
        => _currentStage = s;

    public void SaveStealItems(List<StealItem> items)
    {
        _itemList = new List<StealItem>(items);
    }

    // Store 씬 시작 시 초기호
    public List<StealItem> LoadStealItems()
    {
        _hasBag = false;
        _hasSpeed = false;
        _hasNavigation = false;
        _hasValueSearch = false;
        // 아이템과 가격 리스트를 반환
        return new List<StealItem>(_itemList);
    }
}