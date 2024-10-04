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
/// 驱动角色进行向前移动，向左右换跑道，跳跃
/// </summary>
public class PlayerController : MonoBehaviour
{
	[Header("速度相关")]
	[SerializeField]
	private float curSpeed=5;//当前的速度
	[SerializeField]
	private float jumpValue = 7;//跳跃时向上的速度
	private float gravity = 20;//重力
	[SerializeField]
	private Vector3 resultantVelocity;//合成速度，z，x，y
	Vector3 xDirection;

	[Header("地面检测相关")]
	CharacterController characterController;
	public bool isGrounded = true;
	public float raycastDistance = 0.1f; // 射线的长度，用来检测地面
	public LayerMask groundLayer;        // 地面所在的层，用来确保只检测地面

	[Header("玩家位置相关")]
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
		//TODO 播放Jump
		resultantVelocity.y += jumpValue;
	}
	void QuickGround()
	{
		resultantVelocity.y -= jumpValue * 3;
	}
	void MoveLeft()
	{
		if (standPosition != Position.Left)//当前不在左跑道，才能左跳
		{
			//TODO 动画
			xDirection = Vector3.left;
			//下面是更新标准位置，为了最后落在正确的位置
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
			//TODO 动画
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
		//Debug.Log("被触发");
		var inputResult= (e as InputAtcionEventArgs).InputDirection;
		if (inputResult == InputDirection.Left)//左跳
		{
			MoveLeft();
		}
		else if (inputResult == InputDirection.Right)//右跳
		{
			MoveRight();
		}
		//Debug.Log(inputResult);
		if (inputResult == InputDirection.Down)
		{
			//TODO 直接播放Roll
			//Debug.Log("下降");
		}
		//Debug.Log(isGrounded);
		if (isGrounded)//在地面
		{
			resultantVelocity.y = 0f;//清除y轴上的残存速度，以免影响跳跃
			//TODO 没有翻滚，没有左右移动，就默认播放Run
			if (inputResult == InputDirection.Up)
			{
				JumpUp();
				//Debug.Log("跳跃");
			}
		}
		else//不在地面
		{
			if (inputResult == InputDirection.Down)
			{
				Debug.Log(characterController.isGrounded);
				QuickGround();
				//Debug.Log("快速下降");
			}
			//TODO 没有处于起跳，翻滚，就默认播放JumpLoop
		}
	}

	private void OnDrawGizmos()
	{
		DebugGizmosDrawer.DrawGroundCheckLine(transform.position, Vector3.down, raycastDistance);
	}
}
