using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有UI面板的根
/// </summary>
public class RootCanvas : MonoBehaviour
{
	private void Start()
	{
		UIManager.Instance.OpenPanel(UIContants.PlayPanel);//默认打开Play面板
		UIManager.Instance.OpenPanel(UIContants.Navigation);
	}
}
