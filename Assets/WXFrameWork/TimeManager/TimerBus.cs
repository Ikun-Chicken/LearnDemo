using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TimerBus
{
	// 定义一个列表存储多个定时器对象
	private List<TimerItem> timers;
	public TimerBus()
	{
		timers = new List<TimerItem>(); // 初始化定时器列表
	}

	// 添加一个定时器并返回对应的 ITimer 接口
	public ITimer AddTimer(float delay, Action action, int count)
	{
		TimerItem timer = new TimerItem(delay, action, count); // 创建一个新的 TimerItem 对象
		timers.Add(timer); // 将新创建的定时器添加到列表中
		return timer; // 返回新创建的定时器对象
	}

	// 更新所有定时器状态
	public void Update(float deltaTime)
	{
		for (int i = timers.Count - 1; i >= 0; i--)
		{
			TimerItem timer = timers[i]; // 获取当前索引处的定时器对象
			if (timer.Update(deltaTime)) // 调用定时器对象的 Update 方法，传入时间间隔 deltaTime
			{
				timers.RemoveAt(i); // 如果定时器返回需要被回收，则从列表中移除该定时器
			}
		}
	}
}
