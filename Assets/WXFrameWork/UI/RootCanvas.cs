using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootCanvas : MonoBehaviour
{
	private void Start()
	{
		UIManager.Instance.OpenPanel(UIContants.Main);
	}
}
