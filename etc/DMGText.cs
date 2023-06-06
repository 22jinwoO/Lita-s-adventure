using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DMGText : MonoBehaviour
{
    Camera mainCamera;  // ī�޶� ����
    public float speed; //  ���ڰ� �ö󰡴� �ӵ� ����
    TextMeshPro textMeshPro;    //�ؽ�Ʈ �޽� ���� ����
    Color alpha;    // �÷� ���İ�
    Color spriteAlpha;    // �÷� ���İ�
    public float alphaSpeed;    // �ؽ�Ʈ ���� ���ϴ� �ӵ�
    public float damageValue;   // �ؽ�Ʈ ��������

    public bool isCritical = false; // ũ��Ƽ�� Ȯ���ϴ� ����

    public GameObject criticalOb;
    public SpriteRenderer criticalSR;
    void Start()
    {
        transform.SetParent(GameManager.instance.transform); //���Ͱ� �׾ ������� �ʰ� �θ� ���ӸŴ����� ��ȯ
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); //MainCamera ������Ʈ ã�Ƽ� �������� �־���
        textMeshPro = GetComponent<TextMeshPro>();
        alpha = textMeshPro.color;
        spriteAlpha = criticalSR.color;
        
        if (isCritical) // �÷��̾��� ������ ũ��Ƽ���̸�
        {
            textMeshPro.text = damageValue.ToString()+"!";  // ���� �Դ� ���� ������ �� �ؽ�Ʈ�� ����
            textMeshPro.fontStyle = FontStyles.Bold;    //�ؽ�Ʈ ���� ����
            criticalOb.SetActive(isCritical);   //ũ��Ƽ�� �̹��� ���ӿ�����Ʈ Ȱ��ȭ
            alpha.g = 0;
        }
        else
        {
            textMeshPro.text = damageValue.ToString();  // ���� �Դ� ���� ������ �� �ؽ�Ʈ�� ����
        }
        Destroy(gameObject, 2); //2�� �� ���ӿ�����Ʈ ����
        
    }

    void Update()
    {
        transform.forward = mainCamera.transform.forward;   // ������ �ؽ�Ʈ ������ ����
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0)); // y�� �������� �ؽ�Ʈ�� �ö󰡵��� ����
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);  // textMeshPro.color�� ���� ���� 0�� ���� ����������� ��
        textMeshPro.color = alpha;  // textmeshpro.color�� ������ ����

        if (isCritical)
        {
            spriteAlpha.a= Mathf.Lerp(spriteAlpha.a, 0, Time.deltaTime * alphaSpeed);  // textMeshPro.color�� ���� ���� 0�� ���� ����������� ��
            criticalSR.color=spriteAlpha;
        }

    }
}
