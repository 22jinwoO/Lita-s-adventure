using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RandomBox : MonoBehaviour
{
    [Header("아이템 리스트 배열")]
    public Item[] weaponItems;
    [Header("아이템 확률 총합치")]
    public int total = 0;

    public List<WeaponInfo> weaponList = new List<WeaponInfo>();

    void Start()
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            // 스크립트가 활성화 되면 카드 덱의 모든 카드의 총 가중치를 구해줍니다.
            total += weaponList[i].weight;
        }
        
    }
    private void Update()
    {

    }

    //반환하는 값의 자료형이 weaponList라서 적음 반환형이 weaponList
    public int RandomBoxChoice()
    {
        int weight = 0;
        int selectNum = 0;
        //(total * Random.Range(0.0f, 1.0f) 이 부분이 토탈이 100이 넘어가도 Random.Range(0.0f, 1.0f)을 더해줘서 토탈을 100으로 인식되게 해줌
        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f)); //가중치 확률에서는 total 자체를 100으로 보고 total이 100이 넘어가도 확률을 계산해서 0.0~ 1.0의 범위를 곱해줘도 상관이 없음
        for (int i = 0; i < weaponList.Count; i++)
        {
            weight += weaponList[i].weight;
            if (selectNum <= weight)
            {
                return i;
            }
        }

        return 0;
    }
}
