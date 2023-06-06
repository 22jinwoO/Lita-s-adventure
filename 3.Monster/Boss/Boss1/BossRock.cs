using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : MonoBehaviour
{
    Rigidbody rigid;
    bool isShoot;
    float time;
    public GameObject playerOb;
    NewPlayer newPlayerSc;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(GainPower());
        playerOb = GameObject.FindObjectOfType<CharacterController>().gameObject;
        newPlayerSc = playerOb.GetComponent<NewPlayer>();

    }

    void Update()
    {
        time += Time.deltaTime;
        if (time>7)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator GainPower()
    {
        float time = 0;
        
        while (!isShoot)
        {
            time += Time.deltaTime;
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            rigid.AddTorque(transform.right *34, ForceMode.Acceleration);    //x축 회전때문에 보스오브젝트가 바라보는 방향으로 굴러감
            if (time>=2f)
            {
                isShoot = true;
            }
            yield return null;
        }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Wall")
        {
            Destroy(gameObject);
        }
        else if (other.tag=="Player")
        {
            Destroy(gameObject);
            if (!newPlayerSc.isDamaged) //플레이어가 무적상태가 아닐때
            {
                newPlayerSc.StartCoroutine("Damaged", 20); // 데미지 주는 함수 전달
            }
            
            
        }
    }
}
