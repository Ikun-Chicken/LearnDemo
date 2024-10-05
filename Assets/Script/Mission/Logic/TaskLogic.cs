using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 任务的逻辑其实就是进度更新、任务完成后领奖、开启下一个任务、开启分支任务等
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
	/// 获取任务数据
	/// </summary>
	/// <param name="cb">回调</param>
	public void GetTaskData(Action cb)
	{
		m_taskData.GetTaskDataFromDB(cb);
	}

	/// <summary>
	/// 更新任务进度
	/// </summary>
	/// <param name="chainId">链id</param>
	/// <param name="subId">任务子id</param>
	/// <param name="deltaProgress">进度增加量</param>
	/// <param name="cb">回调</param>
	public void AddProgress(int chainId, int subId, int deltaProgress, Action<int, bool> cb)
	{
		TaskDataItem data = m_taskData.GetData(chainId, subId);
		if (null != data)
		{
			data.progress += deltaProgress;//加进度
			m_taskData.AddOrUpdateData(data);//更新
			TaskCfgItem cfg = TaskConfiguration.Instance.GetCfgItem(data.task_chain_id, data.task_sub_id);
			if (null != cfg)
				cb(0, data.progress >= cfg.target_amount);//如果达到条件，获得奖励
			else
				cb(-1, false);
		}
		else
		{
			cb(-1, false);
		}
	}

	//是否领奖的状态字段为award_is_get ，为0表示未领奖，为1表示已领过奖。
	//领完奖的任务从列表中移除，并开启下一个任务（如果配置了开启支线任务，
	//则还需要配套开启对应的支线任务），
	/// <summary>
	/// 领取任务奖励
	/// </summary>
	/// <param name="chainId">链id</param>
	/// <param name="subId">任务子id</param>
	/// <param name="cb">回调</param>
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
			m_taskData.AddOrUpdateData(data);//更新任务数据
			GoNext(chainId, subId);//开启下一个任务
			TaskCfgItem cfg = TaskConfiguration.Instance.GetCfgItem(data.task_chain_id, data.task_sub_id);
			cb(0, cfg.award);//获得奖励
		}
		else
		{
			cb(-2, "{}");
		}
	}
	//遍历所有达成进度且未领奖的任务，支线领奖，
	//并开启每个领完奖的任务的下一个任务（如果配置了开启支线任务，则还需要配套开启对应的支线任务），
	/// <summary>
	/// 一键领取所有任务的奖励
	/// </summary>
	/// <param name="cb"></param>
	public void OneKeyGetAward(Action<int, string> cb)
	{
		int totalGold = 0;//累计奖励
		var tmpTaskDatas = new List<TaskDataItem>(m_taskData.TaskDatas);

		for (int i = 0, cnt = tmpTaskDatas.Count; i < cnt; ++i)
		{
			//得到任务数据
			TaskDataItem oneTask = tmpTaskDatas[i];
			//得到配置
			var cfg =TaskConfiguration.Instance.GetCfgItem(oneTask.task_chain_id, oneTask.task_sub_id);
			if (oneTask.progress >= cfg.target_amount && 0 == oneTask.award_is_get)
			{
				oneTask.award_is_get = 1;
				m_taskData.AddOrUpdateData(oneTask);//更新任务数据
				JsonData awardJd = JsonMapper.ToObject(cfg.award);//提取奖励的金币数量
				totalGold += int.Parse(awardJd["gold"].ToString());
				GoNext(oneTask.task_chain_id, oneTask.task_sub_id);
			}
		}
		if (totalGold > 0)
		{
			JsonData totalAward = new JsonData();
			totalAward["gold"] = totalGold;//奖励金币
			cb(0, JsonMapper.ToJson(totalAward));
		}
		else
		{
			cb(-1, null);
		}
	}
	//约定任务id递增，开启下一个任务就是查找id+1的任务并开启。
	//支线任务的开启是open_chain字段，格式链id|任务子id，多个支线以,
	//号隔开，例：3|1,5|1表示开启链3的子任务1和链5的子任务1，
	/// <summary>
	/// 开启下一个任务（含支线）
	/// </summary>
	/// <param name="chainId">链id</param>
	/// <param name="subId">任务子id</param>
	private void GoNext(int chainId, int subId)
	{
		TaskDataItem data = m_taskData.GetData(chainId, subId);
		TaskCfgItem cfg = TaskConfiguration.Instance.GetCfgItem(data.task_chain_id, data.task_sub_id);
		//下一个任务
		TaskCfgItem nextCfg = TaskConfiguration.Instance.GetCfgItem(data.task_chain_id, data.task_sub_id + 1);

		if (1 == data.award_is_get)
		{
			// 移除掉已领奖的任务
			m_taskData.RemoveData(chainId, subId);

			// 如果有下一个任务，开启下一个任务
			if (null != nextCfg)
			{
				TaskDataItem dataItem = new TaskDataItem();
				dataItem.task_chain_id = nextCfg.task_chain_id;
				dataItem.task_sub_id = nextCfg.task_sub_id;
				dataItem.progress = 0;
				dataItem.award_is_get = 0;
				m_taskData.AddOrUpdateData(dataItem);
			}

			// 开启支线任务
			if (!string.IsNullOrEmpty(cfg.open_chain))
			{
				// 开启新分支,使用逗号将 open_chain 字符串分割成一个字符串数组
				string[] chains = cfg.open_chain.Split(',');
				for (int i = 0, len = chains.Length; i < len; ++i)
				{
					//使用竖线将支线任务链字符串分割成两个部分,即任务链id和子任务id
					string[] task = chains[i].Split('|');
					//例：3|1,5|1表示开启链3的子任务1和链5的子任务1，
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


