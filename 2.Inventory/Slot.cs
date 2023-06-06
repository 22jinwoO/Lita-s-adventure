using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Header("상점인지 아닌지 구분하기 위한 bool 자료형")]
    public bool isShop; //상점인지 아닌지
    [HideInInspector]
    public Shop shopSc; //상점 스크립트

    [Header("이 슬롯이 포션 슬롯인지 확인하기 위한 bool자료형")]
    public bool isPotionSLot; //상점 스크립트
    Image potionImg;


    [Header("무기 장착인지 구분하기 위한 bool 자료형")]
    public bool isCheckEquip=false;
    [Header("아이템 이미지")]
    [SerializeField] Image image;
    [Header("해당 아이템 개수")]
    public int Itemcnt = 1;
    [SerializeField]
    Text itemcntTxt;

    [Header("슬롯 버튼")]
    public Button slotBtn;
    bool isCanPotion=false;
    float coolTime;

    public Inventory inventorySc;// 인벤토리 스크립트
    [Header("다른 스크립트에서 사용하는 아이템 자료형 변수")]
    public Item _item; //주고받는거

    public Item item    //프로퍼티

    {
        get
        {

            return _item;
        }
        set
        {

            _item = value;
            if (_item != null)  //item이 비어있지 않으면
            {

                image.sprite = item.itemImage;  // 슬롯 이미지를 아이템이미지로 바꾸고
                image.color = new Color(1, 1, 1, 1);    //투명도 1로 설정
           
            }
            else // 슬롯에 아이템이 없으면
            {
                image.color = new Color(1, 1, 1, 0); // 슬롯 이미지 투명도 0
            }
        }
    }
    private void Awake()
    {
        slotBtn = GetComponent<Button>();
        inventorySc = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    private void Start()
    {
        if (_item != null)  // 아이템이 비어있지 않으면
        {
            image.sprite = item.itemImage;  //슬롯이미지를 아이템 이미지로
            image.color = new Color(1, 1, 1, 1);    //투명도 1
            if (isCheckEquip && _item.weaponInfo.weaponDmg == 0)  //장착아이템 칸이 비어있으면 투명도 0으로 설정
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }

        slotBtn.onClick.AddListener(ClickSlot);

        if (gameObject.name=="EquipItem")   //게임오브젝트의 이름이 EquipItem이면
        {
            UiManager.instance.equipItemSc = this;  //UiManager.instance.equipItemSc변수값에 EquipItem 게임오브젝트의 Slot.cs를 넣어줌
        }

        if (isPotionSLot)   // 해당 Slot.cs가 포션사용하는 슬롯 버튼이라면
        {
            potionImg=GetComponent<Image>();
            if(DataManager.instance.data.itemlists.Contains(0))
            {
                for (int i = 0; i < inventorySc.items.Count; i++)
                {
                    if (inventorySc.items[i].itemNum == 0)
                    {
                        Itemcnt++;
                    }
                }
                itemcntTxt.text = Itemcnt.ToString();
            }
        }
    }
    private void Update()
    {
        if (isPotionSLot)
        {
            if (!isCanPotion)
            {
                coolTime += Time.deltaTime;
                
            }
            isCanPotion = (coolTime <= 30) ? false : true;
            slotBtn.interactable=isCanPotion;
            potionImg.fillAmount = coolTime / 30;
        }
    }
    public void ClickSlot()    //슬롯을 클릭할떄 호출되는 함수
    {
        if (isShop)
        {
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[0]);
            UiManager.instance.ShowTooltip(item);
            UiManager.instance.inventoryCs.itemUseBtn.gameObject.SetActive(false);  //아이템사용 버튼 게임오브젝트 비활성화
            UiManager.instance.inventoryCs.itemSellBtn.gameObject.SetActive(false);    // 아이템 판매 버튼 게임오브젝트 비활성화
            UiManager.instance.itemBuyBtn.gameObject.SetActive(true);  //아이템 구매 버튼 게임오브젝트 활성화
            UiManager.instance.itemSellPriceTxt.gameObject.SetActive(false); //아이템 판매금액 오브젝트 비활성화
            UiManager.instance.toolTip.transform.position = gameObject.transform.position;
            UiManager.instance.toolTipRectTransform.anchoredPosition += new Vector2(0, -310);   //툴팁창 위치조정
            shopSc.clickItem = item;    //클릭한 아이템을 전달
        }
        else if (!isShop&&!isCheckEquip&&!isPotionSLot) //상점이 아니고 장착무기도 아니고 포션슬롯도 아니라면
        {
            if (item!=null)
            {
                inventorySc.clickItem = item;
                inventorySc.useSlot = this;
                UiManager.instance.ShowTooltip(item);
            }
            

        }
        else if (!isShop && isCheckEquip) //설정창의 장착아이템 스크립트일때
        {
            inventorySc.clickItem = item;
            UiManager.instance.ShowTooltip(item);
        }
        else if (isPotionSLot&&isCanPotion) //클릭한 슬롯이 포션슬롯이고 포션슬롯 사용이 가능할때
        {
            Itemcnt = 0;
            inventorySc.useSlot = this;
            for (int i = 0; i < inventorySc.items.Count; i++)
            {
                if (inventorySc.items[i].itemNum == 0)
                {
                    Itemcnt++;
                }
            }
            if (Itemcnt>0)  // 포션 개수가 0보다 클때
            {
                inventorySc.clickItem = item;
                Itemcnt--;
                coolTime = 0;
                inventorySc.UseItem();
                //inventorySc.FreshSlot();
                
            }
            inventorySc.clickItem = null;

            itemcntTxt.text = Itemcnt.ToString();
        }
    }

  

}
 