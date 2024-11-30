using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class StealUiManager : MonoBehaviour
{
    public Action OnNextButtonClicked = null;
    public Button _retryButton;
    public Button _nextRoundButton;
    public GameObject _resultPanel;
    void Start()
    {
        if (_retryButton != null)
        {
            _retryButton.onClick.AddListener(() => SceneManager.LoadScene("Steal1Scene"));
        }
        if (_nextRoundButton != null)
        {
            _nextRoundButton.onClick.AddListener(() => OnNextButtonClicked?.Invoke());
        }
        OnNextButtonClicked = () => SceneManager.LoadScene("Steal2Scene");
    }

    public void StageWin()
    {
        _resultPanel.SetActive(true);
    }

}
