using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StealUiManager : MonoBehaviour
{
    public Button _retryButton;
    public Button _nextRoundButton;
    public GameObject _resultPanel;

    void Start()
    {
        if (_retryButton != null)
        {
            _retryButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        }

        if (_nextRoundButton != null)
        {
            _nextRoundButton.onClick.AddListener(() => { SceneManager.LoadScene(StaticText.StoreSceneName); });
        }
    }

    public void StageWin()
    {
        _resultPanel.SetActive(true);
    }

}
