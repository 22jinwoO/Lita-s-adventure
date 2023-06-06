using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    [Header("스탯 화면 오브젝트")]
    public GameObject statsUi;

    [Header("유저 공격력 보여주는 텍스트")]
    public Text attackDmgTxt;

    [Header("유저 스킬 공격력 보여주는 텍스트")]
    public Text skillDmgTxt;

    [Header("유저 최대 체력 보여주는 텍스트")]
    public Text maxHpTxt;

    [Header("유저 스킬 재사용 대기시간 보여주는 텍스트")]
    public Text skillCoolTxt;

    [Header("유저 공격속도 보여주는 텍스트")]
    public Text attackSpeedTxt;

    [Header("유저 스킬포인트 보여주는 텍스트")]
    public Text skillPointTxt;

    // -------
    [Header("유저 공격력 플러스 버튼")]
    public Button attackDmgBtn;

    [Header("유저 스킬 공격력 플러스 버튼")]
    public Button skillDmgBtn;

    [Header("유저 최대 체력 플러스 버튼")]
    public Button maxHpBtn;

    [Header("유저 스킬 재사용 대기시간 플러스 버튼")]
    public Button skillCoolBtn;

    [Header("유저 공격속도 플러스 버튼")]
    public Button attackSpeedBtn;

    [Header("스탯창 닫는 버튼")]
    public Button closeStatsUi;

    [Header("장착 아이템 Slot 스크립트")]
    public Slot equipItemSc;
    [Header("플레이어 레벨 Text")]
    public Text lvTxt;
    [Header("플레이어 닉네임 Text")]
    public Text playerNameTxt;

    [Header("플레이어 경험치 바")]
    public Image expBarImg;
    [Header("플레이어 경험치 퍼센트값 텍스트")]
    public Text expGageTxt;

    void Start()
    {
        attackDmgBtn.onClick.AddListener(AttackPlus);
        skillDmgBtn.onClick.AddListener(SkillDmgPlus);
        maxHpBtn.onClick.AddListener(MaxHpPlus);
        skillCoolBtn.onClick.AddListener(SkillCoolDownPlus);
        attackSpeedBtn.onClick.AddListener(CriticalPlus);
        closeStatsUi.onClick.AddListener(CloseStats);

        UiManager.instance.playerInfoBtn.onClick.AddListener(StatsOpen);
    }

    void Update()
    {
        if (statsUi.activeSelf) //스탯창 오브젝트가 활성화 되면
        {
            attackDmgTxt.text = "공격력 : " + DataManager.instance.data.playerAttackDmg.ToString();
            skillDmgTxt.text = "스킬 공격력 : " + DataManager.instance.data.playerSkillDmg.ToString();
            maxHpTxt.text = "최대 체력 : " + DataManager.instance.data.playerMaxHp.ToString();
            skillCoolTxt.text = "스킬 재사용 대기시간 감소 : " + (DataManager.instance.data.playerSkillCool*10).ToString();
            attackSpeedTxt.text = "크리티컬 확률 : " + (DataManager.instance.data.playerCritical).ToString()+ "%";
            skillPointTxt.text = DataManager.instance.data.playerSkillPoint.ToString();
        }
    }
     
    void StatsOpen()
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);   //설정버튼 열릴때 사운드 실행
        lvTxt.text = "Lv.  " + DataManager.instance.data.playerLv.ToString();   //플레이어 레벨 텍스트 반영
        playerNameTxt.text = DataManager.instance.data.playerNickName;  // 플레이어 닉네임 텍스트에 반영
        equipItemSc.isCheckEquip = true;  //장착무기 스크립트 불자료형
        expBarImg.fillAmount = DataManager.instance.data.playerNowExp / DataManager.instance.data.playerMaxExp; // 플레이어 경험치 바 표시
        expGageTxt.text = (expBarImg.fillAmount * 100).ToString("F2") + "%";
    }

    void CloseStats()
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);
        UiManager.instance.toolTip.SetActive(false);    //게임 툴팁 오브젝트 비활성화
        equipItemSc.isCheckEquip = false; //무기 장착인지 구분하기 위한 bool 자료형 false로 변환
        statsUi.SetActive(false);
    }

    void AttackPlus()   //공격 스탯 올리는 함수
    {
        if (DataManager.instance.data.playerSkillPoint > 0) //플레이어 스킬 포인트가 0이 아닐때
        {
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[4]);   //스킬포인트 버튼 클릭할때 사운드실행
            DataManager.instance.data.playerAttackDmg += 10;    //플레이어 공격력 +10
            DataManager.instance.data.playerSkillPoint -= 1;    //플레이어 스킬포인트 --
        }
    }

    void SkillDmgPlus()
    {
        if (DataManager.instance.data.playerSkillPoint > 0) //플레이어 스킬 포인트가 0이 아닐때
        {
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[4]);
            DataManager.instance.data.playerSkillDmg += 10; //플레이어 스킬데미지 +10
            DataManager.instance.data.playerSkillPoint -= 1;
        }
    }

    void MaxHpPlus()
    {
        if (DataManager.instance.data.playerSkillPoint > 0)
        {
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[4]);
            DataManager.instance.data.playerMaxHp += 50;
            DataManager.instance.data.playerSkillPoint -= 1;
        }
    }

    void SkillCoolDownPlus()
    {
        if (DataManager.instance.data.playerSkillPoint > 0)
        {
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[4]);
            DataManager.instance.data.playerSkillCool += 1;
            DataManager.instance.data.playerSkillPoint -= 1;
        }
    }

    void CriticalPlus()
    {
        if (DataManager.instance.data.playerSkillPoint > 0)
        {
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[4]);
            DataManager.instance.data.playerCritical += 1;
            DataManager.instance.data.playerSkillPoint -= 1;
        }
    }
    

}
