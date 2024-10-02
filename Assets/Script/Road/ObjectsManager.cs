using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : SingletonMono<ObjectsManager>
{
	/// <summary>
	/// ������Ʒ
	/// </summary>
	public List<Objects> ListObjects = new List<Objects>();
}
/// <summary>
/// һ����Ʒ
/// </summary>
[System.Serializable]
public class Objects
{
	public List<objItem> ObjectItems = new List<objItem>();
}
/// <summary>
/// ����Objects�е���Ʒ
/// </summary>
[System.Serializable]
public class objItem
{
	// ��Ʒ��GameObjectʵ��
	public GameObject gameobject;
	// ��Ʒ��Objects�е����λ��
	public Vector3 position;
}