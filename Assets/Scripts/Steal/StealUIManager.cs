using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StealUIManager : MonoBehaviour
{
    [Header("Root")] [SerializeField] Transform _itemListRoot;
    
    [Header("Buttons")] [SerializeField] Button _nextRoundButton;
    [SerializeField] Button _acceptButton;
    [SerializeField] Button _denyButton;
    [SerializeField] Button _pickFailOkButton;
    
    [Header("Texts")]
    [SerializeField] TMP_Text _timerText;
    [SerializeField] TMP_Text _bagText;
    
    [Header("Images")] [SerializeField] Image _promptItemImage;
    
    [Header("Panels")] [SerializeField] GameObject _resultPanel;
    [SerializeField] GameObject _promptPanel;
    [SerializeField] GameObject _pickFailPanel;

    bool _isButtonClicked = false;
    bool _promptResult = false;

    void Start()
    {
        _acceptButton.onClick.AddListener(() => OnPickButtonClicked(true));
        _denyButton.onClick.AddListener(() => OnPickButtonClicked(false));
        _pickFailOkButton.onClick.AddListener(OnPickFailOkButtonClicked);
        _nextRoundButton.onClick.AddListener(() => { SceneManager.LoadScene(StaticText.StoreSceneName); });
    }

    public void SetItemList(StealItem[] items)
    {
        foreach (var item in items)
        {
            GameObject go = new GameObject();
            go.name = item.name;
            go.transform.SetParent(_itemListRoot);
            go.transform.localScale = Vector3.one;

            var image = go.AddComponent<Image>();
            image.sprite = item.ItemSprite;
            image.color = Color.white * 0.75f;
        }
    }

    public void UpdateItemList(StealItem item, bool isPicked)
    {
        Image image = _itemListRoot.Find(item.name).GetComponent<Image>();
        if (isPicked) image.color = Color.white;
        else image.color = Color.white * 0.75f;
    }

    public void UpdateTimer(int time)
    {
        _timerText.text = $"남은 시간: {time:D2}";
    }
    
    public void UpdateWeight(int weight, int maxWeight)
    {
        _bagText.text = $"가방:\n{weight:D2}/{maxWeight}";
    }

    public IEnumerator WaitForUserConfirm(StealItem item, Action<bool> onComplete)
    {
        _promptPanel.SetActive(true);
        _promptItemImage.sprite = item.ItemSprite;
        _isButtonClicked = false;

        while (!_isButtonClicked)
        {
            yield return null; // 사용자 클릭 대기
        }

        _promptPanel.SetActive(false);
        onComplete(_promptResult);
    }
    
    // prompt panel button onclick method
    void OnPickButtonClicked(bool value)
    {
        _promptResult = value;
        _isButtonClicked = true;
    }

    public void EnablePickFailPanel()
    {
        _pickFailPanel.SetActive(true);
    }

    void OnPickFailOkButtonClicked()
    {
        _pickFailPanel.SetActive(false);
    }


    public void StageWin()
    {
        _resultPanel.SetActive(true);
    }
}