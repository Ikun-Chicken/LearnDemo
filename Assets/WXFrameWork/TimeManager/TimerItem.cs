using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
// 定时器接口
public interface ITimer
{
	//float Rate { get; } // 获取下一次触发动作的时间间隔

	void Cancel();      // 取消定时器
	void Pause();       // 暂停定时器
	void Resume();      // 恢复定时器
	//void SetScale(float scale); // 设置时间比例
}
public class TimerItem : ITimer
{
	public enum TimerRunState // 定义枚举，表示计时器的运行状态
	{
		Running, // 运行中
		Paused,  // 暂停
		Stoped   // 停止
	}
	private float clock;         // 计时器当前已经过的时间
	private float nextTime;// 下一次触发动作的时间间隔
	private Action action;       // 需要执行的动作
	private int passCount;       // 已经执行过的循环次数
	private int LoopCount;       // 动作执行的循环次数（-1 表示无限循环）
	private TimerRunState _runState; // 计时器的当前运行状态
	//public float Rate => nextTime; // 获取下一次触发动作的时间间隔

	/// <summary>
	/// 定时器对象构造函数
	/// </summary>
	/// <param name="delay">下一次触发动作的时间间隔</param>
	/// <param name="action">需要执行的动作</param>
	/// <param name="count">// 动作执行的循环次数（-1 表示无限循环）</param>
	public TimerItem(float delay, Action action, int count)
	{
		this.action = action;
		//this.scale = scale;
		nextTime = delay;
		clock = 0;
		passCount = 0;
		_runState = TimerRunState.Running;
		LoopCount = count;
	}
	public void Cancel()
	{
		_runState = TimerRunState.Stoped;
	}

	public void Pause()
	{
		if (_runState == TimerRunState.Running)
			_runState = TimerRunState.Paused;
	}

	public void Resume()
	{
		if (_runState == TimerRunState.Paused)
			_runState = TimerRunState.Running;
	}
	/// <summary>
	/// 更新计时器状态，根据 deltaTime 计算时间流逝，触发动作
	/// </summary>
	/// <param name="deltaTime">时间间隔</param>
	/// <returns>是否计时器需要被回收</returns>
	public bool Update(float deltaTime)
	{
		if (_runState == TimerRunState.Stoped)
		{
			return true; // 如果计时器已停止，返回 true 表示需要回收
		}
		if (_runState == TimerRunState.Paused)
		{
			return false; // 如果计时器暂停，返回 false 表示暂停状态，不需要回收
		}
		clock += deltaTime; // 累加时间流逝
		if (clock >= nextTime) // 如果已经过的时间达到了下一次触发动作的时间间隔
		{
			try
			{
				action?.Invoke(); // 执行动作
			}
			catch (Exception e)
			{
				Debug.LogError("Exception in UpdataManager.Update: " + e.Message); // 捕获异常并输出错误日志
				throw; // 重新抛出异常，以便在堆栈跟踪中准确定位到异常位置
			}
			passCount++; // 已执行次数加一
			clock = 0; // 重置已过时间为 0
		}
		if (passCount == LoopCount) // 如果已执行次数达到设定的循环次数
		{
			return true; // 返回 true 表示需要回收计时器
		}
		else
		{
			return false; // 否则返回 false 表示继续保持计时器
		}
	}
}