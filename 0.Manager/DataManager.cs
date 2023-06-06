using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;



[System.Serializable]   //����ȭ
public class Data
{
    [Header("�÷��̾ Ŭ������ ��������")]
    public int clearStage=0;   //�÷��̾ Ŭ������ ��������

    [Header("�÷��̾� ����")]
    public int playerLv = 1;    //�÷��̾� ����

    [Header("�÷��̾� �ִ� hp")]
    public float playerMaxHp = 500;    //�÷��̾� �ִ� hp

    [Header("�÷��̾� ���� hp")]
    public float nowPlayerHp;   //�÷��̾� hp

    [Header("�÷��̾� ũ��Ƽ�� Ȯ��")]
    public int playerCritical=50;   //�÷��̾� ũ��Ƽ�� Ȯ��

    [Header("���� �ӵ�")]
    public float attackSpeed = 0;   //���� �ӵ�

    [Header("���� ������")]
    public float playerAttackDmg = 70;    //���� ������

    [Header("��ų ������")]
    public float playerSkillDmg = 100;    //��ų ������

    [Header("��ų ��Ÿ��")]
    public int playerSkillCool = 0;    //��ų ��Ÿ�� ����

    [Header("�÷��̾� ���� ����ġ")]
    public float playerNowExp; //�÷��̾� ����ġ

    [Header("�÷��̾ ���� ������ ������� �ʿ��� ����ġ")]
    public float playerMaxExp=200; //�÷��̾� �ִ� ����ġ

    [Header("�÷��̾� ��ų ����Ʈ")]
    public int playerSkillPoint;    //�÷��̾� ��ų ����Ʈ ����

    [Header("�÷��̾� �޼�")]
    public int playerMoney;   //�÷��̾� �޼� ����

    [Header("�÷��̾� �г���")]
    public string playerNickName; //�÷��̾� �г���

    [Header("�÷��̾ �����ϰ� �ִ� ����")]
    public int equipWeaponIndex;  //�÷��̾ �����ϰ� �ִ� ����

    [Header("�÷��̾ �����ϰ� �ִ� ������ݷ�")]
    public int equipWeaponDmg=0;    //�÷��̾ �����ϰ� �ִ� ���� ���ݷ�

    [Header("�÷��̾ �����ϰ� �ִ� ���� ����")]
    public string equipWeaponDesc;  //�÷��̾ �����ϰ� �ִ� ���� ���� �ؽ�Ʈ

    [Header("�÷��̾��� �κ��丮 �����۵��� �ѹ��� ������ ����Ʈ")]
    public List<int> itemlists; // �÷��̾��� �κ��丮 �����۵��� �ѹ��� ������ ����Ʈ

    [Header("����� �ͼ� ����� �����̴� ��")]
    public float bgmSliderValue=0;

    [Header("����� �ͼ� ȿ���� �����̴� ��")]
    public float sfxSliderValue=0;


}
public class DataManager : MonoBehaviour
{
    static public DataManager instance; //�̱������� ����� ���� ����
    public Data data;   // DataŬ������ ����ϱ� ���� public ����
    public Inventory inventoryCs;   //�κ��丮 ��ũ��Ʈ ������������ ����
    [Header("���� �� ������ ���� �迭")]
    public Item[] itemKinds;    //�κ��丮 ������ ������ ���� ������ �迭

    public Item equipWeaponItem;    //���� �������� ���� Item�� ����
    public RandomBox randomBoxSc;   //RandomBox��ũ��Ʈ�� ����ϱ� ���� ������ ����
    void Awake()
    {
        if (null == instance)
        {
            //�� Ŭ���� �ν��Ͻ��� ź������ �� �������� instance�� DataManager �ν��Ͻ��� ������� �ʴٸ�, �ڽ��� �־��ش�.
            instance = this;

            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            //gameObject�����ε� �� ��ũ��Ʈ�� ������Ʈ�μ� �پ��ִ� Hierarchy���� ���ӿ�����Ʈ��� ��������, 
            //���� �򰥸� ������ ���� this�� �ٿ��ֱ⵵ �Ѵ�.
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� DataManager�� ������ ���� �ִ�.
            //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
            //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ�(���ο� ���� GameMgr)�� �������ش�.
            Destroy(this.gameObject);
        }

    }
    void Start()
    {

    }

    void Update()
    {
        data.nowPlayerHp = Mathf.Clamp(data.nowPlayerHp, 0, data.playerMaxHp);   //�÷��̾� hp
    }

    public void Save()  //save �Լ�
    {
        data.itemlists.Clear(); //DataŬ������ itemlists ����Ʈ ����ֱ�
        for (int i = 0; i < inventoryCs.items.Count; i++)   //inventoryCs�� items ����Ʈ�� ����ŭ �ݺ��� ����
        {
            data.itemlists.Add(inventoryCs.items[i].itemNum);   //DataŬ������ itemlists�� inventoryCs�� items ����Ʈ�� i��° itemNum �� �߰�
        }
        //���� ���
        string path = Application.persistentDataPath + $"/{data.playerNickName}.json";  

        //������ Ŭ������ json ���·� ��ȯ(������ ����)
        string saveData = JsonUtility.ToJson(data, true);

        //���̽� ���·� ��ȯ�� ���ڿ� ����
        File.WriteAllText(path, saveData);

        print("���� �Ϸ�");
    }

    public void Load()
    {
        randomBoxSc = GameObject.FindObjectOfType<RandomBox>(); //RandomBox ��ũ��Ʈ�� ������ �ִ� ������Ʈ ã�Ƽ� �ֱ�
        inventoryCs = GameObject.FindObjectOfType<Inventory>(); //Inventory ��ũ��Ʈ�� ������ �ִ� ������Ʈ ã�Ƽ� �ֱ�
        inventoryCs.randBoxSc = randomBoxSc;    //inventorycs.randBoxSc ������ randomBoxsc ������ �־��ֱ�

        //�ҷ����⸦ �� ���
        string path = Application.persistentDataPath + $"/{data.playerNickName}.json";
        print("�ҷ����⸦ �� ��� : " + path);
        //������ �����Ѵٸ�
        if (File.Exists(path))
        {
            //���ڿ��� ����� json ���� �о����
            string loadData = File.ReadAllText(path);

            //json�� Ŭ���� ���·� ��ȯ+ �Ҵ�
            data = JsonUtility.FromJson<Data>(loadData);

            if (data.equipWeaponDmg == 0)    //���� �ҷ��� Data�� equipWeaponItem�� weaponDmg�� 0�̶��(���Ⱑ ����ִٸ�)
            {
                
                equipWeaponItem.itemImage = null;
                equipWeaponItem.weaponInfo.weaponDmg = 0;
                equipWeaponItem.itemToolTipName = "�������";
                equipWeaponItem.itemDesc = "���⸦ �������� �ʰ� �ֽ��ϴ�.";
                UiManager.instance.clickSlotItemImg.color = new Color(1, 1, 1, 0); // ���� �̹��� ���� 0
                equipWeaponItem.itemSellPrice = 0;

            }
            else // ���Ⱑ ������� �ʴٸ�
            {
                equipWeaponItem.itemToolTipName = randomBoxSc.weaponItems[data.equipWeaponIndex].itemToolTipName;  //equipWeaponItem.ItemName�� randomBoxSc�� weaponItems �迭�� Data�� ����� equipWeaponIndex�� �ε����� ���� itemToolTipName���� ��
                equipWeaponItem.itemImage = randomBoxSc.weaponItems[data.equipWeaponIndex].itemImage;   //equipWeaponItem.itemImage�� randomBoxSc�� weaponItems �迭�� Data�� ����� equipWeaponIndex�� �ε����� ���� itemImage���� ��
                equipWeaponItem.weaponInfo.weaponDmg = data.equipWeaponDmg; //equipWeaponItem.weaponInfo.weaponDMg�� data.equipWeaponDmg���� ��
                equipWeaponItem.itemDesc = data.equipWeaponDesc;    //equipWeaponItem.itemDesc�� data.equipWeaponDesc���� ��
                equipWeaponItem.itemSellPrice = randomBoxSc.weaponItems[data.equipWeaponIndex].itemSellPrice;   //equipWeaponItem.itemSellPrice�� randomBoxSc�� weaponItems �迭�� Data�� ����� equipWeaponIndex�� �ε����� ���� itemSellPrice���� ��
            }
            
            print("�ҷ����� �Ϸ�");
            inventoryCs.items.Clear();  //inventoryCs�� items ����Ʈ �� ����ֱ�
            for (int i = 0; i < data.itemlists.Count; i++)
            {
                inventoryCs.items.Add(itemKinds[data.itemlists[i]]);

            }
        }
        else
        {
            data.playerMoney = 30000;
            equipWeaponItem.itemImage = randomBoxSc.weaponItems[0].itemImage;   //equipWeaponItem�� Image�� randomBoxSc.weaponItems[0].itemImage �� ��
            equipWeaponItem.itemToolTipName=randomBoxSc.weaponItems[0].itemToolTipName; //equipWeaponItem�� Image�� randomBoxSc.weaponItems[0].itemImage �� ��
            equipWeaponItem.itemDesc=randomBoxSc.weaponItems[0].itemDesc;   //equipWeaponItem�� itemDesc�� randomBoxSc.weaponItems[0].itemDesce �� ��
            equipWeaponItem.weaponInfo.weaponDmg = Random.Range(randomBoxSc.weaponItems[0].weaponInfo.minDmg, randomBoxSc.weaponItems[0].weaponInfo.maxDmg+1);  //equipWeaponItem�� weaponDmg�� randomBoxSc.weaponItems[0]���� minDmg�� maxDmg+1�� ���̰����� �����ϰ� �־���
            equipWeaponItem.itemDesc = equipWeaponItem.itemDesc.Substring(equipWeaponItem.itemDesc.IndexOf(randomBoxSc.weaponItems[0].weaponInfo.maxDmg.ToString()) + 2).Trim(); // itemDesc ���ݷ� ���� �� �����ֱ�
            equipWeaponItem.itemDesc = $"��� : {randomBoxSc.weaponItems[0].weaponInfo.weaponGrade}\n" + equipWeaponItem.itemDesc + $"\n���� ���ݷ�: {equipWeaponItem.weaponInfo.weaponDmg}"; //equipWeaponItem.itemDesc�� ���, ���⼳��, ���ݷ� ������ �ٽ� �ۼ�
            data.equipWeaponDmg = equipWeaponItem.weaponInfo.weaponDmg; // DataManager.Data ���� �ݿ�
            data.equipWeaponDesc = equipWeaponItem.itemDesc;    // DataManager.Data ���� �ݿ�
            data.equipWeaponIndex = 0;  // DataManager.Data ���� �ݿ�
            data.playerAttackDmg += equipWeaponItem.weaponInfo.weaponDmg;   //DataŬ������ playerAttackDmg ������ equipWeaponItem.weaponInfo.weaponDMg�� �����ֱ�
            data.playerSkillDmg += equipWeaponItem.weaponInfo.weaponDmg;    //DataŬ������ playerSkillDmg ������ equipWeaponItem.weaponInfo.weaponDMg�� �����ֱ�
        }


    }

    //���� ���� �� ȣ��
    public void OnApplicationQuit()
    {
        Save();
#if UNITY_EDITOR    //����Ƽ ��ó���� ���
        UnityEditor.EditorApplication.isPlaying = false;    //����Ƽ �����ͷ� �÷����ϰ� ������ false�� �ٲ㼭 �����ϱ�
#else
        Application.Quit();
#endif
    }
}
