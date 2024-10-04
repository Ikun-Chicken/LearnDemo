using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingletonMono<T> :MonoBehaviour where T: MonoBehaviour
{
	private static T instance;
	private static bool applicationIsQuitting = false;
	// 私有构造函数，防止外部实例化
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
					Debug.Log("创建新的单例脚本：" + typeof(T).Name);
				}
				else
				{
					//Debug.Log("找到对应单例脚本，该脚本"+ typeof(T).Name + "挂载在：" +instance.gameObject.name);
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