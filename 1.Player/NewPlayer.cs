using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayer : MonoBehaviour
{
    
    public Animator anim;  // �÷��̾� �ִϸ��̼��� ���� ������
    public Skill skillSc;  // �÷��̾� ��ų ��ũ��Ʈ

    [Header("�÷��̾� ���̽�ƽ(�̵�����)")]
    public PlayerJoystic joysticSc;
    CharacterController cc; //ĳ���� ��Ʈ�ѷ�(����)

    [Header("�÷��̾� �̵���")]
    public Vector3 moveVec;   //���� �̵���

    [Header("�÷��̾� �̵��ӵ�")]
    public float playerMoveSpeed; //�÷��̾� �̵��ӵ�

    [Header("�÷��̾ ������ �� �ֳ� �Ǵ��ϴ� bool�ڷ���")]
    public bool canInput = true;  //    �÷��̾ ������ �� �ֳ� �Ǵ��ϴ� bool�ڷ���


    [Header("�÷��̾� ����, ���� ��Ÿ�� ����")]
    public Button attackBtn;
    public Image atkCool;
    public float atkSpeed;   //���ݼӵ�
    public float atkDelay;   // ���ݵ�����
    public bool isCanAttack = false; // ����Ű �Է°���

    [Header("�÷��̾� �뽬, �뽬 ��Ÿ�� ����")]
    public Button dash_Btn;
    public Image dash_Cool;
    public float dashDelay = 5f; //�뽬 ������
    public bool isCanDash = false;   // �뽬����� �����Ҷ����� ǥ�õǴ� bool�ڷ���
    //
    [Header("�÷��̾� �������� üũ")]
    public bool isDamaged=false;
    
    [HideInInspector]
    public float gravity = -3f; //������ �߷°�

    [Header("�÷��̾� �޽������� �迭")]
    public SkinnedMeshRenderer[] meshs;

    [Header("�÷��̾� ü�¹� �����̴�")]
    public Image playerHpImg;
    public Slider playerHpbar;

    [Header("�÷��̾� ü�� �ؽ�Ʈ")]
    public Text playerHpTxt;

    [Header("�÷��̾� ���� �ؽ�Ʈ")]
    public Text levelTxt;

    [Header("�÷��̾� ���� ������")]
    public Item equipWeapon;

    public GameObject dashEffect;
    private void Awake()
    {
        cc = GetComponent<CharacterController>();   //ĳ���� ��Ʈ�ѷ� ��������
        anim = GetComponent<Animator>();    //�ִϸ����� ��������
        skillSc = GetComponent<Skill>();    // �÷��̾��� skill��ũ��Ʈ ��������
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();

    }

    void Start()
    {
        dash_Btn.onClick.AddListener(() => StartCoroutine("Dash")); // �뽬 ��ư�� ��ŸƮ�ڷ�ƾ �Լ��� Dash�� string �ڷ����� �Ű������� ����
        levelTxt.text = "Lv " + DataManager.instance.data.playerLv.ToString();
        DataManager.instance.data.nowPlayerHp = DataManager.instance.data.playerMaxHp;
    }


    void Update()
    {
        
        playerHpTxt.text = DataManager.instance.data.nowPlayerHp.ToString() + "/" + DataManager.instance.data.playerMaxHp;
        playerHpImg.fillAmount = DataManager.instance.data.nowPlayerHp / DataManager.instance.data.playerMaxHp;

        anim.SetFloat("multipleJoyStickDistance", joysticSc.joysticDistance);
        if (atkDelay < 1.6f - DataManager.instance.data.attackSpeed)  //atkDelay�� 1.6f���� ������
        {
            atkDelay += Time.deltaTime; //atkDelay ������ �������� �ð��� �����ְ�

            isCanAttack = atkDelay > 1.6f - DataManager.instance.data.attackSpeed; //atkDelay�� 1.6f ���� ũ�� inCanAttack�� true�� ����
        }
        if (isCanAttack)   //inCanAttack�� true�϶�
        {
            attackBtn.interactable = true;
        }
        DoDash();   //�뽬�Լ�

        dash_Cool.fillAmount = dashDelay / (10 - DataManager.instance.data.playerSkillCool); //�뽬�� �����̵��� �������Ӹ��� �ٲ�� �뽬�� �����̴� ���
        atkCool.fillAmount = atkDelay / (1.6f - DataManager.instance.data.attackSpeed);   //�⺻ ���� ��� �����̵��� �������Ӹ��� �ٲ�� ������ �����̴� ���

        if (canInput&&joysticSc.isTouch)   //canInput�� true �϶� �÷��̾ ������ �� ����
        {
            Move(); //�÷��̾ ������ �� �ִ� ����� �ϴ� Move()�Լ� ȣ��
        }
        else
        {
            moveVec = new Vector3(0, 0, 0);
            float moveSpeed = moveVec.magnitude;    //�÷��̾� �̵����� ũ�⸦ ����
            anim.SetFloat("move", moveSpeed);
            
            
        }
        if (!cc.isGrounded) // �÷��̾ ���� ���� ������
        {
            moveVec.y += gravity;   //�÷��̾� �̵����� y���� -3�� ��� ������
        }
        else if (cc.isGrounded)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            
        }
        if (cc.enabled!=false)
        {
            cc.Move(moveVec * playerMoveSpeed * joysticSc.joysticDistance * Time.deltaTime);  //�÷��̾� �̵�
        }
        



    }

    #region * Move�Լ� - �÷��̾��� �����̴� ����� ����  - > �÷��̾� ������Ʈ �Լ����� ȣ��
    /// <summary>
    /// ĳ���� ��Ʈ�ѷ��� �÷��̾� ������ ����
    /// �÷��̾ �밢������ �̵��� ���� ������ �ӵ��� �յ� �¿� �̵��ӵ��� ���� �ϱ� ���� ����ȭ ���
    /// �÷��̾� �̵��ӵ� ���� ũ�⸦ magnitude�� ���Ͽ� �̵��ӵ� ���� ���� �÷��̾��� �ٴ� �ִϸ��̼��� �����ϵ��� ����
    /// </summary>
    void Move()
    {
        moveVec = new Vector3(joysticSc.value.x, 0, joysticSc.value.y);  //�÷��̾� �̵��� ����ȭ

        if (!AudioManager.instance.audioWalkkSound.isPlaying)
        {
            AudioManager.instance.audioWalkkSound.pitch = joysticSc.joysticDistance + 0.5f;
            AudioManager.instance.Sound_Walk(AudioManager.instance.soundsWalk[Random.Range(0, 6)]);
            

        }
        transform.LookAt(transform.position + moveVec); //�÷��̾ �̵��� ���� �Ĵٺ� �� �ְ���

        float moveSpeed = moveVec.magnitude;    //�÷��̾� �̵����� ũ�⸦ ����
        anim.SetFloat("move", moveSpeed);

        

        
    }
    #endregion

    #region * Attack�Լ� - isCanAttack�� true�϶� canInput�� false�� �ǰ� �ֺ��� ���͸� Ž���ϴ� skillSc.MonsterCheck()�Լ��� ȣ��
    public void Attack()   //���� ����
    {
        if (isCanAttack)      //isCanAttack 
        {
            canInput = false;   //�÷��̾ ������ �� �ְ� ���������ִ� canInput bool �ڷ����� false�� �Է��Ͽ� ���ݻ��½� �÷��̾ �������̵��� ����
            attackBtn.interactable=false; // if ���ǹ��� Ʈ���ϵ��� attack��ư ������Ʈ ��Ȱ��ȭ
            AudioManager.instance.Sound_Attack(AudioManager.instance.soundsAttack[Random.Range(1,7)]);
            skillSc.MonsterCheck(); //Skill ��ũ��Ʈ�� MonsterCheck�Լ� ȣ��
        }
    }
    #endregion

    #region DoDash �Լ�- dashDelay���� ���� isCanDash�� true�� �ǵ��� ����
    /// <summary>
    /// �÷��̾ �뽬�� �� �� �ֵ��� ����ϴ� �Լ�
    /// �������� ��Ÿ�Ӱ����� ������ dashDelay������ time.DeltaTime���� ��Ӵ����ִٰ� ���������� dashDelay�� Ŀ����
    /// bool �ڷ��� ���� isCanDash �� true�� �ٲ��ְ� �뽬��ư ������Ʈ�� Ȱ��ȭ ������
    /// </summary>
    void DoDash()
    {
        if (10f - DataManager.instance.data.playerSkillCool > dashDelay)    //���� �뽬 �����̼ӵ��� 2f���� ���� ������
        {
            
            dashDelay += Time.deltaTime; //dashDelayrkqtdp time.deltaTime ���� ��� �����ְ�
            isCanDash = 10f - DataManager.instance.data.playerSkillCool < dashDelay; //dashDelay���� 10f���� Ŀ���� isCanDash bool �ڷ����� true�� �������
        }
        
        if (isCanDash)
        {
            dash_Btn.interactable=true;
        }
    }

    #endregion

    #region * Dash �ڷ�ƾ �Լ� - �뽬��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    /// <summary>
    /// ȣ�� �� Dash��ư�� ��Ȱ��ȭ �ǰ� canINput�� false�� �Ǿ� �Է��� �Ұ������� ĳ���Ͱ� �ٶ󺸴� �������� 1�� ���� �̵���
    /// </summary>
    /// <returns></returns>
    IEnumerator Dash()  //�뽬 �ڷ�ƾ �Լ�
    {
        dashEffect.SetActive(true); //�뽬 ����Ʈ ���ӿ�����Ʈ Ȱ��ȭ
        dash_Btn.interactable = false;  //�뽬 ��ư ��Ȱ��ȭ
        skillSc.isSkillDoing = true;    // ��ų ������ΰ� Ȯ���ϴ� bool�ڷ���
        AudioManager.instance.Sound_Attack(AudioManager.instance.soundsAttack[0]);
        canInput = false;   // �÷��̾� �̵� �Ұ�
        dashDelay = 0;
        anim.SetTrigger("dash");    //�뽬 �ִϸ��̼� ����
        playerMoveSpeed *= 2.5f;
        float time = 0;
        
        while (time<1f) //time �� 1�� ���� ������ ����
        {
            time += Time.deltaTime;
            cc.Move(transform.forward * playerMoveSpeed * Time.deltaTime); // �÷��̾� z��������� �̵�
            isDamaged = true;
            yield return null;

        }


        dashEffect.SetActive(false);
        canInput = true;    //�÷��̾� �̵� ����
        skillSc.isSkillDoing = false;
        isDamaged = false;
        playerMoveSpeed *= 0.4f;
    }

    #endregion

    #region * RealAttack()�Լ� - ���ݾִϸ��̼��� Ȱ��ȭ�ǰ� atkDelay�� 0���� ����
    /// <summary>
    /// 
    /// </summary>
    public void RealAttack()    //�÷��̾� ���� �ִϸ��̼� �����ϴ� �Լ�
    {
        anim.SetTrigger("attack");    // ���� �ִϸ��̼� Ȱ��ȭ
        atkDelay = 0;   //���� ������ 0
        
    }
    #endregion

    #region * Damaged�Լ� - �÷��̾ ���Ϳ��� ���ݴ����������� ����Ǵ� �Լ�(���� ���� ������ ����������� ȣ��)
    IEnumerator Damaged(float monsterDmg)
    {
        isDamaged = true;   //�÷��̾ Ÿ���� �Ծ����� Ȯ���ϴ� bool�ڷ���
        anim.SetTrigger("triggerDamaged");
        foreach (SkinnedMeshRenderer playerPart in meshs)  //�ǰݽ� �÷��̾� ĳ���� ���� �ٲ�
        {
            playerPart.material.color = Color.yellow;   //Ÿ�ݽ� ��Ų�޽������� Material color yellow��
        }
        DataManager.instance.data.nowPlayerHp -= monsterDmg;
        if (DataManager.instance.data.nowPlayerHp <= 0)
        {
            AudioManager.instance.Sound_Attack(AudioManager.instance.soundsAttack[7]);  //���ӿ��� ���� ����
            AudioManager.instance.audioWalkkSound.Stop();
            anim.SetFloat("move", 0);   //�̵� �ִϸ��̼� ��� X
            joysticSc.rect_Joystick.position = joysticSc.rect_Joystick.parent.position; //�Ͼ�� ���̽�ƽ ��ġ ���� �θ� ��ġ�� ����
            //**�÷��̾� �̵��� 0
            joysticSc.value.x = 0;
            joysticSc.value.y = 0;
            moveVec = new Vector3(0, 0, 0);
            //*
            anim.SetBool("isDead", true);   // �÷��̾� ���� �ִϸ��̼� ����
            joysticSc.isTouch = false;  //�÷��̾ ���̽�ƽ ��ġ������ Ȯ���ϴ� bool�ڷ��� �� false �� ����
            joysticSc.enabled = false;  //�÷��̾� ���̽�ƽ ��ũ��Ʈ ��Ȱ��ȭ
            gameObject.GetComponent<NewPlayer>().enabled = false;   //�÷��̾� ��ũ��Ʈ ��Ȱ��ȭ
            skillSc.enabled = false;    //�÷��̾� ��ų��ũ��Ʈ ��Ȱ��ȭ
            cc.enabled = false; //ĳ���� ��Ʈ�ѷ� ������Ʈ ��Ȱ��ȭ
            GameObject.FindObjectOfType<StageManager>().WaveFailEnding();   // ���̺� ���������� �ߴ� �˾�â Ȱ��ȭ ��Ű�� �Լ� ����
        }
        //�˹� �����
        yield return new WaitForSeconds(1.3f);

        foreach (SkinnedMeshRenderer playerPart in meshs)  //�ǰ� �� �÷��̾� ĳ���� ���� ����
        {
            playerPart.material.color = Color.white;
        }
        isDamaged = false;
    }
#endregion

    void ClearStage(bool clear) //�������� Ŭ���� ������ ����Ǵ� �Լ�
    {
        anim.SetBool("isClearStage",clear);
    }
}

