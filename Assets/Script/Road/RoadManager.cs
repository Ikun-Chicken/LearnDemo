using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadManager : MonoBehaviour
{
	[Header("����ѭ�����õ�·")]
	public GameObject floorOnRunning; // ��ǰ��������ܵĵ������
	public GameObject floorForward;   // ǰ�������νӵĵ������
	public static RoadManager instance; 

	void Start()
	{
		instance = this; 
	}
	/// <summary>
	/// ɾ�������ϵ���Ʒ
	/// </summary>
	/// <param name="floor">��Ҫɾ����Ʒ�ĵ������</param>
	void RemoveItem(GameObject floor)
	{
		// ����floor�����е�������"Item"��ͨ������Ʒ������
		var item = floor.transform.Find("Item");
		if (item != null)
		{
			// ����Item�е�����������
			foreach (var child in item)
			{
				// ��childת��ΪTransform����
				Transform childTransform = child as Transform;
				if (childTransform != null)
				{
					// ����������
					Destroy(childTransform.gameObject);
				}
			}
		}
	}

	/// <summary>
	/// ���������µ���Ʒ
	/// </summary>
	/// <param name="floor">��Ҫ�����Ʒ�ĵ������</param>
	void AddItem(GameObject floor)
	{
		// ����floor�����е�������"Item"
		var item = floor.transform.Find("Item");
		if (item != null)
		{
			var objManager = ObjectsManager.Instance;
			if (objManager != null && objManager.ListObjects != null && objManager.ListObjects.Count > 0)
			{
				// �����Patterns�б���ѡ��һ��
				var objects = objManager.ListObjects[Random.Range(0, objManager.ListObjects.Count)];
				if (objects != null && objects.ObjectItems != null && objects.ObjectItems.Count > 0)
				{
					// ����objects�е�����PatternItems��ʵ������������Ʒ
					foreach (var obj in objects.ObjectItems)
					{
						var newObj = Instantiate(obj.gameobject); // ʵ������Ʒ
						newObj.transform.parent = item; // ��ʵ��������Ʒ����ΪItem��������
						newObj.transform.localPosition = obj.position; // ������Ʒ�ľֲ�λ��
					}
				}
			}
		}
	}

	void Update()
	{
		// ����ҵ�ǰλ�õ�zֵ����floorOnRunning�����zֵ+һ��ʱ�����������л�
		if (transform.position.z > floorOnRunning.transform.position.z + 100)
		{
			RemoveItem(floorOnRunning); // �Ƴ���ǰ�������Ʒ
			AddItem(floorOnRunning); // ����µ���Ʒ����ǰ����

			// ����ǰ�����λ����ǰ�ƶ���floorForward��ǰ��
			floorOnRunning.transform.position = new Vector3(0, 0, floorForward.transform.position.z + 200);

			// ����floorOnRunning��floorForward�����ã�ʹ���µ����Ϊ��ǰ����
			GameObject temp = floorOnRunning;
			floorOnRunning = floorForward;
			floorForward = temp;
		}
	}
}
