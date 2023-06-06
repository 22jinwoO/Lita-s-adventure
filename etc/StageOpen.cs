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

    #region * ClickStage()함수- 스테이지 클릭 시 실행되는 함수
    void ClickStage()
    {
        DataManager.instance.Save();
        UiManager.instance.blindImageOb.SetActive(true);   //블라인드 이미지 활성화
        LoadingManager.LoadScene(gameObject.name);    //클릭한 게임오브젝트의 이름을 가진 씬을 불러오고

        //DataManager.instance.data.clearStage = int.Parse(Regex.Replace(gameObject.name, @"\D",""));  //플레이어의 현재 스테이지에 게임오브젝트의 이름에서 숫자부분만 출력하여 넣어줌
        
        playerOb.GetComponent<NewPlayer>().canInput = true; //플레이어 이동 가능
        UiManager.instance.onPortal = false;    //onPortal false

        // 공격버튼 이미지 복구
        UiManager.instance.AttackBtnImg.sprite = UiManager.instance.atkBtnImg;
        UiManager.instance.AttackBackBtnImg.sprite = UiManager.instance.atkBtnImg;

        cc.enabled = false;
        playerOb.transform.position = new Vector3(0, 1, 0);
        playerInfoBtn.interactable = false;

        //cc.enabled = true;
    }
    #endregion
}
