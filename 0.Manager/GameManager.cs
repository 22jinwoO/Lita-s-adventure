using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("플레이어 경험치 텍스트")]
    public Text playerLvTxt;
    [Header("라운드 몬스터 경험치")]
    public float roundMonsterExp=0;

    [Header("라운드 코인")]
    public int roundCoin = 0;

    static public GameManager instance;
    
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
            //DataManager.instance.Load();
        }
        else if (instance != this)
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            Destroy(this.gameObject);
        }
        

    }
   
    void Start()
    {
        AudioManager.instance.Sound_Bgm(AudioManager.instance.soundsBgm[1]);    //bgm사운드 실행
    }


    void Update()
    {

    }
    public void LevelUp()
    {
        if (DataManager.instance.data.playerNowExp>= DataManager.instance.data.playerMaxExp) //DataManager의 data클래스 playerNowExp 값이 DataManager.instance.data.playerMaxExp보다 크거나 같을때
        {
            DataManager.instance.data.playerLv++;   //플레이어 레벨 +1
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[0]);  //레벨업 사운드 실행
            playerLvTxt.text="Lv.  "+DataManager.instance.data.playerLv.ToString();   // 플레이어 레벨 text에 반영
            DataManager.instance.data.playerNowExp -= DataManager.instance.data.playerMaxExp;   //플레이어 현재 경험치 값에 플레이어 현재 경험치 값-플레이어 레벨업에 필요한 경험치 뺸값 넣어줌
            DataManager.instance.data.playerMaxExp += 100;  //플레이어 레벨업에 필요한 경험치 +100
            DataManager.instance.data.playerMaxHp += 50;    //플레이어 최대체력 +50
            DataManager.instance.data.nowPlayerHp = DataManager.instance.data.playerMaxHp; //플레이어 현재체력값이 최대체력값만큼 상승
            DataManager.instance.data.playerAttackDmg += 5; //플레이어 공격력 +5
            DataManager.instance.data.playerSkillDmg += 5;  //플레이어 스킬 공격력 +5
            DataManager.instance.data.playerSkillPoint += 6;    //플레이어 스킬포인트 +6
        }
    }
}
