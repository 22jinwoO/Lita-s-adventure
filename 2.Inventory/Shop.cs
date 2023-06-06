using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Image messagePopUp;  //���� �˾�â
    public Inventory InvetorySc;
    public Item clickItem;  //Slot.cs���� isShop�� ture�϶� Ŭ���� ������ �������� �޾ƿ�


    void Start()
    {
        Inventory inventorySc = GameObject.FindObjectOfType<Inventory>().GetComponent<Inventory>();
        InvetorySc = inventorySc;
    }
    public void ClickYes()  // ������ ���� Ȯ�� �� ȣ��
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[2]);
        if (DataManager.instance.data.playerMoney >= clickItem.itemBuyPrice)
        {
            InvetorySc.AddItem(clickItem);
            DataManager.instance.data.playerMoney -= clickItem.itemBuyPrice;
        }
        UiManager.instance.buyPopUp.SetActive(false);   //������ ���� �˾�â ��Ȱ��ȭ
        UiManager.instance.toolTip.SetActive(false);   //������ ���� ������Ʈ ��Ȱ��ȭ
        UiManager.instance.itemBuyBtn.gameObject.SetActive(false);  //������ ���� ��ư ���ӿ�����Ʈ ��Ȱ��ȭ
    }
    public void ClickNo()   // ������ ���� ���� �� ȣ��
    {

        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);
        UiManager.instance.buyPopUp.SetActive(false);   //������ ���� �˾�â ��Ȱ��ȭ
        UiManager.instance.toolTip.SetActive(false);   //������ ���� ������Ʈ ��Ȱ��ȭ
        UiManager.instance.itemBuyBtn.gameObject.SetActive(false);  //������ ���� ��ư ���ӿ�����Ʈ ��Ȱ��ȭ
        messagePopUp.gameObject.SetActive(false);
    }
}
