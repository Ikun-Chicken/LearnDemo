using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AudioManager : SingletonMono<AudioManager>
{
    private AudioSource m_AudioSource;
    //缓存加载过的音频文件
    private Dictionary<string, AudioClip> m_dicAudio;
	private void Awake()
	{
		m_AudioSource = GetComponent<AudioSource>();
		m_dicAudio=new Dictionary<string, AudioClip>();
	}
	//从指定路径加载出音频
	public AudioClip LoadAudio(string path) => (AudioClip)Resources.Load(path);

	//获得音频文件
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
