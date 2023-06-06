using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Image messagePopUp;  //구매 팝업창
    public Inventory InvetorySc;
    public Item clickItem;  //Slot.cs에서 isShop이 ture일때 클릭한 슬롯의 아이템을 받아옴


    void Start()
    {
        Inventory inventorySc = GameObject.FindObjectOfType<Inventory>().GetComponent<Inventory>();
        InvetorySc = inventorySc;
    }
    public void ClickYes()  // 아이템 구매 확정 시 호출
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[2]);
        if (DataManager.instance.data.playerMoney >= clickItem.itemBuyPrice)
        {
            InvetorySc.AddItem(clickItem);
            DataManager.instance.data.playerMoney -= clickItem.itemBuyPrice;
        }
        UiManager.instance.buyPopUp.SetActive(false);   //아이템 구매 팝업창 비활성화
        UiManager.instance.toolTip.SetActive(false);   //아이템 툴팁 오브젝트 비활성화
        UiManager.instance.itemBuyBtn.gameObject.SetActive(false);  //아이템 구매 버튼 게임오브젝트 비활성화
    }
    public void ClickNo()   // 아이템 구매 거절 시 호출
    {

        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);
        UiManager.instance.buyPopUp.SetActive(false);   //아이템 구매 팝업창 비활성화
        UiManager.instance.toolTip.SetActive(false);   //아이템 툴팁 오브젝트 비활성화
        UiManager.instance.itemBuyBtn.gameObject.SetActive(false);  //아이템 구매 버튼 게임오브젝트 비활성화
        messagePopUp.gameObject.SetActive(false);
    }
}
