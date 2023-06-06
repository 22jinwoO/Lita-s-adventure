using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAura : MonoBehaviour
{
    Rigidbody rb;
    Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rb.AddForce(transform.forward * 6, ForceMode.Impulse);
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Monster")
        {
            if (other.GetComponent<Enemy>().monsterHp > 0)
            {
                other.SendMessage("MonsterDamaged", DataManager.instance.data.playerSkillDmg);
            }
            
            
        }
    }
}
