using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MyTimerManager : SingletonMono<MyTimerManager>
{
	// 游戏开始后的运行时间（单位：秒）
	//public float RunningTime => Time.time;
	private TimerBus _defaultTimerBus;  // 定时器总线
	// 构造函数，初始化默认定时器总线
	public MyTimerManager()
	{
		_defaultTimerBus = new TimerBus();
	}
	/// <summary>
	/// 添加一个有限次数执行的定时器对象到总线
	/// </summary>
	public ITimer AddFiniteTimer(float delay, Action action, int count = 1)
	{
		return _defaultTimerBus.AddTimer(delay, action, count);
	}
	/// <summary>
	/// 添加一个循环执行的定时器对象到总线
	/// </summary>
	public ITimer AddLoopingTimer(float delay, Action action)
	{
		return _defaultTimerBus.AddTimer(delay, action, -1);
	}
	private void Update()
	{
		_defaultTimerBus.Update(Time.deltaTime);
	}
}