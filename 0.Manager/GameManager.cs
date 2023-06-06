using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("�÷��̾� ����ġ �ؽ�Ʈ")]
    public Text playerLvTxt;
    [Header("���� ���� ����ġ")]
    public float roundMonsterExp=0;

    [Header("���� ����")]
    public int roundCoin = 0;

    static public GameManager instance;
    
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
            //DataManager.instance.Load();
        }
        else if (instance != this)
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
            //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
            //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ�(���ο� ���� GameMgr)�� �������ش�.
            Destroy(this.gameObject);
        }
        

    }
   
    void Start()
    {
        AudioManager.instance.Sound_Bgm(AudioManager.instance.soundsBgm[1]);    //bgm���� ����
    }


    void Update()
    {

    }
    public void LevelUp()
    {
        if (DataManager.instance.data.playerNowExp>= DataManager.instance.data.playerMaxExp) //DataManager�� dataŬ���� playerNowExp ���� DataManager.instance.data.playerMaxExp���� ũ�ų� ������
        {
            DataManager.instance.data.playerLv++;   //�÷��̾� ���� +1
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[0]);  //������ ���� ����
            playerLvTxt.text="Lv.  "+DataManager.instance.data.playerLv.ToString();   // �÷��̾� ���� text�� �ݿ�
            DataManager.instance.data.playerNowExp -= DataManager.instance.data.playerMaxExp;   //�÷��̾� ���� ����ġ ���� �÷��̾� ���� ����ġ ��-�÷��̾� �������� �ʿ��� ����ġ �A�� �־���
            DataManager.instance.data.playerMaxExp += 100;  //�÷��̾� �������� �ʿ��� ����ġ +100
            DataManager.instance.data.playerMaxHp += 50;    //�÷��̾� �ִ�ü�� +50
            DataManager.instance.data.nowPlayerHp = DataManager.instance.data.playerMaxHp; //�÷��̾� ����ü�°��� �ִ�ü�°���ŭ ���
            DataManager.instance.data.playerAttackDmg += 5; //�÷��̾� ���ݷ� +5
            DataManager.instance.data.playerSkillDmg += 5;  //�÷��̾� ��ų ���ݷ� +5
            DataManager.instance.data.playerSkillPoint += 6;    //�÷��̾� ��ų����Ʈ +6
        }
    }
}
