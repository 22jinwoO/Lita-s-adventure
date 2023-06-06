using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]   //프로젝트창에서 우클릭했을때 생성할 수 있게끔 하는 기능
public class Item : ScriptableObject
{
    //스크랩터블 오브젝트는 모노비헤비어가 없어서 컴포넌트로 추가할 수 없다
    //그래서 퍼블릭 변수로 넣어줘야함
    //아이템 스크랩터블 오브젝트 생성 -- 스크립트로 오브젝트 만듦
   
    public enum ItemType { 장착무기, 포션, 랜덤박스 , 무기, 몬스터아이템}
    [Header("아이템 종류")]
    public ItemType itemType;
    [Header("아이템 넘버")]
    public int itemNum;
    [Header("아이템 설명")]
    [TextArea]
    public string itemDesc;
    [Header("아이템 이름")]
    public string itemName;
    [Header("툴팁창에 표시될 아이템 이름")]
    public string itemToolTipName;
    [Header("아이템 이미지")]
    public Sprite itemImage;
    [Header("무기 스탯 정보")]
    public WeaponInfo weaponInfo;
    [Header("아이템 구매 가격")]
    public int itemBuyPrice;
    [Header("아이템 판매 가격")]
    public int itemSellPrice;

}

