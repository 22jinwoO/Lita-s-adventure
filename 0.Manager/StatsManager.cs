using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    [Header("���� ȭ�� ������Ʈ")]
    public GameObject statsUi;

    [Header("���� ���ݷ� �����ִ� �ؽ�Ʈ")]
    public Text attackDmgTxt;

    [Header("���� ��ų ���ݷ� �����ִ� �ؽ�Ʈ")]
    public Text skillDmgTxt;

    [Header("���� �ִ� ü�� �����ִ� �ؽ�Ʈ")]
    public Text maxHpTxt;

    [Header("���� ��ų ���� ���ð� �����ִ� �ؽ�Ʈ")]
    public Text skillCoolTxt;

    [Header("���� ���ݼӵ� �����ִ� �ؽ�Ʈ")]
    public Text attackSpeedTxt;

    [Header("���� ��ų����Ʈ �����ִ� �ؽ�Ʈ")]
    public Text skillPointTxt;

    // -------
    [Header("���� ���ݷ� �÷��� ��ư")]
    public Button attackDmgBtn;

    [Header("���� ��ų ���ݷ� �÷��� ��ư")]
    public Button skillDmgBtn;

    [Header("���� �ִ� ü�� �÷��� ��ư")]
    public Button maxHpBtn;

    [Header("���� ��ų ���� ���ð� �÷��� ��ư")]
    public Button skillCoolBtn;

    [Header("���� ���ݼӵ� �÷��� ��ư")]
    public Button attackSpeedBtn;

    [Header("����â �ݴ� ��ư")]
    public Button closeStatsUi;

    [Header("���� ������ Slot ��ũ��Ʈ")]
    public Slot equipItemSc;
    [Header("�÷��̾� ���� Text")]
    public Text lvTxt;
    [Header("�÷��̾� �г��� Text")]
    public Text playerNameTxt;

    [Header("�÷��̾� ����ġ ��")]
    public Image expBarImg;
    [Header("�÷��̾� ����ġ �ۼ�Ʈ�� �ؽ�Ʈ")]
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
        if (statsUi.activeSelf) //����â ������Ʈ�� Ȱ��ȭ �Ǹ�
        {
            attackDmgTxt.text = "���ݷ� : " + DataManager.instance.data.playerAttackDmg.ToString();
            skillDmgTxt.text = "��ų ���ݷ� : " + DataManager.instance.data.playerSkillDmg.ToString();
            maxHpTxt.text = "�ִ� ü�� : " + DataManager.instance.data.playerMaxHp.ToString();
            skillCoolTxt.text = "��ų ���� ���ð� ���� : " + (DataManager.instance.data.playerSkillCool*10).ToString();
            attackSpeedTxt.text = "ũ��Ƽ�� Ȯ�� : " + (DataManager.instance.data.playerCritical).ToString()+ "%";
            skillPointTxt.text = DataManager.instance.data.playerSkillPoint.ToString();
        }
    }
     
    void StatsOpen()
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);   //������ư ������ ���� ����
        lvTxt.text = "Lv.  " + DataManager.instance.data.playerLv.ToString();   //�÷��̾� ���� �ؽ�Ʈ �ݿ�
        playerNameTxt.text = DataManager.instance.data.playerNickName;  // �÷��̾� �г��� �ؽ�Ʈ�� �ݿ�
        equipItemSc.isCheckEquip = true;  //�������� ��ũ��Ʈ ���ڷ���
        expBarImg.fillAmount = DataManager.instance.data.playerNowExp / DataManager.instance.data.playerMaxExp; // �÷��̾� ����ġ �� ǥ��
        expGageTxt.text = (expBarImg.fillAmount * 100).ToString("F2") + "%";
    }

    void CloseStats()
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);
        UiManager.instance.toolTip.SetActive(false);    //���� ���� ������Ʈ ��Ȱ��ȭ
        equipItemSc.isCheckEquip = false; //���� �������� �����ϱ� ���� bool �ڷ��� false�� ��ȯ
        statsUi.SetActive(false);
    }

    void AttackPlus()   //���� ���� �ø��� �Լ�
    {
        if (DataManager.instance.data.playerSkillPoint > 0) //�÷��̾� ��ų ����Ʈ�� 0�� �ƴҶ�
        {
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[4]);   //��ų����Ʈ ��ư Ŭ���Ҷ� �������
            DataManager.instance.data.playerAttackDmg += 10;    //�÷��̾� ���ݷ� +10
            DataManager.instance.data.playerSkillPoint -= 1;    //�÷��̾� ��ų����Ʈ --
        }
    }

    void SkillDmgPlus()
    {
        if (DataManager.instance.data.playerSkillPoint > 0) //�÷��̾� ��ų ����Ʈ�� 0�� �ƴҶ�
        {
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[4]);
            DataManager.instance.data.playerSkillDmg += 10; //�÷��̾� ��ų������ +10
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
