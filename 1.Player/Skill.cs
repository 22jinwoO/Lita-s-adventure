using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{

    Animator anim;  //�ִϸ����� ��������

    Rigidbody rigid;    //������ �ٵ� ��������

    [HideInInspector]
    public NewPlayer NewplayerSc;   //�÷��̾� ��ũ��Ʈ

    CharacterController cc; // ĳ���� ��Ʈ�ѷ�
    Vector3 monsterDir; //����� ���� ��ġ�� ����
    float speed;    //�÷��̾� �̵��ӵ�

    [Header("�⺻ ���� ������ �ִ� ���� �ݶ��̴� ����Ʈ")]
    public Collider[] hitColliders; //�⺻ ���� ������ �ִ� ���� �ݶ��̴� ����Ʈ

    [Header("�÷��̾� ��ų ���� ���������Ǿ ������ ���̾�")]
    public LayerMask DeTectMonster;   //���������Ǿ ������ ���̾��

    [Header("�÷��̾� ")]

    [Header("�÷��̾� ��ų 2���� ��������� �ƴ��� Ȯ���ϱ� ���� bool�ڷ���")]
    public bool isSwordMove = false;   //��ų 2���� ��������� �ƴ��� Ȯ���ϱ� ���� bool�ڷ���
    [Header("�÷��̾� ��ų 2�� ������ �����ϴ� ���͵��� ������ ����Ʈ")]
    public List<GameObject> monsters = new List<GameObject>();  //�÷��̾� ��ų 2�� ������ �����ϴ� ���͵��� ������ ����Ʈ

    [Header("�÷��̾� ��ų 2�� ������ ����ϴ� �ݶ��̴�")]
    public BoxCollider boxCol;  // ��ų 2���� ���� �ڽ� �ݶ��̴�

    [Header("��ų 3�� �ƿ�� ������Ʈ")]
    public GameObject swordAuraPrefab;  //��ų 3���� ���� ������ ������Ʈ

    [Header("�÷��̾� ��ų ��Ÿ�ӽ� ��ư �̹��� filled ����")]
    public Image skill1_Cool;
    public Image skill2_Cool;
    public Image skill3_Cool;

    //�÷��̾� ��ų ��Ÿ�� ����
    public float[] skill_delays = new float[3];    //��ų 1,2,3 ������ ������ �迭

    [Header("��ų 1,2,3 �� ��� �������� �������� ���� bool�ڷ��� �迭")]
    [SerializeField]
    bool[] isCanSkill = new bool[] {false,false,false}; //��ų 1,2,3 �� ��� �������� �������� ���� bool�ڷ��� �迭

    [Header("��ų ��ư �迭")]
    public Button[] skill_Btns; //��ų ��ư �迭

    [Header("��ų ��Ÿ�� �̹��� �迭")]
    public Image[] skill_Cools; //��ų ��Ÿ�� �̹��� �迭

    [Header("��ų ��Ÿ�� �迭 ����")]
    [SerializeField]
    float[] skill_CoolValues={ 15, 20, 25 };    //��ų ��Ÿ�� �迭 ���� ����

    public bool isSkillDoing=false;
    public GameObject skill2_effect;
    public GameObject skill3_effect;

    public Text destinationTxt;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();  //rigdbody ��������
        cc = GetComponent<CharacterController>();   //ĳ���� ��Ʈ�ѷ� ��������
        speed = GetComponent<NewPlayer>().playerMoveSpeed;    //NewPlayer ��ũ��Ʈ���� speed�� ��������
        NewplayerSc = GetComponent<NewPlayer>();    //NewPlayer ��ũ��Ʈ ��������
        anim = GetComponent<Animator>();    //Animator ������Ʈ ��������
        int swordFire = 0;
        int swordMove = 1;
        int fallDownSword = 2;

        skill_Btns[0].onClick.AddListener(() => StartCoroutine(DoSkill(swordFire))); //��ų 1��ư�� Skill��ũ��Ʈ�� doskill�Լ��� swordFire�� string�ڷ����� �Ű������� ����
        skill_Btns[1].onClick.AddListener(() => StartCoroutine(DoSkill(swordMove))); //��ų 2��ư�� Skill��ũ��Ʈ�� doskill�Լ��� swordMove�� string�ڷ����� �Ű������� ����
        skill_Btns[2].onClick.AddListener(() => StartCoroutine(DoSkill(fallDownSword))); //��ų 3��ư�� Skill��ũ��Ʈ�� doskill�Լ��� fallDownSword�� string�ڷ����� �Ű������� ����

       
    }

    void Update()
    {

        //������Ʈ x ����Ű�� ��������
        //�ڷ�ƾ�Լ� ������Ʈ��ó�� ���
        SkillCoolTimeCheck();
    }

    private void OnTriggerEnter(Collider other) //Trigger üũ�Ǿ������� �浹�� ȣ��
    {
        if (other.tag == "Monster" && isSwordMove)   //�浹�� ��ü�� �±װ� �����̰� isSwordMove�� true�϶�
        {
            print("�����߰�");
            monsters.Add(other.gameObject); //��ų2���� �ݶ��̴��� Ȱ��ȭ �Ǿ������� �浹�� ���͵��� ����Ʈ�� ������ �߰���
        }

        else if (other.tag == "Portal")   //�÷��̾ ��Ż�̶� �浹���϶�
        {
            AudioManager.instance.Sound_Item(AudioManager.instance.soundsItem[4]);
            UiManager.instance.onPortal = true; //UiManager�� onPortal �� true
            UiManager.instance.portalName = other.name; //���� �浹���� ��Ż�̸� ����
        }

        if (other.name=="StagePortal")
        {
            // ���ݹ�ư �̹��� �������� �̹����� ����
            UiManager.instance.AttackBtnImg.sprite = UiManager.instance.goStageBtnImg;
            UiManager.instance.AttackBackBtnImg.sprite = UiManager.instance.goStageBtnImg;
        }

        if (other.name == "StorePortal")
        {
            // ���ݹ�ư �̹��� ���� �̹����� ����
            UiManager.instance.AttackBtnImg.sprite = UiManager.instance.goShopBtnImg;
            UiManager.instance.AttackBackBtnImg.sprite = UiManager.instance.goShopBtnImg;
            StartCoroutine(UiManager.instance.NpcTalk(true)); // NPC ��ȣ�ۿ� �Լ� ȣ��
        }
    }

    private void OnTriggerExit(Collider other)  //�浹�� �������� ȣ��
    {
        if (other.tag == "Portal")  //�浹�� ���� ��ü�� �±װ� Portal�̶��
        {
            UiManager.instance.onPortal = false; //UiManager�� onPortal �� false
            UiManager.instance.portalName = null;   //���� �浹���� ��Ż�̸� null
        }

        if (other.name == "StagePortal"|| other.name == "StorePortal")
        {
            // ���ݹ�ư �̹��� ����
            UiManager.instance.AttackBtnImg.sprite = UiManager.instance.atkBtnImg;
            UiManager.instance.AttackBackBtnImg.sprite = UiManager.instance.atkBtnImg;
            if (other.name == "StorePortal")
            {
                StartCoroutine(UiManager.instance.NpcTalk(false)); // NPC ��ȣ�ۿ� �Լ� ȣ��
            }
        }
    }

    #region * MonsterCheck()�Լ� - �÷��̾� �ֺ��� ���Ͱ� �ֳ� ���� üũ�ϴ� �Լ�
    public void MonsterCheck()
    {
        hitColliders = Physics.OverlapSphere(transform.position, 6f, DeTectMonster);    //���������Ǿ�� DetectMonster�� �����ϴ� hitColider����
        if (hitColliders.Length == 0)    // �ݶ��̴� ����Ʈ�� ���� ���ٸ�
        {

            NewplayerSc.RealAttack();   // �÷��̾ ���ڸ����� �����ϵ��� PNewplayerSc.RealAttack() �Լ� ȣ��
            NewplayerSc.canInput = true;    //�÷��̾� �̵� �����ϵ��� canInput�� true
        }

        else
        {

            float nearDistance = Vector3.Distance(transform.position, hitColliders[0].transform.position);    //hitColliders ����Ʈ ������ ù��° ������ ��ġ�� �÷��̾��� ��ġ �Ÿ��� nearDistance���� �־��ְ�
            GameObject nearMonster = hitColliders[0].gameObject;    //nearMonster ������Ʈ�� hitColliders�� ù��° ���ӿ�����Ʈ�� �־���

            for (int i = 0; i < hitColliders.Length; i++)    //hitColliders����Ʈ�� ����ŭ �ݺ��� �ݺ�
            {

                float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);  //hitColliders ����Ʈ ������ i��° ������ ��ġ�� �÷��̾��� ��ġ �Ÿ��� distance���� �־��ְ�

                if (nearDistance > distance)  //nearDistance���� i��° ���Ϳ��� �Ÿ��� distance�� ���ؼ� distance ���� �� ������ nearDistance���� distance ���� �ְ� �� ���ӿ�����Ʈ�� nearMonster�� �����ϵ���
                {
                    nearDistance = distance;
                    nearMonster = hitColliders[i].gameObject;
                }

            }
            StartCoroutine(DoCheck(nearMonster.transform, "attack",nearMonster.GetComponent<Enemy>().isGround));    //��ŸƮ �ڷ�ƾ�Լ� Docheck�� "attack"�� �Ű������� ����
        }

        #endregion

    }
    #region * Docheck �ڷ�ƾ �Լ� - �÷��̾ � �ൿ�� ���� üũ�ϴ� �Լ��� �Ű������� Transform�ڷ��� monsterDistance, string�ڷ��� doing�� ��� / MonsterCheck()�Լ����� ȣ��, DoSkill�Լ����� 2�� ȣ��
    //Ư����Ȳ������ ����ؾ��ϴµ� update��ó�� ��� ȣ���ؾ� �Ҷ� �ڷ�ƾ�� ������Ʈ��ó�� ���
    IEnumerator DoCheck(Transform monsterPosition, string doing, bool isGround)     //�÷��̾� �ֺ��� �ִ� ���� Ȯ�� �� �� ���Ϳ��� �޷���� �Լ�
    {

        NewplayerSc.canInput = false;   //�÷��̾� �Է� �Ұ����ϵ��� canInput �� false�� ����
        transform.LookAt(monsterPosition);    // �÷��̾�� �Ÿ��� �ش��ϴ� ������ ��ġ�� �ٶ󺸰�
        monsterDir = monsterPosition.position - transform.position; //��ó���� ��ġ���� �÷��̾��� ��ġ�� ������ Vector3������ �־��ְ�
        monsterDir = monsterDir.normalized;   //monsterDir ����ȭ ���͸� ���� ���� ����ȭ


        switch (doing)  // ���ݻ����϶�, �����幫�� ��ų ������϶�, ���ٿ� ������ �̵����϶� ���
        {
            case "attack":  //attack�� �Ű������� �޾��� ��
                if (monsterPosition != null&&(monsterPosition.position - transform.position).magnitude > 2f&&isGround==true)
                {
                    anim.SetTrigger("dash");  //�뽬 �ִϸ��̼� Ȱ��ȭ
                }
                
                while (monsterPosition!= null && (monsterPosition.position - transform.position).magnitude > 2f && isGround == true)    //���� �Ÿ����� �����Ÿ����� Ŀ��������
                {
                    cc.Move(monsterDir * speed * 3 * Time.deltaTime); //�÷��̾� ������Ʈ �̵�
                    yield return null;
                }
                monsterPosition.SendMessage("MonsterDamaged", DataManager.instance.data.playerAttackDmg);    //���Ϳ��� SendMassge�Լ��� Damaged�Լ� ȣ���Ͽ� �÷��̾� attackDmg��ŭ ����HP ��� ��
                NewplayerSc.RealAttack();  //�÷��̾� �⺻����
                yield return new WaitForSeconds(0.8f);
                NewplayerSc.canInput = true;    //�÷��̾� �̵� ����
                break;


            case "swordMove":   //��ų 2�� ������϶�
                AudioManager.instance.Sound_Skill(AudioManager.instance.soundsSKill[Random.Range(2,4)]);
                
                boxCol.enabled = false; //��ų 2���� ���� �ڽ��ݶ��̴� ��Ȱ��ȭ
                isSwordMove = false;    //�÷��̾ �浹�Ҷ����� ���� ����Ʈ�� �߰��ϴ� �ڵ� �������� �ʵ��� isSwordMove�� false�� ����
                anim.SetTrigger("dash");  //�ִϸ��̼� Ȱ��ȭ

                Vector3 destination = monsterPosition.position + transform.forward*3; // �� ������ ���͸� �������� ���� ��ǥ������ ���� ��ġ���� �÷��̾��� forward�� ������
                //destinationTxt.text= "��ǥ : "+destination.ToString();
                while (monsterPosition!=null&&(destination - transform.position).magnitude > 1.2f )   // ��ǥ������ ���������� �̵�
                {
                    Time.timeScale = 0;
                    cc.Move(monsterDir * speed * 4 * Time.unscaledDeltaTime);   //�÷��̾� �̵�
                    skill2_effect.SetActive(true);
                    destination.y = transform.position.y;   //�÷��̾ ���� �ھƿ����� �ʵ��� �ϱ� ���� ����
                    print("���� ���Ⱚ:"+monsterDir);
                    print("�̵����� ���Ⱚ:"+(destination - transform.position).normalized);
                    yield return null; //�� �����Ӿ� ����

                }

                yield return new WaitForSecondsRealtime(1f);
                skill2_effect.SetActive(false);
                Time.timeScale = 1;
                foreach (var item in monsters)  //���� ����Ʈ ������ item�� �־�
                {
                    item.SendMessage("MonsterDamaged", DataManager.instance.data.playerSkillDmg * 1.2f);    //item���� sendMessage�Լ��� damaged �Լ� ȣ��
                    item.GetComponent<Collider>().isTrigger = false;
                }

                monsters.RemoveRange(0, monsters.Count);    //�������� ���� ����Ʈ ���� ����

                isSkillDoing = false;
                NewplayerSc.canInput = true;    //�÷��̾� �̵� ���� bool�ڷ���
                NewplayerSc.isDamaged = false;  //�÷��̾� �������� ����
                break;



            case "fallDownSword":   //��ų 3�� ������϶�
                skill3_effect.SetActive(true);
                AudioManager.instance.Sound_Skill(AudioManager.instance.soundsSKill[5]);
                while (monsterPosition!= null && (monsterPosition.position - transform.position).magnitude > 2f)    //�Ÿ����� �����Ÿ����� Ŭ�� ����
                {
                    cc.Move(monsterDir * speed * 4 * Time.deltaTime); //�÷��̾� ������Ʈ �̵�
                    yield return null;
                }
                
                isSkillDoing=false;
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); //while���� ������ �÷��̾��� ������ transform.eulerAngles.y�� ����
                break;
        }
    }
    #endregion

    #region * Doskill �ڷ�ƾ �Լ� - ��ų�̸��� �Ű������� �޾� ��ų�̸����� Ư���ൿ�� �ϵ��� �ϴ� �Լ�
    public IEnumerator DoSkill(int skiilName)   //��ų ��� �Լ� , �Ű������� int �� �޾ƿ�(������ ���� string������ �۾Ƽ�)
    {

        switch (skiilName)  //��ų�̸��� ���� �������� ���
        {
            case 0:
                if (isCanSkill[0]&& !isSkillDoing)
                {
                    AudioManager.instance.Sound_Skill(AudioManager.instance.soundsSKill[0]);
                    isSkillDoing = true;
                    skill_delays[0] = 0;
                    

                    GameObject aura1;   //�ƿ�� ������Ʈ 1
                    GameObject aura2;   //�ƿ�� ������Ʈ 2
                    GameObject aura3;   //�ƿ�� ������Ʈ 3



                    anim.SetTrigger("swordAura");
                    aura1 = Instantiate(swordAuraPrefab, transform.position + new Vector3(0, 0.7f, 0), transform.rotation);   //�÷��̾� ��ġ�� �÷��̾� ������ swordAuraPrefab�� aura1�� ������
                    aura1.gameObject.name = "aura1";

                    Quaternion aura2Dir = Quaternion.Euler(new Vector3(0, 30, 0));  //aura2�� ���� ������ ���� aura2Dir ����
                    aura2 = Instantiate(swordAuraPrefab, aura1.transform.position, aura1.transform.rotation * aura2Dir);    //aura1��ġ�� aura1������ aura2Dir�� ������ swordAuraPrefab�� �����ؼ� aura2�� ���� ,rotation���� *�� +�� ����.
                    aura2.gameObject.name = "aura2";

                    Quaternion aura3Dir = Quaternion.Euler(new Vector3(0, -30, 0)); //aura3�� ���� ������ ���� aura3Dir ����
                    aura3 = Instantiate(swordAuraPrefab, aura1.transform.position, aura1.transform.rotation * aura3Dir);    //aura1��ġ�� aura1������ aura3Dir�� ������ swordAuraPrefab�� �����ؼ� aura3�� ���� ,rotation���� *�� +�� ����.
                    aura3.gameObject.name = "aura3";

                    yield return new WaitForSeconds(0.2f);


                    Destroy(aura1, 3);  //3�ʵ� aura1 ������Ʈ �ı�
                    Destroy(aura2, 3);  //3�ʵ� aura2 ������Ʈ �ı�
                    Destroy(aura3, 3);  //3�ʵ� aura3 ������Ʈ �ı�
                    isSkillDoing = false;
                }
                break;

            case 1:   //2��° ��ų �̸� swordMove�϶�
                if (isCanSkill[1]&& !isSkillDoing)
                {
                    AudioManager.instance.Sound_Skill(AudioManager.instance.soundsSKill[1]);
                    isSkillDoing = true;
                    skill_delays[1] = 0;
                    NewplayerSc.isDamaged = true;   //�÷��̾� ��������
                    
                    yield return new WaitForSeconds(1.2f);
                    boxCol.enabled = true;  //�Ͻ������� �ڽ��ݶ��̴��� Ȱ��ȭ ��Ŵ
                    isSwordMove = true; //NewPlayer ��ũ��Ʈ�� OnTriggerEnter�Լ��� ����ϱ����� isSwordMove�� ture ��ȯ
                    float farDistance = 0;
                    GameObject farMonster = null;
                    yield return new WaitForSeconds(0.1f);
                    if (monsters.Count == 0)  //���� �ݶ��̴��� �ε��� ���͵��� ���ٸ�
                    {
                        NewplayerSc.isDamaged = false;   //�÷��̾� ��������
                        boxCol.enabled = false; //��ų 2���� ���� �ڽ��ݶ��̴� ��Ȱ��ȭ
                        isSwordMove = false;    //isSwordMove�� false�� ��ȯ�Ͽ� ����Ʈ�� �� �߰� �ȵǵ��� ��
                        NewplayerSc.canInput = true;    //�÷��̾� Ű�Է� ����
                        isSkillDoing = false;
                        skill_delays[1] = 15;
                    }

                    else
                    {
                        farDistance = Vector3.Distance(transform.position, monsters[0].transform.position);    //monsters����Ʈ�� ù��° ���� ��ġ�� �÷��̾��� ��ġ�� �Ÿ��� farDistance���� �־���
                        farMonster = monsters[0].gameObject; //farMonster�� monsters����Ʈ�� ù��° ������Ʈ�� ����


                        for (int i = 0; i < monsters.Count; i++)    // 0���� ���� monsters����Ʈ ��������
                        {
                            monsters[i].GetComponent<Collider>().isTrigger = true;
                            float distance = Vector3.Distance(transform.position, monsters[i].transform.position);
                            if (farDistance < distance) //���� �� �Ÿ��� ���ο� �Ÿ����� ���� ������
                            {
                                farDistance = distance; //���� �հŸ��� ���ο� �Ÿ� �� ����
                                farMonster = monsters[i];   //���� �� ���Ϳ� i��° ����Ʈ ������Ʈ �� ����
                            }
                        }
                        StartCoroutine(DoCheck(farMonster.transform, "swordMove",true));  //���� �� ���Ϳ��� �޷����� �Լ� ����

                    }
                }
                break;


            case 2:   //��ų 3�� ��ų �̸��� fallDownSword �϶�
                if (isCanSkill[2]&&!isSkillDoing)
                {
                    AudioManager.instance.Sound_Skill(AudioManager.instance.soundsSKill[4]);
                    isSkillDoing = true;
                    skill_delays[2] = 0;
                    float time = 0;
                    NewplayerSc.canInput = false;   //�÷��̾� �������� �Ұ����ϵ��� canInput=false 
                    
                    while (time < 1.3f)    //time�� 2�ʺ��� ����������
                    {
                        anim.SetFloat("move", speed);   //Move�ִϸ��̼��� Ȱ��ȭ �ϰ�
                        time += Time.deltaTime;     //time �� ��� ���� �����ָ鼭
                        cc.Move(transform.forward * speed * 2 * Time.deltaTime);    //�÷��̾ 2�ʵ��� �÷��̾ �ٶ󺸴� �������� �̵���
                        yield return null;
                    }
                    anim.SetFloat("move", 0);   //move�ִϸ��̼� ��0
                    time = 0;
                    anim.SetTrigger("fallDown");    //fallDown�ִϸ��̼� Ȱ��ȭ
                    while (time < 2)    //time�� 2�ʺ��� ����������
                    {
                        time += Time.deltaTime; //Ÿ�ӿ� ��� ���� �����ָ鼭
                        NewplayerSc.gravity = 0;    //�Ͻ������� �߷� 0���� ����
                        cc.Move(transform.up * speed * 2 * Time.deltaTime); //�÷��̾ ���� �̵���

                        anim.SetFloat("multipleJump", 0.25f);   //�ִϸ��̼��� ������ �ൿ�ϱ� ���� multipleJump�� 0.25f�� �־���
                        yield return null;
                    }
                    Collider[] skillHitCols; //��ų3 ������ �ִ� ���� �ݶ��̴� ����Ʈ

                    skillHitCols = Physics.OverlapSphere(transform.position + (-transform.up * 7 ), 6f, DeTectMonster);  //���������Ǿ�ȿ� �ִ� �ݶ��̴����� skillHitCols�ȿ� �־���

                    if (skillHitCols.Length == 0)   // skillHitCols�� ���̰� 0�̸� -> Ž���� ���Ͱ� ������
                    {
                        NewplayerSc.gravity = -3;   //�÷��̾�� �߷� ���� ��
                        anim.SetFloat("move", 0);
                        skill_delays[2] = 15;
                        NewplayerSc.canInput = true;    //�÷��̾� �̵� �����ϵ��� ����
                        isSkillDoing = false;
                        break;
                    }

                    float farDistance2 = Vector3.Distance(transform.position, skillHitCols[0].transform.position);    //skillHitCols�ݶ��̴��� ù��° ���� ��ġ�� �÷��̾��� ��ġ���� �Ÿ��� farDistance2�� �־��ְ�
                    GameObject farMonster2 = skillHitCols[0].gameObject;     //skillHitCols�ݶ��̴��� ù��° �� ������Ʈ�� farMonster2�� �־���

                    for (int i = 0; i < skillHitCols.Length; i++)    //�ݶ��̴� ����Ʈ�� ����ŭ �ݺ��� �ݺ�
                    {

                        float distance = Vector3.Distance(transform.position, skillHitCols[i].transform.position); //skillHitCols�� i��° ������Ʈ ��ġ�� �÷��̾�� �Ÿ��� ���� ���� distance�� �־��� ��
                        if (farDistance2 < distance)  //�Ÿ��� ���ؼ� distance�� ���� �� �ָ�
                        {
                            farDistance2 = distance;    //farDistance2�� distance ���� �־��ְ�
                            farMonster2 = skillHitCols[i].gameObject;   //farMonster2 ������Ʈ�� skillHitCols�� i��° ���� ������Ʈ�� �־���
                        }

                    }
                    anim.SetFloat("multipleJump", 0.3f);    //multiplejump�� 0.3���� ��ȯ

                    StartCoroutine(DoCheck(farMonster2.transform, "fallDownSword",true));    //��ŸƮ �ڷ�ƾ �Լ� Docheck ȣ��

                    for (int i = 0; i < skillHitCols.Length; i++)
                    {
                        skillHitCols[i].gameObject.SendMessage("MonsterDamaged", DataManager.instance.data.playerSkillDmg * 2f); //skillHitCols�� ����Ʈ���鿡�� sendMessage�� �Լ� ȣ��
                    }
                    yield return new WaitForSeconds(0.32f);
                    NewplayerSc.gravity = -3;   //�÷��̾� �߷� ����
                    anim.SetFloat("move", 0);
                    NewplayerSc.canInput = true;    //�÷��̾� �̵������ϵ��� ����
                    yield return new WaitForSeconds(2f);
                    skill3_effect.SetActive(false);
                }
                break;


        }
    }
    #endregion

    void SkillCoolTimeCheck()   //��ų ��Ÿ�� �Լ�
    {
        for (int i = 0; i < 3; i++)
        {
            if (!isCanSkill[i]) // i��° ��ų 
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
        Gizmos.color = Color.red;   //�÷��̾� �⺻ ���� ������ ���� �����
        Gizmos.DrawWireSphere(transform.position, 6f);
        Gizmos.color = Color.blue; //�÷��̾� ��ų 3���� ���� �����
        Gizmos.DrawWireSphere(transform.position + (-transform.up * 7), 6f);

    }
}
