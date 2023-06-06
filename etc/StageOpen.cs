using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
public class StageOpen : MonoBehaviour
{
    public GameObject playerOb;
    Button stageBtn;

    [SerializeField]
    Button playerInfoBtn;

    CharacterController cc;
    private void Awake()
    {
        stageBtn = GetComponent<Button>();
        playerInfoBtn = UiManager.instance.playerInfoBtn;
        playerOb = GameObject.FindObjectOfType<CharacterController>().gameObject;
    }
    void Start()
    {
        stageBtn.onClick.AddListener(ClickStage);
        cc = playerOb.GetComponent<CharacterController>();
    }
   
    void Update()
    {
        
    }

    #region * ClickStage()�Լ�- �������� Ŭ�� �� ����Ǵ� �Լ�
    void ClickStage()
    {
        DataManager.instance.Save();
        UiManager.instance.blindImageOb.SetActive(true);   //����ε� �̹��� Ȱ��ȭ
        LoadingManager.LoadScene(gameObject.name);    //Ŭ���� ���ӿ�����Ʈ�� �̸��� ���� ���� �ҷ�����

        //DataManager.instance.data.clearStage = int.Parse(Regex.Replace(gameObject.name, @"\D",""));  //�÷��̾��� ���� ���������� ���ӿ�����Ʈ�� �̸����� ���ںκи� ����Ͽ� �־���
        
        playerOb.GetComponent<NewPlayer>().canInput = true; //�÷��̾� �̵� ����
        UiManager.instance.onPortal = false;    //onPortal false

        // ���ݹ�ư �̹��� ����
        UiManager.instance.AttackBtnImg.sprite = UiManager.instance.atkBtnImg;
        UiManager.instance.AttackBackBtnImg.sprite = UiManager.instance.atkBtnImg;

        cc.enabled = false;
        playerOb.transform.position = new Vector3(0, 1, 0);
        playerInfoBtn.interactable = false;

        //cc.enabled = true;
    }
    #endregion
}
