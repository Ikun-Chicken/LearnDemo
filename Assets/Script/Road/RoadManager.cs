using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadManager : MonoBehaviour
{
	[Header("两段循环利用的路")]
	public GameObject floorOnRunning; // 当前玩家正在跑的地面对象
	public GameObject floorForward;   // 前方即将衔接的地面对象
	public static RoadManager instance; 

	void Start()
	{
		instance = this; 
	}
	/// <summary>
	/// 删除地面上的物品
	/// </summary>
	/// <param name="floor">需要删除物品的地面对象</param>
	void RemoveItem(GameObject floor)
	{
		// 查找floor对象中的子物体"Item"（通常是物品容器）
		var item = floor.transform.Find("Item");
		if (item != null)
		{
			// 遍历Item中的所有子物体
			foreach (var child in item)
			{
				// 将child转换为Transform类型
				Transform childTransform = child as Transform;
				if (childTransform != null)
				{
					// 销毁子物体
					Destroy(childTransform.gameObject);
				}
			}
		}
	}

	/// <summary>
	/// 向地面添加新的物品
	/// </summary>
	/// <param name="floor">需要添加物品的地面对象</param>
	void AddItem(GameObject floor)
	{
		// 查找floor对象中的子物体"Item"
		var item = floor.transform.Find("Item");
		if (item != null)
		{
			var objManager = ObjectsManager.Instance;
			if (objManager != null && objManager.ListObjects != null && objManager.ListObjects.Count > 0)
			{
				// 随机从Patterns列表中选择一组
				var objects = objManager.ListObjects[Random.Range(0, objManager.ListObjects.Count)];
				if (objects != null && objects.ObjectItems != null && objects.ObjectItems.Count > 0)
				{
					// 遍历objects中的所有PatternItems，实例化并放置物品
					foreach (var obj in objects.ObjectItems)
					{
						var newObj = Instantiate(obj.gameobject); // 实例化物品
						newObj.transform.parent = item; // 将实例化的物品设置为Item的子物体
						newObj.transform.localPosition = obj.position; // 设置物品的局部位置
					}
				}
			}
		}
	}

	void Update()
	{
		// 当玩家当前位置的z值超过floorOnRunning地面的z值+一半时，触发地面切换
		if (transform.position.z > floorOnRunning.transform.position.z + 100)
		{
			RemoveItem(floorOnRunning); // 移除当前地面的物品
			AddItem(floorOnRunning); // 添加新的物品到当前地面

			// 将当前地面的位置向前移动到floorForward的前方
			floorOnRunning.transform.position = new Vector3(0, 0, floorForward.transform.position.z + 200);

			// 交换floorOnRunning和floorForward的引用，使得新地面成为当前地面
			GameObject temp = floorOnRunning;
			floorOnRunning = floorForward;
			floorForward = temp;
		}
	}
}
