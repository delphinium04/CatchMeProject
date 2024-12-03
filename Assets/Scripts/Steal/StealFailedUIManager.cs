using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StealFailedUIManager : MonoBehaviour
{
    public Button _retryButton;

    public void OnClicked()
    {
        SceneManager.LoadScene($"{StaticText.StealGameSceneName} {GameDataManager.Instance._currentStage}");
    }
}