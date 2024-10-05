using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������߼���ʵ���ǽ��ȸ��¡�������ɺ��콱��������һ�����񡢿�����֧�����
/// </summary>

public class TaskLogic : MonoBehaviour
{
	private TaskData m_taskData;
	public TaskLogic()
	{
		m_taskData = new TaskData();
	}
	public List<TaskDataItem> TaskDatas
	{
		get { return m_taskData.TaskDatas; }
	}

	/// <summary>
	/// ��ȡ��������
	/// </summary>
	/// <param name="cb">�ص�</param>
	public void GetTaskData(Action cb)
	{
		m_taskData.GetTaskDataFromDB(cb);
	}

	/// <summary>
	/// �����������
	/// </summary>
	/// <param name="chainId">��id</param>
	/// <param name="subId">������id</param>
	/// <param name="deltaProgress">����������</param>
	/// <param name="cb">�ص�</param>
	public void AddProgress(int chainId, int subId, int deltaProgress, Action<int, bool> cb)
	{
		TaskDataItem data = m_taskData.GetData(chainId, subId);
		if (null != data)
		{
			data.progress += deltaProgress;//�ӽ���
			m_taskData.AddOrUpdateData(data);//����
			TaskCfgItem cfg = TaskConfiguration.Instance.GetCfgItem(data.task_chain_id, data.task_sub_id);
			if (null != cfg)
				cb(0, data.progress >= cfg.target_amount);//����ﵽ��������ý���
			else
				cb(-1, false);
		}
		else
		{
			cb(-1, false);
		}
	}

	//�Ƿ��콱��״̬�ֶ�Ϊaward_is_get ��Ϊ0��ʾδ�콱��Ϊ1��ʾ���������
	//���꽱��������б����Ƴ�����������һ��������������˿���֧������
	//����Ҫ���׿�����Ӧ��֧�����񣩣�
	/// <summary>
	/// ��ȡ������
	/// </summary>
	/// <param name="chainId">��id</param>
	/// <param name="subId">������id</param>
	/// <param name="cb">�ص�</param>
	public void GetAward(int chainId, int subId, Action<int, string> cb)
	{
		TaskDataItem data = m_taskData.GetData(chainId, subId);
		if (null == data)
		{
			cb(-1, "{}");
			return;
		}
		if (0 == data.award_is_get)
		{
			data.award_is_get = 1;
			m_taskData.AddOrUpdateData(data);//������������
			GoNext(chainId, subId);//������һ������
			TaskCfgItem cfg = TaskConfiguration.Instance.GetCfgItem(data.task_chain_id, data.task_sub_id);
			cb(0, cfg.award);//��ý���
		}
		else
		{
			cb(-2, "{}");
		}
	}
	//�������д�ɽ�����δ�콱������֧���콱��
	//������ÿ�����꽱���������һ��������������˿���֧����������Ҫ���׿�����Ӧ��֧�����񣩣�
	/// <summary>
	/// һ����ȡ��������Ľ���
	/// </summary>
	/// <param name="cb"></param>
	public void OneKeyGetAward(Action<int, string> cb)
	{
		int totalGold = 0;//�ۼƽ���
		var tmpTaskDatas = new List<TaskDataItem>(m_taskData.TaskDatas);

		for (int i = 0, cnt = tmpTaskDatas.Count; i < cnt; ++i)
		{
			//�õ���������
			TaskDataItem oneTask = tmpTaskDatas[i];
			//�õ�����
			var cfg =TaskConfiguration.Instance.GetCfgItem(oneTask.task_chain_id, oneTask.task_sub_id);
			if (oneTask.progress >= cfg.target_amount && 0 == oneTask.award_is_get)
			{
				oneTask.award_is_get = 1;
				m_taskData.AddOrUpdateData(oneTask);//������������
				JsonData awardJd = JsonMapper.ToObject(cfg.award);//��ȡ�����Ľ������
				totalGold += int.Parse(awardJd["gold"].ToString());
				GoNext(oneTask.task_chain_id, oneTask.task_sub_id);
			}
		}
		if (totalGold > 0)
		{
			JsonData totalAward = new JsonData();
			totalAward["gold"] = totalGold;//�������
			cb(0, JsonMapper.ToJson(totalAward));
		}
		else
		{
			cb(-1, null);
		}
	}
	//Լ������id������������һ��������ǲ���id+1�����񲢿�����
	//֧������Ŀ�����open_chain�ֶΣ���ʽ��id|������id�����֧����,
	//�Ÿ���������3|1,5|1��ʾ������3��������1����5��������1��
	/// <summary>
	/// ������һ�����񣨺�֧�ߣ�
	/// </summary>
	/// <param name="chainId">��id</param>
	/// <param name="subId">������id</param>
	private void GoNext(int chainId, int subId)
	{
		TaskDataItem data = m_taskData.GetData(chainId, subId);
		TaskCfgItem cfg = TaskConfiguration.Instance.GetCfgItem(data.task_chain_id, data.task_sub_id);
		//��һ������
		TaskCfgItem nextCfg = TaskConfiguration.Instance.GetCfgItem(data.task_chain_id, data.task_sub_id + 1);

		if (1 == data.award_is_get)
		{
			// �Ƴ������콱������
			m_taskData.RemoveData(chainId, subId);

			// �������һ�����񣬿�����һ������
			if (null != nextCfg)
			{
				TaskDataItem dataItem = new TaskDataItem();
				dataItem.task_chain_id = nextCfg.task_chain_id;
				dataItem.task_sub_id = nextCfg.task_sub_id;
				dataItem.progress = 0;
				dataItem.award_is_get = 0;
				m_taskData.AddOrUpdateData(dataItem);
			}

			// ����֧������
			if (!string.IsNullOrEmpty(cfg.open_chain))
			{
				// �����·�֧,ʹ�ö��Ž� open_chain �ַ����ָ��һ���ַ�������
				string[] chains = cfg.open_chain.Split(',');
				for (int i = 0, len = chains.Length; i < len; ++i)
				{
					//ʹ�����߽�֧���������ַ����ָ����������,��������id��������id
					string[] task = chains[i].Split('|');
					//����3|1,5|1��ʾ������3��������1����5��������1��
					TaskDataItem subChainDataItem = new TaskDataItem();
					subChainDataItem.task_chain_id = int.Parse(task[0]);
					subChainDataItem.task_sub_id = int.Parse(task[1]);
					subChainDataItem.progress = 0;
					subChainDataItem.award_is_get = 0;
					m_taskData.AddOrUpdateData(subChainDataItem);
				}
			}
		}
	}

}


