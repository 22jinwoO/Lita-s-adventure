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

    //플레이어 스크립트
    NewPlayer NewPlayerSc;
    CharacterController cc;
    [Header("포탈 이용 관련")]
    public string portalName;
    public bool onPortal = false;

    [Header("포탈 기능 관련(스테이지, 상점 Ui 오픈, 스탯창 오픈)")]
    public GameObject stageChoice;
    public GameObject Shop;
    public GameObject statsOb;

    [HideInInspector]//플레이어 버튼
    public Button attackBtn;
    
    [Header("플레이어 인벤토리 관련")]
    public GameObject Bag;
    public GameObject Coin;    // 플레이어 메소 담당 Ui
    public GameObject CloseOb; // 인벤토리 창 닫는 버튼 오브젝트
    public Button InvenBtn;    //인벤토리 버튼
    public Slot[] inventorySlots;  // 인벤토리 슬롯 배열들
    public Inventory inventoryCs;  //인벤토리 스크립트

    [Header("아이템 툴팁 관련")]
    public Button itemUseBtn;   //아이템 툴팁 사용버튼
    public Button itemSellBtn;  //아이템 툴팁 판매 버튼
    public Button itemBuyBtn;  //아이템 툴팁 구매 버튼
    public Button toolTipCloseBtn;  //아이템 툴팁 닫는 버튼
    public Text itemSellPriceTxt;   //아이템 툴팁 판매 가격 보여주는 텍스트
    Text itemNameTxt;
    Text itemInfoTxt;
    GameObject toolTipBaseIneer;
    public GameObject toolTip;  //아이템 툴팁 오브젝트

    public GameObject buyPopUp;  //아이템 구매 팝업 오브젝트

    public Transform shopSlotTr;    //상점 아이템 슬롯 위치
    [HideInInspector]
    public bool shopOpen=false; // 아이템 툴팁창이 열렸을때 상점이 같이 열려있으면 판매버튼이 동작하게끔 구분지어주는 bool자료형

    [Header("플레이어 닉네임 관련")]

    public Text playerNicknameTxt;
    [Header("플레이어 장착 무기")]
    public Item equipItem;
    public Slot equipItemSc;

    [Header("플레이어 공격 버튼 이미지")]
    public Image AttackBtnImg;
    public Image AttackBackBtnImg;

    [Header("플레이어 상황에 따른 버튼 이미지")]
    public Sprite goStageBtnImg;
    public Sprite goShopBtnImg;
    public Sprite atkBtnImg;

    GameObject SettingOb;

    [Header("플레이어 정보 버튼")]
    public Button playerInfoBtn;

    [Header("슬롯의 클릭 아이템 이미지")]
    public Image clickSlotItemImg;
    [Header("툴팁창 RectTransform")]
    public RectTransform toolTipRectTransform;

    [Header("블라인드 이미지 게임오브젝트")]
    public GameObject blindImageOb;
    static public UiManager instance;

    [Header("스테이지 버튼들")]
    public GameObject[] stageBtns= new GameObject[4];

    // 스테이지 맵의 다음 스테이지를 알려주는 핀 이미지
    Image mapPintr;

    public TextMesh storeNpcTxtMesh;

    void Awake()
    {
        if (null == instance)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance!=this)
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            Destroy(this.gameObject);
        }
    }
    void Start()
    {

        playerNicknameTxt.text = DataManager.instance.data.playerNickName;
        NewPlayerSc = GameObject.FindObjectOfType<CharacterController>().GetComponent<NewPlayer>(); //플레이어 오브젝트 가져오기
        cc = NewPlayerSc.GetComponent<CharacterController>();
        attackBtn.onClick.AddListener(ClickAttack);
        InvenBtn = GameObject.FindGameObjectWithTag("InvenBtn").GetComponent<Button>();
        InvenBtn.onClick.AddListener(InventoryOpen);

        SceneManager.sceneLoaded += LoadedsceneEvent;   //씬 이벤트 함수 적용 씬불러올때마다 함수를 등록해서 사용하는식

        GetSetting();
        
        playerInfoBtn.onClick.AddListener(ClickPlayerInfo); //플레이어 정보UI에 플레이어 정보창 여는 함수 연결


    }
    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        if (scene.name=="Village")  //씬 이름이 village일때만 GetSetting 함수 실행
        {
            GetSetting();
        }
        if (scene.name!="LoadingScene") //씬 이름이 LoadingScene이 아니면 CharacterController 활성화
        {
            cc.enabled = true;
        }
    }
    void Update()
    {
    }
    void ClickAttack()  //공격 버튼 클릭 시 포탈사용인지 공격인지 확인하는 함수
    {
        if (onPortal) // onPortal이 true일때
        {
            PortalUiOpen(portalName);   //portalName을 매개변수로 사용하는 portalUiOpen 함수 실행
        }
        else
        {
            NewPlayerSc.Attack(); //공격함수 실행
        }
    }
    public void PortalUiOpen(string portalName) //포탈이름에 해당되는 기능을 사용가능하게 하는 함수
    {
        
        switch (portalName)
        {
            case "StagePortal":     //portalName이 StagePortal이라면
                
                if (!stageChoice.activeSelf)    //stageChoice 게임오브젝트가 활성화 되어 있지 않다면
                {
                    for (int i = 0; i < DataManager.instance.data.clearStage + 1; i++)
                    {
                        stageBtns[i].GetComponent<Button>().interactable = true;
                    }

                    mapPintr.rectTransform.position = stageBtns[DataManager.instance.data.clearStage].GetComponent<Image>().rectTransform.position + new Vector3(0, 23, 0);
                    stageChoice.SetActive(true);    //stageChoice 게임오브젝트가 활성화하고
                    
                    NewPlayerSc.canInput = false;   //player 이동 불가

                }
                else //stageChoice 게임오브젝트가 활성화 되어있다면
                {
                    stageChoice.SetActive(false);   //stageChoice 게임오브젝트 비활성화하고
                    NewPlayerSc.canInput = true;    //player 이동 가능
                }
                break;


            case "StorePortal": //portalName이 StorePortal이라면

                InventoryOpen();    //인벤토리 활성화 하는 함수

                if (!Shop.activeSelf)   // shop이 활성화 되어있지 않다면(상점 창이 열려있지 않다면)
                {
                    Shop.SetActive(true);   //상점 게임오브젝트를 활성화
                    shopOpen=true;  //shopOpen bool자료형 true
                    CloseOb.GetComponent<Button>().interactable = false;    // 인벤토리 닫는 버튼  interactable false로 지정
                    NewPlayerSc.canInput = false;   //플레이어 이동 불가
                }
                else // shop이 활성화 되어있다면
                {
                    Shop.SetActive(false);  // shop 게임오브젝트 비활성화
                    toolTip.SetActive(false);   //toolTip 게임오브젝트 비활성화
                    shopOpen = false;   //shopOpen =false로
                    CloseOb.GetComponent<Button>().interactable = true; //CloseOb 버튼 interactable 활성화
                    NewPlayerSc.canInput = true;    //플레이어 이동 가능
                }
                break;
        }
    }
    void ClickPlayerInfo()
    {
        statsOb.SetActive(true);    //statsOb 게임오브젝트가 활성화
    }
    public void InventoryOpen() //인벤토리 열릴때 실행되는 함수
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]); // 사운드 실행
        if (!Bag.activeSelf)    //인벤토리 창이 활성화 되어있지 않다면
        {
            Bag.SetActive(true);    //인벤토리 창을 활성화
            InvenBtn.interactable = false;  //인벤토리 버튼 비활성화
            CloseOb.SetActive(true);    //인벤토리 창 닫는 버튼 활성화
            Coin.SetActive(true);   //인벤토리에 코인 오브젝트 활성화
            NewPlayerSc.canInput = false;   // 플레이어 움직임 불가

        }
        else // 인벤토리 창이 활성화 되어 있다면 반대로 실행
        {
            Bag.SetActive(false);   
            InvenBtn.interactable = true;
            CloseOb.SetActive(false);
            Coin.SetActive(false);
            NewPlayerSc.canInput = true;
        }
    }

    public void InventoryClose()    //인벤토리 창 닫는 버튼 눌렀을떄 실행되는 함수
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);
        NewPlayerSc.canInput = true;    // 플레이어 움직임 가능
        InvenBtn.interactable = true;   //인벤토리 버튼 활성화
        
        // 인벤토리와 관련된 오브젝트들 비활성화
        
        Bag.SetActive(false);
        Coin.SetActive(false);
        CloseOb.SetActive(false);
        toolTip.SetActive(false);
    }
   
    public void ComeBackHome()  //마을로 돌아가는 함수
    {
        blindImageOb.SetActive(true);
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);
        Time.timeScale = 1;
        if (DataManager.instance.data.nowPlayerHp<=0)
        {
            NewPlayerSc.joysticSc.enabled = true;   // 비활성화 됐던 스크립트 활성화
            NewPlayerSc.skillSc.enabled= true;
            NewPlayerSc.enabled = true;
            NewPlayerSc.anim.SetBool("isDead", false);
            print(DataManager.instance.data.nowPlayerHp);
            DataManager.instance.data.nowPlayerHp = DataManager.instance.data.playerMaxHp * 0.5f;
            print(DataManager.instance.data.nowPlayerHp);
        }
        DataManager.instance.Save();    // 마을로 돌아갈때 세이브 함수실행
        LoadingManager.LoadScene("Village");    // Village 씬 가기전 loadingScene 불러오기
        cc.enabled = false; //캐릭터 컨트롤러 비활성화하기(씬이 이동되었을때 플레이어의 중력이 적용되지 않게 하기 위해)
        playerOb.transform.position = new Vector3(157.547f, 0.21f, -69.15f);    //플레이어 마을에서의 위치좌표
        playerOb.SendMessage("ClearStage", false);  //플레이어 애니메이션 바꾸는 함수 실행
        GameObject.Find("Clear").transform.GetChild(0).gameObject.SetActive(false);
    }

    public void RetryStage()    // 해당 스테이지 재시작하는 함수
    {
        blindImageOb.SetActive(true);
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);
        string nowScene=SceneManager.GetActiveScene().name;
        cc.enabled = false; //캐릭터 컨트롤러 비활성화하기(씬이 이동되었을때 플레이어의 중력이 적용되지 않게 하기 위해)
        playerOb.transform.position = Vector3.zero;
        LoadingManager.LoadScene(nowScene);
        playerOb.SendMessage("ClearStage", false);
        GameObject.Find("Clear").transform.GetChild(0).gameObject.SetActive(false);
    }


    public void ShowTooltip(Item item)  //아이템 툴팁 보여주는 함수
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[0]);   //아이템 클릭버튼 사운드 실행
        toolTip.SetActive(true);    //toolTip 게임오브젝트를 활성화
        inventoryCs.itemUseBtn.gameObject.SetActive(true);  //아이템사용 버튼 게임오브젝트 활성화
        inventoryCs.itemSellBtn.gameObject.SetActive(true);    // 아이템 판매 버튼 게임오브젝트 활성화
        itemBuyBtn.gameObject.SetActive(true);  //아이템 구매 버튼 게임오브젝트 비활성화
        itemSellPriceTxt.gameObject.SetActive(true);
        if (item.itemName== "EquipWeapon" && equipItemSc.isCheckEquip) //equipItemSc(Slot.cs)의 checkEquip이 true라면
        {
            //toolTip.SetActive(true);    //tooltip 게임오브젝트 활성화
            print("장착아이템스크립트");
            inventoryCs.itemUseBtn.gameObject.SetActive(false); //inventoryCs.itemUseBtn.gameObject 활성화
            itemBuyBtn.gameObject.SetActive(false);  //아이템 구매 버튼 게임오브젝트 비활성화

            toolTip.transform.position = equipItemSc.gameObject.transform.position; //tool의 transform.position은 equipItemSc.gameObject.transform.position으로 설정
            clickSlotItemImg.sprite = item.itemImage;

            itemNameTxt.text = item.itemToolTipName;   //baseInner의 첫번째 자식오브젝트의 text컴포넌트 값을 item.itemToolTipName로 넣음
            itemInfoTxt.text = item.itemDesc;  //baseInner의 두번째 자식오브젝트의 text 컴포넌트 값을 item.itemDesc로 넣음
            itemSellPriceTxt.text = "판매금액 : $" + item.itemSellPrice.ToString(); //baseInner의 세번째 자식오브젝트의 text 컴포넌트 값을  "판매금액 : $" + item.itemSellPrice.ToString()으로 넣음
        }
        else if (Shop.activeSelf)
        {
            inventoryCs.itemUseBtn.gameObject.SetActive(false);  //아이템사용 버튼 게임오브젝트 활성화
            itemBuyBtn.gameObject.SetActive(false);  //아이템 구매 버튼 게임오브젝트 비활성화
        }
        else
        {
            inventoryCs.itemSellBtn.gameObject.SetActive(false);    // 아이템 판매 버튼 게임오브젝트 비활성화
            itemBuyBtn.gameObject.SetActive(false);  //아이템 구매 버튼 게임오브젝트 비활성화
        }
        for (int i = 0; i < inventorySlots.Length; i++)     //inventroySlots의 갯수만큼 for문 반복
        {
            if (inventorySlots[i].item== inventoryCs.clickItem) //inventorySlots의 i번째 item 값이 invertoryCs.clickItem과 같다면
            {
                //toolTip.SetActive(true);    //toolTip 게임오브젝트를 활성화
                toolTip.transform.position = inventorySlots[i].gameObject.transform.position;   //toolTip 게임오브젝트의 위치를 inventorySlots[i].gameObject의 위치로 설정
                break;
            }
        }
        clickSlotItemImg.sprite = item.itemImage;

        if (item.itemImage == null)
        {
            clickSlotItemImg.color = new Color(1, 1, 1, 0); // 슬롯 이미지 투명도 0
        }
        else
        {

            clickSlotItemImg.color = new Color(1, 1, 1, 1); // 슬롯 이미지 투명도 0
        }
        
        
        itemNameTxt.text = item.itemToolTipName;   //baseInner의 첫번째 자식오브젝트의 text컴포넌트 값을 item.itemToolTipName로 넣음
        itemInfoTxt.text = item.itemDesc;  //baseInner의 두번째 자식오브젝트의 text 컴포넌트 값을 item.itemDesc로 넣음
        itemSellPriceTxt.text = "판매금액 : $" + item.itemSellPrice.ToString(); //baseInner의 세번째 자식오브젝트의 text 컴포넌트 값을  "판매금액 : $" + item.itemSellPrice.ToString()으로 넣음

        toolTipRectTransform.anchoredPosition+= new Vector2(0, -310);   //툴팁창 위치조정

    }
    // 상점에서 아이템 구매시 팝업창 뜨는 함수
    public void OpenBuypopUp()
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[0]);   //아이템 클릭버튼 사운드 실행
        buyPopUp.SetActive(true);
    }
    public void CloseToolTip()  //ToolTip창 닫는 함수 (툴팁창 닫는 버튼에 연결)
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);   //버튼 사운드 실행
        if (equipItemSc.isCheckEquip) //만약 Slot.cs의 checkEquip가 true라면
        {
            inventoryCs.itemUseBtn.gameObject.SetActive(true);  // 아이템 사용버튼 활성화
        }
        toolTip.SetActive(false);   //툴팁 비활성화
    }

    //NPC 상호작용 대사 함수
    public IEnumerator NpcTalk(bool isShop)
    {
        storeNpcTxtMesh.gameObject.SetActive(true);
        if (isShop)
        {
            storeNpcTxtMesh.text = "어서와!! 기다리고 있었어!!";
        }
        else
        {
            storeNpcTxtMesh.text = "고마워~ 다음에 또 와줘!!";
        }
        yield return new WaitForSeconds(3);
        storeNpcTxtMesh.gameObject.SetActive(false);
    }
    void GetSetting()   //시작할때마다 세팅에 필요한 값을 가져오는 함수(LoadSceneEvent 함수에서 실행)
    {
        AudioManager.instance.Sound_Bgm(AudioManager.instance.soundsBgm[1]);    //villiage Scene BGM 실행하는 함수
        stageChoice = GameObject.Find("Stage").transform.GetChild(0).gameObject;//Stage의 0번째 자식 오브젝트를 찾기
        Shop = GameObject.Find("Store").transform.GetChild(0).gameObject; //Store의 0번째 자식 오브젝트를 찾기
        statsOb = GameObject.FindGameObjectWithTag("StatsUi").transform.GetChild(0).gameObject; //StatsUi 갖고 있는 게임오브젝트 찾아서 statsOb에 넣어주기

        // 인벤토리를 위해 오브젝트 가져오기
        inventoryCs = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();  //Inventory 태그를 갖고 있는 게임오브젝트의 Inventroy 스크립트 컴포넌트를 inventoryCs에 넣어줌
        Bag = inventoryCs.transform.GetChild(0).gameObject; //inventoryCs의 0번째 자식에 Bag 게임오브젝트 변수에 넣어줌
        Coin = inventoryCs.transform.GetChild(1).gameObject; //inventoryCs의 0번째 자식에 Bag 게임오브젝트 변수에 넣어줌
        CloseOb = inventoryCs.transform.GetChild(2).gameObject; //inventoryCs의 0번째 자식에 Bag 게임오브젝트 변수에 넣어줌
        CloseOb.GetComponent<Button>().onClick.AddListener(InventoryClose); //inventoryCs의 0번째 자식에 Bag 게임오브젝트 변수에 넣어줌

        //아이템 툴팁을 위해 오브젝트 가져오기
        toolTip = GameObject.FindGameObjectWithTag("ItemToolTip").transform.GetChild(0).gameObject; //toolTip 게임오브젝트 변수값 넣어주기
        buyPopUp = GameObject.FindGameObjectWithTag("ItemToolTip").transform.GetChild(1).gameObject;    //buyPopUp 게임오브젝트 변수값 넣어주기
        
        clickSlotItemImg =toolTip.transform.GetChild(4).GetComponent<Image>();   //toolTip게임오브젝트의 자식오브젝트중 아이템 이미지를 넣어줄 이미지 가져오기
        
        toolTipBaseIneer = toolTip.transform.GetChild(0).gameObject;    //아이템 툴팁창 내부 게임오브젝트
        clickSlotItemImg=toolTip.transform.GetChild(4).GetComponent<Image>();   //툴팁 오브젝트 자식에서 클릭아이템이미지 넣을 이미지 가져오기
        toolTipRectTransform = toolTip.GetComponent<RectTransform>();   //렉트트랜스폼 조정을 위해 툴팁창의 렉트트랜스폼 컴포넌트 값 가져오기
        itemNameTxt = toolTipBaseIneer.transform.GetChild(0).GetComponent<Text>();   //아이템 이름
        itemInfoTxt = toolTipBaseIneer.transform.GetChild(1).GetComponent<Text>();   //아이템 정보
        itemSellPriceTxt = toolTipBaseIneer.transform.GetChild(2).GetComponent<Text>(); //아이템 판매가격

        itemUseBtn = toolTip.transform.GetChild(1).GetComponent<Button>(); // toolTip 게임오브젝트의 두번째 자식의 버튼 컴포넌트를 itemUseBtn 변수에 넣어줌
        inventoryCs.itemUseBtn = itemUseBtn;    //inventory 오브젝트의 itemUseBtn 변수값에 UiManager의 itemUseBtn 값을 넣어줌
        itemSellBtn = toolTip.transform.GetChild(2).GetComponent<Button>(); // toolTip 게임오브젝트의 세번째 자식의 버튼 컴포넌트를 itemSellBtn 변수에 넣어줌
        inventoryCs.itemSellBtn = itemSellBtn;  //inventory 오브젝트의 itemSellBtn 변수값에 UiManager의 itemSellBtn 값을 넣어줌

        itemBuyBtn = toolTip.transform.GetChild(5).GetComponent<Button>(); // toolTip 게임오브젝트의 세번째 자식의 버튼 컴포넌트를 itemSellBtn 변수에 넣어줌
        itemBuyBtn.onClick.AddListener(OpenBuypopUp);   // 아이템 구매 팝업 열리는 함수 연결

        inventoryCs.itemUseBtn.onClick.AddListener(inventoryCs.UseItem);    //inventoryCs.itemUseBtn에 useItem함수 연결
        inventoryCs.itemSellBtn.onClick.AddListener(inventoryCs.SellItem);  // inventoryCs.itemSellBtn에 inventoryCs.SellItem함수 연결

        toolTipCloseBtn = toolTip.transform.GetChild(3).GetComponent<Button>(); // toolTip의 4번째 자식 오브젝트의 Button 컴포넌트를 변수값에 toolTipCloseBtn넣어줌
        toolTipCloseBtn.onClick.AddListener(CloseToolTip);  //toolTipCloseBtn에 closeToolTip(툴팁 닫는 함수) 연결
        inventorySlots = inventoryCs.GetComponent<Inventory>().slots;   // inventoryCs의 Inventory 스크립트 컴포넌트의 slots 배열 값을 inventorySlots 변수값에 넣어줌

        SettingOb = GameObject.FindObjectOfType<SettingManager>().gameObject;   //  SettingManager 스크립트 컴포넌트를 가지고 있는 게임오브젝트를 찾아서 SettingOb 변수값에 넣어줌
        equipItemSc= statsOb.transform.GetChild(3).transform.GetChild(1).transform.GetChild(0).GetComponent<Slot>();   //  
        inventoryCs.equipItemSc = equipItemSc;  //inventoryCs.equipItemSc 변수에 장착아이템 Slot 스크립트 넣어줌
        InvenBtn.interactable = true;   // 인벤토리 버튼 활성화

        playerInfoBtn.interactable = true;  //플레이어 정보창 버튼 활성화
        blindImageOb.SetActive(false);   //블라인드 이미지 비활성화

        //스테이지 버튼 게임오브젝트들 가져오기
        for (int i = 0; i < 4; i++)
        {
            print(GameObject.FindGameObjectWithTag("StageBtn").transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).gameObject.name);
            stageBtns[i] = GameObject.FindGameObjectWithTag("StageBtn").transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).gameObject;
            
        }

        // 스테이지 위의 뜨는 핀
        mapPintr = GameObject.FindGameObjectWithTag("StageBtn").transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
        // NPC의 대사
        storeNpcTxtMesh = GameObject.FindGameObjectWithTag("NPC").transform.GetChild(0).GetComponent<TextMesh>();
    }

    
}
