using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndSceneManager : MonoBehaviour
{
    public Image[] _images;
    public TMP_Text _comment;
    
    void Start()
    {
        _images[(int)GameDataManager.Instance._houseType].gameObject.SetActive(true);
        string comment = GameDataManager.Instance._houseType switch
        {
            GameDataManager.HouseType.SeoulStation => "서울역 안에 나앉게됐다!\n뭘 먹고 살아야 하지?",
            GameDataManager.HouseType.SmallRoom => "나름 살만한 집을 얻었다!\n이 정도면 만족해~",
            GameDataManager.HouseType.GoodRoom => "좋은 집을 얻었다!\n넓직하고 훌륭한 집이야!",
            GameDataManager.HouseType.Building => "한남더힐 입주 성공!\n역시 돈이 최고야!",
            _ => "여긴 어디지?"
        };
        _comment.text = comment;
    }

    public void OnClick()
    {
        Application.Quit();
    }
}