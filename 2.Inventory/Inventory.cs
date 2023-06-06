using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Inventory : MonoBehaviour
{
    [Header("�÷��̾� ������ ����Ʈ")]
    public List<Item> items;    //�������� ���� ����Ʈ

    [Header("�÷��̾ �����ִ� ������ ���� ����Ʈ")]
    public List<Item> itemTypes = new List<Item>(); // �������� ������ ������ ����Ʈ

    [SerializeField]
    Transform slotParent;   //Slot�� �θ� �Ǵ� Bag�� ���� ��

    public Slot[] slots;   //Bag�� ������ ��ϵ� Slot�� ���� ��
    public Slot useSlot;    //������ Ŭ���� ������ ������ SLot.cs?

    public Text playerMoneyTxt; //�÷��̾� �޼� �ؽ�Ʈ
    
    public RandomBox randBoxSc;    //�����ڽ� ��ũ��Ʈ
    
    [Header("�κ��丮 ������ ���� ����")]
    public Item clickItem;
    public Button itemUseBtn;
    public Button itemSellBtn;

    [Header("���� ������")]
    public Slot equipItemSc;
    public Item equipItem;

    [Header("���� ��ư ���Խ�ũ��Ʈ")]
    public Slot potionItemSc;

    //------------------
    private void OnValidate()   //����Ƽ �����Ϳ��� �ٷ� �۵��ϴ� ����
                                //ó�� �κ��丮�� �ҽ��� ����ϸ� ConSoleâ�� ������ ������ Bag�� �־��ָ� Slots�� Slot���� �ڵ� ���
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
    private void Awake()
    {
        DataManager.instance.Load();
    }
    void Start()
    {
        
        
        FreshSlot();    //������ ���۵Ǹ� items�� ����ִ� �������� �κ��丮�� �־���
        
        randBoxSc = DataManager.instance.randomBoxSc;
        equipItem = equipItemSc._item;
    }
    private void Update()
    {
        playerMoneyTxt.text = DataManager.instance.data.playerMoney.ToString();
    }
    #region * ���ӽ����Ҷ��� ������ ����, ������ ���� �κ��丮�� �ʱ�ȭ�ϴ� �Լ�(FreshSlot)
    /// <summary>
    /// ���� ���� �� DataManager�� �ִ� item����Ʈ�� �κ��丮 ���Կ� �ҷ���
    /// ù i ���� 0���� ���� �� i�� dataManager�� ������ ����Ʈ�� ������ �۰� i�� �κ��丮 ������ ������ �������� ����������
    /// ������ i��° �����ۿ� dataManager�� �����۸���Ʈ i��° �����۰��� �߰���
    /// �κ��丮�� �������� ������ slot�� i��° ������ ���� null
    /// </summary>
    public void FreshSlot()
    {
        // ������ ���� ����Ʈ �ʱ�ȭ
        itemTypes.Clear();

        // ���Կ� �ִ� ��� ������ ���� 1�� �ʱ�ȭ + �۾� �� ���̰�
        foreach (var slot in slots)
        {
            slot.Itemcnt = 1;
            slot.transform.GetChild(0).gameObject.SetActive(false);
        }

        // ������ ����Ʈ�� �ִ� ������ŭ �ݺ��ؼ� ���� ä���
        for (int i = 0; i < items.Count; i++)
        {
            // ������ ���� ����Ʈ�� ���� �������̶�� (�ߺ�X > ���� ����)
            if (!itemTypes.Contains(items[i]))
            {
                // ���Կ� �߰� (���� ��ȣ�� ���� �����ϴ� ������ ����)
                slots[itemTypes.Count].item = items[i];       
                // ������ ���� ����Ʈ�� �߰�
                itemTypes.Add(items[i]);
            }

            // ������ ���� ����Ʈ�� �̹� �ִ� �������̶�� (�ߺ�O > ��������)
            else
            {
                // ������ ���� ����Ʈ �� �� ������
                int itemNumber = itemTypes.IndexOf(items[i]);

                // �ش� ��ȣ�� ������ ���� �ִ� ī��Ʈ �̹��� Ȱ��ȭ
                slots[itemNumber].transform.GetChild(0).gameObject.SetActive(true);

                // �ش� ��ȣ�� ������ ���� �ִ� ī��Ʈ ���� �� ���
                slots[itemNumber].Itemcnt++;
                Text ItemCntTxt = slots[itemNumber].GetComponentInChildren<Text>();
                ItemCntTxt.text = slots[itemNumber].Itemcnt.ToString();
            }
        }

        // ������ ä�� ������ ���� ����Ʈ�� �������� ���� ~ ���� ���������� null�� ä���
        for (int i = itemTypes.Count; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    }
    #endregion

    #region * ������ �߰��Լ�(AddItem)
    /// <summary>
    /// Shop ��ũ��Ʈ���� ������ ���Ž� ȣ��
    /// AddItem �Լ��� Item �ڷ����� �Ű������� �޾ƿͼ� �÷��̾ �����ϰ� �ִ� ������ ������ �κ��丮 ����ĭ���� ������ �����͸Ŵ����� ������ ����Ʈ�� Item�ڷ��� �Ű����� ���� �߰����ְ�
    /// �÷��̾ �����ϰ� �ִ� �������� ������ ������ �������� ���ų� Ŭ�� ������ ���� ���ֽ��ϴ� �˾�â�� �Բ� �������� �߰��� �� ����
    /// </summary>
    /// <param name="_item"></param>
    public void AddItem(Item _item)
    {
        // ������ �������� ������ ������ ���� ����
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

    public void UseItem()   //�κ��丮 ������ ����Ҷ� ȣ��Ǵ� �Լ�
    {
        if (UiManager.instance.toolTip!=null)  //�������� ��������� ����� ������ ����â�� ��Ȱ��ȭ�ǵ��� �ϴ� �κ�
        {
            UiManager.instance.toolTip.SetActive(false);
        }
        switch (clickItem.itemType) //Ŭ���� ������ Ÿ�Կ� ���� �з�
        {
            case Item.ItemType.�����ڽ�:
                AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[2]);
                UseRandomBox(clickItem);
                break;
            case Item.ItemType.����:
                AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[1]);
                DataManager.instance.data.nowPlayerHp += Mathf.Round(DataManager.instance.data.playerMaxHp*0.3f);


                if (useSlot!=null&&!useSlot.isPotionSLot&& potionItemSc.Itemcnt>0)  // ������ ������ �ƴϰ� �κ��丮 â���� ������ ����ϸ�
                {
                    
                    potionItemSc.Itemcnt--; //���� ���� ��ư�� ���ళ�� --;
                    potionItemSc.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = potionItemSc.Itemcnt.ToString();
                }

                items.Remove(clickItem);
                useSlot = null;
                FreshSlot();
                break;

            case Item.ItemType.����:
                DataManager.instance.data.playerAttackDmg -= equipItem.weaponInfo.weaponDmg;    // �÷��̾� �⺻ ���ݷ� ���� �����ߴ� ���� ���ݷ� ���ֱ�
                DataManager.instance.data.playerSkillDmg -= equipItem.weaponInfo.weaponDmg;     // �÷��̾� ��ų ���ݷ� ���� �����ߴ� ���� ���ݷ� ���ֱ�
                AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[3]);
                equipItemSc._item.itemImage = clickItem.itemImage;  // ���� ���� ������ �̹��� = ���� ������ ���� �̹���
                equipItem.itemImage = clickItem.itemImage;          // ���� ���� ������ �̹��� = ���� ������ ���� �̹���
                equipItem.itemToolTipName = clickItem.itemToolTipName;  // ���� ���� ���� ����â ������ �̸� = ���� ������ ���� �̸�
                equipItem.weaponInfo.weaponIndex = clickItem.weaponInfo.weaponIndex;    // ���� ���� ������ ���� �ε��� = ���� ������ ���� �ε���
                equipItem.weaponInfo.weaponGrade = clickItem.weaponInfo.weaponGrade;    // ���� ���� ������ ���� ��� = ���� ������ ���� ���
                equipItem.weaponInfo.weaponDmg = Random.Range(clickItem.weaponInfo.minDmg, clickItem.weaponInfo.maxDmg + 1);    // ���� ������ ���ݷ� �� = ���� ������ ������ ���ݹ��� �� ������ ��
                equipItem.itemSellPrice = clickItem.itemSellPrice;
                equipItem.itemDesc = clickItem.itemDesc; //+$"\n���� ���ݷ�: {equipItem.weaponInfo.weaponDmg}

                if (equipItem.weaponInfo.weaponGrade==WeaponGrade.�����帮) // ���� ����� �����̶�� ���ݷ� ������ ������ 3��° ��¥���� ����
                    equipItem.itemDesc = equipItem.itemDesc.Substring(equipItem.itemDesc.IndexOf(clickItem.weaponInfo.maxDmg.ToString()) + 3).Trim();

                else // ���� ����� ������ �ƴ϶�� ���ݷ� ������ ������ 2��° ��¥���� ����
                    equipItem.itemDesc = equipItem.itemDesc.Substring(equipItem.itemDesc.IndexOf(clickItem.weaponInfo.maxDmg.ToString()) + 2).Trim();

                // ���� ���� ������ ���� ������ �ݿ�
                equipItem.itemDesc = $"��� : {equipItem.weaponInfo.weaponGrade}\n" + equipItem.itemDesc + $"\n���� ���ݷ�: {equipItem.weaponInfo.weaponDmg}";

                equipItem.itemToolTipName = clickItem.itemToolTipName;
                // ������ �Ŵ����� ���������� ���� ����
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
    public void SellItem()  //�������� �Ǹ��Ҷ� ȣ��Ǵ� �Լ�
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[3]);
        if (UiManager.instance.shopOpen)
        {
            // �� ���Կ� �ִ� ������ �����
            DataManager.instance.data.playerMoney += clickItem.itemSellPrice; // ������ �ǸŰ��� �÷��̾� ��� �������� �����ֱ�
            if (clickItem.itemNum==0) // ���� Ŭ���� �������� �����̸�
            {
                //���� ������ ������ ���� -1, ���� ������ ������ ���� �ؽ�Ʈ�� �ݿ�
                potionItemSc.Itemcnt--;
                potionItemSc.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = potionItemSc.Itemcnt.ToString();
            }
            items.Remove(clickItem);
            FreshSlot();
            UiManager.instance.toolTip.SetActive(false);


        }
        else if (equipItemSc.isCheckEquip)    //����â�� ��� �������⸦ �Ǹ��Ҷ�
        {
            itemUseBtn.gameObject.SetActive(true);
            //** ���� ���� ���Կ� ���� ������
            DataManager.instance.data.playerAttackDmg -= equipItem.weaponInfo.weaponDmg;
            DataManager.instance.data.playerSkillDmg -= equipItem.weaponInfo.weaponDmg;
            // �� ���Կ� �ִ� ������ �����
            DataManager.instance.data.equipWeaponDmg = 0;
            DataManager.instance.data.playerMoney += clickItem.itemSellPrice;
            Image itemImg = equipItemSc.GetComponent<Image>();
            itemImg.sprite = null;
            itemImg.color = new Color(1, 1, 1, 0);
            equipItem.weaponInfo.weaponGrade = WeaponGrade.�븻;
            equipItem.itemImage = null;
            equipItem.itemToolTipName = "�������";
            equipItem.itemDesc = "���⸦ �������� �ʰ� �ֽ��ϴ�.";
            equipItem.weaponInfo.weaponDmg = 0;
            equipItem.itemSellPrice=0;
            //* ���� ���� ���Կ� ���� ������
            UiManager.instance.toolTip.SetActive(false);
        }
    }

    void UseRandomBox(Item _item)
    {
        

        Item item = randBoxSc.weaponItems[randBoxSc.RandomBoxChoice()];
        // ����ġ ������ ���� ������ �̸��� ���� ���ҽ� ������ �ִ� ���� ����
        print(item.itemName);

        // �� ���Կ� �ִ� ������ �����
        items.Remove(_item);

        // ������ �κ��丮�� �߰�
        AddItem(item);

    }
}
