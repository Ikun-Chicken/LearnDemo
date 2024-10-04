using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ֱ��Ӧ���������ĸ����������ָ��
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
	bool activeInput;//�Ƿ�ס�����
	Vector3 mousePos;//��¼��갴����һ�̵�λ��

	private void Update()
	{
		Test();
		GetInputDirection();
	}
	void GetInputDirection()
	{
		inputDirection = InputDirection.NULL;
		if (Input.GetMouseButtonDown(0))//���������һ�̣���¼���λ��
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
				activeInput = false;//���һ�������������Ϊfalse
				this.DispatchEvent(EventConstants.InputAction,
					new InputAtcionEventArgs { InputDirection=inputDirection} );
				//������������ȥ
				//Debug.Log("�����¼�");
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
