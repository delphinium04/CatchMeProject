using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Dijkstra
{
    public class DijkstraUIManager : MonoBehaviour
    {
        public Action OnResetButtonClicked = null;
        public Action OnConfirmButtonClicked = null;
        public Action OnNextButtonClicked = null;
        
        [Header("For Game")]
        public TMP_Text _userPathText;
        public TMP_Text _timerText;
        public TMP_Text _stageText;
        public TMP_Text _targetNodeText;
        public Button _resetButton;
        public Button _confirmButton;
        public Button _nextRoundButton;
        public GameObject _resultPanel;

        [Header("For Failed Scene")]
        public Button _retryButton; 

        void Start()
        {
            _resultPanel?.SetActive(false);

            if(_resetButton != null)
                _resetButton.onClick.AddListener(() => OnResetButtonClicked?.Invoke()); 
            if(_confirmButton != null)
                _confirmButton.onClick.AddListener(() => OnConfirmButtonClicked?.Invoke());  
            if(_nextRoundButton != null)
                _nextRoundButton.onClick.AddListener(() => OnNextButtonClicked?.Invoke());
            if(_retryButton != null)
                _retryButton.onClick.AddListener(() => SceneManager.LoadScene("Dijkstra"));
        }

        #region INGAME

        public void SetStage(int stageNumber, int targetNode)
        {
            _stageText.text = $"스테이지 {stageNumber}";
            _targetNodeText.text = $"목표 도착점: {targetNode}";
        }

        public void SetUserPath(params int[] path)
        {
            string text = String.Join(" > ", path);
            _userPathText.text = text;
        }

        public void SetTimerText(float t)
        {
            string text = $"Timer: {t:F2}";
            _timerText.text = text;
        }

        public void StageWin()
        {
            _resultPanel.SetActive(true);
        }

        #endregion INGAME
        }
}