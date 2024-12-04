using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StealFailedUIManager : MonoBehaviour
{
    public void OnClicked()
    {
        SceneManager.LoadScene($"{StaticText.StealGameSceneName} {GameDataManager.Instance._currentStage}");
    }
}