using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("몬스터 프리팹")]
    public GameObject[] MonsterPref;
    [Header("현재 죽은 몬스터 수")]
    public int deadMonster = 0;
    [Header("현재 웨이브")]
    public int wave;
    [Header("1단계 웨이브 몬스터 수")]
    public int wave1monsterCnt;
    [Header("2단계 웨이브 몬스터 수")]
    public int wave2monsterCnt;
    [Header("3단계 웨이브 몬스터 수")]
    public int wave3monsterCnt;
    [Header("스테이지 클리어 체크하는 bool자료형")]
    public bool stageClear = false;

    int thisStage = 0;
    public GameObject settingOb;


    [HideInInspector]
    public Text waveTxt;    // 현재 웨이브 텍스트
    [HideInInspector]
    public Text secTimeTxt; // 스테이지에서 있던 초단위를 보여주는 텍스트
    [HideInInspector]
    public Text minTimeTxt; // 스테이지에서 있던 분단위를 보여주는 텍스트
    [HideInInspector]
    public Text waveMonsterTxt; //웨이브 몬스터 수를 보여주는 텍스트
    [HideInInspector]
    public Text deadMonsterTxt; //웨이브에서 죽은 몬스터 수를 보여주는 텍스트
    
    public Button settingBtn;   //설정 버튼
    
    public Button settingGoBackVillageBtn;  //마을로 돌아가기 버튼
    
    public Button closeSettingBtn;  //설정 닫기 버튼

    public Text stageExpTxt;
    public Text coinTxt;

    [HideInInspector]
    public Button retryBtn; // 해당 스테이지를 재시도하는 버튼
    [HideInInspector]
    public Button goHomeBtn;    //마을로 돌아가는 버튼

    [Header("스테이지 클리어시 활성화되는 팝업창 게임오브젝트")]
    public GameObject clearPopupOb;
    [Header("스테이지 클리어시 실패시 팝업창 게임오브젝트")]
    public GameObject failPopupOb;

    [SerializeField]
    Button failGohomeBtn;

    public GameObject playerOb;
    public float time;
    float min=0;    //시간담당 초 변수
    public float sec=0; //시간담당 분 변수

    string stageName;
    public CameraGameOverEffect CameraGameOverEffectSC; //메인 카메라 오브젝트

    public GameObject playerCanvas;
    public GameObject StageCanvas;

    void Start()
    {
        UiManager.instance.blindImageOb.SetActive(false);   //블라인드 이미지 비활성화
        AudioManager.instance.Sound_Bgm(AudioManager.instance.soundsBgm[2]);
        stageName = SceneManager.GetActiveScene().name;
        WaveMonsterResponse(stageName);
        retryBtn.onClick.AddListener(UiManager.instance.RetryStage);    
        goHomeBtn.onClick.AddListener(UiManager.instance.ComeBackHome);
        failGohomeBtn.onClick.AddListener(UiManager.instance.ComeBackHome); 
        settingBtn.onClick.AddListener(ClickSettingOpen);   
        closeSettingBtn.onClick.AddListener(ClickCloseSetting); 
        settingGoBackVillageBtn.onClick.AddListener(UiManager.instance.ComeBackHome);   
        playerOb=GameObject.FindObjectOfType<CharacterController>().gameObject; 
        UiManager.instance.InvenBtn.interactable = false;   //
        failPopupOb.SetActive(false); //
        CameraGameOverEffectSC = GameObject.FindObjectOfType<CameraGameOverEffect>();   //카메라 흑백화면 만드는 함수 실행
        playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
        wave = 1;
    }


    void Update()
    {
        if (!stageClear)    // 스테이지가 클리어 되기 전까지 시간체크
        {
            sec += Time.deltaTime;
            if (sec >= 60)
            {
                min++;
                sec = 0;
            }

            waveTxt.text = wave.ToString(); //현재 웨이브 단계
            minTimeTxt.text = min.ToString("00");   // 게임 실행 분 텍스트
            secTimeTxt.text = sec.ToString(":00");  //게임 실행 초 텍스트
            deadMonsterTxt.text = deadMonster.ToString();   //현재 처치한 몬스터 수 나타내는 텍스트

        }

    }
    void ClickSettingOpen() //설정버튼 클릭하면 실행되는 함수
    {
        settingOb.SetActive(true);  //설정 오브젝트 활성화
        Time.timeScale = 0; // 시간의 흐름을 멈춤
    }
    void ClickCloseSetting()    //설정팝업 닫는 버튼 클릭하면 실행되는 함수
    {
        settingOb.SetActive(false); //설정 오브젝트 비활성화
        Time.timeScale = 1; //시간이 다시 시작됨
    }
    #region * WaveMonsterResponse함수 : 스테이지의 웨이브마다 몬스터를 생성하는 함수
    public void WaveMonsterResponse(string sceneName)   //스테이지의 웨이브마다 몬스터를 생성하는 함수
    {
        switch (sceneName)
        {
            case "Stage_1":
                thisStage = 1;
                if (wave == 1)  //웨이브가 1단계라면
                {
                    waveMonsterTxt.text = "/ " + wave1monsterCnt.ToString();  //웨이브 1단계 몬스터 수 text에 반영
                    for (int i = 0; i < wave1monsterCnt; i++)
                    {
                        Instantiate(MonsterPref[0], new Vector3(Random.Range(-9, 12), 0, Random.Range(-6, 12)), Quaternion.identity);   //스테이지 내의 랜덤좌표 위치에 몬스터 생성
                    }
                }
                else if (wave == 2) //웨이브 2단계 몬스터 수 text에 반영
                {
                    waveMonsterTxt.text = "/ " + wave2monsterCnt.ToString();  //웨이브 1단계 몬스터 수 text에 반영
                    for (int i = 0; i < wave2monsterCnt; i++)
                    {
                        Instantiate(MonsterPref[0], new Vector3(Random.Range(-9, 12), 0, Random.Range(-6, 12)), Quaternion.identity);
                    }

                }
                else if (wave == 3) //웨이브 3단계 몬스터 수 text에 반영
                {
                    AudioManager.instance.Sound_Bgm(AudioManager.instance.soundsBgm[3]);    //보스 스테이지 사운드 실행
                    for (int i = 0; i < wave3monsterCnt; i++)
                    {
                        Instantiate(MonsterPref[1], Vector3.zero, Quaternion.identity);
                    }
                }
                break;


            case "Stage_2":
                thisStage = 2;
                if (wave == 1)  //웨이브가 1단계라면
                {
                    waveMonsterTxt.text = "/ " + wave1monsterCnt.ToString();  //웨이브 1단계 몬스터 수 text에 반영
                    for (int i = 0; i < wave1monsterCnt-2; i++)
                    {
                        Instantiate(MonsterPref[0], new Vector3(Random.Range(-9, 12), 0, Random.Range(-6, 12)), Quaternion.identity);   //스테이지 내의 랜덤좌표 위치에 몬스터 생성
                    }
                    for (int i = 0; i < wave1monsterCnt - 2; i++)
                    {
                        Instantiate(MonsterPref[1], new Vector3(Random.Range(-9, 12), 0, Random.Range(-6, 12)), Quaternion.identity);   //스테이지 내의 랜덤좌표 위치에 몬스터 생성
                    }
                }
                else if (wave == 2) //웨이브 2단계 몬스터 수 text에 반영
                {
                    waveMonsterTxt.text = "/ " + wave1monsterCnt.ToString();  //웨이브 1단계 몬스터 수 text에 반영

                    

                    for (int i = 0; i < wave1monsterCnt - 2; i++)
                    {
                        Instantiate(MonsterPref[0], new Vector3(Random.Range(-9, 12), 0, Random.Range(-6, 12)), Quaternion.identity);   //스테이지 내의 랜덤좌표 위치에 몬스터 생성
                    }
                    for (int i = 0; i < wave1monsterCnt - 2; i++)
                    {
                        Instantiate(MonsterPref[1], new Vector3(Random.Range(-9, 12), 0, Random.Range(-6, 12)), Quaternion.identity);   //스테이지 내의 랜덤좌표 위치에 몬스터 생성
                    }

                }
                else if (wave == 3) //웨이브 3단계 몬스터 수 text에 반영
                {
                    AudioManager.instance.Sound_Bgm(AudioManager.instance.soundsBgm[3]);    //보스 스테이지 사운드 실행
                    for (int i = 0; i < wave3monsterCnt; i++)
                    {
                        Instantiate(MonsterPref[2], Vector3.zero, Quaternion.identity);
                    }
                }
                break;

            default:
                break;
        }

    }

    #endregion

    #region
    public void WaveMonsterDeadCheck()  //웨이브 몬스터가 죽었는지 체크하는 함수
    {
        switch (wave)
        {
            case 1: //1단계 웨이브일때
                if (deadMonster == wave1monsterCnt) //죽은 몬스터 수와 웨이브 1단계 몬스터 수가 같아졌을때
                {
                    wave++;
                    deadMonster = 0;
                    waveMonsterTxt.text = "/ " + wave2monsterCnt.ToString();
                    WaveMonsterResponse(stageName);
                }
                break;
            case 2:
                if (deadMonster == wave2monsterCnt)
                {
                    print("두번쨰 웨이브 클리어");
                    wave++;
                    deadMonster = 0;
                    waveMonsterTxt.text = "/ " + wave3monsterCnt.ToString();
                    WaveMonsterResponse(stageName);
                }
                break;
            case 3:
                if (deadMonster == wave3monsterCnt)
                {
                    print("세번쨰 웨이브 클리어");
                    //wave = 1;
                    deadMonster = 0;
                    time = 0;
                    stageClear = true;

                    Invoke("WaveClearEnding", 5f);
                }
                break;
        }

    }
    #endregion

    void WaveClearEnding()   //웨이브 클리어시 실행되는 함수
    {
        clearPopupOb.SetActive(true);   //스테이지 클리어 팝업창 활성화
        Animator anim =playerOb.GetComponent<Animator>();
        anim.SetBool("isClearStage", stageClear); //플레이어가 클리어한 애니메이션 실행되게 매개변수로 bool자료형 넣고 함수 실행
        float stageExp = GameManager.instance.roundMonsterExp;  // 해당 스테이지에서 얻은 경험치 값
        stageExpTxt.text = "+ " + stageExp.ToString("F2") + " Exp"; //해당 스테이지에서 얻은 경험치 값 text에 반영
        coinTxt.text = "+ " + GameManager.instance.roundCoin.ToString() + " 코인"; //해당 스테이지에서 얻은 코인 값 text에 반영
        DataManager.instance.data.playerNowExp += GameManager.instance.roundMonsterExp; //플레이어 현재 경험치 값에 라운드에서 얻은 경험치값 더해줌
        DataManager.instance.data.playerMoney += GameManager.instance.roundCoin;    //플레이어가 갖고있는 코인값에 라운드에서 얻은 코인값 더해줌
        GameManager.instance.roundCoin = 0; //라운드코인 수 0으로 변경
        GameManager.instance.roundMonsterExp = 0;   //라운드 경험치 값 0으로 변경
        if (thisStage > DataManager.instance.data.clearStage)
        {
            DataManager.instance.data.clearStage += 1;
        }
        GameManager.instance.LevelUp(); // GameManager의 레벨업함수 실행
    }

    public void WaveFailEnding()
    {
        CameraGameOverEffectSC.gameOverCameraEffect();
        
        playerCanvas.SetActive(false);
        StageCanvas.SetActive(false);
        StartCoroutine(FailStage());
    }
    IEnumerator FailStage()
    {
        yield return new WaitForSeconds(7);
        UiManager.instance.blindImageOb.SetActive(true);
        CameraGameOverEffectSC.message.SetActive(false);   
        CameraGameOverEffectSC.grayScale = 0;
        playerCanvas.SetActive(true);
        StageCanvas.SetActive(true);
        UiManager.instance.ComeBackHome();

    }
}
