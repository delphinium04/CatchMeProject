using System;
using Dijkstra;
using Dijkstra.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DijkstraOverUIManager : DijkstraUIBase
{
    public enum Texts
    {
        FailText,
    }

    public enum Buttons
    {
        RetryButton,
    }

    void Awake()
    {
        Bind<TMP_Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        
        Get<Button>((int)Buttons.RetryButton).onClick.AddListener(() =>
        {
            SceneManager.LoadScene(StaticText.DijkstraGameSceneName);
        });
    }

    void Start()
    {
        // 게임 패배 요인에 따른 Text 세팅
        GameFail fail = (GameFail) PlayerPrefs.GetInt(StaticText.PlayerPrefGameOverSign);
        string text = fail switch
        {
            GameFail.TimeOver => StaticText.TimeOverMessage,
            GameFail.TooSlow => StaticText.TooSlowMessage,
            GameFail.WrongWay => StaticText.WrongWayMessage,
            _ => throw new ArgumentOutOfRangeException(nameof(fail), fail, null)
        };
        
        Get<TMP_Text>((int)Texts.FailText).text = text;
    }
}
