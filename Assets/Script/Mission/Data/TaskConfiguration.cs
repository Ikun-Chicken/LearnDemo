using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//类的字段名要与json的字段相同
/// <summary>
/// 任务配置结构
/// </summary>
public class TaskCfgItem
{
	public int task_chain_id;
	public int task_sub_id;
	public string icon;
	public string desc;
	public string task_target;
	public int target_amount;
	public string award;
	public string open_chain;
}

public class TaskConfiguration :Singleton<TaskConfiguration>
{
	//为了方便在内存中索引配置表，我们使用字典来存储，定义一个用来存放配置数据的字典：
	// 任务配置，(链id : 子任务id : TaskCfgItem)
	// 第一个键用于表示唯一的任务链（主/分都有）,
	// 因为一个任务链下有多个任务，所以需要再用一个Dic作为value，用task_sub_id作为键来找到具体的任务
	private Dictionary<int, Dictionary<int, TaskCfgItem>> m_cfg;

	/// <summary>
	/// 读取配置，从Resources文件中加载任务配置
	/// </summary>
	public void LoadCfg()
	{
		// 初始化任务配置字典，键为task_chain_id，值为另一个字典，存储子任务ID及其对应的任务配置项
		m_cfg = new Dictionary<int, Dictionary<int, TaskCfgItem>>();
		// 从Resources目录加载名为"task_cfg"的文本资源，并将其读取为文本字符串
		string txt = Resources.Load<TextAsset>("Tables/task_cfg").text;
		// 使用LitJson将读取的文本转换为JsonData对象，方便后续解析
		JsonData jd = JsonMapper.ToObject<JsonData>(txt);

		// 遍历JsonData对象中的每一项数据
		for (int i = 0, cnt = jd.Count; i < cnt; ++i)
		{
			// 取出当前任务配置的Json数据,即任务配置中的一条记录
			JsonData itemJd = jd[i];
			// 使用JsonMapper将当前任务配置的Json数据反序列化为TaskCfgItem对象
			TaskCfgItem cfgItem = JsonMapper.ToObject<TaskCfgItem>(itemJd.ToJson());
			// 如果当前任务链ID还没有在字典中，则创建一个新的子字典
			if (!m_cfg.ContainsKey(cfgItem.task_chain_id))
			{
				//添加任务链
				m_cfg[cfgItem.task_chain_id] = new Dictionary<int, TaskCfgItem>();
			}
			//在此任务链下添加具体任务
			m_cfg[cfgItem.task_chain_id].Add(cfgItem.task_sub_id, cfgItem);
		}
	}

	/// <summary>
	/// 获取配置项，用于快速获取具体任务配置
	/// </summary>
	/// <param name="chainId">链id</param>
	/// <param name="taskSubId">任务子id</param>
	/// <returns></returns>
	public TaskCfgItem GetCfgItem(int chainId, int taskSubId)
	{
		if (m_cfg.ContainsKey(chainId) && m_cfg[chainId].ContainsKey(taskSubId))
			return m_cfg[chainId][taskSubId];
		return null;
	}
}
