using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dijkstra
{
    public class DijkstraUIManager : MonoBehaviour
    {
        List<int> _userPath;
        public TMP_Text _userPathText;
        public TMP_Text _timerText;
        public UnityEngine.UI.Button _resetButton;

        void Awake()
        {
            _userPath = new List<int>();
        }

        void Start()
        {
            UpdateUserPath();
            _resetButton.onClick.AddListener(() => { _userPath.Clear(); UpdateUserPath(); });
        }

        void UpdateUserPath()
        {
            if (_userPath.Count == 0)
            {
                _userPathText.text = "";
                return;
            }
            string t = string.Join(">", _userPath);
            _userPathText.text = t;
        }
    
        public void SetUserPath(params int[] path)
        {
            _userPath.AddRange(path);
            UpdateUserPath();
        }
        
        public void SetTimerText(float t)
        {
            string text = $"Timer: {t:F2}";
            _timerText.text = text;
        }
    }
}