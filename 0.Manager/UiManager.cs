using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject playerOb;

    //�÷��̾� ��ũ��Ʈ
    NewPlayer NewPlayerSc;
    CharacterController cc;
    [Header("��Ż �̿� ����")]
    public string portalName;
    public bool onPortal = false;

    [Header("��Ż ��� ����(��������, ���� Ui ����, ����â ����)")]
    public GameObject stageChoice;
    public GameObject Shop;
    public GameObject statsOb;

    [HideInInspector]//�÷��̾� ��ư
    public Button attackBtn;
    
    [Header("�÷��̾� �κ��丮 ����")]
    public GameObject Bag;
    public GameObject Coin;    // �÷��̾� �޼� ��� Ui
    public GameObject CloseOb; // �κ��丮 â �ݴ� ��ư ������Ʈ
    public Button InvenBtn;    //�κ��丮 ��ư
    public Slot[] inventorySlots;  // �κ��丮 ���� �迭��
    public Inventory inventoryCs;  //�κ��丮 ��ũ��Ʈ

    [Header("������ ���� ����")]
    public Button itemUseBtn;   //������ ���� ����ư
    public Button itemSellBtn;  //������ ���� �Ǹ� ��ư
    public Button itemBuyBtn;  //������ ���� ���� ��ư
    public Button toolTipCloseBtn;  //������ ���� �ݴ� ��ư
    public Text itemSellPriceTxt;   //������ ���� �Ǹ� ���� �����ִ� �ؽ�Ʈ
    Text itemNameTxt;
    Text itemInfoTxt;
    GameObject toolTipBaseIneer;
    public GameObject toolTip;  //������ ���� ������Ʈ

    public GameObject buyPopUp;  //������ ���� �˾� ������Ʈ

    public Transform shopSlotTr;    //���� ������ ���� ��ġ
    [HideInInspector]
    public bool shopOpen=false; // ������ ����â�� �������� ������ ���� ���������� �ǸŹ�ư�� �����ϰԲ� ���������ִ� bool�ڷ���

    [Header("�÷��̾� �г��� ����")]

    public Text playerNicknameTxt;
    [Header("�÷��̾� ���� ����")]
    public Item equipItem;
    public Slot equipItemSc;

    [Header("�÷��̾� ���� ��ư �̹���")]
    public Image AttackBtnImg;
    public Image AttackBackBtnImg;

    [Header("�÷��̾� ��Ȳ�� ���� ��ư �̹���")]
    public Sprite goStageBtnImg;
    public Sprite goShopBtnImg;
    public Sprite atkBtnImg;

    GameObject SettingOb;

    [Header("�÷��̾� ���� ��ư")]
    public Button playerInfoBtn;

    [Header("������ Ŭ�� ������ �̹���")]
    public Image clickSlotItemImg;
    [Header("����â RectTransform")]
    public RectTransform toolTipRectTransform;

    [Header("����ε� �̹��� ���ӿ�����Ʈ")]
    public GameObject blindImageOb;
    static public UiManager instance;

    [Header("�������� ��ư��")]
    public GameObject[] stageBtns= new GameObject[4];

    // �������� ���� ���� ���������� �˷��ִ� �� �̹���
    Image mapPintr;

    public TextMesh storeNpcTxtMesh;

    void Awake()
    {
        if (null == instance)
        {
            //�� Ŭ���� �ν��Ͻ��� ź������ �� �������� instance�� ���ӸŴ��� �ν��Ͻ��� ������� �ʴٸ�, �ڽ��� �־��ش�.
            instance = this;

            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            //gameObject�����ε� �� ��ũ��Ʈ�� ������Ʈ�μ� �پ��ִ� Hierarchy���� ���ӿ�����Ʈ��� ��������, 
            //���� �򰥸� ������ ���� this�� �ٿ��ֱ⵵ �Ѵ�.
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance!=this)
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
            //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
            //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ�(���ο� ���� GameMgr)�� �������ش�.
            Destroy(this.gameObject);
        }
    }
    void Start()
    {

        playerNicknameTxt.text = DataManager.instance.data.playerNickName;
        NewPlayerSc = GameObject.FindObjectOfType<CharacterController>().GetComponent<NewPlayer>(); //�÷��̾� ������Ʈ ��������
        cc = NewPlayerSc.GetComponent<CharacterController>();
        attackBtn.onClick.AddListener(ClickAttack);
        InvenBtn = GameObject.FindGameObjectWithTag("InvenBtn").GetComponent<Button>();
        InvenBtn.onClick.AddListener(InventoryOpen);

        SceneManager.sceneLoaded += LoadedsceneEvent;   //�� �̺�Ʈ �Լ� ���� ���ҷ��ö����� �Լ��� ����ؼ� ����ϴ½�

        GetSetting();
        
        playerInfoBtn.onClick.AddListener(ClickPlayerInfo); //�÷��̾� ����UI�� �÷��̾� ����â ���� �Լ� ����


    }
    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        if (scene.name=="Village")  //�� �̸��� village�϶��� GetSetting �Լ� ����
        {
            GetSetting();
        }
        if (scene.name!="LoadingScene") //�� �̸��� LoadingScene�� �ƴϸ� CharacterController Ȱ��ȭ
        {
            cc.enabled = true;
        }
    }
    void Update()
    {
    }
    void ClickAttack()  //���� ��ư Ŭ�� �� ��Ż������� �������� Ȯ���ϴ� �Լ�
    {
        if (onPortal) // onPortal�� true�϶�
        {
            PortalUiOpen(portalName);   //portalName�� �Ű������� ����ϴ� portalUiOpen �Լ� ����
        }
        else
        {
            NewPlayerSc.Attack(); //�����Լ� ����
        }
    }
    public void PortalUiOpen(string portalName) //��Ż�̸��� �ش�Ǵ� ����� ��밡���ϰ� �ϴ� �Լ�
    {
        
        switch (portalName)
        {
            case "StagePortal":     //portalName�� StagePortal�̶��
                
                if (!stageChoice.activeSelf)    //stageChoice ���ӿ�����Ʈ�� Ȱ��ȭ �Ǿ� ���� �ʴٸ�
                {
                    for (int i = 0; i < DataManager.instance.data.clearStage + 1; i++)
                    {
                        stageBtns[i].GetComponent<Button>().interactable = true;
                    }

                    mapPintr.rectTransform.position = stageBtns[DataManager.instance.data.clearStage].GetComponent<Image>().rectTransform.position + new Vector3(0, 23, 0);
                    stageChoice.SetActive(true);    //stageChoice ���ӿ�����Ʈ�� Ȱ��ȭ�ϰ�
                    
                    NewPlayerSc.canInput = false;   //player �̵� �Ұ�

                }
                else //stageChoice ���ӿ�����Ʈ�� Ȱ��ȭ �Ǿ��ִٸ�
                {
                    stageChoice.SetActive(false);   //stageChoice ���ӿ�����Ʈ ��Ȱ��ȭ�ϰ�
                    NewPlayerSc.canInput = true;    //player �̵� ����
                }
                break;


            case "StorePortal": //portalName�� StorePortal�̶��

                InventoryOpen();    //�κ��丮 Ȱ��ȭ �ϴ� �Լ�

                if (!Shop.activeSelf)   // shop�� Ȱ��ȭ �Ǿ����� �ʴٸ�(���� â�� �������� �ʴٸ�)
                {
                    Shop.SetActive(true);   //���� ���ӿ�����Ʈ�� Ȱ��ȭ
                    shopOpen=true;  //shopOpen bool�ڷ��� true
                    CloseOb.GetComponent<Button>().interactable = false;    // �κ��丮 �ݴ� ��ư  interactable false�� ����
                    NewPlayerSc.canInput = false;   //�÷��̾� �̵� �Ұ�
                }
                else // shop�� Ȱ��ȭ �Ǿ��ִٸ�
                {
                    Shop.SetActive(false);  // shop ���ӿ�����Ʈ ��Ȱ��ȭ
                    toolTip.SetActive(false);   //toolTip ���ӿ�����Ʈ ��Ȱ��ȭ
                    shopOpen = false;   //shopOpen =false��
                    CloseOb.GetComponent<Button>().interactable = true; //CloseOb ��ư interactable Ȱ��ȭ
                    NewPlayerSc.canInput = true;    //�÷��̾� �̵� ����
                }
                break;
        }
    }
    void ClickPlayerInfo()
    {
        statsOb.SetActive(true);    //statsOb ���ӿ�����Ʈ�� Ȱ��ȭ
    }
    public void InventoryOpen() //�κ��丮 ������ ����Ǵ� �Լ�
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]); // ���� ����
        if (!Bag.activeSelf)    //�κ��丮 â�� Ȱ��ȭ �Ǿ����� �ʴٸ�
        {
            Bag.SetActive(true);    //�κ��丮 â�� Ȱ��ȭ
            InvenBtn.interactable = false;  //�κ��丮 ��ư ��Ȱ��ȭ
            CloseOb.SetActive(true);    //�κ��丮 â �ݴ� ��ư Ȱ��ȭ
            Coin.SetActive(true);   //�κ��丮�� ���� ������Ʈ Ȱ��ȭ
            NewPlayerSc.canInput = false;   // �÷��̾� ������ �Ұ�

        }
        else // �κ��丮 â�� Ȱ��ȭ �Ǿ� �ִٸ� �ݴ�� ����
        {
            Bag.SetActive(false);   
            InvenBtn.interactable = true;
            CloseOb.SetActive(false);
            Coin.SetActive(false);
            NewPlayerSc.canInput = true;
        }
    }

    public void InventoryClose()    //�κ��丮 â �ݴ� ��ư �������� ����Ǵ� �Լ�
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);
        NewPlayerSc.canInput = true;    // �÷��̾� ������ ����
        InvenBtn.interactable = true;   //�κ��丮 ��ư Ȱ��ȭ
        
        // �κ��丮�� ���õ� ������Ʈ�� ��Ȱ��ȭ
        
        Bag.SetActive(false);
        Coin.SetActive(false);
        CloseOb.SetActive(false);
        toolTip.SetActive(false);
    }
   
    public void ComeBackHome()  //������ ���ư��� �Լ�
    {
        blindImageOb.SetActive(true);
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);
        Time.timeScale = 1;
        if (DataManager.instance.data.nowPlayerHp<=0)
        {
            NewPlayerSc.joysticSc.enabled = true;   // ��Ȱ��ȭ �ƴ� ��ũ��Ʈ Ȱ��ȭ
            NewPlayerSc.skillSc.enabled= true;
            NewPlayerSc.enabled = true;
            NewPlayerSc.anim.SetBool("isDead", false);
            print(DataManager.instance.data.nowPlayerHp);
            DataManager.instance.data.nowPlayerHp = DataManager.instance.data.playerMaxHp * 0.5f;
            print(DataManager.instance.data.nowPlayerHp);
        }
        DataManager.instance.Save();    // ������ ���ư��� ���̺� �Լ�����
        LoadingManager.LoadScene("Village");    // Village �� ������ loadingScene �ҷ�����
        cc.enabled = false; //ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ�ϱ�(���� �̵��Ǿ����� �÷��̾��� �߷��� ������� �ʰ� �ϱ� ����)
        playerOb.transform.position = new Vector3(157.547f, 0.21f, -69.15f);    //�÷��̾� ���������� ��ġ��ǥ
        playerOb.SendMessage("ClearStage", false);  //�÷��̾� �ִϸ��̼� �ٲٴ� �Լ� ����
        GameObject.Find("Clear").transform.GetChild(0).gameObject.SetActive(false);
    }

    public void RetryStage()    // �ش� �������� ������ϴ� �Լ�
    {
        blindImageOb.SetActive(true);
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);
        string nowScene=SceneManager.GetActiveScene().name;
        cc.enabled = false; //ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ�ϱ�(���� �̵��Ǿ����� �÷��̾��� �߷��� ������� �ʰ� �ϱ� ����)
        playerOb.transform.position = Vector3.zero;
        LoadingManager.LoadScene(nowScene);
        playerOb.SendMessage("ClearStage", false);
        GameObject.Find("Clear").transform.GetChild(0).gameObject.SetActive(false);
    }


    public void ShowTooltip(Item item)  //������ ���� �����ִ� �Լ�
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[0]);   //������ Ŭ����ư ���� ����
        toolTip.SetActive(true);    //toolTip ���ӿ�����Ʈ�� Ȱ��ȭ
        inventoryCs.itemUseBtn.gameObject.SetActive(true);  //�����ۻ�� ��ư ���ӿ�����Ʈ Ȱ��ȭ
        inventoryCs.itemSellBtn.gameObject.SetActive(true);    // ������ �Ǹ� ��ư ���ӿ�����Ʈ Ȱ��ȭ
        itemBuyBtn.gameObject.SetActive(true);  //������ ���� ��ư ���ӿ�����Ʈ ��Ȱ��ȭ
        itemSellPriceTxt.gameObject.SetActive(true);
        if (item.itemName== "EquipWeapon" && equipItemSc.isCheckEquip) //equipItemSc(Slot.cs)�� checkEquip�� true���
        {
            //toolTip.SetActive(true);    //tooltip ���ӿ�����Ʈ Ȱ��ȭ
            print("���������۽�ũ��Ʈ");
            inventoryCs.itemUseBtn.gameObject.SetActive(false); //inventoryCs.itemUseBtn.gameObject Ȱ��ȭ
            itemBuyBtn.gameObject.SetActive(false);  //������ ���� ��ư ���ӿ�����Ʈ ��Ȱ��ȭ

            toolTip.transform.position = equipItemSc.gameObject.transform.position; //tool�� transform.position�� equipItemSc.gameObject.transform.position���� ����
            clickSlotItemImg.sprite = item.itemImage;

            itemNameTxt.text = item.itemToolTipName;   //baseInner�� ù��° �ڽĿ�����Ʈ�� text������Ʈ ���� item.itemToolTipName�� ����
            itemInfoTxt.text = item.itemDesc;  //baseInner�� �ι�° �ڽĿ�����Ʈ�� text ������Ʈ ���� item.itemDesc�� ����
            itemSellPriceTxt.text = "�Ǹűݾ� : $" + item.itemSellPrice.ToString(); //baseInner�� ����° �ڽĿ�����Ʈ�� text ������Ʈ ����  "�Ǹűݾ� : $" + item.itemSellPrice.ToString()���� ����
        }
        else if (Shop.activeSelf)
        {
            inventoryCs.itemUseBtn.gameObject.SetActive(false);  //�����ۻ�� ��ư ���ӿ�����Ʈ Ȱ��ȭ
            itemBuyBtn.gameObject.SetActive(false);  //������ ���� ��ư ���ӿ�����Ʈ ��Ȱ��ȭ
        }
        else
        {
            inventoryCs.itemSellBtn.gameObject.SetActive(false);    // ������ �Ǹ� ��ư ���ӿ�����Ʈ ��Ȱ��ȭ
            itemBuyBtn.gameObject.SetActive(false);  //������ ���� ��ư ���ӿ�����Ʈ ��Ȱ��ȭ
        }
        for (int i = 0; i < inventorySlots.Length; i++)     //inventroySlots�� ������ŭ for�� �ݺ�
        {
            if (inventorySlots[i].item== inventoryCs.clickItem) //inventorySlots�� i��° item ���� invertoryCs.clickItem�� ���ٸ�
            {
                //toolTip.SetActive(true);    //toolTip ���ӿ�����Ʈ�� Ȱ��ȭ
                toolTip.transform.position = inventorySlots[i].gameObject.transform.position;   //toolTip ���ӿ�����Ʈ�� ��ġ�� inventorySlots[i].gameObject�� ��ġ�� ����
                break;
            }
        }
        clickSlotItemImg.sprite = item.itemImage;

        if (item.itemImage == null)
        {
            clickSlotItemImg.color = new Color(1, 1, 1, 0); // ���� �̹��� ���� 0
        }
        else
        {

            clickSlotItemImg.color = new Color(1, 1, 1, 1); // ���� �̹��� ���� 0
        }
        
        
        itemNameTxt.text = item.itemToolTipName;   //baseInner�� ù��° �ڽĿ�����Ʈ�� text������Ʈ ���� item.itemToolTipName�� ����
        itemInfoTxt.text = item.itemDesc;  //baseInner�� �ι�° �ڽĿ�����Ʈ�� text ������Ʈ ���� item.itemDesc�� ����
        itemSellPriceTxt.text = "�Ǹűݾ� : $" + item.itemSellPrice.ToString(); //baseInner�� ����° �ڽĿ�����Ʈ�� text ������Ʈ ����  "�Ǹűݾ� : $" + item.itemSellPrice.ToString()���� ����

        toolTipRectTransform.anchoredPosition+= new Vector2(0, -310);   //����â ��ġ����

    }
    // �������� ������ ���Ž� �˾�â �ߴ� �Լ�
    public void OpenBuypopUp()
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[0]);   //������ Ŭ����ư ���� ����
        buyPopUp.SetActive(true);
    }
    public void CloseToolTip()  //ToolTipâ �ݴ� �Լ� (����â �ݴ� ��ư�� ����)
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);   //��ư ���� ����
        if (equipItemSc.isCheckEquip) //���� Slot.cs�� checkEquip�� true���
        {
            inventoryCs.itemUseBtn.gameObject.SetActive(true);  // ������ ����ư Ȱ��ȭ
        }
        toolTip.SetActive(false);   //���� ��Ȱ��ȭ
    }

    //NPC ��ȣ�ۿ� ��� �Լ�
    public IEnumerator NpcTalk(bool isShop)
    {
        storeNpcTxtMesh.gameObject.SetActive(true);
        if (isShop)
        {
            storeNpcTxtMesh.text = "���!! ��ٸ��� �־���!!";
        }
        else
        {
            storeNpcTxtMesh.text = "����~ ������ �� ����!!";
        }
        yield return new WaitForSeconds(3);
        storeNpcTxtMesh.gameObject.SetActive(false);
    }
    void GetSetting()   //�����Ҷ����� ���ÿ� �ʿ��� ���� �������� �Լ�(LoadSceneEvent �Լ����� ����)
    {
        AudioManager.instance.Sound_Bgm(AudioManager.instance.soundsBgm[1]);    //villiage Scene BGM �����ϴ� �Լ�
        stageChoice = GameObject.Find("Stage").transform.GetChild(0).gameObject;//Stage�� 0��° �ڽ� ������Ʈ�� ã��
        Shop = GameObject.Find("Store").transform.GetChild(0).gameObject; //Store�� 0��° �ڽ� ������Ʈ�� ã��
        statsOb = GameObject.FindGameObjectWithTag("StatsUi").transform.GetChild(0).gameObject; //StatsUi ���� �ִ� ���ӿ�����Ʈ ã�Ƽ� statsOb�� �־��ֱ�

        // �κ��丮�� ���� ������Ʈ ��������
        inventoryCs = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();  //Inventory �±׸� ���� �ִ� ���ӿ�����Ʈ�� Inventroy ��ũ��Ʈ ������Ʈ�� inventoryCs�� �־���
        Bag = inventoryCs.transform.GetChild(0).gameObject; //inventoryCs�� 0��° �ڽĿ� Bag ���ӿ�����Ʈ ������ �־���
        Coin = inventoryCs.transform.GetChild(1).gameObject; //inventoryCs�� 0��° �ڽĿ� Bag ���ӿ�����Ʈ ������ �־���
        CloseOb = inventoryCs.transform.GetChild(2).gameObject; //inventoryCs�� 0��° �ڽĿ� Bag ���ӿ�����Ʈ ������ �־���
        CloseOb.GetComponent<Button>().onClick.AddListener(InventoryClose); //inventoryCs�� 0��° �ڽĿ� Bag ���ӿ�����Ʈ ������ �־���

        //������ ������ ���� ������Ʈ ��������
        toolTip = GameObject.FindGameObjectWithTag("ItemToolTip").transform.GetChild(0).gameObject; //toolTip ���ӿ�����Ʈ ������ �־��ֱ�
        buyPopUp = GameObject.FindGameObjectWithTag("ItemToolTip").transform.GetChild(1).gameObject;    //buyPopUp ���ӿ�����Ʈ ������ �־��ֱ�
        
        clickSlotItemImg =toolTip.transform.GetChild(4).GetComponent<Image>();   //toolTip���ӿ�����Ʈ�� �ڽĿ�����Ʈ�� ������ �̹����� �־��� �̹��� ��������
        
        toolTipBaseIneer = toolTip.transform.GetChild(0).gameObject;    //������ ����â ���� ���ӿ�����Ʈ
        clickSlotItemImg=toolTip.transform.GetChild(4).GetComponent<Image>();   //���� ������Ʈ �ڽĿ��� Ŭ���������̹��� ���� �̹��� ��������
        toolTipRectTransform = toolTip.GetComponent<RectTransform>();   //��ƮƮ������ ������ ���� ����â�� ��ƮƮ������ ������Ʈ �� ��������
        itemNameTxt = toolTipBaseIneer.transform.GetChild(0).GetComponent<Text>();   //������ �̸�
        itemInfoTxt = toolTipBaseIneer.transform.GetChild(1).GetComponent<Text>();   //������ ����
        itemSellPriceTxt = toolTipBaseIneer.transform.GetChild(2).GetComponent<Text>(); //������ �ǸŰ���

        itemUseBtn = toolTip.transform.GetChild(1).GetComponent<Button>(); // toolTip ���ӿ�����Ʈ�� �ι�° �ڽ��� ��ư ������Ʈ�� itemUseBtn ������ �־���
        inventoryCs.itemUseBtn = itemUseBtn;    //inventory ������Ʈ�� itemUseBtn �������� UiManager�� itemUseBtn ���� �־���
        itemSellBtn = toolTip.transform.GetChild(2).GetComponent<Button>(); // toolTip ���ӿ�����Ʈ�� ����° �ڽ��� ��ư ������Ʈ�� itemSellBtn ������ �־���
        inventoryCs.itemSellBtn = itemSellBtn;  //inventory ������Ʈ�� itemSellBtn �������� UiManager�� itemSellBtn ���� �־���

        itemBuyBtn = toolTip.transform.GetChild(5).GetComponent<Button>(); // toolTip ���ӿ�����Ʈ�� ����° �ڽ��� ��ư ������Ʈ�� itemSellBtn ������ �־���
        itemBuyBtn.onClick.AddListener(OpenBuypopUp);   // ������ ���� �˾� ������ �Լ� ����

        inventoryCs.itemUseBtn.onClick.AddListener(inventoryCs.UseItem);    //inventoryCs.itemUseBtn�� useItem�Լ� ����
        inventoryCs.itemSellBtn.onClick.AddListener(inventoryCs.SellItem);  // inventoryCs.itemSellBtn�� inventoryCs.SellItem�Լ� ����

        toolTipCloseBtn = toolTip.transform.GetChild(3).GetComponent<Button>(); // toolTip�� 4��° �ڽ� ������Ʈ�� Button ������Ʈ�� �������� toolTipCloseBtn�־���
        toolTipCloseBtn.onClick.AddListener(CloseToolTip);  //toolTipCloseBtn�� closeToolTip(���� �ݴ� �Լ�) ����
        inventorySlots = inventoryCs.GetComponent<Inventory>().slots;   // inventoryCs�� Inventory ��ũ��Ʈ ������Ʈ�� slots �迭 ���� inventorySlots �������� �־���

        SettingOb = GameObject.FindObjectOfType<SettingManager>().gameObject;   //  SettingManager ��ũ��Ʈ ������Ʈ�� ������ �ִ� ���ӿ�����Ʈ�� ã�Ƽ� SettingOb �������� �־���
        equipItemSc= statsOb.transform.GetChild(3).transform.GetChild(1).transform.GetChild(0).GetComponent<Slot>();   //  
        inventoryCs.equipItemSc = equipItemSc;  //inventoryCs.equipItemSc ������ ���������� Slot ��ũ��Ʈ �־���
        InvenBtn.interactable = true;   // �κ��丮 ��ư Ȱ��ȭ

        playerInfoBtn.interactable = true;  //�÷��̾� ����â ��ư Ȱ��ȭ
        blindImageOb.SetActive(false);   //����ε� �̹��� ��Ȱ��ȭ

        //�������� ��ư ���ӿ�����Ʈ�� ��������
        for (int i = 0; i < 4; i++)
        {
            print(GameObject.FindGameObjectWithTag("StageBtn").transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).gameObject.name);
            stageBtns[i] = GameObject.FindGameObjectWithTag("StageBtn").transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).gameObject;
            
        }

        // �������� ���� �ߴ� ��
        mapPintr = GameObject.FindGameObjectWithTag("StageBtn").transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        // NPC�� ���
        storeNpcTxtMesh = GameObject.FindGameObjectWithTag("NPC").transform.GetChild(0).GetComponent<TextMesh>();
    }

    
}
