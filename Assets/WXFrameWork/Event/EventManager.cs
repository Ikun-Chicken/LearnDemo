
using System;
using System.Collections.Generic;

using UnityEngine.Events;
public class EventManager : Singleton<EventManager>
{
	//为了更快地找到我们想要触发的事件，我们用字典来存储
	//Value 是 EventHandler，一个通用的委托,是所有数据事件类的基类（包含不带数据）
	//如果要真正传递一个非空的参数,
	//需要自定义一个类继承于 EventArgs，然后定义成员变量来存储需要传递的参数
	private Dictionary<string, EventHandler> _events 
		= new Dictionary<string, EventHandler>();
	public void AddEventListener(string eventName, EventHandler handler)
	{
		if (_events.ContainsKey(eventName))
		{
			_events[eventName] += handler;
		}
		else
		{
			_events.Add(eventName, handler);
		}
	}

	public void RemoveEventListener(string eventName, EventHandler handler)
	{
		if (_events.ContainsKey(eventName))
		{
			_events[eventName] -= handler;
			if (_events[eventName] == null)
			{
				_events.Remove(eventName);
			}
		}
	}
	/// <summary>
	/// 触发事件无参数
	/// </summary>
	/// <param name="eventName"></param>
	/// <param name="sender"></param>
	public void DispatchEvent(string eventName,object sender)
	{
		if (_events.ContainsKey(eventName))
		{
			_events[eventName].Invoke(sender,EventArgs.Empty);
		}
	}
	/// <summary>
	/// 触发事件有参数
	/// </summary>
	public void DispatchEvent(string eventName, object sender,EventArgs args)
	{
		if (_events.ContainsKey(eventName))
		{
			_events[eventName].Invoke(sender, args);
		}
	}
	public void Clear()
	{
		_events.Clear();
	}
}

/// <summary>
/// 便于触发事件的扩展类,
/// this object 使得一个 Object 类能够使用该方法，所以任何类都能够直接使用
/// </summary>
public static class EventTriggerExt
{
	/// <summary>
	/// 触发事件（无参数）
	/// </summary>
	/// <param name="sender">触发源</param>
	/// <param name="eventName">事件名</param>
	public static void DispatchEvent(this object sender, string eventName)
	{
		EventManager.Instance.DispatchEvent(eventName, sender);
	}
	/// <summary>
	/// 触发事件（有参数）
	/// </summary>
	/// <param name="sender">触发源</param>
	/// <param name="eventName">事件名</param>
	/// <param name="args">事件参数</param>
	public static void DispatchEvent(this object sender, string eventName, EventArgs args)
	{
		EventManager.Instance.DispatchEvent(eventName, sender, args);
	}

}
