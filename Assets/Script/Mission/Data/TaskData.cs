using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���ڶ�ȡ�������ݣ�д��������
/// </summary>
public class TaskData
{
	//����ӱ��ػ��߷�������ȡ����������
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
	/// �����ݿ��ȡ��������
	/// </summary>
	/// <param name="cb"></param>
	public void GetTaskDataFromDB(Action cb)
	{
		// ������������ͨ�ţ������ݿ��ж�ȡ�����﴿�ͻ��˽���ģ�⣬ֱ��ʹ��PlayerPrefs�ӿͻ��˱��ض�ȡ
		string jsonStr = PlayerPrefs.GetString("TASK_DATA", "[{'task_chain_id':1,'task_sub_id':1,'progress':0,'award_is_get':0}]");
		var taskList = JsonMapper.ToObject<List<TaskDataItem>>(jsonStr);
		for (int i = 0, cnt = taskList.Count; i < cnt; ++i)
		{
			AddOrUpdateData(taskList[i]);
		}
		cb();
	}

	/// <summary>
	/// д���ݵ����ݿ�
	/// </summary>
	private void SaveDataToDB()
	{
	    string jsonStr = JsonMapper.ToJson(m_taskDatas);
		PlayerPrefs.SetString("TASK_DATA", jsonStr);
	}

	/// <summary>
	/// ��ȡĳ����������
	/// </summary>
	/// <param name="chainId">��id</param>
	/// <param name="subId">������id</param>
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
	/// ��ӻ��������
	/// </summary>
	public void AddOrUpdateData(TaskDataItem itemData)
	{
		bool isUpdateExist = false;//�Ƿ�������Ѿ����ڵ�
		for (int i = 0, cnt = m_taskDatas.Count; i < cnt; ++i)
		{
			var item = m_taskDatas[i];
			if (itemData.task_chain_id == item.task_chain_id && itemData.task_sub_id == item.task_sub_id)
			{
				// ��������
				m_taskDatas[i] = itemData;
				isUpdateExist = true;
				break;
			}
		}
		if (!isUpdateExist)
			m_taskDatas.Add(itemData);//ֱ�Ӽ��������
		// ����ȷ����������ǰ��
		m_taskDatas.Sort((a, b) =>
		{
			return a.task_chain_id.CompareTo(b.task_chain_id);
		});
		SaveDataToDB();
	}

	/// <summary>
	/// �Ƴ���������
	/// </summary>
	/// <param name="chainId">��id</param>
	/// <param name="subId">������id</param>
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
/// ��������
/// </summary>
public class TaskDataItem
{
	// ��id
	public int task_chain_id;
	// ������id
	public int task_sub_id;
	// ����
	public int progress;
	// �����Ƿ���ȡ�ˣ�0��δ����ȡ��1���ѱ���ȡ
	public short award_is_get;
}
