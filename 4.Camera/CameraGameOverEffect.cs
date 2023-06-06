using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGameOverEffect : MonoBehaviour
{
    Material cameraMaterial;
    public float grayScale = 0.0f;

    public GameObject message;
    float appliedTime = 2.0f;

    void Start()
    {
        cameraMaterial = new Material(Shader.Find("Custom/Grayscale"));
    }

    //��ó�� ȿ��.  destination �̹���(���� ȭ��)��  source�̹����� ��ü
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        cameraMaterial.SetFloat("_Grayscale", grayScale);
        Graphics.Blit(source, destination, cameraMaterial);
    }

    public void gameOverCameraEffect()
    {
        StartCoroutine(gameOverEffect());
    }

    private IEnumerator gameOverEffect()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < appliedTime)
        {
            elapsedTime += Time.deltaTime;

            grayScale = elapsedTime / appliedTime;
            yield return null;
        }

        message.SetActive(true);
    }
}
