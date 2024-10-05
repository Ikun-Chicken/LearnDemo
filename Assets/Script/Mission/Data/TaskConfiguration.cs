using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����ֶ���Ҫ��json���ֶ���ͬ
/// <summary>
/// �������ýṹ
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
	//Ϊ�˷������ڴ����������ñ�����ʹ���ֵ����洢������һ����������������ݵ��ֵ䣺
	// �������ã�(��id : ������id : TaskCfgItem)
	// ��һ�������ڱ�ʾΨһ������������/�ֶ��У�,
	// ��Ϊһ�����������ж������������Ҫ����һ��Dic��Ϊvalue����task_sub_id��Ϊ�����ҵ����������
	private Dictionary<int, Dictionary<int, TaskCfgItem>> m_cfg;

	/// <summary>
	/// ��ȡ���ã���Resources�ļ��м�����������
	/// </summary>
	public void LoadCfg()
	{
		// ��ʼ�����������ֵ䣬��Ϊtask_chain_id��ֵΪ��һ���ֵ䣬�洢������ID�����Ӧ������������
		m_cfg = new Dictionary<int, Dictionary<int, TaskCfgItem>>();
		// ��ResourcesĿ¼������Ϊ"task_cfg"���ı���Դ���������ȡΪ�ı��ַ���
		string txt = Resources.Load<TextAsset>("Tables/task_cfg").text;
		// ʹ��LitJson����ȡ���ı�ת��ΪJsonData���󣬷����������
		JsonData jd = JsonMapper.ToObject<JsonData>(txt);

		// ����JsonData�����е�ÿһ������
		for (int i = 0, cnt = jd.Count; i < cnt; ++i)
		{
			// ȡ����ǰ�������õ�Json����,�����������е�һ����¼
			JsonData itemJd = jd[i];
			// ʹ��JsonMapper����ǰ�������õ�Json���ݷ����л�ΪTaskCfgItem����
			TaskCfgItem cfgItem = JsonMapper.ToObject<TaskCfgItem>(itemJd.ToJson());
			// �����ǰ������ID��û�����ֵ��У��򴴽�һ���µ����ֵ�
			if (!m_cfg.ContainsKey(cfgItem.task_chain_id))
			{
				//���������
				m_cfg[cfgItem.task_chain_id] = new Dictionary<int, TaskCfgItem>();
			}
			//�ڴ�����������Ӿ�������
			m_cfg[cfgItem.task_chain_id].Add(cfgItem.task_sub_id, cfgItem);
		}
	}

	/// <summary>
	/// ��ȡ��������ڿ��ٻ�ȡ������������
	/// </summary>
	/// <param name="chainId">��id</param>
	/// <param name="taskSubId">������id</param>
	/// <returns></returns>
	public TaskCfgItem GetCfgItem(int chainId, int taskSubId)
	{
		if (m_cfg.ContainsKey(chainId) && m_cfg[chainId].ContainsKey(taskSubId))
			return m_cfg[chainId][taskSubId];
		return null;
	}
}
