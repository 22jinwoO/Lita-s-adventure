using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DMGText : MonoBehaviour
{
    Camera mainCamera;  // 카메라 변수
    public float speed; //  문자가 올라가는 속도 변수
    TextMeshPro textMeshPro;    //텍스트 메쉬 프로 변수
    Color alpha;    // 컬러 알파값
    Color spriteAlpha;    // 컬러 알파값
    public float alphaSpeed;    // 텍스트 투명도 변하는 속도
    public float damageValue;   // 텍스트 데미지값

    public bool isCritical = false; // 크리티컬 확인하는 변수

    public GameObject criticalOb;
    public SpriteRenderer criticalSR;
    void Start()
    {
        transform.SetParent(GameManager.instance.transform); //몬스터가 죽어도 사라지지 않게 부모 게임매니저로 변환
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); //MainCamera 오브젝트 찾아서 변수값에 넣어줌
        textMeshPro = GetComponent<TextMeshPro>();
        alpha = textMeshPro.color;
        spriteAlpha = criticalSR.color;
        
        if (isCritical) // 플레이어의 공격이 크리티컬이면
        {
            textMeshPro.text = damageValue.ToString()+"!";  // 몬스터 입는 피해 데미지 값 텍스트에 적용
            textMeshPro.fontStyle = FontStyles.Bold;    //텍스트 굵기 설정
            criticalOb.SetActive(isCritical);   //크리티컬 이미지 게임오브젝트 활성화
            alpha.g = 0;
        }
        else
        {
            textMeshPro.text = damageValue.ToString();  // 몬스터 입는 피해 데미지 값 텍스트에 적용
        }
        Destroy(gameObject, 2); //2초 뒤 게임오브젝트 삭제
        
    }

    void Update()
    {
        transform.forward = mainCamera.transform.forward;   // 데미지 텍스트 빌보드 적용
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0)); // y축 방향으로 텍스트가 올라가도록 설정
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);  // textMeshPro.color의 투명도 값이 0에 점점 가까워지도록 함
        textMeshPro.color = alpha;  // textmeshpro.color에 투명도값 적용

        if (isCritical)
        {
            spriteAlpha.a= Mathf.Lerp(spriteAlpha.a, 0, Time.deltaTime * alphaSpeed);  // textMeshPro.color의 투명도 값이 0에 점점 가까워지도록 함
            criticalSR.color=spriteAlpha;
        }

    }
}
