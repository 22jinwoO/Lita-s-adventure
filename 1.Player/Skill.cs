using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{

    Animator anim;  //애니메이터 가져오기

    Rigidbody rigid;    //리지드 바디 가져오기

    [HideInInspector]
    public NewPlayer NewplayerSc;   //플레이어 스크립트

    CharacterController cc; // 캐릭터 컨트롤러
    Vector3 monsterDir; //가까운 몬스터 위치값 저장
    float speed;    //플레이어 이동속도

    [Header("기본 공격 범위에 있는 몬스터 콜라이더 리스트")]
    public Collider[] hitColliders; //기본 공격 범위에 있는 몬스터 콜라이더 리스트

    [Header("플레이어 스킬 사용시 오버랩스피어에 감지될 레이어")]
    public LayerMask DeTectMonster;   //오버랩스피어에 감지될 레이어들

    [Header("플레이어 ")]

    [Header("플레이어 스킬 2번이 사용중인지 아닌지 확인하기 위한 bool자료형")]
    public bool isSwordMove = false;   //스킬 2번이 사용중인지 아닌지 확인하기 위한 bool자료형
    [Header("플레이어 스킬 2번 범위에 존재하는 몬스터들을 저장할 리스트")]
    public List<GameObject> monsters = new List<GameObject>();  //플레이어 스킬 2번 범위에 존재하는 몬스터들을 저장할 리스트

    [Header("플레이어 스킬 2번 범위를 담당하는 콜라이더")]
    public BoxCollider boxCol;  // 스킬 2번을 위한 박스 콜라이더

    [Header("스킬 3번 아우라 오브젝트")]
    public GameObject swordAuraPrefab;  //스킬 3번을 위한 프리팹 오브젝트

    [Header("플레이어 스킬 쿨타임시 버튼 이미지 filled 관련")]
    public Image skill1_Cool;
    public Image skill2_Cool;
    public Image skill3_Cool;

    //플레이어 스킬 쿨타임 관련
    public float[] skill_delays = new float[3];    //스킬 1,2,3 딜레이 값들의 배열

    [Header("스킬 1,2,3 이 사용 가능한지 구분짓기 위한 bool자료형 배열")]
    [SerializeField]
    bool[] isCanSkill = new bool[] {false,false,false}; //스킬 1,2,3 이 사용 가능한지 구분짓기 위한 bool자료형 배열

    [Header("스킬 버튼 배열")]
    public Button[] skill_Btns; //스킬 버튼 배열

    [Header("스킬 쿨타임 이미지 배열")]
    public Image[] skill_Cools; //스킬 쿨타임 이미지 배열

    [Header("스킬 쿨타임 배열 값들")]
    [SerializeField]
    float[] skill_CoolValues={ 15, 20, 25 };    //스킬 쿨타임 배열 값들 지정

    public bool isSkillDoing=false;
    public GameObject skill2_effect;
    public GameObject skill3_effect;

    public Text destinationTxt;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();  //rigdbody 가져오기
        cc = GetComponent<CharacterController>();   //캐릭터 컨트롤러 가져오기
        speed = GetComponent<NewPlayer>().playerMoveSpeed;    //NewPlayer 스크립트에서 speed값 가져오기
        NewplayerSc = GetComponent<NewPlayer>();    //NewPlayer 스크립트 가져오기
        anim = GetComponent<Animator>();    //Animator 컴포넌트 가져오기
        int swordFire = 0;
        int swordMove = 1;
        int fallDownSword = 2;

        skill_Btns[0].onClick.AddListener(() => StartCoroutine(DoSkill(swordFire))); //스킬 1버튼에 Skill스크립트의 doskill함수에 swordFire을 string자료형의 매개변수로 전달
        skill_Btns[1].onClick.AddListener(() => StartCoroutine(DoSkill(swordMove))); //스킬 2버튼에 Skill스크립트의 doskill함수에 swordMove을 string자료형의 매개변수로 전달
        skill_Btns[2].onClick.AddListener(() => StartCoroutine(DoSkill(fallDownSword))); //스킬 3버튼에 Skill스크립트의 doskill함수에 fallDownSword을 string자료형의 매개변수로 전달

       
    }

    void Update()
    {

        //업데이트 x 공격키만 눌렀을때
        //코루틴함수 업데이트문처럼 사용
        SkillCoolTimeCheck();
    }

    private void OnTriggerEnter(Collider other) //Trigger 체크되어있을때 충돌시 호출
    {
        if (other.tag == "Monster" && isSwordMove)   //충돌한 물체의 태그가 몬스터이고 isSwordMove가 true일때
        {
            print("몬스터추가");
            monsters.Add(other.gameObject); //스킬2번의 콜라이더가 활성화 되어있을때 충돌된 몬스터들을 리스트에 값들을 추가함
        }

        else if (other.tag == "Portal")   //플레이어가 포탈이랑 충돌중일때
        {
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[4]);
            UiManager.instance.onPortal = true; //UiManager의 onPortal 값 true
            UiManager.instance.portalName = other.name; //현재 충돌중인 포탈이름 전달
        }

        if (other.name=="StagePortal")
        {
            // 공격버튼 이미지 스테이지 이미지로 변경
            UiManager.instance.AttackBtnImg.sprite = UiManager.instance.goStageBtnImg;
            UiManager.instance.AttackBackBtnImg.sprite = UiManager.instance.goStageBtnImg;
        }

        if (other.name == "StorePortal")
        {
            // 공격버튼 이미지 상점 이미지로 변경
            UiManager.instance.AttackBtnImg.sprite = UiManager.instance.goShopBtnImg;
            UiManager.instance.AttackBackBtnImg.sprite = UiManager.instance.goShopBtnImg;
            StartCoroutine(UiManager.instance.NpcTalk(true)); // NPC 상호작용 함수 호출
        }
    }

    private void OnTriggerExit(Collider other)  //충돌이 끝났을때 호출
    {
        if (other.tag == "Portal")  //충돌이 끝난 물체의 태그가 Portal이라면
        {
            UiManager.instance.onPortal = false; //UiManager의 onPortal 값 false
            UiManager.instance.portalName = null;   //현재 충돌중인 포탈이름 null
        }

        if (other.name == "StagePortal"|| other.name == "StorePortal")
        {
            // 공격버튼 이미지 복구
            UiManager.instance.AttackBtnImg.sprite = UiManager.instance.atkBtnImg;
            UiManager.instance.AttackBackBtnImg.sprite = UiManager.instance.atkBtnImg;
            if (other.name == "StorePortal")
            {
                StartCoroutine(UiManager.instance.NpcTalk(false)); // NPC 상호작용 함수 호출
            }
        }
    }

    #region * MonsterCheck()함수 - 플레이어 주변에 몬스터가 있나 없나 체크하는 함수
    public void MonsterCheck()
    {
        hitColliders = Physics.OverlapSphere(transform.position, 6f, DeTectMonster);    //오버랩스피어로 DetectMonster만 추출하는 hitColider생성
        if (hitColliders.Length == 0)    // 콜라이더 리스트에 값이 없다면
        {

            NewplayerSc.RealAttack();   // 플레이어가 제자리에서 공격하도록 PNewplayerSc.RealAttack() 함수 호출
            NewplayerSc.canInput = true;    //플레이어 이동 가능하도록 canInput값 true
        }

        else
        {

            float nearDistance = Vector3.Distance(transform.position, hitColliders[0].transform.position);    //hitColliders 리스트 값에서 첫번째 몬스터의 위치와 플레이어의 위치 거리를 nearDistance값에 넣어주고
            GameObject nearMonster = hitColliders[0].gameObject;    //nearMonster 오브젝트에 hitColliders의 첫번째 게임오브젝트를 넣어줌

            for (int i = 0; i < hitColliders.Length; i++)    //hitColliders리스트의 수만큼 반복문 반복
            {

                float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);  //hitColliders 리스트 값에서 i번째 몬스터의 위치와 플레이어의 위치 거리를 distance값에 넣어주고

                if (nearDistance > distance)  //nearDistance값과 i번째 몬스터와의 거리인 distance를 비교해서 distance 값이 더 가까우면 nearDistance값에 distance 값을 넣고 그 게임오브젝트를 nearMonster에 저장하도록
                {
                    nearDistance = distance;
                    nearMonster = hitColliders[i].gameObject;
                }

            }
            StartCoroutine(DoCheck(nearMonster.transform, "attack",nearMonster.GetComponent<Enemy>().isGround));    //스타트 코루틴함수 Docheck에 "attack"을 매개변수로 전달
        }

        #endregion

    }
    #region * Docheck 코루틴 함수 - 플레이어가 어떤 행동을 할지 체크하는 함수로 매개변수로 Transform자료형 monsterDistance, string자료형 doing을 사용 / MonsterCheck()함수에서 호출, DoSkill함수에서 2번 호출
    //특정상황에서만 사용해야하는데 update문처럼 계속 호출해야 할때 코루틴을 업데이트문처럼 사용
    IEnumerator DoCheck(Transform monsterPosition, string doing, bool isGround)     //플레이어 주변에 있는 몬스터 확인 후 그 몬스터에게 달려드는 함수
    {

        NewplayerSc.canInput = false;   //플레이어 입력 불가능하도록 canInput 값 false로 설정
        transform.LookAt(monsterPosition);    // 플레이어는 거리에 해당하는 몬스터의 위치를 바라보고
        monsterDir = monsterPosition.position - transform.position; //근처몬스터 위치에서 플레이어의 위치를 뺀값을 Vector3변수에 넣어주고
        monsterDir = monsterDir.normalized;   //monsterDir 정규화 몬스터를 향한 방향 정규화


        switch (doing)  // 공격상태일때, 스워드무브 스킬 사용중일때, 폴다운 스워드 이동중일때 사용
        {
            case "attack":  //attack을 매개변수로 받았을 때
                if (monsterPosition != null&&(monsterPosition.position - transform.position).magnitude > 2f&&isGround==true)
                {
                    anim.SetTrigger("dash");  //대쉬 애니메이션 활성화
                }
                
                while (monsterPosition!= null && (monsterPosition.position - transform.position).magnitude > 2f && isGround == true)    //몬스터 거리값이 일정거리보다 커질때동안
                {
                    cc.Move(monsterDir * speed * 3 * Time.deltaTime); //플레이어 오브젝트 이동
                    yield return null;
                }
                monsterPosition.SendMessage("MonsterDamaged", DataManager.instance.data.playerAttackDmg);    //몬스터에게 SendMassge함수로 Damaged함수 호출하여 플레이어 attackDmg만큼 몬스터HP 닳게 함
                NewplayerSc.RealAttack();  //플레이어 기본공격
                yield return new WaitForSeconds(0.8f);
                NewplayerSc.canInput = true;    //플레이어 이동 가능
                break;


            case "swordMove":   //스킬 2번 사용중일때
                AudioManager.instance.Sound_Skill(AudioManager.instance.soundsSKill[Random.Range(2,4)]);
                
                boxCol.enabled = false; //스킬 2번을 위한 박스콜라이더 비활성화
                isSwordMove = false;    //플레이어가 충돌할때마다 몬스터 리스트에 추가하는 코드 실행하지 않도록 isSwordMove값 false로 변경
                anim.SetTrigger("dash");  //애니메이션 활성화

                Vector3 destination = monsterPosition.position + transform.forward*3; // 맨 마지막 몬스터를 지나가기 위해 목표지점을 몬스터 위치에서 플레이어의 forward를 더해줌
                //destinationTxt.text= "좌표 : "+destination.ToString();
                while (monsterPosition!=null&&(destination - transform.position).magnitude > 1.2f )   // 목표지점에 닿을때까지 이동
                {
                    Time.timeScale = 0;
                    cc.Move(monsterDir * speed * 4 * Time.unscaledDeltaTime);   //플레이어 이동
                    skill2_effect.SetActive(true);
                    destination.y = transform.position.y;   //플레이어가 위로 솟아오르지 않도록 하기 위해 설정
                    print("원래 방향값:"+monsterDir);
                    print("이동중인 방향값:"+(destination - transform.position).normalized);
                    yield return null; //한 프레임씩 적용

                }

                yield return new WaitForSecondsRealtime(1f);
                skill2_effect.SetActive(false);
                Time.timeScale = 1;
                foreach (var item in monsters)  //몬스터 리스트 값들을 item에 넣어
                {
                    item.SendMessage("MonsterDamaged", DataManager.instance.data.playerSkillDmg * 1.2f);    //item마다 sendMessage함수로 damaged 함수 호출
                    item.GetComponent<Collider>().isTrigger = false;
                }

                monsters.RemoveRange(0, monsters.Count);    //마지막에 몬스터 리스트 값들 삭제

                isSkillDoing = false;
                NewplayerSc.canInput = true;    //플레이어 이동 가능 bool자료형
                NewplayerSc.isDamaged = false;  //플레이어 무적판정 해제
                break;



            case "fallDownSword":   //스킬 3번 사용중일때
                skill3_effect.SetActive(true);
                AudioManager.instance.Sound_Skill(AudioManager.instance.soundsSKill[5]);
                while (monsterPosition!= null && (monsterPosition.position - transform.position).magnitude > 2f)    //거리값이 일정거리보다 클때 동안
                {
                    cc.Move(monsterDir * speed * 4 * Time.deltaTime); //플레이어 오브젝트 이동
                    yield return null;
                }
                
                isSkillDoing=false;
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); //while문이 끝나면 플레이어의 각도는 transform.eulerAngles.y로 설정
                break;
        }
    }
    #endregion

    #region * Doskill 코루틴 함수 - 스킬이름을 매개변수로 받아 스킬이름마다 특정행동을 하도록 하는 함수
    public IEnumerator DoSkill(int skiilName)   //스킬 사용 함수 , 매개변수로 int 값 받아옴(데이터 값이 string형보다 작아서)
    {

        switch (skiilName)  //스킬이름에 따라 구분지어 사용
        {
            case 0:
                if (isCanSkill[0]&& !isSkillDoing)
                {
                    AudioManager.instance.Sound_Skill(AudioManager.instance.soundsSKill[0]);
                    isSkillDoing = true;
                    skill_delays[0] = 0;
                    

                    GameObject aura1;   //아우라 오브젝트 1
                    GameObject aura2;   //아우라 오브젝트 2
                    GameObject aura3;   //아우라 오브젝트 3



                    anim.SetTrigger("swordAura");
                    aura1 = Instantiate(swordAuraPrefab, transform.position + new Vector3(0, 0.7f, 0), transform.rotation);   //플레이어 위치에 플레이어 각도에 swordAuraPrefab을 aura1에 복사함
                    aura1.gameObject.name = "aura1";

                    Quaternion aura2Dir = Quaternion.Euler(new Vector3(0, 30, 0));  //aura2의 방향 설정을 위해 aura2Dir 생성
                    aura2 = Instantiate(swordAuraPrefab, aura1.transform.position, aura1.transform.rotation * aura2Dir);    //aura1위치와 aura1각도에 aura2Dir을 더해준 swordAuraPrefab을 복사해서 aura2에 대입 ,rotation에서 *는 +랑 같음.
                    aura2.gameObject.name = "aura2";

                    Quaternion aura3Dir = Quaternion.Euler(new Vector3(0, -30, 0)); //aura3의 방향 설정을 위해 aura3Dir 생성
                    aura3 = Instantiate(swordAuraPrefab, aura1.transform.position, aura1.transform.rotation * aura3Dir);    //aura1위치와 aura1각도에 aura3Dir을 더해준 swordAuraPrefab을 복사해서 aura3에 대입 ,rotation에서 *는 +랑 같음.
                    aura3.gameObject.name = "aura3";

                    yield return new WaitForSeconds(0.2f);


                    Destroy(aura1, 3);  //3초뒤 aura1 오브젝트 파괴
                    Destroy(aura2, 3);  //3초뒤 aura2 오브젝트 파괴
                    Destroy(aura3, 3);  //3초뒤 aura3 오브젝트 파괴
                    isSkillDoing = false;
                }
                break;

            case 1:   //2번째 스킬 이름 swordMove일때
                if (isCanSkill[1]&& !isSkillDoing)
                {
                    AudioManager.instance.Sound_Skill(AudioManager.instance.soundsSKill[1]);
                    isSkillDoing = true;
                    skill_delays[1] = 0;
                    NewplayerSc.isDamaged = true;   //플레이어 무적판정
                    
                    yield return new WaitForSeconds(1.2f);
                    boxCol.enabled = true;  //일시적으로 박스콜라이더를 활성화 시킴
                    isSwordMove = true; //NewPlayer 스크립트의 OnTriggerEnter함수를 사용하기위해 isSwordMove를 ture 변환
                    float farDistance = 0;
                    GameObject farMonster = null;
                    yield return new WaitForSeconds(0.1f);
                    if (monsters.Count == 0)  //만약 콜라이더에 부딪힌 몬스터들이 없다면
                    {
                        NewplayerSc.isDamaged = false;   //플레이어 무적판정
                        boxCol.enabled = false; //스킬 2번을 위한 박스콜라이더 비활성화
                        isSwordMove = false;    //isSwordMove를 false로 변환하여 리스트에 값 추가 안되도록 함
                        NewplayerSc.canInput = true;    //플레이어 키입력 가능
                        isSkillDoing = false;
                        skill_delays[1] = 15;
                    }

                    else
                    {
                        farDistance = Vector3.Distance(transform.position, monsters[0].transform.position);    //monsters리스트의 첫번째 몬스터 위치와 플레이어의 위치의 거리를 farDistance값에 넣어줌
                        farMonster = monsters[0].gameObject; //farMonster에 monsters리스트의 첫번째 오브젝트를 넣음


                        for (int i = 0; i < monsters.Count; i++)    // 0부터 몬스터 monsters리스트 갯수까지
                        {
                            monsters[i].GetComponent<Collider>().isTrigger = true;
                            float distance = Vector3.Distance(transform.position, monsters[i].transform.position);
                            if (farDistance < distance) //가장 먼 거리가 새로운 거리보다 값이 작으면
                            {
                                farDistance = distance; //가장 먼거리에 새로운 거리 값 대입
                                farMonster = monsters[i];   //가장 먼 몬스터에 i번째 리스트 오브젝트 값 대입
                            }
                        }
                        StartCoroutine(DoCheck(farMonster.transform, "swordMove",true));  //가장 먼 몬스터에게 달려가는 함수 실행

                    }
                }
                break;


            case 2:   //스킬 3번 스킬 이름이 fallDownSword 일때
                if (isCanSkill[2]&&!isSkillDoing)
                {
                    AudioManager.instance.Sound_Skill(AudioManager.instance.soundsSKill[4]);
                    isSkillDoing = true;
                    skill_delays[2] = 0;
                    float time = 0;
                    NewplayerSc.canInput = false;   //플레이어 움직임이 불가능하도록 canInput=false 
                    
                    while (time < 1.3f)    //time이 2초보다 작을때동안
                    {
                        anim.SetFloat("move", speed);   //Move애니메이션을 활성화 하고
                        time += Time.deltaTime;     //time 에 계속 값을 더해주면서
                        cc.Move(transform.forward * speed * 2 * Time.deltaTime);    //플레이어가 2초동안 플레이어가 바라보는 방향으로 이동함
                        yield return null;
                    }
                    anim.SetFloat("move", 0);   //move애니메이션 값0
                    time = 0;
                    anim.SetTrigger("fallDown");    //fallDown애니메이션 활성화
                    while (time < 2)    //time이 2초보다 작을때동안
                    {
                        time += Time.deltaTime; //타임에 계속 값을 더해주면서
                        NewplayerSc.gravity = 0;    //일시적으로 중력 0으로 설정
                        cc.Move(transform.up * speed * 2 * Time.deltaTime); //플레이어가 위로 이동함

                        anim.SetFloat("multipleJump", 0.25f);   //애니메이션을 느리게 행동하기 위해 multipleJump에 0.25f값 넣어줌
                        yield return null;
                    }
                    Collider[] skillHitCols; //스킬3 범위에 있는 몬스터 콜라이더 리스트

                    skillHitCols = Physics.OverlapSphere(transform.position + (-transform.up * 7 ), 6f, DeTectMonster);  //오버랩스피어안에 있는 콜라이더들을 skillHitCols안에 넣어줌

                    if (skillHitCols.Length == 0)   // skillHitCols의 길이가 0이면 -> 탐지된 몬스터가 없을때
                    {
                        NewplayerSc.gravity = -3;   //플레이어에게 중력 적용 후
                        anim.SetFloat("move", 0);
                        skill_delays[2] = 15;
                        NewplayerSc.canInput = true;    //플레이어 이동 가능하도록 설정
                        isSkillDoing = false;
                        break;
                    }

                    float farDistance2 = Vector3.Distance(transform.position, skillHitCols[0].transform.position);    //skillHitCols콜라이더의 첫번째 값의 위치와 플레이어의 위치간의 거리를 farDistance2에 넣어주고
                    GameObject farMonster2 = skillHitCols[0].gameObject;     //skillHitCols콜라이더의 첫번째 값 오브젝트를 farMonster2에 넣어줌

                    for (int i = 0; i < skillHitCols.Length; i++)    //콜라이더 리스트의 수만큼 반복문 반복
                    {

                        float distance = Vector3.Distance(transform.position, skillHitCols[i].transform.position); //skillHitCols의 i번째 오브젝트 위치와 플레이어간의 거리를 구한 값을 distance에 넣어준 후
                        if (farDistance2 < distance)  //거리를 비교해서 distance의 값이 더 멀면
                        {
                            farDistance2 = distance;    //farDistance2에 distance 값을 넣어주고
                            farMonster2 = skillHitCols[i].gameObject;   //farMonster2 오브젝트의 skillHitCols의 i번째 게임 오브젝트를 넣어줌
                        }

                    }
                    anim.SetFloat("multipleJump", 0.3f);    //multiplejump값 0.3으로 변환

                    StartCoroutine(DoCheck(farMonster2.transform, "fallDownSword",true));    //스타트 코루틴 함수 Docheck 호출

                    for (int i = 0; i < skillHitCols.Length; i++)
                    {
                        skillHitCols[i].gameObject.SendMessage("MonsterDamaged", DataManager.instance.data.playerSkillDmg * 2f); //skillHitCols의 리스트값들에게 sendMessage로 함수 호출
                    }
                    yield return new WaitForSeconds(0.32f);
                    NewplayerSc.gravity = -3;   //플레이어 중력 적용
                    anim.SetFloat("move", 0);
                    NewplayerSc.canInput = true;    //플레이어 이동가능하도록 설정
                    yield return new WaitForSeconds(2f);
                    skill3_effect.SetActive(false);
                }
                break;


        }
    }
    #endregion

    void SkillCoolTimeCheck()   //스킬 쿨타임 함수
    {
        for (int i = 0; i < 3; i++)
        {
            if (!isCanSkill[i]) // i번째 스킬 
            {
                skill_Btns[i].interactable = false;
                skill_delays[i] += Time.deltaTime;
            }
            skill_Btns[i].interactable = (isCanSkill[i]) ? true : false;
            isCanSkill[i] = (skill_delays[i]>= skill_CoolValues[i] - DataManager.instance.data.playerSkillCool) ? true : false;
            skill_Cools[i].fillAmount= skill_delays[i]/ (skill_CoolValues[i] - DataManager.instance.data.playerSkillCool);

        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;   //플레이어 기본 공격 범위를 위한 기즈모
        Gizmos.DrawWireSphere(transform.position, 6f);
        Gizmos.color = Color.blue; //플레이어 스킬 3번을 위한 기즈모
        Gizmos.DrawWireSphere(transform.position + (-transform.up * 7), 6f);

    }
}
