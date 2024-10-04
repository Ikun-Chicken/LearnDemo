using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 分别对应左右上下四个方向的输入指令
/// </summary>
public enum InputDirection
{
	NULL,
	Left,
	Right,
	Up,
	Down
}
public class PlayerInput : MonoBehaviour
{
	InputDirection inputDirection;
	bool activeInput;//是否按住了鼠标
	Vector3 mousePos;//记录鼠标按下那一刻的位置

	private void Update()
	{
		Test();
		GetInputDirection();
	}
	void GetInputDirection()
	{
		inputDirection = InputDirection.NULL;
		if (Input.GetMouseButtonDown(0))//按下鼠标那一刻，记录鼠标位置
		{
			activeInput = true;
			mousePos = Input.mousePosition;
		}
		if (Input.GetMouseButton(0) && activeInput)
		{
			Vector3 vec = Input.mousePosition - mousePos;
			//Debug.Log(vec.magnitude);
			if (vec.magnitude > 10)
			{
				var angleY = Mathf.Acos(Vector3.Dot(vec.normalized, Vector2.up)) * Mathf.Rad2Deg;
				var anglex = Mathf.Acos(Vector3.Dot(vec.normalized, Vector2.right)) * Mathf.Rad2Deg;
				//Debug.Log(anglex);
				//Debug.Log(angleY);
				if (angleY <= 45)
				{
					inputDirection = InputDirection.Up;
				}
				else if (angleY >= 135)
				{
					inputDirection = InputDirection.Down;
				}
				else if (anglex <= 45)
				{
					inputDirection = InputDirection.Right;
				}
				else if (anglex >= 135)
				{
					inputDirection = InputDirection.Left;
				}
				activeInput = false;//完成一次输入操作，置为false
				this.DispatchEvent(EventConstants.InputAction,
					new InputAtcionEventArgs { InputDirection=inputDirection} );
				//把输入结果传过去
				//Debug.Log("触发事件");
			}
		}
	}
	void Test()
	{
        if (Input.GetKeyDown(KeyCode.P))
        {
			this.DispatchEvent(EventConstants.Begin);
			AudioManager.Instance.PlayBGM();
        }
		if (Input.GetKeyDown(KeyCode.C))
		{
			AudioManager.Instance.PlaySound(AudioConstans.coin);
		}
	}
}
