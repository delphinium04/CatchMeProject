using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    List<int> _bestPath;
    List<int> _userPath;
    public TMP_Text bestPathText;
    public TMP_Text userPathText;
    public UnityEngine.UI.Button resetButton;
    public UnityEngine.UI.Button timeButton;

    void Start()
    {
        _bestPath = new List<int>();
        _userPath = new List<int>();
        UpdateBestPath();
        UpdateUserPath();
        resetButton.onClick.AddListener(() => { _userPath.Clear(); UpdateUserPath(); });
        timeButton.onClick.AddListener(() =>
        {
            
        });
    }

    void UpdateUserPath()
    {
        if (_userPath.Count == 0)
        {
            userPathText.text = "";
            return;
        }
        string t = "";
        for (int i = 0; i < _userPath.Count - 1; i++)
            t += $"{_userPath[i]} -> ";
        t += $"{_userPath[^1]}";
        userPathText.text = t;
    }

    void UpdateBestPath()
    {
        if (_bestPath.Count == 0)
        {
            bestPathText.text = "";
            return;
        }

        string t = "";
        for (int i = 0; i < _bestPath.Count - 1; i++)
            t += $"{_bestPath[i]} -> ";
        t += $"{_bestPath[^1]}";
        bestPathText.text = t;
    }
    
    public void SetBestPath(params int[] path)
    {
        _bestPath.Clear();
        _bestPath.AddRange(path);
        UpdateBestPath();
    }
    
    public void SetUserPath(params int[] path)
    {
        _userPath.AddRange(path);
        UpdateUserPath();
    }
}