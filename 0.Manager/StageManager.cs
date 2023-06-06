using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("���� ������")]
    public GameObject[] MonsterPref;
    [Header("���� ���� ���� ��")]
    public int deadMonster = 0;
    [Header("���� ���̺�")]
    public int wave;
    [Header("1�ܰ� ���̺� ���� ��")]
    public int wave1monsterCnt;
    [Header("2�ܰ� ���̺� ���� ��")]
    public int wave2monsterCnt;
    [Header("3�ܰ� ���̺� ���� ��")]
    public int wave3monsterCnt;
    [Header("�������� Ŭ���� üũ�ϴ� bool�ڷ���")]
    public bool stageClear = false;

    int thisStage = 0;
    public GameObject settingOb;


    [HideInInspector]
    public Text waveTxt;    // ���� ���̺� �ؽ�Ʈ
    [HideInInspector]
    public Text secTimeTxt; // ������������ �ִ� �ʴ����� �����ִ� �ؽ�Ʈ
    [HideInInspector]
    public Text minTimeTxt; // ������������ �ִ� �д����� �����ִ� �ؽ�Ʈ
    [HideInInspector]
    public Text waveMonsterTxt; //���̺� ���� ���� �����ִ� �ؽ�Ʈ
    [HideInInspector]
    public Text deadMonsterTxt; //���̺꿡�� ���� ���� ���� �����ִ� �ؽ�Ʈ
    
    public Button settingBtn;   //���� ��ư
    
    public Button settingGoBackVillageBtn;  //������ ���ư��� ��ư
    
    public Button closeSettingBtn;  //���� �ݱ� ��ư

    public Text stageExpTxt;
    public Text coinTxt;

    [HideInInspector]
    public Button retryBtn; // �ش� ���������� ��õ��ϴ� ��ư
    [HideInInspector]
    public Button goHomeBtn;    //������ ���ư��� ��ư

    [Header("�������� Ŭ����� Ȱ��ȭ�Ǵ� �˾�â ���ӿ�����Ʈ")]
    public GameObject clearPopupOb;
    [Header("�������� Ŭ����� ���н� �˾�â ���ӿ�����Ʈ")]
    public GameObject failPopupOb;

    [SerializeField]
    Button failGohomeBtn;

    public GameObject playerOb;
    public float time;
    float min=0;    //�ð���� �� ����
    public float sec=0; //�ð���� �� ����

    string stageName;
    public CameraGameOverEffect CameraGameOverEffectSC; //���� ī�޶� ������Ʈ

    public GameObject playerCanvas;
    public GameObject StageCanvas;

    void Start()
    {
        UiManager.instance.blindImageOb.SetActive(false);   //����ε� �̹��� ��Ȱ��ȭ
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
        CameraGameOverEffectSC = GameObject.FindObjectOfType<CameraGameOverEffect>();   //ī�޶� ���ȭ�� ����� �Լ� ����
        playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
        wave = 1;
    }


    void Update()
    {
        if (!stageClear)    // ���������� Ŭ���� �Ǳ� ������ �ð�üũ
        {
            sec += Time.deltaTime;
            if (sec >= 60)
            {
                min++;
                sec = 0;
            }

            waveTxt.text = wave.ToString(); //���� ���̺� �ܰ�
            minTimeTxt.text = min.ToString("00");   // ���� ���� �� �ؽ�Ʈ
            secTimeTxt.text = sec.ToString(":00");  //���� ���� �� �ؽ�Ʈ
            deadMonsterTxt.text = deadMonster.ToString();   //���� óġ�� ���� �� ��Ÿ���� �ؽ�Ʈ

        }

    }
    void ClickSettingOpen() //������ư Ŭ���ϸ� ����Ǵ� �Լ�
    {
        settingOb.SetActive(true);  //���� ������Ʈ Ȱ��ȭ
        Time.timeScale = 0; // �ð��� �帧�� ����
    }
    void ClickCloseSetting()    //�����˾� �ݴ� ��ư Ŭ���ϸ� ����Ǵ� �Լ�
    {
        settingOb.SetActive(false); //���� ������Ʈ ��Ȱ��ȭ
        Time.timeScale = 1; //�ð��� �ٽ� ���۵�
    }
    #region * WaveMonsterResponse�Լ� : ���������� ���̺긶�� ���͸� �����ϴ� �Լ�
    public void WaveMonsterResponse(string sceneName)   //���������� ���̺긶�� ���͸� �����ϴ� �Լ�
    {
        switch (sceneName)
        {
            case "Stage_1":
                thisStage = 1;
                if (wave == 1)  //���̺갡 1�ܰ���
                {
                    waveMonsterTxt.text = "/ " + wave1monsterCnt.ToString();  //���̺� 1�ܰ� ���� �� text�� �ݿ�
                    for (int i = 0; i < wave1monsterCnt; i++)
                    {
                        Instantiate(MonsterPref[0], new Vector3(Random.Range(-9, 12), 0, Random.Range(-6, 12)), Quaternion.identity);   //�������� ���� ������ǥ ��ġ�� ���� ����
                    }
                }
                else if (wave == 2) //���̺� 2�ܰ� ���� �� text�� �ݿ�
                {
                    waveMonsterTxt.text = "/ " + wave2monsterCnt.ToString();  //���̺� 1�ܰ� ���� �� text�� �ݿ�
                    for (int i = 0; i < wave2monsterCnt; i++)
                    {
                        Instantiate(MonsterPref[0], new Vector3(Random.Range(-9, 12), 0, Random.Range(-6, 12)), Quaternion.identity);
                    }

                }
                else if (wave == 3) //���̺� 3�ܰ� ���� �� text�� �ݿ�
                {
                    AudioManager.instance.Sound_Bgm(AudioManager.instance.soundsBgm[3]);    //���� �������� ���� ����
                    for (int i = 0; i < wave3monsterCnt; i++)
                    {
                        Instantiate(MonsterPref[1], Vector3.zero, Quaternion.identity);
                    }
                }
                break;


            case "Stage_2":
                thisStage = 2;
                if (wave == 1)  //���̺갡 1�ܰ���
                {
                    waveMonsterTxt.text = "/ " + wave1monsterCnt.ToString();  //���̺� 1�ܰ� ���� �� text�� �ݿ�
                    for (int i = 0; i < wave1monsterCnt-2; i++)
                    {
                        Instantiate(MonsterPref[0], new Vector3(Random.Range(-9, 12), 0, Random.Range(-6, 12)), Quaternion.identity);   //�������� ���� ������ǥ ��ġ�� ���� ����
                    }
                    for (int i = 0; i < wave1monsterCnt - 2; i++)
                    {
                        Instantiate(MonsterPref[1], new Vector3(Random.Range(-9, 12), 0, Random.Range(-6, 12)), Quaternion.identity);   //�������� ���� ������ǥ ��ġ�� ���� ����
                    }
                }
                else if (wave == 2) //���̺� 2�ܰ� ���� �� text�� �ݿ�
                {
                    waveMonsterTxt.text = "/ " + wave1monsterCnt.ToString();  //���̺� 1�ܰ� ���� �� text�� �ݿ�

                    

                    for (int i = 0; i < wave1monsterCnt - 2; i++)
                    {
                        Instantiate(MonsterPref[0], new Vector3(Random.Range(-9, 12), 0, Random.Range(-6, 12)), Quaternion.identity);   //�������� ���� ������ǥ ��ġ�� ���� ����
                    }
                    for (int i = 0; i < wave1monsterCnt - 2; i++)
                    {
                        Instantiate(MonsterPref[1], new Vector3(Random.Range(-9, 12), 0, Random.Range(-6, 12)), Quaternion.identity);   //�������� ���� ������ǥ ��ġ�� ���� ����
                    }

                }
                else if (wave == 3) //���̺� 3�ܰ� ���� �� text�� �ݿ�
                {
                    AudioManager.instance.Sound_Bgm(AudioManager.instance.soundsBgm[3]);    //���� �������� ���� ����
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
    public void WaveMonsterDeadCheck()  //���̺� ���Ͱ� �׾����� üũ�ϴ� �Լ�
    {
        switch (wave)
        {
            case 1: //1�ܰ� ���̺��϶�
                if (deadMonster == wave1monsterCnt) //���� ���� ���� ���̺� 1�ܰ� ���� ���� ����������
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
                    print("�ι��� ���̺� Ŭ����");
                    wave++;
                    deadMonster = 0;
                    waveMonsterTxt.text = "/ " + wave3monsterCnt.ToString();
                    WaveMonsterResponse(stageName);
                }
                break;
            case 3:
                if (deadMonster == wave3monsterCnt)
                {
                    print("������ ���̺� Ŭ����");
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

    void WaveClearEnding()   //���̺� Ŭ����� ����Ǵ� �Լ�
    {
        clearPopupOb.SetActive(true);   //�������� Ŭ���� �˾�â Ȱ��ȭ
        Animator anim =playerOb.GetComponent<Animator>();
        anim.SetBool("isClearStage", stageClear); //�÷��̾ Ŭ������ �ִϸ��̼� ����ǰ� �Ű������� bool�ڷ��� �ְ� �Լ� ����
        float stageExp = GameManager.instance.roundMonsterExp;  // �ش� ������������ ���� ����ġ ��
        stageExpTxt.text = "+ " + stageExp.ToString("F2") + " Exp"; //�ش� ������������ ���� ����ġ �� text�� �ݿ�
        coinTxt.text = "+ " + GameManager.instance.roundCoin.ToString() + " ����"; //�ش� ������������ ���� ���� �� text�� �ݿ�
        DataManager.instance.data.playerNowExp += GameManager.instance.roundMonsterExp; //�÷��̾� ���� ����ġ ���� ���忡�� ���� ����ġ�� ������
        DataManager.instance.data.playerMoney += GameManager.instance.roundCoin;    //�÷��̾ �����ִ� ���ΰ��� ���忡�� ���� ���ΰ� ������
        GameManager.instance.roundCoin = 0; //�������� �� 0���� ����
        GameManager.instance.roundMonsterExp = 0;   //���� ����ġ �� 0���� ����
        if (thisStage > DataManager.instance.data.clearStage)
        {
            DataManager.instance.data.clearStage += 1;
        }
        GameManager.instance.LevelUp(); // GameManager�� �������Լ� ����
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
