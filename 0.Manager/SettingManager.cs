using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    [Header("게임 설정 게임오브젝트")]
    public GameObject settingOb;
    [Header("게임 설정 ")]
    public Button settingBtn;
    [Header("게임 설정 닫는 버튼")]
    public Button settingClosBtn;
    

    [Header("게임 저장 버튼")]
    public Button saveBtn;
    [Header("게임 종료 버튼")]
    public Button gameExitBtn;

    [Header("브금믹서")]
    public AudioMixer bgmMixer;

    [Header("효과음믹서")]
    public AudioMixer sfxMixer;

    [Header("브금 슬라이더")]
    public Slider bgmAudioSlider;

    [Header("효과음 슬라이더")]
    public Slider sfxAudioSlider;

    private void Awake()
    {

    }
    void Start()
    {
        bgmAudioSlider.value = DataManager.instance.data.bgmSliderValue;
        sfxAudioSlider.value = DataManager.instance.data.sfxSliderValue;

        bgmMixer.SetFloat("BGM", bgmAudioSlider.value);
        bgmMixer.SetFloat("SFX", sfxAudioSlider.value);

        settingBtn.onClick.AddListener(SettingOpen);
        settingClosBtn.onClick.AddListener(SettingClose);
        
        saveBtn.onClick.AddListener(()=>AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]));
        saveBtn.onClick.AddListener(DataManager.instance.Save);

        gameExitBtn.onClick.AddListener(() => AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]));
        gameExitBtn.onClick.AddListener(DataManager.instance.OnApplicationQuit);
        
    }

    void Update()
    {
        
    }

    void SettingOpen()  // 게임설정 열리는 함수
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);   //설정버튼 열릴때 사운드 실행
        settingOb.SetActive(true);  //게임 설정 게임오브젝트 활성화
    }  

    void SettingClose() // 게임설정 닫는 함수
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);
        
        settingOb.SetActive(false); // 게임설정 오브젝트 비활성화
    }

    public void BGMAudioControl()   //BGM 스크롤 바에 반영하여 조절하는 함수
    {
        float sound = bgmAudioSlider.value;

        if (sound == -40f)
        {
            bgmMixer.SetFloat("BGM", -80);
        }
        else bgmMixer.SetFloat("BGM", sound);

        DataManager.instance.data.bgmSliderValue= bgmAudioSlider.value;
    }
    public void SFXAudioControl()   //효과음 스크롤 바에 반영하여 효과음 조절하는 함수
    {
        float sound = sfxAudioSlider.value;

        if (sound == -40f)
        {
            sfxMixer.SetFloat("SFX", -80);
        }
        else sfxMixer.SetFloat("SFX", sound);

        DataManager.instance.data.sfxSliderValue = sfxAudioSlider.value;
    }
}
