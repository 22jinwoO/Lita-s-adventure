using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Inventory : MonoBehaviour
{
    [Header("플레이어 아이템 리스트")]
    public List<Item> items;    //아이템을 담을 리스트

    [Header("플레이어가 갖고있는 아이템 종류 리스트")]
    public List<Item> itemTypes = new List<Item>(); // 아이템의 종류만 저장할 리스트

    [SerializeField]
    Transform slotParent;   //Slot의 부모가 되는 Bag을 담을 곳

    public Slot[] slots;   //Bag의 하위에 등록된 Slot을 담을 곳
    public Slot useSlot;    //포션을 클릭한 아이템 슬롯의 SLot.cs?

    public Text playerMoneyTxt; //플레이어 메소 텍스트
    
    public RandomBox randBoxSc;    //랜덤박스 스크립트
    
    [Header("인벤토리 아이템 툴팁 관련")]
    public Item clickItem;
    public Button itemUseBtn;
    public Button itemSellBtn;

    [Header("장착 아이템")]
    public Slot equipItemSc;
    public Item equipItem;

    [Header("포션 버튼 슬롯스크립트")]
    public Slot potionItemSc;

    //------------------
    private void OnValidate()   //유니티 에디터에서 바로 작동하는 역할
                                //처음 인벤토리에 소스를 등록하면 ConSole창에 에러가 뜨지만 Bag을 넣어주면 Slots에 Slot들이 자동 등록
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
    private void Awake()
    {
        DataManager.instance.Load();
    }
    void Start()
    {
        
        
        FreshSlot();    //게임이 시작되면 items에 들어있는 아이템을 인벤토리에 넣어줌
        
        randBoxSc = DataManager.instance.randomBoxSc;
        equipItem = equipItemSc._item;
    }
    private void Update()
    {
        playerMoneyTxt.text = DataManager.instance.data.playerMoney.ToString();
    }
    #region * 게임시작할때나 아이템 구매, 아이템 사용시 인벤토리에 초기화하는 함수(FreshSlot)
    /// <summary>
    /// 게임 실행 시 DataManager에 있는 item리스트들 인벤토리 슬롯에 불러옴
    /// 첫 i 값은 0으로 선언 후 i가 dataManager의 아이템 리스트의 수보다 작고 i가 인벤토리 슬롯의 갯수의 갯수보다 적을때까지
    /// 슬롯의 i번째 아이템에 dataManager의 아이템리스트 i번째 아이템값을 추가함
    /// 인벤토리에 아이템이 없으면 slot의 i번째 아이템 값은 null
    /// </summary>
    public void FreshSlot()
    {
        // 아이템 종류 리스트 초기화
        itemTypes.Clear();

        // 슬롯에 있는 모든 아이템 개수 1로 초기화 + 글씨 안 보이게
        foreach (var slot in slots)
        {
            slot.Itemcnt = 1;
            slot.transform.GetChild(0).gameObject.SetActive(false);
        }

        // 아이템 리스트에 있는 개수만큼 반복해서 슬롯 채우기
        for (int i = 0; i < items.Count; i++)
        {
            // 아이템 종류 리스트에 없는 아이템이라면 (중복X > 슬롯 증가)
            if (!itemTypes.Contains(items[i]))
            {
                // 슬롯에 추가 (슬롯 번호는 현재 존재하는 아이템 개수)
                slots[itemTypes.Count].item = items[i];       
                // 아이템 종류 리스트에 추가
                itemTypes.Add(items[i]);
            }

            // 아이템 종류 리스트에 이미 있는 아이템이라면 (중복O > 숫자증가)
            else
            {
                // 아이템 종류 리스트 중 몇 번인지
                int itemNumber = itemTypes.IndexOf(items[i]);

                // 해당 번호의 슬롯이 갖고 있는 카운트 이미지 활성화
                slots[itemNumber].transform.GetChild(0).gameObject.SetActive(true);

                // 해당 번호의 슬롯이 갖고 있는 카운트 증가 후 출력
                slots[itemNumber].Itemcnt++;
                Text ItemCntTxt = slots[itemNumber].GetComponentInChildren<Text>();
                ItemCntTxt.text = slots[itemNumber].Itemcnt.ToString();
            }
        }

        // 슬롯을 채운 아이템 종류 리스트의 개수부터 시작 ~ 슬롯 마지막까지 null로 채우기
        for (int i = itemTypes.Count; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    }
    #endregion

    #region * 아이템 추가함수(AddItem)
    /// <summary>
    /// Shop 스크립트에서 아이템 구매시 호출
    /// AddItem 함수는 Item 자료형을 매개변수로 받아와서 플레이어가 소유하고 있는 아이템 갯수가 인벤토리 슬롯칸보다 적을때 데이터매니저의 아이템 리스트에 Item자료형 매개변수 값을 추가해주고
    /// 플레이어가 소유하고 있는 아이템의 갯수가 슬롯의 갯수보다 같거나 클때 슬롯이 가득 차있습니다 팝업창과 함께 아이템을 추가할 수 없다
    /// </summary>
    /// <param name="_item"></param>
    public void AddItem(Item _item)
    {
        // 슬롯의 개수보다 아이템 종류가 적을 때만
        if (itemTypes.Count < slots.Length)
        {
            items.Add(_item);
            if (_item.itemNum == 0)
            {
                potionItemSc.Itemcnt++;
                potionItemSc.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = potionItemSc.Itemcnt.ToString();
            }
            FreshSlot();
        }

    }
    #endregion

    public void UseItem()   //인벤토리 아이템 사용할때 호출되는 함수
    {
        if (UiManager.instance.toolTip!=null)  //아이템을 사용했을때 사용한 아이템 툴팁창도 비활성화되도록 하는 부분
        {
            UiManager.instance.toolTip.SetActive(false);
        }
        switch (clickItem.itemType) //클릭한 아이템 타입에 따라 분류
        {
            case Item.ItemType.랜덤박스:
                AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[2]);
                UseRandomBox(clickItem);
                break;
            case Item.ItemType.포션:
                AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[1]);
                DataManager.instance.data.nowPlayerHp += Mathf.Round(DataManager.instance.data.playerMaxHp*0.3f);


                if (useSlot!=null&&!useSlot.isPotionSLot&& potionItemSc.Itemcnt>0)  // 물약사용 슬롯이 아니고 인벤토리 창에서 포션을 사용하면
                {
                    
                    potionItemSc.Itemcnt--; //물약 슬롯 버튼의 물약개수 --;
                    potionItemSc.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = potionItemSc.Itemcnt.ToString();
                }

                items.Remove(clickItem);
                useSlot = null;
                FreshSlot();
                break;

            case Item.ItemType.무기:
                DataManager.instance.data.playerAttackDmg -= equipItem.weaponInfo.weaponDmg;    // 플레이어 기본 공격력 값에 장착했던 무기 공격력 빼주기
                DataManager.instance.data.playerSkillDmg -= equipItem.weaponInfo.weaponDmg;     // 플레이어 스킬 공격력 값에 장착했던 무기 공격력 빼주기
                AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[3]);
                equipItemSc._item.itemImage = clickItem.itemImage;  // 장착 무기 아이템 이미지 = 새로 장착한 무기 이미지
                equipItem.itemImage = clickItem.itemImage;          // 장착 무기 아이템 이미지 = 새로 장착한 무기 이미지
                equipItem.itemToolTipName = clickItem.itemToolTipName;  // 장착 무기 슬롯 툴팁창 아이템 이름 = 새로 장착한 무기 이름
                equipItem.weaponInfo.weaponIndex = clickItem.weaponInfo.weaponIndex;    // 장착 무기 슬롯의 무기 인덱스 = 새로 장착한 무기 인덱스
                equipItem.weaponInfo.weaponGrade = clickItem.weaponInfo.weaponGrade;    // 장착 무기 슬롯의 무기 등급 = 새로 장착한 무기 등급
                equipItem.weaponInfo.weaponDmg = Random.Range(clickItem.weaponInfo.minDmg, clickItem.weaponInfo.maxDmg + 1);    // 장착 무기의 공격력 값 = 새로 장착한 무기의 공격범위 중 랜덤한 값
                equipItem.itemSellPrice = clickItem.itemSellPrice;
                equipItem.itemDesc = clickItem.itemDesc; //+$"\n무기 공격력: {equipItem.weaponInfo.weaponDmg}

                if (equipItem.weaponInfo.weaponGrade==WeaponGrade.레전드리) // 무기 등급이 레전이라면 공격력 범위의 마지막 3번째 글짜까지 삭제
                    equipItem.itemDesc = equipItem.itemDesc.Substring(equipItem.itemDesc.IndexOf(clickItem.weaponInfo.maxDmg.ToString()) + 3).Trim();

                else // 무기 등급이 레전이 아니라면 공격력 범위의 마지막 2번째 글짜까지 삭제
                    equipItem.itemDesc = equipItem.itemDesc.Substring(equipItem.itemDesc.IndexOf(clickItem.weaponInfo.maxDmg.ToString()) + 2).Trim();

                // 장착 무기 아이템 관련 데이터 반영
                equipItem.itemDesc = $"등급 : {equipItem.weaponInfo.weaponGrade}\n" + equipItem.itemDesc + $"\n무기 공격력: {equipItem.weaponInfo.weaponDmg}";

                equipItem.itemToolTipName = clickItem.itemToolTipName;
                // 데이터 매니저에 장착무기의 값들 저장
                DataManager.instance.data.equipWeaponDmg = equipItem.weaponInfo.weaponDmg;
                DataManager.instance.data.equipWeaponIndex = equipItem.weaponInfo.weaponIndex;
                DataManager.instance.data.equipWeaponDesc = equipItem.itemDesc;

                Image itemImg =equipItemSc.GetComponent<Image>();
                itemImg.sprite = equipItem.itemImage;
                itemImg.color = new Color(1, 1, 1, 1);
                DataManager.instance.data.playerAttackDmg += equipItem.weaponInfo.weaponDmg;
                DataManager.instance.data.playerSkillDmg += equipItem.weaponInfo.weaponDmg;
                items.Remove(clickItem);
                FreshSlot();
                break;
        }
    }
    public void SellItem()  //아이템을 판매할때 호출되는 함수
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[3]);
        if (UiManager.instance.shopOpen)
        {
            // 이 슬롯에 있던 아이템 지우기
            DataManager.instance.data.playerMoney += clickItem.itemSellPrice; // 아이템 판매가격 플레이어 골드 보유량에 더해주기
            if (clickItem.itemNum==0) // 만약 클릭한 아이템이 포션이면
            {
                //포션 퀵슬롯 아이템 개수 -1, 포션 퀵슬롯 아이템 개수 텍스트에 반영
                potionItemSc.Itemcnt--;
                potionItemSc.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = potionItemSc.Itemcnt.ToString();
            }
            items.Remove(clickItem);
            FreshSlot();
            UiManager.instance.toolTip.SetActive(false);


        }
        else if (equipItemSc.isCheckEquip)    //설정창을 열어서 장착무기를 판매할때
        {
            itemUseBtn.gameObject.SetActive(true);
            //** 장착 무기 슬롯에 관한 데이터
            DataManager.instance.data.playerAttackDmg -= equipItem.weaponInfo.weaponDmg;
            DataManager.instance.data.playerSkillDmg -= equipItem.weaponInfo.weaponDmg;
            // 이 슬롯에 있던 아이템 지우기
            DataManager.instance.data.equipWeaponDmg = 0;
            DataManager.instance.data.playerMoney += clickItem.itemSellPrice;
            Image itemImg = equipItemSc.GetComponent<Image>();
            itemImg.sprite = null;
            itemImg.color = new Color(1, 1, 1, 0);
            equipItem.weaponInfo.weaponGrade = WeaponGrade.노말;
            equipItem.itemImage = null;
            equipItem.itemToolTipName = "비어있음";
            equipItem.itemDesc = "무기를 장착하지 않고 있습니다.";
            equipItem.weaponInfo.weaponDmg = 0;
            equipItem.itemSellPrice=0;
            //* 장착 무기 슬롯에 관한 데이터
            UiManager.instance.toolTip.SetActive(false);
        }
    }

    void UseRandomBox(Item _item)
    {
        

        Item item = randBoxSc.weaponItems[randBoxSc.RandomBoxChoice()];
        // 가중치 랜덤에 의해 선정된 이름에 따라 리소스 폴더에 있는 무기 선택
        print(item.itemName);

        // 이 슬롯에 있던 아이템 지우기
        items.Remove(_item);

        // 아이템 인벤토리에 추가
        AddItem(item);

    }
}
