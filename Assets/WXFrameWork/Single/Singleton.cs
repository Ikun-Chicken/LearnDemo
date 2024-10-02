using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Singleton<T> where T : new()
{
	private static T instance;

	// 私有构造函数，防止外部实例化
	protected Singleton() { }

	public static T Instance
	{
		get
		{
			if (instance == null)
				instance = new T();
			return instance;
		}
	}
}
