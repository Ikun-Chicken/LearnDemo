using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : SingletonMono<ObjectsManager>
{
	/// <summary>
	/// 几组物品
	/// </summary>
	public List<Objects> ListObjects = new List<Objects>();
}
/// <summary>
/// 一组物品
/// </summary>
[System.Serializable]
public class Objects
{
	public List<objItem> ObjectItems = new List<objItem>();
}
/// <summary>
/// 放在Objects中的物品
/// </summary>
[System.Serializable]
public class objItem
{
	// 物品的GameObject实例
	public GameObject gameobject;
	// 物品在Objects中的相对位置
	public Vector3 position;
}