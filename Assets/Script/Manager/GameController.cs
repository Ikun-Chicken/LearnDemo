using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public bool IsPause {  get; private set; }
	public bool IsPlay {  get; set; }

	public static GameController instance;
	void Start()
	{
		instance = this;
		IsPause = true;
		IsPlay = true;
	}

	public void Play()
	{
		IsPause = false;
	}

	public void Pause()
	{
		IsPause = true;
	}

	public void Resume()
	{
		IsPause = false;
	}

	public void Restart()
	{
		GameAttribute.instance.Reset();//调用游戏数据类中的重置数据
		PlayerController.instance.Reset();
		PlayerController.instance.Play();
	}

	public void Exit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}


}
