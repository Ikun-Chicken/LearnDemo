using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingletonMono<T> :MonoBehaviour where T: MonoBehaviour
{
	private static T instance;
	private static bool applicationIsQuitting = false;
	// ˽�й��캯������ֹ�ⲿʵ����
	protected SingletonMono() { }

	public static T Instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
					"' already destroyed on application quit." +
					" Won't create again - returning null.");
				return null;
			}

			if (instance == null)
			{
				instance = FindObjectOfType<T>();
				if (instance == null)
				{
					GameObject obj = new GameObject(typeof(T).Name);
					instance = obj.AddComponent<T>();
					DontDestroyOnLoad(obj);
					Debug.Log("�����µĵ����ű���" + typeof(T).Name);
				}
				else
				{
					//Debug.Log("�ҵ���Ӧ�����ű����ýű�"+ typeof(T).Name + "�����ڣ�" +instance.gameObject.name);
				}
			}
			return instance;
		}
	}
	protected virtual void OnDestroy()
	{
		applicationIsQuitting = true;
	}
}