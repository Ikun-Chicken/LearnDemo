using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
public enum Position
{
	Left,
	Middle,
	Right
}
/// <summary>
/// ������ɫ������ǰ�ƶ��������һ��ܵ�����Ծ
/// </summary>
public class PlayerController : MonoBehaviour
{
	[Header("�ٶ����")]
	[SerializeField]
	private float curSpeed=5;//��ǰ���ٶ�
	[SerializeField]
	private float jumpValue = 7;//��Ծʱ���ϵ��ٶ�
	private float gravity = 20;//����
	[SerializeField]
	private Vector3 resultantVelocity;//�ϳ��ٶȣ�z��x��y
	Vector3 xDirection;

	[Header("���������")]
	CharacterController characterController;
	public bool isGrounded = true;
	public float raycastDistance = 0.1f; // ���ߵĳ��ȣ�����������
	public LayerMask groundLayer;        // �������ڵĲ㣬����ȷ��ֻ������

	[Header("���λ�����")]
	public Position standPosition;
	public Position fromPosition;
	private void Start()
	{
		characterController = GetComponent<CharacterController>();
		standPosition = Position.Middle;
	}
	private void OnEnable()
	{
		EventManager.Instance.AddEventListener(EventConstants.InputAction,ExecuteAction);
		EventManager.Instance.AddEventListener(EventConstants.Begin, InitSpeed);
	}
	private void OnDisable()
	{
		EventManager.Instance.RemoveEventListener(EventConstants.InputAction, ExecuteAction);
		EventManager.Instance.RemoveEventListener(EventConstants.Begin, InitSpeed);
	}
	private void Update()
	{
		DetectionGround();
		MoveToStandPosition();
		MoveForward();	
	}
	void DetectionGround()
	{
		isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer);
	}
	void MoveForward()
	{
		resultantVelocity.y -= gravity * Time.deltaTime;
		characterController.Move((xDirection * 10+resultantVelocity) * Time.deltaTime);
	}
	void MoveToStandPosition()
	{
		if (standPosition == Position.Left)
		{
			if (transform.position.x <= -1.7f)
			{
				xDirection = Vector3.zero;
				transform.position = new Vector3(-1.7f, transform.position.y, transform.position.z);
			}
		}
		if (standPosition == Position.Middle)
		{
			if (fromPosition == Position.Left)
			{
				if (transform.position.x > 0)
				{
					xDirection = Vector3.zero;
					transform.position = new Vector3(0, transform.position.y, transform.position.z);
				}
			}
			else if (fromPosition == Position.Right)
			{
				if (transform.position.x < 0)
				{
					xDirection = Vector3.zero;
					transform.position = new Vector3(0, transform.position.y, transform.position.z);
				}
			}
		}

		if (standPosition == Position.Right)
		{
			if (transform.position.x >= 1.7f)
			{
				xDirection = Vector3.zero;
				transform.position = new Vector3(1.7f, transform.position.y, transform.position.z);
			}
		}
	}
	void InitSpeed(object sender,EventArgs e)
	{
		resultantVelocity.z = curSpeed;
	}
	void JumpUp()
	{
		//TODO ����Jump
		resultantVelocity.y += jumpValue;
	}
	void QuickGround()
	{
		resultantVelocity.y -= jumpValue * 3;
	}
	void MoveLeft()
	{
		if (standPosition != Position.Left)//��ǰ�������ܵ�����������
		{
			//TODO ����
			xDirection = Vector3.left;
			//�����Ǹ��±�׼λ�ã�Ϊ�����������ȷ��λ��
			if (standPosition == Position.Middle)
			{
				standPosition = Position.Left;
				fromPosition = Position.Middle;
			}
			else if (standPosition == Position.Right)
			{
				standPosition = Position.Middle;
				fromPosition = Position.Right;
			}
		}
	}
	void MoveRight()
	{
		if (standPosition != Position.Right)
		{
			//TODO ����
			xDirection = Vector3.right;
			if (standPosition == Position.Middle)
			{
				standPosition = Position.Right;
				fromPosition = Position.Middle;
			}
			else if (standPosition == Position.Left)
			{
				standPosition = Position.Middle;
				fromPosition = Position.Left;
			}
		}
	}

	void ExecuteAction(object sender,EventArgs e)
	{
		//Debug.Log("������");
		var inputResult= (e as InputAtcionEventArgs).InputDirection;
		if (inputResult == InputDirection.Left)//����
		{
			MoveLeft();
		}
		else if (inputResult == InputDirection.Right)//����
		{
			MoveRight();
		}
		//Debug.Log(inputResult);
		if (inputResult == InputDirection.Down)
		{
			//TODO ֱ�Ӳ���Roll
			//Debug.Log("�½�");
		}
		//Debug.Log(isGrounded);
		if (isGrounded)//�ڵ���
		{
			resultantVelocity.y = 0f;//���y���ϵĲд��ٶȣ�����Ӱ����Ծ
			//TODO û�з�����û�������ƶ�����Ĭ�ϲ���Run
			if (inputResult == InputDirection.Up)
			{
				JumpUp();
				//Debug.Log("��Ծ");
			}
		}
		else//���ڵ���
		{
			if (inputResult == InputDirection.Down)
			{
				Debug.Log(characterController.isGrounded);
				QuickGround();
				//Debug.Log("�����½�");
			}
			//TODO û�д�����������������Ĭ�ϲ���JumpLoop
		}
	}

	private void OnDrawGizmos()
	{
		DebugGizmosDrawer.DrawGroundCheckLine(transform.position, Vector3.down, raycastDistance);
	}
}
