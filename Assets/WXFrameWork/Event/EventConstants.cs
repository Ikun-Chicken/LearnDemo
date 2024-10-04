using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventConstants
{
	/// <summary>
	/// ��Ϸ�����������¼�
	/// </summary>
	public const string InputAction = nameof(InputAction);
	/// <summary>
	/// ��Ϸ��ʼ�¼�
	/// </summary>
	public const string Begin = nameof(Begin);
}

public class InputAtcionEventArgs : EventArgs
{
	public InputDirection InputDirection;
}
