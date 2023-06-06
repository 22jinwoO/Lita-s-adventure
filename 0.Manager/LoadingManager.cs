using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public Camera cam;
    //public EventSystem eventsys;
    public Image progressBar;
    public Text tipTxt;
    public Text touchClickTxt;
    public Text progressTxt;
    Touch tempTouch;
    public static string nextSceneName;
    Scene scene; //함수 안에 선언하여 사용한다.
    public static void LoadScene(string sceneName)  //씬 이벤트 등록함수
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        scene =SceneManager.GetActiveScene();
        StartCoroutine(LoadNextScene());
        AudioManager.instance.Sound_Bgm(AudioManager.instance.soundsBgm[4]);
        if (GameObject.FindGameObjectWithTag("MainCamera")!=null)
        {
            cam.GetComponent<AudioListener>().enabled = false;
            //eventsys.enabled = false;
        }
        int rand = Random.Range(0, 5);
        switch (rand)
        {
            case 0:
                tipTxt.text = "Tip. 3가지 스킬 쿨타임은 15초, 20초, 25초 입니다.";
                break;
            case 1:
                tipTxt.text = "Tip. 웨이브는 3단계로 이루어져 있으며 보스를 잡으면 클리어 합니다.";
                break;
            case 2:
                tipTxt.text = "Tip. 대쉬기 사용 시 플레이어는 무적 상태입니다.";
                break;
            case 3:
                tipTxt.text = "Tip. 2번째 스킬 사용시 플레이어는 무적 상태입니다.";
                break;
            case 4:
                tipTxt.text = "Tip. 포션 사용시 플레이어 최대 Hp의 30%가 회복됩니다.";
                break;
        }
    }
    private void Update()
    {
        
    }

    // 다른 씬에서 부를 수 있는 함수

    IEnumerator LoadNextScene() // 다음 씬이 로딩될때 호출되는 함수
    {
        // 다음 씬으로 전환을 걸어놓고 다른 일 할 수 있게
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);

        // 로딩이 되더라도 씬전환 못하게 기다리기 (op.progress 0.9 까지만)
        op.allowSceneActivation = false;
        float time = 0;
        
       
        // 로딩이 끝나기 전까지 반복
        while (!op.isDone)
        {
            float gage = progressBar.fillAmount * 100;
            progressTxt.text = gage.ToString("F0") + "%";
            if (op.progress < 0.9f)
            {
                // 로딩 과정을 프로그래스바에 적용
                progressBar.fillAmount = op.progress;
                

            }
            else // 페이크 로딩 (1초)
            {
                time += Time.deltaTime*0.2f;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1, time);
                if (progressBar.fillAmount>=1)
                {
                    touchClickTxt.gameObject.SetActive(true);
                }
                //프로그레스 바가 다 채워지면 씬 전환
                if ((progressBar.fillAmount>=1&&Input.GetMouseButtonDown(0))||(progressBar.fillAmount >= 1 && Input.touchCount >= 1))//Input.touchCount>=1 ,Input.GetMouseButtonDown(0)
                {

                    

                    op.allowSceneActivation=true;

                }
            }
            yield return null;
        }
        
    }
}
