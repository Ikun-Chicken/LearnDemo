using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventConstants
{
	/// <summary>
	/// 游戏中手势输入事件
	/// </summary>
	public const string InputAction = nameof(InputAction);
	/// <summary>
	/// 游戏开始事件
	/// </summary>
	public const string Begin = nameof(Begin);
}

public class InputAtcionEventArgs : EventArgs
{
	public InputDirection InputDirection;
}
