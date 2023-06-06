using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    [Header("플레이어 오브젝트")]
    GameObject playerOb;
    [Header("플레이어 오브젝트 Vector3 좌표")]
    Vector3 target; // 플레이어 현재 위치값 저장하는 변수
    bool isCollision;   // 플레이어와 충돌을 확인하는 bool 자료형
    float yRotate;  // y축 회전값 float 자료형
    public int coinPrice;   //coin 가격 int 자료형
    void Start()
    {
        playerOb = GameObject.FindObjectOfType<CharacterController>().gameObject;
        float y = transform.position.y;
        y = 0.7f;
        transform.position = new Vector3(transform.position.x, y, transform.position.z); // 필드 위에 위치하도록 y축 위치 고정
    }

    void Update()
    {
        yRotate += Time.deltaTime*50;
        transform.rotation = Quaternion.Euler(0, yRotate, 90);  // 코인 회전
        
        if (isCollision)    //플레이어 코인 흡수 콜라이더화 충돌하면 isCollision = true가 됨
        {
            target = playerOb.transform.position;
            transform.position = Vector3.Lerp(transform.position, target, 0.08f);    //게임오브젝트의 위치값이 타겟의 위치값에 점점 가까워지도록
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Magnet")    // 마그넷 태그를 가진 콜라이더와 충돌하면
        {
            isCollision = true;
        }
        if (other.tag=="CoinRange") //coinRange 태그를 가진 콜라이더와 충돌하면
        {
            GameManager.instance.roundCoin += coinPrice;    //GameManager의 roundCoin 변수에 coinPrice 더해주기
            isCollision = false;
            Destroy(gameObject);
        }
    }

}
