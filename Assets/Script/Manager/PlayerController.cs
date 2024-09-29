using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum Position
{
	Left,
	Middle,
	Right
}
public enum InputDirection
{
	NULL,
	Left,
	Right,
	Up,
	Down
}

public class PlayerController : MonoBehaviour
{
	public float Speed { get; private set; } = 1;
	public float Init_speed { get; private set; } = 5;
	private float maxSpeed = 20;
	InputDirection inputDirection;

	bool activeInput;//是否激活输入，即鼠标按下那一刻就是激活
	Vector3 activePoint_mousePos;//用来记录激活那一刻鼠标的位置
	private Animator animator;
	public static PlayerController instance;
	CharacterController characterController;
	Position standPosition;//当前的位置
	Position fromPosition;//从哪个位置来的
	public Text Text_Magnet;
	public Text Text_Shoe;
	public Text Text_Star;
	public Text Text_Multiply;

	//TODO：技能时间这部分应该分离出去
	public float quickMoveTimeLeft;
	public float magnetTimeLeft;
	public float multiplyTimeLeft;
	public float shoeTimeLeft;
	/// <summary>
	/// 超过这个值，就可以增加一次速率
	/// </summary>
	private float speedAddDistance = 300;
	/// <summary>
	/// 下一次增加的速率
	/// </summary>
	private float speedAddRate = 0.5f;
	/// <summary>
	/// 累加计时器，当经过一定时间，增加速度
	/// </summary>
	private float speedAddTimer = 0;
	/// <summary>
	/// 玩家沿着x轴移动的方向
	/// </summary>
	Vector3 xDirection;
	/// <summary>
	/// 不知何用
	/// </summary>
	Vector3 moveDirection;
	public bool CanDoubleJump { get; private set; } = true;
	bool doubleJump = false;
	float jumpValue = 7;//跳跃的高度

	float gravity = 20;//重力
	void Start () {
        instance = this;
        Speed = Init_speed;//初始化速度
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        standPosition = Position.Middle;//初始位置为中间
        StartCoroutine(UpdateAction());
	}
	/// <summary>
	/// 接收输入
	/// </summary>
	void GetInputDirection()
	{
		inputDirection = InputDirection.NULL;
		if (Input.GetMouseButtonDown(0))//按下那一刻
		{
			activeInput = true;
			activePoint_mousePos = Input.mousePosition;//记录那一刻的鼠标位置
		}
		if (Input.GetMouseButton(0) && activeInput)//一直按住
		{
			Vector3 vec = Input.mousePosition - activePoint_mousePos;//鼠标当前位置减去记录的位置，得到一个向量
			if (vec.magnitude > 20)//如果是有效的滑动
			{
				//计算 vec 向量与 Vector2.up（即向上方向）之间的夹角 angleY：
				//Mathf.Acos 返回点积的反余弦值，得到 vec 与 Vector2.up 的夹角（弧度制）。
				//* Mathf.Rad2Deg 将弧度转换为角度。
				var angleY = Mathf.Acos(Vector3.Dot(vec.normalized, Vector2.up)) * Mathf.Rad2Deg;
				var anglex = Mathf.Acos(Vector3.Dot(vec.normalized, Vector2.right)) * Mathf.Rad2Deg;
				if (angleY <= 45)
				{
					inputDirection = InputDirection.Up;
					//AudioManager.instance.PlaySlideAudio();
				}
				else if (angleY >= 135)
				{
					inputDirection = InputDirection.Down;
					//AudioManager.instance.PlaySlideAudio();
				}
				else if (anglex <= 45)
				{
					inputDirection = InputDirection.Right;
					//AudioManager.instance.PlaySlideAudio();
				}
				else if (anglex >= 135)
				{
					inputDirection = InputDirection.Left;
					//AudioManager.instance.PlaySlideAudio();
				}
				activeInput = false;
				//Debug.Log(inputDirection);
			}
		}
	}
	void MoveLeft()
	{
		if (standPosition != Position.Left)
		{
			GetComponent<Animation>().Stop();
			//TODO ：播放左跳

			xDirection = Vector3.left;

			if (standPosition == Position.Middle)
			{
				standPosition = Position.Left;//当前方向切换到Left
				fromPosition = Position.Middle;//更新from
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
			GetComponent<Animation>().Stop();
			//TODO ：播放右跳

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
	/// <summary>
	/// 根据枚举值，来进行左移动，右移动，当移动到目标点，即可停下
	/// </summary>
	void MoveLeftRight()
	{
		if (inputDirection == InputDirection.Left)
		{
			MoveLeft();
		}
		else if (inputDirection == InputDirection.Right)
		{
			MoveRight();
		}
		//下面的if判断是否到达当前位置
		if (standPosition == Position.Left)
		{
			if (transform.position.x <= -1.7f)
			{
				xDirection = Vector3.zero;//停下
				transform.position = new Vector3(-1.7f, transform.position.y, transform.position.z);
			}
		}
		if (standPosition == Position.Middle)
		{
			if (fromPosition == Position.Left)//如果是从左边来的
			{
				if (transform.position.x > 0)//因为我们不希望x>0,因为从左边来的
				{
					xDirection = Vector3.zero;
					transform.position = new Vector3(0, transform.position.y, transform.position.z);
				}
			}
			else if (fromPosition == Position.Right)//如果是从右边来的
			{
				if (transform.position.x < 0)//因为我们不希望x<0,因为从右边来的
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
	/// <summary>
	/// 不断往前移动
	/// </summary>
	void MoveForward()
	{
		if (inputDirection == InputDirection.Down)
		{
			//TODO：播放翻滚
		}
		if (characterController.isGrounded)
		{
			moveDirection = Vector3.zero;//TODO?

			if (true)//TODO：只要不处于左跳，右跳，翻滚状态
			{
				//TODO：播放RUN
			}
			if (inputDirection == InputDirection.Up)
			{
				//TODO ：JumpUp();
				if (CanDoubleJump)
					doubleJump = true;
			}
		}
		else//处理在空中的逻辑
		{
			if (inputDirection == InputDirection.Down)
			{
				QuickGround();
			}
			if (inputDirection == InputDirection.Up)
			{
				if (doubleJump)
				{
					JumpDouble();
					doubleJump = false;
				}
			}

			if (true)//TODO ：不是向上，不是翻滚，不是二段
			{
				//TODO ：播放跳跃循环
			}
		}

		moveDirection.z = Speed;//为什么要不断赋值？不浪费性能吗？TODO
		moveDirection.y -= gravity * Time.deltaTime;//在地面也要一直赋值？不浪费性能吗？TODO
		characterController.Move((xDirection * 10 + moveDirection) * Time.deltaTime);
	}
	void JumpDouble()
	{
		//TOOD ：播放二段跳
		moveDirection.y += jumpValue * 1.3f;
	}
	void QuickGround()
	{
		moveDirection.y -= jumpValue * 3;
	}
	/// <summary>
	/// 设置速度，但不超过最大速度
	/// </summary>
	/// <param name="newSpeed"></param>
	private void SetSpeed(float newSpeed)
	{
		if (newSpeed <= maxSpeed)
			Speed = newSpeed;
		else
			Speed = maxSpeed;
	}
	private void UpdateSpeed()
	{
		speedAddTimer += Speed * Time.deltaTime;//这里为什么乘Speed？
		if (speedAddTimer >= speedAddDistance)//speedAddTimer积累到一定程度（speedAddDistance）就设置速度
		{
			SetSpeed(Speed + speedAddRate);
			speedAddTimer = 0;
		}
	}
	IEnumerator UpdateAction()
	{
		while (GameAttribute.instance.Life > 0)
		{
			if (GameController.instance.IsPlay && !GameController.instance.IsPause)
			{
				GetInputDirection();
				MoveLeftRight();//控制水平移动
				MoveForward();//不断往前移动
				UpdateSpeed();
			}
			else
			{
				//TODO：调用停止动画
			}
			yield return 0;
		}
		//Debug.Log("game over");
		Speed = 0;
		GameController.instance.IsPlay = false;
		//TODO：这是啥意思？xDirection = Vector3.zero;
		//TODO：调用死亡动画
		//TODO：用dotween插件实现某个缓动效果
		yield return new WaitForSeconds(3);//等待3s弹出UI面板
		Debug.Log("restart");
		
		//UIController.instance.ShowRestartUI();
		//UIController.instance.HidePauseUI();
	}

	private void Update()
	{
		UpdateItemTime();
	}
	/// <summary>
	/// 更新UI界面上的技能CD显示，这里应该分离出一个类解耦
	/// </summary>
	private void UpdateItemTime()
	{
		Text_Magnet.text = GetTime(magnetTimeLeft);
		Text_Multiply.text = GetTime(multiplyTimeLeft);
		Text_Shoe.text = GetTime(shoeTimeLeft);
		Text_Star.text = GetTime(quickMoveTimeLeft);
	}

	private string GetTime(float time)
	{
		if (time <= 0)
			return "";
		return ((int)time + 1).ToString() + "s";//不断的字符串拼接是否消耗性能？
	}
}
