using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;    //�÷��̾�
    public Vector3 offset;

    float shakeTime = 0.4f; //ī�޶� ��鸲 ���ӽð� ����
    float shakeSpeed = 1f;  //ī�޶� ��鸮�� �ӵ�
    float shakeAmount = 1f; // ī�޶� ��鸮�� ũ��
    public bool isShaked=false;
    Transform cam;
    private void Start()
    {
        cam = transform;
    }
    void Update()
    {

        //ī�޶��� �������� �÷��̾� �����ǿ��ٰ� offset������ ���ϱ�
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
