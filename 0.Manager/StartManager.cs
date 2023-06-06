using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [HideInInspector]
    public Button startBtn;
    public GameObject loginOb;
    public Button loginBtn;
    public Text nicknameTxt;
    void Start()
    {
        Application.targetFrameRate = 60;   //����� ������ 60
        startBtn.onClick.AddListener(LoginTxt);
        loginBtn.onClick.AddListener(GoHome);
    }
    void Update()
    {
        
    }

    public void LoginTxt()  //�α��� ȭ�� Ȱ��ȭ��Ű�� �Լ�
    {
        loginOb.SetActive(true);
    }

    public void GoHome()  //������ ���ư��� �Լ�
    {
        
        LoadingManager.LoadScene("Village");
        //ĳ���� ��Ʈ�ѷ� Ȱ��ȭ �Ǿ������� ���������� �̵� �ȵ�

        DataManager.instance.data.playerNickName= nicknameTxt.text;
    }
}
