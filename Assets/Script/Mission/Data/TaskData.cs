using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用于读取任务数据，写任务数据
/// </summary>
public class TaskData
{
	//缓存从本地或者服务器读取的任务数据
	private List<TaskDataItem> m_taskDatas;
	public TaskData()
	{
		m_taskDatas = new List<TaskDataItem>();
	}
	public List<TaskDataItem> TaskDatas
	{
		get { return m_taskDatas; }
	}
	/// <summary>
	/// 从数据库读取任务数据
	/// </summary>
	/// <param name="cb"></param>
	public void GetTaskDataFromDB(Action cb)
	{
		// 正规是与服务端通信，从数据库中读取，这里纯客户端进行模拟，直接使用PlayerPrefs从客户端本地读取
		string jsonStr = PlayerPrefs.GetString("TASK_DATA", "[{'task_chain_id':1,'task_sub_id':1,'progress':0,'award_is_get':0}]");
		var taskList = JsonMapper.ToObject<List<TaskDataItem>>(jsonStr);
		for (int i = 0, cnt = taskList.Count; i < cnt; ++i)
		{
			AddOrUpdateData(taskList[i]);
		}
		cb();
	}

	/// <summary>
	/// 写数据到数据库
	/// </summary>
	private void SaveDataToDB()
	{
	    string jsonStr = JsonMapper.ToJson(m_taskDatas);
		PlayerPrefs.SetString("TASK_DATA", jsonStr);
	}

	/// <summary>
	/// 获取某个任务数据
	/// </summary>
	/// <param name="chainId">链id</param>
	/// <param name="subId">任务子id</param>
	/// <returns></returns>
	public TaskDataItem GetData(int chainId, int subId)
	{
		for (int i = 0, cnt = m_taskDatas.Count; i < cnt; ++i)
		{
			TaskDataItem item = m_taskDatas[i];
			if (chainId == item.task_chain_id && subId == item.task_sub_id)
				return item;
		}
		return null;
	}

	/// <summary>
	/// 添加或更新任务
	/// </summary>
	public void AddOrUpdateData(TaskDataItem itemData)
	{
		bool isUpdateExist = false;//是否更新了已经存在的
		for (int i = 0, cnt = m_taskDatas.Count; i < cnt; ++i)
		{
			var item = m_taskDatas[i];
			if (itemData.task_chain_id == item.task_chain_id && itemData.task_sub_id == item.task_sub_id)
			{
				// 更新数据
				m_taskDatas[i] = itemData;
				isUpdateExist = true;
				break;
			}
		}
		if (!isUpdateExist)
			m_taskDatas.Add(itemData);//直接加入任务表
		// 排序，确保主线在最前面
		m_taskDatas.Sort((a, b) =>
		{
			return a.task_chain_id.CompareTo(b.task_chain_id);
		});
		SaveDataToDB();
	}

	/// <summary>
	/// 移除任务数据
	/// </summary>
	/// <param name="chainId">链id</param>
	/// <param name="subId">任务子id</param>
	public void RemoveData(int chainId, int subId)
	{
		for (int i = 0, cnt = m_taskDatas.Count; i < cnt; ++i)
		{
			var item = m_taskDatas[i];
			if (chainId == item.task_chain_id && subId == item.task_sub_id)
			{
				m_taskDatas.Remove(item);
				SaveDataToDB();
				return;
			}
		}
	}
}
/// <summary>
/// 任务数据
/// </summary>
public class TaskDataItem
{
	// 链id
	public int task_chain_id;
	// 任务子id
	public int task_sub_id;
	// 进度
	public int progress;
	// 奖励是否被领取了，0：未被领取，1：已被领取
	public short award_is_get;
}
