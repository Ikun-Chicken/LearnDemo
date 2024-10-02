
using System.Collections.Generic;

using UnityEngine.Events;
public class EventManager : Singleton<EventManager>
{
	private Dictionary<string, UnityAction<object>> _events 
		= new Dictionary<string, UnityAction<object>>();
	public void AddEventListener(string eventName, UnityAction<object> action)
	{
		if (_events.ContainsKey(eventName))
		{
			_events[eventName] += action;
		}
		else
		{
			_events.Add(eventName, action);
		}
	}
	public void RemoveEventListener(string eventName,UnityAction<object> action)
	{
		if (_events.ContainsKey(eventName))
		{
			_events[eventName] -= action;
			if (_events[eventName] == null)
			{
				_events.Remove(eventName);
			}
		}
	}

	public void DispatchEvent(string eventName,object info)
	{
		if (_events.ContainsKey(eventName))
		{
			_events[eventName].Invoke(info);
		}
	}

	public void Clear()
	{
		_events.Clear();
	}
}