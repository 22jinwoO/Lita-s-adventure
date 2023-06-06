using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJoystic : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{   //IPointerDownHandler: �հ������� ��ġ�� ���������� Ȥ�� ���콺�� Ŭ���� ����������, IPointerUpHandler�հ����� ������ ���콺 Ŭ���� ������ , IDragHandler �հ����� �巡���ϰų� Ŀ���� �巡��������

    [SerializeField] 
    RectTransform rect_Background;
    
    public RectTransform rect_Joystick;

    [Header("�÷��̾� ���̽�ƽ���� �Ÿ�")]
    float radius;
    public float joysticDistance;

    public GameObject playerOb;
    public NewPlayer newPlayerSc;

    [Header("���̽�ƽ�� �̵���")]
    public Vector2 value;
    public bool isTouch = false;
    
    void Start()
    {
        radius = rect_Background.rect.width * 0.5f; //radius�� ��׶����� ������ ���� ��
    }

    void Update()
    {

    }
    public void OnDrag(PointerEventData eventData)
    {
        value = eventData.position - (Vector2)rect_Background.position;
        // ���콺 ���� ��ǥ���� ������ ��׶��� ��ǥ�� ��
        
        value = Vector2.ClampMagnitude(value, radius);
        //(1,4) (-3, 5) ����������ŭ ���̽�ƽ ��κ��� ����
        rect_Joystick.localPosition = value;
        joysticDistance = Vector2.Distance(rect_Background.position, rect_Joystick.position)/radius;
        value = value.normalized;//��� ����ȭ �ӵ����� ������ ���Ⱚ�� ���� ��

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        AudioManager.instance.audioWalkkSound.Stop();
        rect_Joystick.localPosition = Vector3.zero;
        value=Vector3.zero;

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.instance.Sound_Walk(AudioManager.instance.soundsWalk[Random.Range(0,6)]);

        isTouch = true;
    }


}
