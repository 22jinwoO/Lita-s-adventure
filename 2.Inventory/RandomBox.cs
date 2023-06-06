using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RandomBox : MonoBehaviour
{
    [Header("������ ����Ʈ �迭")]
    public Item[] weaponItems;
    [Header("������ Ȯ�� ����ġ")]
    public int total = 0;

    public List<WeaponInfo> weaponList = new List<WeaponInfo>();

    void Start()
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            // ��ũ��Ʈ�� Ȱ��ȭ �Ǹ� ī�� ���� ��� ī���� �� ����ġ�� �����ݴϴ�.
            total += weaponList[i].weight;
        }
        
    }
    private void Update()
    {

    }

    //��ȯ�ϴ� ���� �ڷ����� weaponList�� ���� ��ȯ���� weaponList
    public int RandomBoxChoice()
    {
        int weight = 0;
        int selectNum = 0;
        //(total * Random.Range(0.0f, 1.0f) �� �κ��� ��Ż�� 100�� �Ѿ�� Random.Range(0.0f, 1.0f)�� �����༭ ��Ż�� 100���� �νĵǰ� ����
        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f)); //����ġ Ȯ�������� total ��ü�� 100���� ���� total�� 100�� �Ѿ�� Ȯ���� ����ؼ� 0.0~ 1.0�� ������ �����൵ ����� ����
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
