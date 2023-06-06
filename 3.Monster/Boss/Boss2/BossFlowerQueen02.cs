using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFlowerQueen02 : Enemy
{
    [Header("패턴 2 플레이어 추적 오브젝트 발사하는 포지션")]
    public Transform attackPos1;
    public Transform attackPos2;

    public BoxCollider atkBoxcol;
    public int speed;
    [Header("플라워 퀸 360도 회전할때 사용하는 프리팹")]
    public GameObject floweBullet;

    [Header("플레이어 추적하는 프리팹")]
    public GameObject flowerBall;

    [SerializeField]
    [Header("플라워 퀸 패턴 1번을 위한 오브젝트 풀링 리스트")]
    GameObject[] flowerBullets= new GameObject[12];

    NewPlayer newPlayerSc;
    public Text nowHpTxt;
    public Text bossNameTxt;


    bool isDoingSkill = false;


    // Start is called before the first frame update
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
        anim = GetComponent<Animator>();

        // 오브젝트 풀링값들 비활성화하여 리스트에 추가해주기

        for (int i = 0; i < 12; i++)
        {
            GameObject pref=Instantiate(floweBullet);
            flowerBullets[i] = pref;
            pref.SetActive(false);

        }
        StartCoroutine(Think());
    }

    // Update is called once per frame
    void Update()
    {
        nowHpTxt.text = monsterHp.ToString("F0") + "/" + monsterInfo.monsterMaxHp.ToString();

        if (isGround)
        {
            rigd.constraints = RigidbodyConstraints.FreezeRotationZ;
        }
    }

    IEnumerator Think()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = new Vector3(transform.position.x, 0.270797f, transform.position.z);
        if (atkBoxcol.isTrigger!=true)
        {
            atkBoxcol.isTrigger = true;
        }

        int rand = Random.Range(0,8);


        switch (rand)
        {
            case 0:
            case 1: // 360도로 회전한 후 flowerBullets 오브젝트 풀링 값들 활성화하는 함수
                yield return new WaitForSeconds(1f);
                anim.SetTrigger("tSkill");
                yield return new WaitForSeconds(7f);
                for (int i = 0; i < flowerBullets.Length; i++)
                {
                    flowerBullets[i].SetActive(false);
                }
                break;

            case 2:
            case 3:
            case 4:// 플레이어 추적하는 flowerBall 생성하는 패턴
                yield return new WaitForSeconds(1f);
                transform.LookAt(playerOb.transform.position);  // �÷��̾� ��ġ �ٶ󺸱�
                anim.SetTrigger("tAttack");
                GameObject pref1=Instantiate(flowerBall, attackPos1.position,Quaternion.identity);
                GameObject pref2=Instantiate(flowerBall, attackPos2.position, Quaternion.identity);
                yield return new WaitForSeconds(4);
                Destroy(pref1,1.5f);
                Destroy(pref2,1.5f);
                break;

            case 5:
            case 6:              
            case 7: //공중으로 이동하여 구형보간으로 플레이어 위치로 이동
                yield return new WaitForSeconds(1f);
                isGround = false;
                float time = 0;
                while (time < 1f)
                {
                    rigd.AddForce(transform.up* speed);
                    
                    time += Time.deltaTime;
                    yield return null;
                }

                yield return new WaitForSeconds(1f);
                rigd.velocity = Vector3.zero;
                

                yield return new WaitForSeconds(1.5f);
                Vector3 startPosition = rigd.position;  //시작 위치
                Vector3 endPosition = playerOb.transform.position;  // 도착 위치
                Vector3 center = (startPosition + endPosition) * 0.5f;  // 포물선의 중앙 위치값

                float distance = Vector3.Distance(startPosition, endPosition); // 거리

                
                transform.LookAt(endPosition);  // 도착하는 위치 바라보기
                transform.rotation = Quaternion.Euler(50, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);  // 공격자세로 오브젝트 각도 조절

                center.y += 3; //포지션 아래로 그려지도록 중앙 y 값 올리기

                startPosition -= center;    //startposition 위치값을 center위치값 기준으로 나타내기 위해 빼줌
                endPosition -= center;  //endPosition 위치값을 center위치값 기준으로 나타내기 위해 빼줌



                rigd.freezeRotation = true;
                
                for (float t = 0; t < 1.1f; t += Time.deltaTime)
                {
                    
                    Vector3 point = Vector3.Slerp(startPosition, endPosition, t);
                    // 포물선을 아래록 ㅡ리기 위해서 center를 뺴줬으므로 다시 더해서 원상 복구
                    
                    point += center;
                    rigd.position = point;

                    anim.SetBool("isMove",true);
                    yield return null;
                }


                transform.rotation = Quaternion.Euler(0, 0, 0);
                yield return new WaitForSeconds(0.2f);
                transform.LookAt(playerOb.transform.position);  // 플레이어 바라보기
                anim.SetBool("isMove", false);
                yield return new WaitForSeconds(3);
                rigd.freezeRotation = false;
                isGround = true;
                break;
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(Think());

    }
    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!newPlayerSc.isDamaged)
            {
                newPlayerSc.StartCoroutine("Damaged", monsterInfo.monsterAtkDmg);
            }
        }

    }

    // 360도 회전할때 애니메이션 이벤트에서 호출하는 함수
    public void SpinLaunch()
    {
        int euler = 0;

        for (int i = 0; i < flowerBullets.Length; i++)
        {
            
            flowerBullets[i].transform.position=transform.position;
            flowerBullets[i].transform.rotation = Quaternion.Euler(0, euler, 0);
            flowerBullets[i].SetActive(true);
            euler += 30;
        }
    }
}
