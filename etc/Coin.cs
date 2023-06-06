using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    [Header("�÷��̾� ������Ʈ")]
    GameObject playerOb;
    [Header("�÷��̾� ������Ʈ Vector3 ��ǥ")]
    Vector3 target; // �÷��̾� ���� ��ġ�� �����ϴ� ����
    bool isCollision;   // �÷��̾�� �浹�� Ȯ���ϴ� bool �ڷ���
    float yRotate;  // y�� ȸ���� float �ڷ���
    public int coinPrice;   //coin ���� int �ڷ���
    void Start()
    {
        playerOb = GameObject.FindObjectOfType<CharacterController>().gameObject;
        float y = transform.position.y;
        y = 0.7f;
        transform.position = new Vector3(transform.position.x, y, transform.position.z); // �ʵ� ���� ��ġ�ϵ��� y�� ��ġ ����
    }

    void Update()
    {
        yRotate += Time.deltaTime*50;
        transform.rotation = Quaternion.Euler(0, yRotate, 90);  // ���� ȸ��
        
        if (isCollision)    //�÷��̾� ���� ��� �ݶ��̴�ȭ �浹�ϸ� isCollision = true�� ��
        {
            target = playerOb.transform.position;
            transform.position = Vector3.Lerp(transform.position, target, 0.08f);    //���ӿ�����Ʈ�� ��ġ���� Ÿ���� ��ġ���� ���� �����������
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Magnet")    // ���׳� �±׸� ���� �ݶ��̴��� �浹�ϸ�
        {
            isCollision = true;
        }
        if (other.tag=="CoinRange") //coinRange �±׸� ���� �ݶ��̴��� �浹�ϸ�
        {
            GameManager.instance.roundCoin += coinPrice;    //GameManager�� roundCoin ������ coinPrice �����ֱ�
            isCollision = false;
            Destroy(gameObject);
        }
    }

}
