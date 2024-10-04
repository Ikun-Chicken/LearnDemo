using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AudioManager : SingletonMono<AudioManager>
{
    private AudioSource m_AudioSource;
    //������ع�����Ƶ�ļ�
    private Dictionary<string, AudioClip> m_dicAudio;
	private void Awake()
	{
		m_AudioSource = GetComponent<AudioSource>();
		m_dicAudio=new Dictionary<string, AudioClip>();
	}
	//��ָ��·�����س���Ƶ
	public AudioClip LoadAudio(string path) => (AudioClip)Resources.Load(path);

	//�����Ƶ�ļ�
	private AudioClip GetAudio(string path)
	{
		if(!m_dicAudio.ContainsKey(path)) m_dicAudio[path]=LoadAudio(path);
		return m_dicAudio[path];
	}
	public void PlayBGM(string name=AudioConstans.bgm)
	{
		m_AudioSource.Stop();
		m_AudioSource.clip= GetAudio(name);
		m_AudioSource.Play();
	}
	public void StopBGM()=>m_AudioSource.Stop();
	public void PlaySound(string path)
	{
		m_AudioSource.PlayOneShot(LoadAudio(path));
	}
}
