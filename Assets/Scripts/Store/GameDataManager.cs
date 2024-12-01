using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameDataManager instance;

    // 아이템과 가격을 저장하는 2차원 배열 리스트
    public List<int[]> itemAndValue = new List<int[]>(); // [아이템, 가격] 형태로 저장

    // Awake는 객체가 초기화될 때 호출됨
    private void Awake()
    {
        // 싱글톤 패턴을 사용하여 중복된 인스턴스 생성 방지
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 존재하는 인스턴스가 있으면 새로운 객체를 파괴
        }
    }

    // 데이터를 저장하는 함수
    public void SaveData(List<int> items, List<int> priceList)
    {
        itemAndValue.Clear(); // 기존의 데이터를 초기화

        // 아이템과 가격을 묶어서 itemAndValue 리스트에 추가
        foreach (int item in items)
        {
            itemAndValue.Add(new int[] { item, priceList[item] }); // 아이템 번호와 가격을 배열로 저장
        }

        // 저장된 데이터 출력 (디버그용)
        Debug.Log("SaveData 호출됨. 저장된 데이터:");
        foreach (var pair in itemAndValue)
        {
            Debug.Log($"Item: {pair[0]}, Price: {pair[1]}"); // 아이템과 가격 로그 출력
        }
    }

    // 저장된 아이템과 가격 데이터를 반환하는 함수
    public List<int[]> GetItemAndValue()
    {
        // 데이터를 반환하기 전에 로그 출력 (디버그용)
        Debug.Log("GetItemAndValue 호출됨. 반환할 데이터:");
        foreach (var pair in itemAndValue)
        {
            Debug.Log($"Item: {pair[0]}, Price: {pair[1]}"); // 아이템과 가격 로그 출력
        }

        // 아이템과 가격 리스트를 반환
        return itemAndValue;
    }
}
