using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class StealManager : MonoBehaviour
{
    const int STAGE1_ITEM_TYPE_MAX = (int)ItemType.Diamond;
    const int STAGE2_ITEM_TYPE_MAX = (int)ItemType.Carkey;
    const int STAGE3_ITEM_TYPE_MAX = (int)ItemType.Gold;

    StealUIManager _uiManager;

    public NpcBehaviour[] _people;
    public StealItem[] _itemExistInScene; // Scene에 있는 모든 아이템

    public List<StealItem> _bag = new List<StealItem>(); // 유저가 고른 아이템
    public int _maxBagSize = 10;
    public int _currentBagSize = 0;

    void Awake()
    {
        _uiManager = FindObjectOfType<StealUIManager>();
    }

    void Start()
    {
        GameSetting();
    }

    void GameSetting()
    {
        // item
        if (GameDataManager.Instance.HasBag)
            _maxBagSize += 5;
        AssignItemToNpc();
        StartCoroutine("SetTimer", 30);
    }
    
    
    void AssignItemToNpc()
    {
        _people = GameObject.FindGameObjectsWithTag("NPC").Select(npc => npc.GetComponent<NpcBehaviour>()).ToArray();
        var allResources = Resources.LoadAll<StealItem>("ItemSteal").ToList();

        int maxItemEnum = GameDataManager.Instance._currentStage switch
        {
            1 => STAGE1_ITEM_TYPE_MAX,
            2 => STAGE2_ITEM_TYPE_MAX,
            3 => STAGE3_ITEM_TYPE_MAX,
            _ => throw new ArgumentOutOfRangeException()
        };

        List<StealItem> list = allResources.Where(e =>
            (int)e.ItemType <= maxItemEnum).ToList();

        _itemExistInScene = new StealItem[_people.Length];

        for (var index = 0; index < _people.Length; index++)
        {
            StealItem currentItem = list[Random.Range(0, list.Count)];
            list.Remove(currentItem);
            _people[index]._item = currentItem;
            _itemExistInScene[index] = currentItem;
        }

        _uiManager.SetItemList(_itemExistInScene);
    }
    
    public void RemoveItem(StealItem item)
    {
        _bag.Remove(item);
        _currentBagSize -= item.ItemWeight;
        _uiManager.UpdateItemList(item, false);
        _uiManager.UpdateWeight(_currentBagSize, _maxBagSize);
    }


    // Called from PlayerBehaviour
    public void TryPickItem(NpcBehaviour npc)
    {
        var item = npc._item;
        if (_bag.Contains(item))
        {
            _uiManager.EnablePickFailPanel("이미 들고 있습니다!");
            return;
        }
        if (_maxBagSize < _currentBagSize + item.ItemWeight)
        {
            _uiManager.EnablePickFailPanel("가방이 꽉 찼습니다!");
            return;
        }

        StartCoroutine(_uiManager.WaitForUserConfirm(item, pickSuccess =>
        {
            if (!pickSuccess) return;
            _bag.Add(item);
            _currentBagSize += item.ItemWeight;
            _uiManager.UpdateItemList(item, true);
            _uiManager.UpdateWeight(_currentBagSize, _maxBagSize);
        }));
    }

    IEnumerator SetTimer(int time)
    {
        while (--time >= 0)
        {
            _uiManager.UpdateTimer(time);
            yield return new WaitForSeconds(1);
        }

        GameOver();
    }

    void GameOver()
    {
        SceneManager.LoadScene(StaticText.StealGameOverSceneName);
    }

    public void StageEnd()
    {
        GameDataManager.Instance.SaveStealItems(_bag);
        _uiManager.StageWin();
    }

    List<StealItem> BagAlgorithm(List<StealItem> items, int maxBagSize)
    {
        List<int> weight = new List<int>();
        List<int> price = new List<int>();
        List<StealItem> bestBag = new List<StealItem>();

        weight.AddRange(_itemExistInScene.Select(item => item.ItemWeight));
        price.AddRange(_itemExistInScene.Select(item => item.ItemWeight));

        int n = weight.Count;
        int[,] calculatedHistory = new int[n + 1, maxBagSize + 1];

        for (int i = 1; i <= n; i++)
        {
            for (int w = 1; w <= maxBagSize; w++)
            {
                if (weight[i - 1] <= w)
                {
                    calculatedHistory[i, w] = Mathf.Max(calculatedHistory[i - 1, w],
                        calculatedHistory[i - 1, w - weight[i - 1]] + price[i - 1]);
                }
                else
                {
                    calculatedHistory[i, w] = calculatedHistory[i - 1, w];
                }
            }
        }

        int currentWeight = maxBagSize;
        for (int i = n; i > 0; i--)
        {
            if (calculatedHistory[i, currentWeight] != calculatedHistory[i - 1, currentWeight])
            {
                bestBag.Add(items[i - 1]);
                currentWeight -= weight[i - 1];
            }
        }

        return bestBag;
    }
}