using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����UI���ĸ�
/// </summary>
public class RootCanvas : MonoBehaviour
{
	private void Start()
	{
		UIManager.Instance.OpenPanel(UIContants.PlayPanel);//Ĭ�ϴ�Play���
		UIManager.Instance.OpenPanel(UIContants.Navigation);
	}
}
