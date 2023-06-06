using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("���Ͱ� ���� �ִ��� Ȯ���ϴ� ���ڷ���")]
    public bool isGround;

    [Header("���� ����(��ũ���ͺ� ������Ʈ)")]
    public MonsterInfo monsterInfo;
    //���� Hp��
    public Slider monsterHpBar;

    [Header("���� ���� Hp")]
    public float monsterHp;

    public StageManager waveSc;

    [Header("���Ͱ� ��Ҵ��� �׾����� üũ�ϴ� bool�ڷ���")]
    public bool monsterDead=false;

    [Header("�÷��̾� ������Ʈ")]
    protected GameObject playerOb;

    [Header("�÷��̾� �������� bool�ڷ���")]
    public bool isDamaged;

    [Header("����������")]
    public GameObject coinPref;

    [Header("������ ���� ��Ʈ ����")]
    public BoxCollider slimeHitArea;
    
    [Header("������ ���� ��Ʈ ����")]
    public BoxCollider rushMonsterHitArea;

    [Header("���Ÿ� ���� ����ü ���ӿ�����Ʈ ������")]
    public GameObject monsterBulletPref;


    [Header("���� ���� ���� Ȯ�� bool�ڷ���")]
    public bool isAttack= true;

    [Header("�÷��̾���� �Ÿ� flaot ����")]
    public float playerDistance;

    NavMeshAgent nav;   //�׺�޽� ������Ʈ ����
    
    public Animator anim;
    [Header("���� ���� ��Ÿ�� ����")]
    public float attackCool = 0;    //���� ���� ��Ÿ�� ����
    bool isCollision;
    
    public enum monsterState    //���� ���� ����ü
    {
        Idle,
        Walk,
        Attack,
    }

    public enum monsterType //���� ������ ����
    {
        Normal,
        Rush,
        Shoot,
    }
    public monsterType myMonsterType;

    [Header("���� ���� ����")]
    public monsterState state = monsterState.Idle;

    [Header("���� ������ �ؽ�Ʈ")]
    public GameObject DamageTxtPref;

    [Header("���� ������ �ؽ�Ʈ Ʈ������")]
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
        // ** �ʿ��� ���� ��������

        monsterHpBar=GetComponentInChildren<Slider>();  //���� ü�¹� �����̴� ��������
        monsterHpBar.maxValue = monsterInfo.monsterMaxHp;   //monsterHpBar�����̴��� maxValue�� monsterInfo ������ ��ũ��Ʈ�� monsterMaxHp�� ����
        monsterHp = monsterInfo.monsterMaxHp;   //monsterInfo ������ ��ũ��Ʈ�� �� ����
        monsterHpBar.value = monsterHp; //monsterInfo ������ ��ũ��Ʈ�� �� ����
        gameObject.name = monsterInfo.monsterName;  //monsterInfo ������ ��ũ��Ʈ�� �� ����
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
        playerDistance=Vector3.Distance(transform.position, playerOb.transform.position);   //���ʹ� ������Ʈ�� �÷��̾� ������Ʈ���� �Ÿ��� �־��ֱ�
        anim.SetFloat("monsterVelocity", nav.velocity.magnitude);   //���� �̵��� �־��ֱ�

        switch (myMonsterType)  //���� Ÿ�Կ� ���� ����
        {
            case monsterType.Normal:    //�Ϲ� ���� �϶�
                switch (state) // ���� FSM
                {
                    case monsterState.Idle: //���� ���°� Idle�϶�
                        nav.isStopped = true;   //���� �̵� ���߱�
                        if (playerDistance <= 6)    //�÷��̾���� �Ÿ��� 6 �����϶�
                        {
                            state = monsterState.Walk; // ���� ���� Walk�� �ٲ�
                        }
                        break;

                    case monsterState.Walk: //walk �����϶�
                        Walk(); //Walk�Լ� ����
                        break;

                    case monsterState.Attack:
                        StartCoroutine(Attack());
                        break;
                }
                break;
            case monsterType.Rush:  //������ �����̸�
                switch (state)
                {
                    case monsterState.Idle:
                        anim.SetBool("isWalk", false);  // iswalk �ִϸ��̼� ����x
                        nav.isStopped = true;   //���� �̵� ���߱�
                        if (playerDistance <= 6)    //�÷��̾���� �Ÿ��� 6 �����϶�
                        {
                            state = monsterState.Walk; // ���� ���� Walk�� �ٲ�
                        }
                        break;
                    case monsterState.Walk:
                        Walk(); //Walk�Լ� ����
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
                        nav.isStopped = true;   //���� �̵� ���߱�
                        if (playerDistance <= 7)    //�÷��̾���� �Ÿ��� 7 �����϶�
                        {
                            state = monsterState.Walk; // ���� ���� Walk�� �ٲ�
                        }
                        break;
                    case monsterState.Walk:
                        Walk(); //Walk�Լ� ����
                        break;
                    case monsterState.Attack:
                        StartCoroutine(Attack());
                        break;
                }
                break;
        }
        

    }

    void Walk() //���� �÷��̾� �������� �̵��ϴ� �Լ�
    {
        switch (myMonsterType)
        {
            case monsterType.Normal:
                if (playerDistance > 6) // �÷��̾���� �Ÿ��� 6���� ũ��
                {
                    state = monsterState.Idle;  // ���� ���� Idle�� ��ȯ
                }
                else if (playerDistance <= 2)   //�÷��̾���� �Ÿ��� 2���ų� ������
                {
                    state = monsterState.Attack;    //���� ���� Attack���� ��ȯ
                }
                else// �÷��̾���� �Ÿ��� 6���� �۰ų� ������
                {
                    nav.SetDestination(playerOb.transform.position);    //���� �׺���̼� ������ �÷��̾� ��ġ�� ����
                    nav.isStopped = false;
                }
                break;
            case monsterType.Rush:
                if (playerDistance > 7) // �÷��̾���� �Ÿ��� 7���� ũ��
                {
                    state = monsterState.Idle;  // ���� ���� Idle�� ��ȯ
                }
                else if (playerDistance <= 5)   //�÷��̾���� �Ÿ��� 4���ų� ������
                {
                    transform.LookAt(playerOb.transform);   //���� ���°� attack�϶� �÷��̾� �ٶ󺸵��� ����
                    state = monsterState.Attack;    //���� ���� Attack���� ��ȯ
                }
                else// �÷��̾���� �Ÿ��� 7���� �۰ų� ������
                {
                    nav.SetDestination(playerOb.transform.position);    //���� �׺���̼� ������ �÷��̾� ��ġ�� ����
                    nav.isStopped = false;
                }
                break;
            case monsterType.Shoot:
                if (playerDistance > 8) // �÷��̾���� �Ÿ��� 8���� ũ��
                {
                    state = monsterState.Idle;  // ���� ���� Idle�� ��ȯ
                }
                else if (playerDistance <= 5)   //�÷��̾���� �Ÿ��� 5�� ���ų� ������
                {
                    transform.LookAt(playerOb.transform);   //���� ���°� attack�϶� �÷��̾� �ٶ󺸵��� ����
                    state = monsterState.Attack;    //���� ���� Attack���� ��ȯ
                }
                else// �÷��̾���� �Ÿ��� 8���� �۰ų� ������
                {
                    nav.SetDestination(playerOb.transform.position);    //���� �׺���̼� ������ �÷��̾� ��ġ�� ����
                    nav.isStopped = false;
                }
                break;
        }
        
    }
    IEnumerator Attack()    // ���� �����ϴ� �Լ�
    {


        switch (myMonsterType)  // ������ Ÿ�Կ� ����
        {
            case monsterType.Normal:
                attackCool += Time.deltaTime;
                transform.LookAt(playerOb.transform);   //���� ���°� attack�϶� �÷��̾� �ٶ󺸵��� ����
                if (attackCool >= 2)    //attackCool�� 2���� ũ�ų� ������
                {                
                    slimeHitArea.enabled = true;    //���� ���ݹ��� �ݶ��̴� Ȱ��ȭ
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
                    transform.LookAt(playerOb.transform);   //���� ���°� attack�϶� �÷��̾� �ٶ󺸵��� ����
                    nav.enabled = false;    //�׺���̼� ��Ȱ��ȭ
                    rushMonsterHitArea.enabled = true;  //���� ���ݹ��� �ݶ��̴� Ȱ��ȭ
                    yield return new WaitForSeconds(0.1f);
                    anim.SetBool("isWalk", true);   // �̵� �ִϸ��̼� Ȱ��ȭ
                    rigd.AddForce(transform.forward*2.25f, ForceMode.Impulse);   // ������Ʈ �̵�
                    yield return new WaitForSeconds(0.05f);
                    attackCool = 0; // ���� ��Ÿ�� 0���� �ʱ�ȭ
                    anim.SetBool("isDiving", true); // ���̺� �ִϸ��̼� Ȱ��ȭ
                    yield return new WaitForSeconds(1f);
                    anim.SetBool("isDiving", false);    // ���̺� �ִϸ��̼� ��Ȱ��ȭ
                    anim.SetBool("isWalk", false);  // ���� �̵� �ִϸ��̼� ��Ȱ��ȭ
                    yield return new WaitForSeconds(0.5f);
                    rushMonsterHitArea.enabled = false; // ���� ���ݹ��� �ݶ��̴� ��Ȱ��ȭ
                    nav.enabled = true; //���� �׺���̼� Ȱ��ȭ
                    yield return new WaitForSeconds(0.4f);
                    state = monsterState.Walk;
                }

                break;

            case monsterType.Shoot:
                attackCool += Time.deltaTime;
                nav.isStopped = true;   //���� �̵� ���߱�
                if (attackCool > 5)
                {
                    attackCool = 0;
                    anim.SetTrigger("tAttack");
                    transform.LookAt(playerOb.transform);   //���� ���°� attack�϶� �÷��̾� �ٶ󺸵��� ����
                    
                    GameObject monsterBullet1;   //monsterBullet ������Ʈ 1
                    GameObject monsterBullet2;   //monsterBullet ������Ʈ 2
                    GameObject monsterBullet3;   //monsterBullet ������Ʈ 3


                    //���� ��ġ�� ���� ������ monsterBulletPref�� monsterBullet1�� ������
                    monsterBullet1 = Instantiate(monsterBulletPref, transform.position + new Vector3(0, 0.7f, 0), transform.rotation);   

                    monsterBullet1.gameObject.name = "monsterBullet1";

                    Quaternion monsterBullet2Dir = Quaternion.Euler(new Vector3(0, 30, 0));  //monsterBullet2Dir�� ���� ������ ���� aura2Dir ����

                    //monsterBullet1��ġ�� monsterBullet1������ monsterBullet2Dir�� ������ monsterBullet2�� �����ؼ� ���� ,rotation���� *�� +�� ����.
                    monsterBullet2 = Instantiate(monsterBulletPref, monsterBullet1.transform.position, monsterBullet1.transform.rotation * monsterBullet2Dir);    
                    
                    monsterBullet2.gameObject.name = "monsterBullet2";

                    Quaternion monsterBullet3Dir = Quaternion.Euler(new Vector3(0, -30, 0)); //aura3�� ���� ������ ���� aura2Dir ����

                    //monsterBullet1��ġ�� monsterBullet1������ monsterBullet2Dir�� ������  monsterBullet2�� �����ؼ� ���� ,rotation���� *�� +�� ����.
                    monsterBullet3 = Instantiate(monsterBulletPref, monsterBullet1.transform.position, monsterBullet1.transform.rotation * monsterBullet3Dir);    

                    monsterBullet3.gameObject.name = "monsterBullet3";
                    
                    yield return new WaitForSeconds(0.2f);


                    Destroy(monsterBullet1, 4);  //3�ʵ� monsterBullet1 ������Ʈ �ı�
                    Destroy(monsterBullet2, 4);  //3�ʵ� monsterBullet2 ������Ʈ �ı�
                    Destroy(monsterBullet3, 4);  //3�ʵ� monsterBullet3 ������Ʈ �ı�
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
    public void MonsterDamaged(float Dmg)  // ���Ͱ� ���ظ� ������ ����Ǵ� �Լ�
    {
        Mathf.Clamp(monsterHp, 0, monsterHp);   // ���� hp �ּҰ� 0���� ����
        GameObject DmgPref = Instantiate(DamageTxtPref, DmgTextTr); //���� ���� ������ �ؽ�Ʈ ������ �����ؼ� ���ӿ�����Ʈ DmgPref �������� �־���
        DMGText dmgTextSc=DmgPref.GetComponent<DMGText>();
        
        int rand = Random.Range(1,101); // �������� �̱�

        if (rand<=DataManager.instance.data.playerCritical) // �������ڰ� �÷��̾��� ũ��Ƽ�� Ȯ������ ���ų� ������ ũ��Ƽ�� ����
        {
            dmgTextSc.damageValue = Dmg*1.7f;   //�÷��̾� ������ 1.7��
            dmgTextSc.isCritical = true;
            DmgPref.transform.position = DmgTextTr.position;
            monsterHp -= Dmg * 1.7f;
            monsterHpBar.value = monsterHp;
        }
        else //ũ��Ƽ�� ������ �ƴҶ�
        {
            dmgTextSc.damageValue = Dmg;
            DmgPref.transform.position = DmgTextTr.position;
            monsterHp -= Dmg;
            monsterHpBar.value = monsterHp;
        }
        
        if (monsterHp <= 0 &&!monsterDead)  //���Ͱ� �׾��� ��
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

    void DropCoin() //���λ��� �Լ�
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
        if (other.tag=="Player")    //�浹�� ������Ʈ�� �±װ� �÷��̾��϶�
        {
            isDamaged = playerOb.GetComponent<NewPlayer>().isDamaged;

            if (myMonsterType== monsterType.Rush&&state==monsterState.Attack)   //���� Ÿ���� �������̰� �� ������ ���°� ���ݻ��¶��
            {
                anim.SetTrigger("AttackTr");
                rigd.velocity = Vector3.zero;
                state=monsterState.Walk;
            }
            if (!isDamaged) //�÷��̾ ������ �ƴҶ��� �ǰ� �⵵��
            {
                playerOb.SendMessage("Damaged", monsterInfo.monsterAtkDmg);
            }
            
        }
    }

}
