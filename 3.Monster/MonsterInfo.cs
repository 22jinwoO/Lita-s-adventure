using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]   //프로젝트창에서 우클릭했을때 생성할 수 있게끔 하는 기능
public class MonsterInfo : ScriptableObject
{
    //스크랩터블 오브젝트는 모노비헤비어가 없어서 컴포넌트로 추가할 수 없다
    //그래서 퍼블릭 변수로 넣어줘야함
    //아이템 스크랩터블 오브젝트 생성 -- 스크립트로 오브젝트 만듦
    [Header("몬스터 이름")]
    public string monsterName;
    [Header("몬스터 최대 HP")]
    public int monsterMaxHp;
    [Header("몬스터 이동속도")]
    public float monsterSpeed;
    [Header("몬스터 공격 대미지")]
    public int monsterAtkDmg;
    [Header("몬스터 경험치")]
    public float monsterExp;
    [Header("몬스터 드랍 코인")]
    public int monsterCoin;


}