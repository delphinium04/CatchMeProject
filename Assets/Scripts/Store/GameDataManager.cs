using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public enum ActiveItemType
    {
        BagSize,
        Speed,
        ValueSearch,
        Navigation
    }

    public static GameDataManager Instance { get; private set; }

    // 아이템과 가격을 저장하는 2차원 배열 리스트
    List<StealItem> _itemList = new(); // [아이템, 가격] 형태로 저장

    public int _currentStage { get; private set; } = 1;

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

    public List<StealItem> LoadStealItems()
    {
        // 아이템과 가격 리스트를 반환
        return new List<StealItem>(_itemList);
    }
}