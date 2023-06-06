using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayer : MonoBehaviour
{
    
    public Animator anim;  // 플레이어 애니메이션을 위해 가져옴
    public Skill skillSc;  // 플레이어 스킬 스크립트

    [Header("플레이어 조이스틱(이동관련)")]
    public PlayerJoystic joysticSc;
    CharacterController cc; //캐릭터 컨트롤러(유저)

    [Header("플레이어 이동값")]
    public Vector3 moveVec;   //유저 이동값

    [Header("플레이어 이동속도")]
    public float playerMoveSpeed; //플레이어 이동속도

    [Header("플레이어가 동작할 수 있나 판단하는 bool자료형")]
    public bool canInput = true;  //    플레이어가 동작할 수 있나 판단하는 bool자료형


    [Header("플레이어 공격, 공격 쿨타임 관련")]
    public Button attackBtn;
    public Image atkCool;
    public float atkSpeed;   //공격속도
    public float atkDelay;   // 공격딜레이
    public bool isCanAttack = false; // 공격키 입력가능

    [Header("플레이어 대쉬, 대쉬 쿨타임 관련")]
    public Button dash_Btn;
    public Image dash_Cool;
    public float dashDelay = 5f; //대쉬 딜레이
    public bool isCanDash = false;   // 대쉬사용이 가능할때마다 표시되는 bool자료형
    //
    [Header("플레이어 무적상태 체크")]
    public bool isDamaged=false;
    
    [HideInInspector]
    public float gravity = -3f; //게임의 중력값

    [Header("플레이어 메쉬렌더러 배열")]
    public SkinnedMeshRenderer[] meshs;

    [Header("플레이어 체력바 슬라이더")]
    public Image playerHpImg;
    public Slider playerHpbar;

    [Header("플레이어 체력 텍스트")]
    public Text playerHpTxt;

    [Header("플레이어 레벨 텍스트")]
    public Text levelTxt;

    [Header("플레이어 장착 아이템")]
    public Item equipWeapon;

    public GameObject dashEffect;
    private void Awake()
    {
        cc = GetComponent<CharacterController>();   //캐릭터 컨트롤러 가져오기
        anim = GetComponent<Animator>();    //애니메이터 가져오기
        skillSc = GetComponent<Skill>();    // 플레이어의 skill스크립트 가져오기
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();

    }

    void Start()
    {
        dash_Btn.onClick.AddListener(() => StartCoroutine("Dash")); // 대쉬 버튼에 스타트코루틴 함수에 Dash을 string 자료형의 매개변수로 전달
        levelTxt.text = "Lv " + DataManager.instance.data.playerLv.ToString();
        DataManager.instance.data.nowPlayerHp = DataManager.instance.data.playerMaxHp;
    }


    void Update()
    {
        
        playerHpTxt.text = DataManager.instance.data.nowPlayerHp.ToString() + "/" + DataManager.instance.data.playerMaxHp;
        playerHpImg.fillAmount = DataManager.instance.data.nowPlayerHp / DataManager.instance.data.playerMaxHp;

        anim.SetFloat("multipleJoyStickDistance", joysticSc.joysticDistance);
        if (atkDelay < 1.6f - DataManager.instance.data.attackSpeed)  //atkDelay가 1.6f보다 작을때
        {
            atkDelay += Time.deltaTime; //atkDelay 변수에 매프레임 시간을 더해주고

            isCanAttack = atkDelay > 1.6f - DataManager.instance.data.attackSpeed; //atkDelay가 1.6f 보다 크면 inCanAttack값 true로 변경
        }
        if (isCanAttack)   //inCanAttack값 true일때
        {
            attackBtn.interactable = true;
        }
        DoDash();   //대쉬함수

        dash_Cool.fillAmount = dashDelay / (10 - DataManager.instance.data.playerSkillCool); //대쉬쿨 딜레이동안 매프레임마다 바뀌는 대쉬쿨 슬라이더 밸류
        atkCool.fillAmount = atkDelay / (1.6f - DataManager.instance.data.attackSpeed);   //기본 공격 대기 딜레이동안 매프레임마다 바뀌는 어택쿨 슬라이더 밸류

        if (canInput&&joysticSc.isTouch)   //canInput이 true 일때 플레이어가 움직일 수 있음
        {
            Move(); //플레이어가 움직일 수 있는 기능을 하는 Move()함수 호출
        }
        else
        {
            moveVec = new Vector3(0, 0, 0);
            float moveSpeed = moveVec.magnitude;    //플레이어 이동값의 크기를 구함
            anim.SetFloat("move", moveSpeed);
            
            
        }
        if (!cc.isGrounded) // 플레이어가 땅에 있지 않을때
        {
            moveVec.y += gravity;   //플레이어 이동값의 y축의 -3을 계속 더해줌
        }
        else if (cc.isGrounded)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            
        }
        if (cc.enabled!=false)
        {
            cc.Move(moveVec * playerMoveSpeed * joysticSc.joysticDistance * Time.deltaTime);  //플레이어 이동
        }
        



    }

    #region * Move함수 - 플레이어의 움직이는 기능을 맡음  - > 플레이어 업데이트 함수에서 호출
    /// <summary>
    /// 캐릭터 컨트롤러로 플레이어 움직임 구현
    /// 플레이어가 대각선으로 이동할 때도 움직임 속도가 앞뒤 좌우 이동속도와 같게 하기 위해 정규화 사용
    /// 플레이어 이동속도 값의 크기를 magnitude로 구하여 이동속도 값에 따라 플레이어의 뛰는 애니메이션이 동작하도록 설정
    /// </summary>
    void Move()
    {
        moveVec = new Vector3(joysticSc.value.x, 0, joysticSc.value.y);  //플레이어 이동값 정규화

        if (!AudioManager.instance.audioWalkkSound.isPlaying)
        {
            AudioManager.instance.audioWalkkSound.pitch = joysticSc.joysticDistance + 0.5f;
            AudioManager.instance.Sound_Walk(AudioManager.instance.soundsWalk[Random.Range(0, 6)]);
            

        }
        transform.LookAt(transform.position + moveVec); //플레이어가 이동한 방향 쳐다볼 수 있게함

        float moveSpeed = moveVec.magnitude;    //플레이어 이동값의 크기를 구함
        anim.SetFloat("move", moveSpeed);

        

        
    }
    #endregion

    #region * Attack함수 - isCanAttack이 true일때 canInput이 false가 되고 주변에 몬스터를 탐색하는 skillSc.MonsterCheck()함수를 호출
    public void Attack()   //유저 공격
    {
        if (isCanAttack)      //isCanAttack 
        {
            canInput = false;   //플레이어가 움직일 수 있게 구분지어주는 canInput bool 자료형에 false값 입력하여 공격상태시 플레이어가 못움직이도록 만듦
            attackBtn.interactable=false; // if 조건문이 트루일동안 attack버튼 오브젝트 비활성화
            AudioManager.instance.Sound_Attack(AudioManager.instance.soundsAttack[Random.Range(1,7)]);
            skillSc.MonsterCheck(); //Skill 스크립트의 MonsterCheck함수 호출
        }
    }
    #endregion

    #region DoDash 함수- dashDelay으로 인해 isCanDash가 true가 되도록 만듦
    /// <summary>
    /// 플레이어가 대쉬를 할 수 있도록 담당하는 함수
    /// 일정기준 쿨타임값보다 작으면 dashDelay변수에 time.DeltaTime값을 계속더해주다가 일정값보다 dashDelay가 커지면
    /// bool 자료형 값의 isCanDash 를 true로 바꿔주고 대쉬버튼 오브젝트를 활성화 시켜줌
    /// </summary>
    void DoDash()
    {
        if (10f - DataManager.instance.data.playerSkillCool > dashDelay)    //유저 대쉬 딜레이속도가 2f보다 값이 작으면
        {
            
            dashDelay += Time.deltaTime; //dashDelayrkqtdp time.deltaTime 값을 계속 더해주고
            isCanDash = 10f - DataManager.instance.data.playerSkillCool < dashDelay; //dashDelay값이 10f보다 커지면 isCanDash bool 자료형을 true로 만들어줌
        }
        
        if (isCanDash)
        {
            dash_Btn.interactable=true;
        }
    }

    #endregion

    #region * Dash 코루틴 함수 - 대쉬버튼 클릭 시 호출되는 함수
    /// <summary>
    /// 호출 시 Dash버튼이 비활성화 되고 canINput이 false가 되어 입력이 불가능해짐 캐릭터가 바라보는 방향으로 1초 동안 이동함
    /// </summary>
    /// <returns></returns>
    IEnumerator Dash()  //대쉬 코루틴 함수
    {
        dashEffect.SetActive(true); //대쉬 이펙트 게임오브젝트 활성화
        dash_Btn.interactable = false;  //대쉬 버튼 비활성화
        skillSc.isSkillDoing = true;    // 스킬 사용중인거 확인하는 bool자료형
        AudioManager.instance.Sound_Attack(AudioManager.instance.soundsAttack[0]);
        canInput = false;   // 플레이어 이동 불가
        dashDelay = 0;
        anim.SetTrigger("dash");    //대쉬 애니메이션 실행
        playerMoveSpeed *= 2.5f;
        float time = 0;
        
        while (time<1f) //time 이 1초 보다 작을때 동안
        {
            time += Time.deltaTime;
            cc.Move(transform.forward * playerMoveSpeed * Time.deltaTime); // 플레이어 z축방향으로 이동
            isDamaged = true;
            yield return null;

        }


        dashEffect.SetActive(false);
        canInput = true;    //플레이어 이동 가능
        skillSc.isSkillDoing = false;
        isDamaged = false;
        playerMoveSpeed *= 0.4f;
    }

    #endregion

    #region * RealAttack()함수 - 공격애니메이션이 활성화되고 atkDelay값 0으로 만듦
    /// <summary>
    /// 
    /// </summary>
    public void RealAttack()    //플레이어 공격 애니메이션 실행하는 함수
    {
        anim.SetTrigger("attack");    // 공격 애니메이션 활성화
        atkDelay = 0;   //공격 딜레이 0
        
    }
    #endregion

    #region * Damaged함수 - 플레이어가 몬스터에게 공격당했을때마다 실행되는 함수(몬스터 공격 범위에 닿았을떄마다 호출)
    IEnumerator Damaged(float monsterDmg)
    {
        isDamaged = true;   //플레이어가 타격을 입었는지 확인하는 bool자료형
        anim.SetTrigger("triggerDamaged");
        foreach (SkinnedMeshRenderer playerPart in meshs)  //피격시 플레이어 캐릭터 색깔 바뀜
        {
            playerPart.material.color = Color.yellow;   //타격시 스킨메쉬렌더러 Material color yellow로
        }
        DataManager.instance.data.nowPlayerHp -= monsterDmg;
        if (DataManager.instance.data.nowPlayerHp <= 0)
        {
            AudioManager.instance.Sound_Attack(AudioManager.instance.soundsAttack[7]);  //게임오버 사운드 실행
            AudioManager.instance.audioWalkkSound.Stop();
            anim.SetFloat("move", 0);   //이동 애니메이션 재생 X
            joysticSc.rect_Joystick.position = joysticSc.rect_Joystick.parent.position; //하얀색 조이스틱 위치 원래 부모 위치로 설정
            //**플레이어 이동값 0
            joysticSc.value.x = 0;
            joysticSc.value.y = 0;
            moveVec = new Vector3(0, 0, 0);
            //*
            anim.SetBool("isDead", true);   // 플레이어 죽음 애니메이션 실행
            joysticSc.isTouch = false;  //플레이어가 조이스틱 터치중인지 확인하는 bool자료형 값 false 로 지정
            joysticSc.enabled = false;  //플레이어 조이스틱 스크립트 비활성화
            gameObject.GetComponent<NewPlayer>().enabled = false;   //플레이어 스크립트 비활성화
            skillSc.enabled = false;    //플레이어 스킬스크립트 비활성화
            cc.enabled = false; //캐릭터 컨트롤러 컴포넌트 비활성화
            GameObject.FindObjectOfType<StageManager>().WaveFailEnding();   // 웨이브 실패했을때 뜨는 팝업창 활성화 시키는 함수 실행
        }
        //넉백 만들기
        yield return new WaitForSeconds(1.3f);

        foreach (SkinnedMeshRenderer playerPart in meshs)  //피격 후 플레이어 캐릭터 색깔 복귀
        {
            playerPart.material.color = Color.white;
        }
        isDamaged = false;
    }
#endregion

    void ClearStage(bool clear) //스테이지 클리어 했을때 실행되는 함수
    {
        anim.SetBool("isClearStage",clear);
    }
}

