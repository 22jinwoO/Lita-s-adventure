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
        Application.targetFrameRate = 60;   //모바일 프레임 60
        startBtn.onClick.AddListener(LoginTxt);
        loginBtn.onClick.AddListener(GoHome);
    }
    void Update()
    {
        
    }

    public void LoginTxt()  //로그인 화면 활성화시키는 함수
    {
        loginOb.SetActive(true);
    }

    public void GoHome()  //마을로 돌아가는 함수
    {
        
        LoadingManager.LoadScene("Village");
        //캐릭터 컨트롤러 활성화 되어있을땐 포지션으로 이동 안됨

        DataManager.instance.data.playerNickName= nicknameTxt.text;
    }
}
