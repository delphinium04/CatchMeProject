using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitStore : MonoBehaviour
{
    public void OnExitButtonClick()
    {
        SceneManager.LoadScene("StealScene");
    }
}
