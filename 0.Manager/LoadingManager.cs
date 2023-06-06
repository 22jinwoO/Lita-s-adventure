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
    Scene scene; //�Լ� �ȿ� �����Ͽ� ����Ѵ�.
    public static void LoadScene(string sceneName)  //�� �̺�Ʈ ����Լ�
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
                tipTxt.text = "Tip. 3���� ��ų ��Ÿ���� 15��, 20��, 25�� �Դϴ�.";
                break;
            case 1:
                tipTxt.text = "Tip. ���̺�� 3�ܰ�� �̷���� ������ ������ ������ Ŭ���� �մϴ�.";
                break;
            case 2:
                tipTxt.text = "Tip. �뽬�� ��� �� �÷��̾�� ���� �����Դϴ�.";
                break;
            case 3:
                tipTxt.text = "Tip. 2��° ��ų ���� �÷��̾�� ���� �����Դϴ�.";
                break;
            case 4:
                tipTxt.text = "Tip. ���� ���� �÷��̾� �ִ� Hp�� 30%�� ȸ���˴ϴ�.";
                break;
        }
    }
    private void Update()
    {
        
    }

    // �ٸ� ������ �θ� �� �ִ� �Լ�

    IEnumerator LoadNextScene() // ���� ���� �ε��ɶ� ȣ��Ǵ� �Լ�
    {
        // ���� ������ ��ȯ�� �ɾ���� �ٸ� �� �� �� �ְ�
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);

        // �ε��� �Ǵ��� ����ȯ ���ϰ� ��ٸ��� (op.progress 0.9 ������)
        op.allowSceneActivation = false;
        float time = 0;
        
       
        // �ε��� ������ ������ �ݺ�
        while (!op.isDone)
        {
            float gage = progressBar.fillAmount * 100;
            progressTxt.text = gage.ToString("F0") + "%";
            if (op.progress < 0.9f)
            {
                // �ε� ������ ���α׷����ٿ� ����
                progressBar.fillAmount = op.progress;
                

            }
            else // ����ũ �ε� (1��)
            {
                time += Time.deltaTime*0.2f;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1, time);
                if (progressBar.fillAmount>=1)
                {
                    touchClickTxt.gameObject.SetActive(true);
                }
                //���α׷��� �ٰ� �� ä������ �� ��ȯ
                if ((progressBar.fillAmount>=1&&Input.GetMouseButtonDown(0))||(progressBar.fillAmount >= 1 && Input.touchCount >= 1))//Input.touchCount>=1 ,Input.GetMouseButtonDown(0)
                {

                    

                    op.allowSceneActivation=true;

                }
            }
            yield return null;
        }
        
    }
}
