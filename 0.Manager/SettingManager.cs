using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    [Header("���� ���� ���ӿ�����Ʈ")]
    public GameObject settingOb;
    [Header("���� ���� ")]
    public Button settingBtn;
    [Header("���� ���� �ݴ� ��ư")]
    public Button settingClosBtn;
    

    [Header("���� ���� ��ư")]
    public Button saveBtn;
    [Header("���� ���� ��ư")]
    public Button gameExitBtn;

    [Header("��ݹͼ�")]
    public AudioMixer bgmMixer;

    [Header("ȿ�����ͼ�")]
    public AudioMixer sfxMixer;

    [Header("��� �����̴�")]
    public Slider bgmAudioSlider;

    [Header("ȿ���� �����̴�")]
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

    void SettingOpen()  // ���Ӽ��� ������ �Լ�
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);   //������ư ������ ���� ����
        settingOb.SetActive(true);  //���� ���� ���ӿ�����Ʈ Ȱ��ȭ
    }  

    void SettingClose() // ���Ӽ��� �ݴ� �Լ�
    {
        AudioManager.instance.Sound_Item(AudioManager.instance.soundsBtn[1]);
        
        settingOb.SetActive(false); // ���Ӽ��� ������Ʈ ��Ȱ��ȭ
    }

    public void BGMAudioControl()   //BGM ��ũ�� �ٿ� �ݿ��Ͽ� �����ϴ� �Լ�
    {
        float sound = bgmAudioSlider.value;

        if (sound == -40f)
        {
            bgmMixer.SetFloat("BGM", -80);
        }
        else bgmMixer.SetFloat("BGM", sound);

        DataManager.instance.data.bgmSliderValue= bgmAudioSlider.value;
    }
    public void SFXAudioControl()   //ȿ���� ��ũ�� �ٿ� �ݿ��Ͽ� ȿ���� �����ϴ� �Լ�
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
