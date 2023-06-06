using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;    //플레이어
    public Vector3 offset;

    float shakeTime = 0.4f; //카메라 흔들림 지속시간 변수
    float shakeSpeed = 1f;  //카메라 흔들리는 속도
    float shakeAmount = 1f; // 카메라 흔들리는 크기
    public bool isShaked=false;
    Transform cam;
    private void Start()
    {
        cam = transform;
    }
    void Update()
    {

        //카메라의 포지션은 플레이어 포지션에다가 offset고정값 더하기
        transform.position = target.position + offset;
        if (isShaked)
        {
            StartCoroutine(Shake());
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        //Vector3 originalPosition = cam.localPosition;
        float elapsedTime = 0;
        while(elapsedTime < shakeTime)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * shakeAmount;
            cam.localPosition=Vector3.Lerp(transform.position, randomPoint, Time.deltaTime*shakeSpeed);
            yield return null;
            //transform.position = target.position + offset;
            elapsedTime +=Time.deltaTime;
        }

        //cam.localPosition = originalPosition;
        isShaked = false;

    }
}
