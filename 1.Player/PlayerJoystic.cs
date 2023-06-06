using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJoystic : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{   //IPointerDownHandler: 손가락으로 터치를 시작했을때 혹은 마우스로 클릭을 시작했을때, IPointerUpHandler손가락을 뗐을때 마우스 클릭을 뗐을때 , IDragHandler 손가락을 드래그하거나 커서를 드래그했을떄

    [SerializeField] 
    RectTransform rect_Background;
    
    public RectTransform rect_Joystick;

    [Header("플레이어 조이스틱간의 거리")]
    float radius;
    public float joysticDistance;

    public GameObject playerOb;
    public NewPlayer newPlayerSc;

    [Header("조이스틱의 이동값")]
    public Vector2 value;
    public bool isTouch = false;
    
    void Start()
    {
        radius = rect_Background.rect.width * 0.5f; //radius에 백그라운드의 반지름 값이 들어감
    }

    void Update()
    {

    }
    public void OnDrag(PointerEventData eventData)
    {
        value = eventData.position - (Vector2)rect_Background.position;
        // 마우스 현재 좌표에서 검은색 백그라운드 좌표를 뺌
        
        value = Vector2.ClampMagnitude(value, radius);
        //(1,4) (-3, 5) 반지름값만큼 조이스틱 흰부분을 가둠
        rect_Joystick.localPosition = value;
        joysticDistance = Vector2.Distance(rect_Background.position, rect_Joystick.position)/radius;
        value = value.normalized;//밸류 정규화 속도값은 빠지고 방향값만 남게 됨

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
