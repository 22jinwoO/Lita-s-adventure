using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [Header("������� ����� �ҽ�")]
    public AudioSource audioBgm;

    [Header("�߰��� ���� ����� �ҽ�")]
    public AudioSource audioWalkkSound;

    [Header("���� ���� ����� �ҽ�")]
    public AudioSource audioAttackSound;

    [Header("��ų ���� ���� ����� �ҽ�")]
    public AudioSource audioEffectSound;

    [Header("������ ���� ����� �ҽ�")]
    public AudioSource audioItemEffectSound;
    //

    [Header("������� ����� Ŭ�� �迭")]
    public AudioClip[] soundsBgm;

    [Header("�߰��� ����� Ŭ�� �迭")]
    public AudioClip[] soundsWalk;

    [Header("���� ����� Ŭ�� �迭")]
    public AudioClip[] soundsAttack;

    [Header("��ų���� ����� Ŭ�� �迭")]
    public AudioClip[] soundsSKill;

    [Header("������ȿ���� ����� Ŭ�� �迭")]
    public AudioClip[] soundsItem;

    [Header("��ưȿ���� ����� Ŭ�� �迭")]
    public AudioClip[] soundsBtn;


    static public AudioManager instance;
    void Awake()
    {
        if (null == instance)
        {
            //�� Ŭ���� �ν��Ͻ��� ź������ �� �������� instance�� ���ӸŴ��� �ν��Ͻ��� ������� �ʴٸ�, �ڽ��� �־��ش�.
            instance = this;

            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            //gameObject�����ε� �� ��ũ��Ʈ�� ������Ʈ�μ� �پ��ִ� Hierarchy���� ���ӿ�����Ʈ��� ��������, 
            //���� �򰥸� ������ ���� this�� �ٿ��ֱ⵵ �Ѵ�.
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
            //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
            //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ�(���ο� ���� GameMgr)�� �������ش�.
            Destroy(this.gameObject);
        }

    }

    void Start()
    {
        audioBgm=GetComponents<AudioSource>()[0];   // ù��° ����� �ҽ� ��������
        audioWalkkSound = GetComponents<AudioSource>()[1];  // �ι�° ����� �ҽ� ��������
        audioAttackSound = GetComponents<AudioSource>()[2]; // ����° ����� �ҽ� ��������
        audioEffectSound = GetComponents<AudioSource>()[3]; // �ݹ�° ����� �ҽ� ��������
        audioItemEffectSound = GetComponents<AudioSource>()[4]; // �ټ���° ����� �ҽ� ��������
        Sound_Bgm(soundsBgm[0]);    //ù ���� BGM ���� ����
    }

    void Update()
    {
        
    }
    public void Sound_Bgm(AudioClip bgmSound)   //BGM ���� �����ϴ� �Լ�(�Ű������� ����� Ŭ��)
    {
        audioBgm.clip = bgmSound;
        audioBgm.Play();
    }
    public void Sound_Walk(AudioClip walkSound) // �ȴ� ���� �����ϴ� �Լ�(�Ű������� ����� Ŭ��)
    {
        audioWalkkSound.clip = walkSound;
        audioWalkkSound.Play();
    }
    public void Sound_Attack(AudioClip attackSound) // �����Ҷ� ���� ���尡 ����Ǵ� �Լ�(�Ű����� ����� Ŭ��)
    {
        audioAttackSound.clip = attackSound;
        audioAttackSound.Play();
    }
    public void Sound_Skill(AudioClip skillSound)   // ��ų�� ����Ҷ� ��ų �������� ����Ǵ� �Լ�(�Ű����� ����� Ŭ��)
    {
        audioEffectSound.clip = skillSound;
        audioEffectSound.Play();
    }

    public void Sound_Item(AudioClip itemSound) // ������ ���� ���尡 ����Ǵ� �Լ� (�Ű������� ����� Ŭ��)
    {
        audioItemEffectSound.clip = itemSound;
        audioItemEffectSound.Play();
    }
}
