using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]   //������Ʈâ���� ��Ŭ�������� ������ �� �ְԲ� �ϴ� ���
public class MonsterInfo : ScriptableObject
{
    //��ũ���ͺ� ������Ʈ�� ������� ��� ������Ʈ�� �߰��� �� ����
    //�׷��� �ۺ� ������ �־������
    //������ ��ũ���ͺ� ������Ʈ ���� -- ��ũ��Ʈ�� ������Ʈ ����
    [Header("���� �̸�")]
    public string monsterName;
    [Header("���� �ִ� HP")]
    public int monsterMaxHp;
    [Header("���� �̵��ӵ�")]
    public float monsterSpeed;
    [Header("���� ���� �����")]
    public int monsterAtkDmg;
    [Header("���� ����ġ")]
    public float monsterExp;
    [Header("���� ��� ����")]
    public int monsterCoin;


}