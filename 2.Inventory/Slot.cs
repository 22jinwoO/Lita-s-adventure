using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Header("�������� �ƴ��� �����ϱ� ���� bool �ڷ���")]
    public bool isShop; //�������� �ƴ���
    [HideInInspector]
    public Shop shopSc; //���� ��ũ��Ʈ

    [Header("�� ������ ���� �������� Ȯ���ϱ� ���� bool�ڷ���")]
    public bool isPotionSLot; //���� ��ũ��Ʈ
    Image potionImg;


    [Header("���� �������� �����ϱ� ���� bool �ڷ���")]
    public bool isCheckEquip=false;
    [Header("������ �̹���")]
    [SerializeField] Image image;
    [Header("�ش� ������ ����")]
    public int Itemcnt = 1;
    [SerializeField]
    Text itemcntTxt;

    [Header("���� ��ư")]
    public Button slotBtn;
    bool isCanPotion=false;
    float coolTime;

    public Inventory inventorySc;// �κ��丮 ��ũ��Ʈ
    [Header("�ٸ� ��ũ��Ʈ���� ����ϴ� ������ �ڷ��� ����")]
    public Item _item; //�ְ�޴°�

    public Item item    //������Ƽ

    {
        get
        {

            return _item;
        }
        set
        {

            _item = value;
            if (_item != null)  //item�� ������� ������
            {

                image.sprite = item.itemImage;  // ���� �̹����� �������̹����� �ٲٰ�
                image.color = new Color(1, 1, 1, 1);    //���� 1�� ����
           
            }
            else // ���Կ� �������� ������
            {
                image.color = new Color(1, 1, 1, 0); // ���� �̹��� ���� 0
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
        if (_item != null)  // �������� ������� ������
        {
            image.sprite = item.itemImage;  //�����̹����� ������ �̹�����
            image.color = new Color(1, 1, 1, 1);    //���� 1
            if (isCheckEquip && _item.weaponInfo.weaponDmg == 0)  //���������� ĭ�� ��������� ���� 0���� ����
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }

        slotBtn.onClick.AddListener(ClickSlot);

        if (gameObject.name=="EquipItem")   //���ӿ�����Ʈ�� �̸��� EquipItem�̸�
        {
            UiManager.instance.equipItemSc = this;  //UiManager.instance.equipItemSc�������� EquipItem ���ӿ�����Ʈ�� Slot.cs�� �־���
        }

        if (isPotionSLot)   // �ش� Slot.cs�� ���ǻ���ϴ� ���� ��ư�̶��
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
    public void ClickSlot()    //������ Ŭ���ҋ� ȣ��Ǵ� �Լ�
    {
        if (isShop)
        {
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[0]);
            UiManager.instance.ShowTooltip(item);
            UiManager.instance.inventoryCs.itemUseBtn.gameObject.SetActive(false);  //�����ۻ�� ��ư ���ӿ�����Ʈ ��Ȱ��ȭ
            UiManager.instance.inventoryCs.itemSellBtn.gameObject.SetActive(false);    // ������ �Ǹ� ��ư ���ӿ�����Ʈ ��Ȱ��ȭ
            UiManager.instance.itemBuyBtn.gameObject.SetActive(true);  //������ ���� ��ư ���ӿ�����Ʈ Ȱ��ȭ
            UiManager.instance.itemSellPriceTxt.gameObject.SetActive(false); //������ �Ǹűݾ� ������Ʈ ��Ȱ��ȭ
            UiManager.instance.toolTip.transform.position = gameObject.transform.position;
            UiManager.instance.toolTipRectTransform.anchoredPosition += new Vector2(0, -310);   //����â ��ġ����
            shopSc.clickItem = item;    //Ŭ���� �������� ����
        }
        else if (!isShop&&!isCheckEquip&&!isPotionSLot) //������ �ƴϰ� �������⵵ �ƴϰ� ���ǽ��Ե� �ƴ϶��
        {
            if (item!=null)
            {
                inventorySc.clickItem = item;
                inventorySc.useSlot = this;
                UiManager.instance.ShowTooltip(item);
            }
            

        }
        else if (!isShop && isCheckEquip) //����â�� ���������� ��ũ��Ʈ�϶�
        {
            inventorySc.clickItem = item;
            UiManager.instance.ShowTooltip(item);
        }
        else if (isPotionSLot&&isCanPotion) //Ŭ���� ������ ���ǽ����̰� ���ǽ��� ����� �����Ҷ�
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
            if (Itemcnt>0)  // ���� ������ 0���� Ŭ��
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
 