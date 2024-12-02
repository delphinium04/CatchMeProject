using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitStore : MonoBehaviour
{
    public void OnExitButtonClick()
    {
        if (GameDataManager.Instance._currentStage == 3) return;
        // SceneManager.LoadScene("StealScene");

        GameDataManager.Instance.SetCurrentStage(GameDataManager.Instance._currentStage + 1);
        SceneManager.LoadScene($"{StaticText.DijkstraGameSceneName} {GameDataManager.Instance._currentStage}");
    }
}
