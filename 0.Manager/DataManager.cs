using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;



[System.Serializable]   //직렬화
public class Data
{
    [Header("플레이어가 클리어한 스테이지")]
    public int clearStage=0;   //플레이어가 클리어한 스테이지

    [Header("플레이어 레벨")]
    public int playerLv = 1;    //플레이어 레벨

    [Header("플레이어 최대 hp")]
    public float playerMaxHp = 500;    //플레이어 최대 hp

    [Header("플레이어 현재 hp")]
    public float nowPlayerHp;   //플레이어 hp

    [Header("플레이어 크리티컬 확률")]
    public int playerCritical=50;   //플레이어 크리티컬 확률

    [Header("공격 속도")]
    public float attackSpeed = 0;   //공격 속도

    [Header("공격 데미지")]
    public float playerAttackDmg = 70;    //공격 데미지

    [Header("스킬 데미지")]
    public float playerSkillDmg = 100;    //스킬 데미지

    [Header("스킬 쿨타임")]
    public int playerSkillCool = 0;    //스킬 쿨타임 변수

    [Header("플레이어 현재 경험치")]
    public float playerNowExp; //플레이어 경험치

    [Header("플레이어가 다음 레벨로 가기까지 필요한 경험치")]
    public float playerMaxExp=200; //플레이어 최대 경험치

    [Header("플레이어 스킬 포인트")]
    public int playerSkillPoint;    //플레이어 스킬 포인트 변수

    [Header("플레이어 메소")]
    public int playerMoney;   //플레이어 메소 변수

    [Header("플레이어 닉네임")]
    public string playerNickName; //플레이어 닉네임

    [Header("플레이어가 장착하고 있는 무기")]
    public int equipWeaponIndex;  //플레이어가 장착하고 있는 무기

    [Header("플레이어가 장착하고 있는 무기공격력")]
    public int equipWeaponDmg=0;    //플레이어가 장착하고 있는 무기 공격력

    [Header("플레이어가 장착하고 있는 무기 설명")]
    public string equipWeaponDesc;  //플레이어가 장착하고 있는 무기 설명 텍스트

    [Header("플레이어의 인벤토리 아이템들의 넘버를 저장할 리스트")]
    public List<int> itemlists; // 플레이어의 인벤토리 아이템들의 넘버를 저장할 리스트

    [Header("오디오 믹서 배경음 슬라이더 값")]
    public float bgmSliderValue=0;

    [Header("오디오 믹서 효과음 슬라이더 값")]
    public float sfxSliderValue=0;


}
public class DataManager : MonoBehaviour
{
    static public DataManager instance; //싱글톤으로 만들기 위해 선언
    public Data data;   // Data클래스를 사용하기 위해 public 선언
    public Inventory inventoryCs;   //인벤토리 스크립트 가져오기위한 변수
    [Header("게임 내 아이템 종류 배열")]
    public Item[] itemKinds;    //인벤토리 아이템 종류를 위한 아이템 배열

    public Item equipWeaponItem;    //장착 아이템을 위한 Item형 변수
    public RandomBox randomBoxSc;   //RandomBox스크립트를 사용하기 위해 선언한 변수
    void Awake()
    {
        if (null == instance)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 DataManager 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 DataManager가 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            Destroy(this.gameObject);
        }

    }
    void Start()
    {

    }

    void Update()
    {
        data.nowPlayerHp = Mathf.Clamp(data.nowPlayerHp, 0, data.playerMaxHp);   //플레이어 hp
    }

    public void Save()  //save 함수
    {
        data.itemlists.Clear(); //Data클래스의 itemlists 리스트 비워주기
        for (int i = 0; i < inventoryCs.items.Count; i++)   //inventoryCs의 items 리스트의 수만큼 반복문 실행
        {
            data.itemlists.Add(inventoryCs.items[i].itemNum);   //Data클래스의 itemlists에 inventoryCs의 items 리스트의 i번째 itemNum 값 추가
        }
        //저장 경로
        string path = Application.persistentDataPath + $"/{data.playerNickName}.json";  

        //저장할 클래스를 json 형태로 전환(가독성 좋게)
        string saveData = JsonUtility.ToJson(data, true);

        //제이슨 형태로 전환된 문자열 저장
        File.WriteAllText(path, saveData);

        print("저장 완료");
    }

    public void Load()
    {
        randomBoxSc = GameObject.FindObjectOfType<RandomBox>(); //RandomBox 스크립트를 가지고 있는 오브젝트 찾아서 넣기
        inventoryCs = GameObject.FindObjectOfType<Inventory>(); //Inventory 스크립트를 가지고 있는 오브젝트 찾아서 넣기
        inventoryCs.randBoxSc = randomBoxSc;    //inventorycs.randBoxSc 변수에 randomBoxsc 변수값 넣어주기

        //불러오기를 할 경로
        string path = Application.persistentDataPath + $"/{data.playerNickName}.json";
        print("불러오기를 할 경로 : " + path);
        //파일이 존재한다면
        if (File.Exists(path))
        {
            //문자열로 저장된 json 파일 읽어오기
            string loadData = File.ReadAllText(path);

            //json을 클래스 형태로 전환+ 할당
            data = JsonUtility.FromJson<Data>(loadData);

            if (data.equipWeaponDmg == 0)    //만약 불러온 Data의 equipWeaponItem의 weaponDmg가 0이라면(무기가 비어있다면)
            {
                
                equipWeaponItem.itemImage = null;
                equipWeaponItem.weaponInfo.weaponDmg = 0;
                equipWeaponItem.itemToolTipName = "비어있음";
                equipWeaponItem.itemDesc = "무기를 장착하지 않고 있습니다.";
                UiManager.instance.clickSlotItemImg.color = new Color(1, 1, 1, 0); // 슬롯 이미지 투명도 0
                equipWeaponItem.itemSellPrice = 0;

            }
            else // 무기가 비어있지 않다면
            {
                equipWeaponItem.itemToolTipName = randomBoxSc.weaponItems[data.equipWeaponIndex].itemToolTipName;  //equipWeaponItem.ItemName을 randomBoxSc의 weaponItems 배열에 Data에 저장된 equipWeaponIndex를 인덱스로 가진 itemToolTipName으로 함
                equipWeaponItem.itemImage = randomBoxSc.weaponItems[data.equipWeaponIndex].itemImage;   //equipWeaponItem.itemImage를 randomBoxSc의 weaponItems 배열에 Data에 저장된 equipWeaponIndex를 인덱스로 가진 itemImage으로 함
                equipWeaponItem.weaponInfo.weaponDmg = data.equipWeaponDmg; //equipWeaponItem.weaponInfo.weaponDMg를 data.equipWeaponDmg으로 함
                equipWeaponItem.itemDesc = data.equipWeaponDesc;    //equipWeaponItem.itemDesc를 data.equipWeaponDesc으로 함
                equipWeaponItem.itemSellPrice = randomBoxSc.weaponItems[data.equipWeaponIndex].itemSellPrice;   //equipWeaponItem.itemSellPrice를 randomBoxSc의 weaponItems 배열에 Data에 저장된 equipWeaponIndex를 인덱스로 가진 itemSellPrice으로 함
            }
            
            print("불러오기 완료");
            inventoryCs.items.Clear();  //inventoryCs의 items 리스트 값 비워주기
            for (int i = 0; i < data.itemlists.Count; i++)
            {
                inventoryCs.items.Add(itemKinds[data.itemlists[i]]);

            }
        }
        else
        {
            data.playerMoney = 30000;
            equipWeaponItem.itemImage = randomBoxSc.weaponItems[0].itemImage;   //equipWeaponItem의 Image를 randomBoxSc.weaponItems[0].itemImage 로 함
            equipWeaponItem.itemToolTipName=randomBoxSc.weaponItems[0].itemToolTipName; //equipWeaponItem의 Image를 randomBoxSc.weaponItems[0].itemImage 로 함
            equipWeaponItem.itemDesc=randomBoxSc.weaponItems[0].itemDesc;   //equipWeaponItem의 itemDesc를 randomBoxSc.weaponItems[0].itemDesce 로 함
            equipWeaponItem.weaponInfo.weaponDmg = Random.Range(randomBoxSc.weaponItems[0].weaponInfo.minDmg, randomBoxSc.weaponItems[0].weaponInfo.maxDmg+1);  //equipWeaponItem의 weaponDmg를 randomBoxSc.weaponItems[0]값의 minDmg와 maxDmg+1의 사이값들중 랜덤하게 넣어줌
            equipWeaponItem.itemDesc = equipWeaponItem.itemDesc.Substring(equipWeaponItem.itemDesc.IndexOf(randomBoxSc.weaponItems[0].weaponInfo.maxDmg.ToString()) + 2).Trim(); // itemDesc 공격력 빼고 다 지워주기
            equipWeaponItem.itemDesc = $"등급 : {randomBoxSc.weaponItems[0].weaponInfo.weaponGrade}\n" + equipWeaponItem.itemDesc + $"\n무기 공격력: {equipWeaponItem.weaponInfo.weaponDmg}"; //equipWeaponItem.itemDesc를 등급, 무기설명, 공격력 순으로 다시 작성
            data.equipWeaponDmg = equipWeaponItem.weaponInfo.weaponDmg; // DataManager.Data 값에 반영
            data.equipWeaponDesc = equipWeaponItem.itemDesc;    // DataManager.Data 값에 반영
            data.equipWeaponIndex = 0;  // DataManager.Data 값에 반영
            data.playerAttackDmg += equipWeaponItem.weaponInfo.weaponDmg;   //Data클래스의 playerAttackDmg 변수에 equipWeaponItem.weaponInfo.weaponDMg값 더해주기
            data.playerSkillDmg += equipWeaponItem.weaponInfo.weaponDmg;    //Data클래스의 playerSkillDmg 변수에 equipWeaponItem.weaponInfo.weaponDMg값 더해주기
        }


    }

    //게임 종료 시 호출
    public void OnApplicationQuit()
    {
        Save();
#if UNITY_EDITOR    //유니티 전처리기 사용
        UnityEditor.EditorApplication.isPlaying = false;    //유니티 에디터로 플레이하고 있으면 false로 바꿔서 종료하기
#else
        Application.Quit();
#endif
    }
}
