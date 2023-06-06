using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [Header("배경음악 오디오 소스")]
    public AudioSource audioBgm;

    [Header("발걸음 사운드 오디오 소스")]
    public AudioSource audioWalkkSound;

    [Header("공격 사운드 오디오 소스")]
    public AudioSource audioAttackSound;

    [Header("스킬 공격 사운드 오디오 소스")]
    public AudioSource audioEffectSound;

    [Header("아이템 사운드 오디오 소스")]
    public AudioSource audioItemEffectSound;
    //

    [Header("배경음악 오디오 클립 배열")]
    public AudioClip[] soundsBgm;

    [Header("발걸음 오디오 클립 배열")]
    public AudioClip[] soundsWalk;

    [Header("공격 오디오 클립 배열")]
    public AudioClip[] soundsAttack;

    [Header("스킬공격 오디오 클립 배열")]
    public AudioClip[] soundsSKill;

    [Header("아이템효과음 오디오 클립 배열")]
    public AudioClip[] soundsItem;

    [Header("버튼효과음 오디오 클립 배열")]
    public AudioClip[] soundsBtn;


    static public AudioManager instance;
    void Awake()
    {
        if (null == instance)
        {
            //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            instance = this;

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            Destroy(this.gameObject);
        }

    }

    void Start()
    {
        audioBgm=GetComponents<AudioSource>()[0];   // 첫번째 오디오 소스 가져오기
        audioWalkkSound = GetComponents<AudioSource>()[1];  // 두번째 오디오 소스 가져오기
        audioAttackSound = GetComponents<AudioSource>()[2]; // 세번째 오디오 소스 가져오기
        audioEffectSound = GetComponents<AudioSource>()[3]; // 넷번째 오디오 소스 가져오기
        audioItemEffectSound = GetComponents<AudioSource>()[4]; // 다섯번째 오디오 소스 가져오기
        Sound_Bgm(soundsBgm[0]);    //첫 시작 BGM 사운드 실행
    }

    void Update()
    {
        
    }
    public void Sound_Bgm(AudioClip bgmSound)   //BGM 사운드 실행하는 함수(매개변수는 오디오 클립)
    {
        audioBgm.clip = bgmSound;
        audioBgm.Play();
    }
    public void Sound_Walk(AudioClip walkSound) // 걷는 사운드 실행하는 함수(매개변수는 오디오 클립)
    {
        audioWalkkSound.clip = walkSound;
        audioWalkkSound.Play();
    }
    public void Sound_Attack(AudioClip attackSound) // 공격할때 음성 사운드가 실행되는 함수(매개변수 오디오 클립)
    {
        audioAttackSound.clip = attackSound;
        audioAttackSound.Play();
    }
    public void Sound_Skill(AudioClip skillSound)   // 스킬을 사용할때 스킬 음성사운드 실행되는 함수(매개변수 오디오 클립)
    {
        audioEffectSound.clip = skillSound;
        audioEffectSound.Play();
    }

    public void Sound_Item(AudioClip itemSound) // 아이템 사용시 사운드가 실행되는 함수 (매개변수는 오디오 클립)
    {
        audioItemEffectSound.clip = itemSound;
        audioItemEffectSound.Play();
    }
}
