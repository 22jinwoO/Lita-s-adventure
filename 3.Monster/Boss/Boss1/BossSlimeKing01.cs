using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSlimeKing01 : Enemy
{
    [Header("보스 패턴을 위한 돌 오브젝트")]
    public GameObject rock;
    public SphereCollider attackRangeColider;
    Vector3 lookVec;
    public GameObject attackRangeOb;
    bool isLook=true;
    NewPlayer newPlayerSc;
    public Text nowHpTxt;
    public Text bossNameTxt;

    bool isDoingSkill=false;
    public GameObject downEffect;   // 이펙트 오브젝트
    [SerializeField]
    BoxCollider monsterBodyCol;  //몬스터 바디 박스콜라이더
    [Range(0, 1)]
    public float t;
    Camera cam;

    void Start()
    {
        Mathf.Clamp(monsterHp, 0, monsterHp);
        monsterHp = monsterInfo.monsterMaxHp;
        gameObject.name = monsterInfo.monsterName;
        monsterHpBar.maxValue = monsterInfo.monsterMaxHp;
        monsterHpBar.value = monsterHp;
        rigd = GetComponent<Rigidbody>();
        playerOb = GameObject.FindObjectOfType<CharacterController>().gameObject;
        newPlayerSc = playerOb.GetComponent<NewPlayer>();
        waveSc = GameObject.Find("StageManager").GetComponent<StageManager>();
        bossNameTxt.text = monsterInfo.monsterName;
        cam=GameObject.FindObjectOfType<Camera>();
        StartCoroutine(Think());
        
    }

    void Update()
    {
        nowHpTxt.text =monsterHp.ToString("F0")+"/" + monsterInfo.monsterMaxHp.ToString();
        lookVec = new Vector3(newPlayerSc.moveVec.x, 0, newPlayerSc.moveVec.z) * 2f;

        if (isLook)
        {
            transform.LookAt(playerOb.transform.position);
        }
        
        
        
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

    }
    
    IEnumerator Think()
    {
        int rand= Random.Range(0, 5);
        switch (rand)
        {
            case 0://돌멩이 굴리기
                yield return new WaitForSeconds(1);
                GameObject rockPref=Instantiate(rock,transform.position+transform.up*2, transform.rotation);
                yield return new WaitForSeconds(1.5f);
                break;

            case 1:
            case 2:// 위로점프 후 착지 패턴
                monsterBodyCol.isTrigger = true;
                yield return new WaitForSeconds(1);
                isGround = false;
                isLook = false;

                rigd.mass = 1;
                StartCoroutine(AttackRangeSign(transform.position+new Vector3(0,0.6f,0)));
                rigd.AddForce(Vector3.up * 9, ForceMode.Impulse);
                yield return new WaitForSeconds(0.3f);
                isDoingSkill = true;
                rigd.mass = 20;
                isLook = true;
                
                yield return new WaitForSeconds(1.7f);
                cam.isShaked = true;
                isGround = true;
                break;

            case 3:
            case 4: //플레이어 위치로 점프              
                yield return new WaitForSeconds(1);
                isLook = false;
                t = 0;

                Vector3 startPosition = rigd.position;  //오브젝트 자신의 위치
                Vector3 endPosition = playerOb.transform.position + lookVec;
                StartCoroutine(AttackRangeSign(endPosition + new Vector3(0, 0.6f, 0)));
                Vector3 center = (startPosition + endPosition) * 0.5f;
                center.y -= 3;
                startPosition -= center;    //startposition 위치값을 center값을 기준으로 나타내기 위해 빼줌
                endPosition -= center;  //endPosition 위치값을 center값을 기준으로 나타내기 위해 빼줌

                yield return new WaitForSeconds(1.5f);
                monsterBodyCol.isTrigger = true;
                isGround = false;
                for (float t = 0; t < 1; t += Time.deltaTime)
                    {
                        Vector3 point = Vector3.Slerp(startPosition, endPosition, t);
                        point += center;
                        rigd.position = point;

                    yield return null;
                }
                isDoingSkill= true;
                cam.isShaked = true;
                //transform.LookAt(playerOb.transform.position + lookVec);
                //Vector3.MoveTowards(transform.position,playerOb.transform.position + lookVec,0.1f);
                yield return new WaitForSeconds(2);
                isLook = true;
                isGround = true;
                break;
        }
        yield return new WaitForSeconds(2);
        monsterBodyCol.isTrigger = false;
        StartCoroutine(Think());

    }

    IEnumerator AttackRangeSign(Vector3 position)
    {
        GameObject attackRangePref=Instantiate(attackRangeOb, position, Quaternion.Euler(90, 0, 0));
        float y = attackRangePref.transform.position.y;
        y = 0.1f;
        attackRangePref.transform.position = new Vector3(position.x, y, position.z);
        float time = 0;

        SpriteRenderer spr = attackRangePref.GetComponent<SpriteRenderer>();

        spr.color = new Color(1, 0, 0, 0.6f);
        while (time <= 1)
        {
            time += 1;
            for (float i = 0.6f; i > 0f; i -= 0.1f)
            {

                spr.color = new Color(1, 0, 0, i);

                yield return new WaitForSeconds(0.05f);
            }


            for (float i = 0f; i < 0.7f; i += 0.1f)
            {
                spr.color = new Color(1, 0, 0, i);


                yield return new WaitForSeconds(0.05f);
            }


            yield return null;
        }
        attackRangePref.SetActive(false); ;
        yield return new WaitForSeconds(0.5f);

        Destroy(attackRangePref);
        yield return new WaitForSeconds(0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            if (!newPlayerSc.isDamaged)
            {
                newPlayerSc.StartCoroutine("Damaged", monsterInfo.monsterAtkDmg);
            }
        }

    }
    IEnumerator KingSlimeAttack()
    {
        downEffect.SetActive(true);
        attackRangeColider.enabled = true;
        yield return new WaitForSeconds(0.3f);
        attackRangeColider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        downEffect.SetActive(false);
        isDoingSkill = false;


    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground" && isDoingSkill)
        {
            StartCoroutine(KingSlimeAttack());
        }
    }
}
