using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitStore : MonoBehaviour
{
    public void OnExitButtonClick()
    {
        if (GameDataManager.Instance._currentStage == 3)
        {
            SceneManager.LoadScene("End");
            return;
        }

        GameDataManager.Instance.SetCurrentStage(GameDataManager.Instance._currentStage + 1);
        SceneManager.LoadScene($"{StaticText.StealGameSceneName} {GameDataManager.Instance._currentStage}");
    }
}
