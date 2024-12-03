using System;
using System.Collections;
using TMPro;
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

    [Header("Texts")] [SerializeField] TMP_Text _timerText;
    [SerializeField] TMP_Text _bagText;
    [SerializeField] TMP_Text _itemStealText;
    [SerializeField] TMP_Text _noticeText;
    [SerializeField] TMP_Text _valueText;
    
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

            var handler = go.AddComponent<StealItemUIHandler>();
            handler._item = item;
        }
    }

    public void UpdateItemList(StealItem item, bool isPicked)
    {
        Image image = _itemListRoot.Find(item.name).GetComponent<Image>();
        image.GetComponent<StealItemUIHandler>()._isActivated = isPicked;
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

        if (GameDataManager.Instance._hasValueSearch)
        {
            _itemStealText.gameObject.SetActive(true);
            _itemStealText.text = $"무게:{item.ItemWeight}           가치:{item.ItemValue}";
        }
        else _itemStealText.gameObject.SetActive(false);

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

    public void EnablePickFailPanel(string message)
    {
        _pickFailPanel.SetActive(true);
        _noticeText.text = message;
    }

    void OnPickFailOkButtonClicked()
    {
        _pickFailPanel.SetActive(false);
    }


    public void StageWin(int currentValue, int maxValue)
    {
        _resultPanel.SetActive(true);
        _valueText.text = $"이번 라운드 최대 가치: {maxValue}원\n현재 물건 가치: {currentValue}원";
    }
}