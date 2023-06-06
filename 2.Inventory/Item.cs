using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]   //������Ʈâ���� ��Ŭ�������� ������ �� �ְԲ� �ϴ� ���
public class Item : ScriptableObject
{
    //��ũ���ͺ� ������Ʈ�� ������� ��� ������Ʈ�� �߰��� �� ����
    //�׷��� �ۺ� ������ �־������
    //������ ��ũ���ͺ� ������Ʈ ���� -- ��ũ��Ʈ�� ������Ʈ ����
   
    public enum ItemType { ��������, ����, �����ڽ� , ����, ���;�����}
    [Header("������ ����")]
    public ItemType itemType;
    [Header("������ �ѹ�")]
    public int itemNum;
    [Header("������ ����")]
    [TextArea]
    public string itemDesc;
    [Header("������ �̸�")]
    public string itemName;
    [Header("����â�� ǥ�õ� ������ �̸�")]
    public string itemToolTipName;
    [Header("������ �̹���")]
    public Sprite itemImage;
    [Header("���� ���� ����")]
    public WeaponInfo weaponInfo;
    [Header("������ ���� ����")]
    public int itemBuyPrice;
    [Header("������ �Ǹ� ����")]
    public int itemSellPrice;

}

