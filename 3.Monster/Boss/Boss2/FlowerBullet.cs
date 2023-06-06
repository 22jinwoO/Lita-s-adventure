using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlowerBullet : MonoBehaviour
{
    public MonsterInfo monsterInfo;
    Transform target;
    NewPlayer playerSc;
    NavMeshAgent nav;
    // Start is called before the first frame update
    private void Awake()
    {
        target = GameObject.FindObjectOfType<CharacterController>().transform;
        playerSc = target.GetComponent<NewPlayer>();
        nav = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(target.position);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && playerSc.isDamaged != true)
        {
            other.SendMessage("Damaged", monsterInfo.monsterAtkDmg + 10);
            Destroy(gameObject);
        }
    }
}
