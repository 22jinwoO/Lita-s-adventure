using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("몬스터가 땅에 있는지 확인하는 불자료형")]
    public bool isGround;

    [Header("몬스터 정보(스크랩터블 오브젝트)")]
    public MonsterInfo monsterInfo;
    //몬스터 Hp바
    public Slider monsterHpBar;

    [Header("몬스터 현재 Hp")]
    public float monsterHp;

    public StageManager waveSc;

    [Header("몬스터가 살았는지 죽었는지 체크하는 bool자료형")]
    public bool monsterDead=false;

    [Header("플레이어 오브젝트")]
    protected GameObject playerOb;

    [Header("플레이어 무적판정 bool자료형")]
    public bool isDamaged;

    [Header("코인프리팹")]
    public GameObject coinPref;

    [Header("슬라임 몬스터 히트 범위")]
    public BoxCollider slimeHitArea;
    
    [Header("돌격형 몬스터 히트 범위")]
    public BoxCollider rushMonsterHitArea;

    [Header("원거리 몬스터 투사체 게임오브젝트 프리팹")]
    public GameObject monsterBulletPref;


    [Header("몬스터 공격 가능 확인 bool자료형")]
    public bool isAttack= true;

    [Header("플레이어와의 거리 flaot 변수")]
    public float playerDistance;

    NavMeshAgent nav;   //네비메쉬 에이전트 변수
    
    public Animator anim;
    [Header("몬스터 공격 쿨타임 변수")]
    public float attackCool = 0;    //몬스터 공격 쿨타임 변수
    bool isCollision;
    
    public enum monsterState    //몬스터 상태 구조체
    {
        Idle,
        Walk,
        Attack,
    }

    public enum monsterType //몬스터 종류에 따라서
    {
        Normal,
        Rush,
        Shoot,
    }
    public monsterType myMonsterType;

    [Header("몬스터 현재 상태")]
    public monsterState state = monsterState.Idle;

    [Header("몬스터 데미지 텍스트")]
    public GameObject DamageTxtPref;

    [Header("몬스터 데미지 텍스트 트랜스폼")]
    public Transform DmgTextTr;

    Vector3 deadps;
    public Rigidbody rigd;

    private void OnDisable()
    {
        Invoke("DropCoin", 1);
        Destroy(gameObject, 2);
        
    }
    void Start()
    {
        // ** 필요한 값들 가져오기

        monsterHpBar=GetComponentInChildren<Slider>();  //몬스터 체력바 슬라이더 가져오기
        monsterHpBar.maxValue = monsterInfo.monsterMaxHp;   //monsterHpBar슬라이더의 maxValue에 monsterInfo 아이템 스크립트의 monsterMaxHp값 적용
        monsterHp = monsterInfo.monsterMaxHp;   //monsterInfo 아이템 스크립트의 값 적용
        monsterHpBar.value = monsterHp; //monsterInfo 아이템 스크립트의 값 적용
        gameObject.name = monsterInfo.monsterName;  //monsterInfo 아이템 스크립트의 값 적용
        waveSc = GameObject.Find("StageManager").GetComponent<StageManager>();
        playerOb = GameObject.FindObjectOfType<CharacterController>().gameObject;

        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nav.speed = monsterInfo.monsterSpeed;

        rigd = GetComponent<Rigidbody>();
        // *
    }

    void Update()
    {
        playerDistance=Vector3.Distance(transform.position, playerOb.transform.position);   //에너미 오브젝트와 플레이어 오브젝트간의 거리값 넣어주기
        anim.SetFloat("monsterVelocity", nav.velocity.magnitude);   //몬스터 이동값 넣어주기

        switch (myMonsterType)  //몬스터 타입에 따라 실행
        {
            case monsterType.Normal:    //일반 몬스터 일때
                switch (state) // 몬스터 FSM
                {
                    case monsterState.Idle: //몬스터 상태가 Idle일때
                        nav.isStopped = true;   //몬스터 이동 멈추기
                        if (playerDistance <= 6)    //플레이어와의 거리가 6 이하일때
                        {
                            state = monsterState.Walk; // 몬스터 상태 Walk로 바뀜
                        }
                        break;

                    case monsterState.Walk: //walk 상태일때
                        Walk(); //Walk함수 실행
                        break;

                    case monsterState.Attack:
                        StartCoroutine(Attack());
                        break;
                }
                break;
            case monsterType.Rush:  //돌격형 몬스터이면
                switch (state)
                {
                    case monsterState.Idle:
                        anim.SetBool("isWalk", false);  // iswalk 애니메이션 실행x
                        nav.isStopped = true;   //몬스터 이동 멈추기
                        if (playerDistance <= 6)    //플레이어와의 거리가 6 이하일때
                        {
                            state = monsterState.Walk; // 몬스터 상태 Walk로 바뀜
                        }
                        break;
                    case monsterState.Walk:
                        Walk(); //Walk함수 실행
                        break;
                    case monsterState.Attack:
                        StartCoroutine(Attack());
                        break;
                }
                break;
            case monsterType.Shoot:
                switch (state)
                {
                    case monsterState.Idle:
                        nav.isStopped = true;   //몬스터 이동 멈추기
                        if (playerDistance <= 7)    //플레이어와의 거리가 7 이하일때
                        {
                            state = monsterState.Walk; // 몬스터 상태 Walk로 바뀜
                        }
                        break;
                    case monsterState.Walk:
                        Walk(); //Walk함수 실행
                        break;
                    case monsterState.Attack:
                        StartCoroutine(Attack());
                        break;
                }
                break;
        }
        

    }

    void Walk() //몬스터 플레이어 방향으로 이동하는 함수
    {
        switch (myMonsterType)
        {
            case monsterType.Normal:
                if (playerDistance > 6) // 플레이어와의 거리가 6보다 크면
                {
                    state = monsterState.Idle;  // 몬스터 상태 Idle로 변환
                }
                else if (playerDistance <= 2)   //플레이어와의 거리가 2같거나 작으면
                {
                    state = monsterState.Attack;    //몬스터 상태 Attack으로 변환
                }
                else// 플레이어와의 거리가 6보다 작거나 같으면
                {
                    nav.SetDestination(playerOb.transform.position);    //몬스터 네비게이션 목적지 플레이어 위치로 설정
                    nav.isStopped = false;
                }
                break;
            case monsterType.Rush:
                if (playerDistance > 7) // 플레이어와의 거리가 7보다 크면
                {
                    state = monsterState.Idle;  // 몬스터 상태 Idle로 변환
                }
                else if (playerDistance <= 5)   //플레이어와의 거리가 4같거나 작으면
                {
                    transform.LookAt(playerOb.transform);   //몬스터 상태가 attack일땐 플레이어 바라보도록 설정
                    state = monsterState.Attack;    //몬스터 상태 Attack으로 변환
                }
                else// 플레이어와의 거리가 7보다 작거나 같으면
                {
                    nav.SetDestination(playerOb.transform.position);    //몬스터 네비게이션 목적지 플레이어 위치로 설정
                    nav.isStopped = false;
                }
                break;
            case monsterType.Shoot:
                if (playerDistance > 8) // 플레이어와의 거리가 8보다 크면
                {
                    state = monsterState.Idle;  // 몬스터 상태 Idle로 변환
                }
                else if (playerDistance <= 5)   //플레이어와의 거리가 5과 같거나 작으면
                {
                    transform.LookAt(playerOb.transform);   //몬스터 상태가 attack일땐 플레이어 바라보도록 설정
                    state = monsterState.Attack;    //몬스터 상태 Attack으로 변환
                }
                else// 플레이어와의 거리가 8보다 작거나 같으면
                {
                    nav.SetDestination(playerOb.transform.position);    //몬스터 네비게이션 목적지 플레이어 위치로 설정
                    nav.isStopped = false;
                }
                break;
        }
        
    }
    IEnumerator Attack()    // 몬스터 공격하는 함수
    {


        switch (myMonsterType)  // 몬스터의 타입에 따라
        {
            case monsterType.Normal:
                attackCool += Time.deltaTime;
                transform.LookAt(playerOb.transform);   //몬스터 상태가 attack일땐 플레이어 바라보도록 설정
                if (attackCool >= 2)    //attackCool이 2보다 크거나 같을때
                {                
                    slimeHitArea.enabled = true;    //몬스터 공격범위 콜라이더 활성화
                    anim.SetTrigger("attack");
                    attackCool = 0;
                    yield return new WaitForSeconds(0.5f);
                    slimeHitArea.enabled = false;
                    state = monsterState.Walk;
                }
                break;

            case monsterType.Rush:
                attackCool += Time.deltaTime;
                

                if (attackCool>3)
                {
                    transform.LookAt(playerOb.transform);   //몬스터 상태가 attack일땐 플레이어 바라보도록 설정
                    nav.enabled = false;    //네비게이션 빌활성화
                    rushMonsterHitArea.enabled = true;  //몬스터 공격범위 콜라이더 활성화
                    yield return new WaitForSeconds(0.1f);
                    anim.SetBool("isWalk", true);   // 이동 애니메이션 활성화
                    rigd.AddForce(transform.forward*2.25f, ForceMode.Impulse);   // 오브젝트 이동
                    yield return new WaitForSeconds(0.05f);
                    attackCool = 0; // 공격 쿨타임 0으로 초기화
                    anim.SetBool("isDiving", true); // 다이빙 애니메이션 활성화
                    yield return new WaitForSeconds(1f);
                    anim.SetBool("isDiving", false);    // 다이빙 애니메이션 비활성화
                    anim.SetBool("isWalk", false);  // 몬스터 이동 애니메이션 비활성화
                    yield return new WaitForSeconds(0.5f);
                    rushMonsterHitArea.enabled = false; // 몬스터 공격범위 콜라이더 비활성화
                    nav.enabled = true; //몬스터 네비게이션 활성화
                    yield return new WaitForSeconds(0.4f);
                    state = monsterState.Walk;
                }

                break;

            case monsterType.Shoot:
                attackCool += Time.deltaTime;
                nav.isStopped = true;   //몬스터 이동 멈추기
                if (attackCool > 5)
                {
                    attackCool = 0;
                    anim.SetTrigger("tAttack");
                    transform.LookAt(playerOb.transform);   //몬스터 상태가 attack일땐 플레이어 바라보도록 설정
                    
                    GameObject monsterBullet1;   //monsterBullet 오브젝트 1
                    GameObject monsterBullet2;   //monsterBullet 오브젝트 2
                    GameObject monsterBullet3;   //monsterBullet 오브젝트 3


                    //몬스터 위치와 몬스터 각도에 monsterBulletPref을 monsterBullet1에 복사함
                    monsterBullet1 = Instantiate(monsterBulletPref, transform.position + new Vector3(0, 0.7f, 0), transform.rotation);   

                    monsterBullet1.gameObject.name = "monsterBullet1";

                    Quaternion monsterBullet2Dir = Quaternion.Euler(new Vector3(0, 30, 0));  //monsterBullet2Dir의 방향 설정을 위해 aura2Dir 생성

                    //monsterBullet1위치와 monsterBullet1각도에 monsterBullet2Dir을 더해준 monsterBullet2를 복사해서 생성 ,rotation에서 *는 +랑 같음.
                    monsterBullet2 = Instantiate(monsterBulletPref, monsterBullet1.transform.position, monsterBullet1.transform.rotation * monsterBullet2Dir);    
                    
                    monsterBullet2.gameObject.name = "monsterBullet2";

                    Quaternion monsterBullet3Dir = Quaternion.Euler(new Vector3(0, -30, 0)); //aura3의 방향 설정을 위해 aura2Dir 생성

                    //monsterBullet1위치와 monsterBullet1각도에 monsterBullet2Dir을 더해준  monsterBullet2를 복사해서 생성 ,rotation에서 *는 +랑 같음.
                    monsterBullet3 = Instantiate(monsterBulletPref, monsterBullet1.transform.position, monsterBullet1.transform.rotation * monsterBullet3Dir);    

                    monsterBullet3.gameObject.name = "monsterBullet3";
                    
                    yield return new WaitForSeconds(0.2f);


                    Destroy(monsterBullet1, 4);  //3초뒤 monsterBullet1 오브젝트 파괴
                    Destroy(monsterBullet2, 4);  //3초뒤 monsterBullet2 오브젝트 파괴
                    Destroy(monsterBullet3, 4);  //3초뒤 monsterBullet3 오브젝트 파괴
                    state = monsterState.Walk;
                }
                if (playerDistance>6)
                {
                    attackCool = 0;
                    state = monsterState.Walk;
                }
                
                break;
        }
        

    }
    public void MonsterDamaged(float Dmg)  // 몬스터가 피해를 입을때 실행되는 함수
    {
        Mathf.Clamp(monsterHp, 0, monsterHp);   // 몬스터 hp 최소값 0으로 제한
        GameObject DmgPref = Instantiate(DamageTxtPref, DmgTextTr); //몬스터 피해 데미지 텍스트 프리팹 복사해서 게임오브젝트 DmgPref 변수값에 넣어줌
        DMGText dmgTextSc=DmgPref.GetComponent<DMGText>();
        
        int rand = Random.Range(1,101); // 랜덤숫자 뽑기

        if (rand<=DataManager.instance.data.playerCritical) // 랜덤숫자가 플레이어의 크리티컬 확률보다 같거나 낮으면 크리티컬 적용
        {
            dmgTextSc.damageValue = Dmg*1.7f;   //플레이어 데미지 1.7배
            dmgTextSc.isCritical = true;
            DmgPref.transform.position = DmgTextTr.position;
            monsterHp -= Dmg * 1.7f;
            monsterHpBar.value = monsterHp;
        }
        else //크리티컬 공격이 아닐때
        {
            dmgTextSc.damageValue = Dmg;
            DmgPref.transform.position = DmgTextTr.position;
            monsterHp -= Dmg;
            monsterHpBar.value = monsterHp;
        }
        
        if (monsterHp <= 0 &&!monsterDead)  //몬스터가 죽었을 때
        {
            Destroy(DmgTextTr.gameObject, 10);
            DmgTextTr.SetParent(GameManager.instance.transform);
            deadps = transform.position;
            waveSc.deadMonster++;
            GameManager.instance.roundMonsterExp += monsterInfo.monsterExp;
            waveSc.WaveMonsterDeadCheck();
            monsterDead = true;
            gameObject.SetActive(false);
        }
    }

    void DropCoin() //코인생성 함수
    {
        int rand = Random.Range(0, 3);
        if (rand<2)
        {

            GameObject coin = Instantiate(coinPref, deadps, Quaternion.Euler(0, 0, 90));

            coin.GetComponent<Coin>().coinPrice = monsterInfo.monsterCoin;
            coin.transform.SetParent(GameManager.instance.transform);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")    //충돌한 오브젝트의 태그가 플레이어일때
        {
            isDamaged = playerOb.GetComponent<NewPlayer>().isDamaged;

            if (myMonsterType== monsterType.Rush&&state==monsterState.Attack)   //몬스터 타입이 돌진형이고 그 몬스터의 상태가 공격상태라면
            {
                anim.SetTrigger("AttackTr");
                rigd.velocity = Vector3.zero;
                state=monsterState.Walk;
            }
            if (!isDamaged) //플레이어가 무적이 아닐때만 피가 닳도록
            {
                playerOb.SendMessage("Damaged", monsterInfo.monsterAtkDmg);
            }
            
        }
    }

}
