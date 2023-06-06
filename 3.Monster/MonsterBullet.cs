using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    public MonsterInfo monsterInfo;
    GameObject playerOb;
    NewPlayer playerSc;
    Rigidbody rb;
    Collider col;

    // Start is called before the first frame update
    private void Awake()
    {
        playerOb = GameObject.FindObjectOfType<CharacterController>().gameObject;
        playerSc = playerOb.GetComponent<NewPlayer>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.forward*8);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player"&& playerSc.isDamaged!=true && gameObject.tag == "MonsterBullet")
        {
            other.SendMessage("Damaged", monsterInfo.monsterAtkDmg);
            Destroy(gameObject);
        }

        if (other.tag == "Player" && playerSc.isDamaged != true && gameObject.name == "FlowerBullet(Clone)")
        {
            other.SendMessage("Damaged", monsterInfo.monsterAtkDmg + 10);
            gameObject.SetActive(false);
        }
    }
}
