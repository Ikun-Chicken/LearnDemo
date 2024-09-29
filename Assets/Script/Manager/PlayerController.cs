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

	bool activeInput;//�Ƿ񼤻����룬����갴����һ�̾��Ǽ���
	Vector3 activePoint_mousePos;//������¼������һ������λ��
	private Animator animator;
	public static PlayerController instance;
	CharacterController characterController;
	Position standPosition;//��ǰ��λ��
	Position fromPosition;//���ĸ�λ������
	public Text Text_Magnet;
	public Text Text_Shoe;
	public Text Text_Star;
	public Text Text_Multiply;

	//TODO������ʱ���ⲿ��Ӧ�÷����ȥ
	public float quickMoveTimeLeft;
	public float magnetTimeLeft;
	public float multiplyTimeLeft;
	public float shoeTimeLeft;
	/// <summary>
	/// �������ֵ���Ϳ�������һ������
	/// </summary>
	private float speedAddDistance = 300;
	/// <summary>
	/// ��һ�����ӵ�����
	/// </summary>
	private float speedAddRate = 0.5f;
	/// <summary>
	/// �ۼӼ�ʱ����������һ��ʱ�䣬�����ٶ�
	/// </summary>
	private float speedAddTimer = 0;
	/// <summary>
	/// �������x���ƶ��ķ���
	/// </summary>
	Vector3 xDirection;
	/// <summary>
	/// ��֪����
	/// </summary>
	Vector3 moveDirection;
	public bool CanDoubleJump { get; private set; } = true;
	bool doubleJump = false;
	float jumpValue = 7;//��Ծ�ĸ߶�

	float gravity = 20;//����
	void Start () {
        instance = this;
        Speed = Init_speed;//��ʼ���ٶ�
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        standPosition = Position.Middle;//��ʼλ��Ϊ�м�
        StartCoroutine(UpdateAction());
	}
	/// <summary>
	/// ��������
	/// </summary>
	void GetInputDirection()
	{
		inputDirection = InputDirection.NULL;
		if (Input.GetMouseButtonDown(0))//������һ��
		{
			activeInput = true;
			activePoint_mousePos = Input.mousePosition;//��¼��һ�̵����λ��
		}
		if (Input.GetMouseButton(0) && activeInput)//һֱ��ס
		{
			Vector3 vec = Input.mousePosition - activePoint_mousePos;//��굱ǰλ�ü�ȥ��¼��λ�ã��õ�һ������
			if (vec.magnitude > 20)//�������Ч�Ļ���
			{
				//���� vec ������ Vector2.up�������Ϸ���֮��ļн� angleY��
				//Mathf.Acos ���ص���ķ�����ֵ���õ� vec �� Vector2.up �ļнǣ������ƣ���
				//* Mathf.Rad2Deg ������ת��Ϊ�Ƕȡ�
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
			//TODO ����������

			xDirection = Vector3.left;

			if (standPosition == Position.Middle)
			{
				standPosition = Position.Left;//��ǰ�����л���Left
				fromPosition = Position.Middle;//����from
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
			//TODO ����������

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
	/// ����ö��ֵ�����������ƶ������ƶ������ƶ���Ŀ��㣬����ͣ��
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
		//�����if�ж��Ƿ񵽴ﵱǰλ��
		if (standPosition == Position.Left)
		{
			if (transform.position.x <= -1.7f)
			{
				xDirection = Vector3.zero;//ͣ��
				transform.position = new Vector3(-1.7f, transform.position.y, transform.position.z);
			}
		}
		if (standPosition == Position.Middle)
		{
			if (fromPosition == Position.Left)//����Ǵ��������
			{
				if (transform.position.x > 0)//��Ϊ���ǲ�ϣ��x>0,��Ϊ���������
				{
					xDirection = Vector3.zero;
					transform.position = new Vector3(0, transform.position.y, transform.position.z);
				}
			}
			else if (fromPosition == Position.Right)//����Ǵ��ұ�����
			{
				if (transform.position.x < 0)//��Ϊ���ǲ�ϣ��x<0,��Ϊ���ұ�����
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
	/// ������ǰ�ƶ�
	/// </summary>
	void MoveForward()
	{
		if (inputDirection == InputDirection.Down)
		{
			//TODO�����ŷ���
		}
		if (characterController.isGrounded)
		{
			moveDirection = Vector3.zero;//TODO?

			if (true)//TODO��ֻҪ����������������������״̬
			{
				//TODO������RUN
			}
			if (inputDirection == InputDirection.Up)
			{
				//TODO ��JumpUp();
				if (CanDoubleJump)
					doubleJump = true;
			}
		}
		else//�����ڿ��е��߼�
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

			if (true)//TODO ���������ϣ����Ƿ��������Ƕ���
			{
				//TODO ��������Ծѭ��
			}
		}

		moveDirection.z = Speed;//ΪʲôҪ���ϸ�ֵ�����˷�������TODO
		moveDirection.y -= gravity * Time.deltaTime;//�ڵ���ҲҪһֱ��ֵ�����˷�������TODO
		characterController.Move((xDirection * 10 + moveDirection) * Time.deltaTime);
	}
	void JumpDouble()
	{
		//TOOD �����Ŷ�����
		moveDirection.y += jumpValue * 1.3f;
	}
	void QuickGround()
	{
		moveDirection.y -= jumpValue * 3;
	}
	/// <summary>
	/// �����ٶȣ�������������ٶ�
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
		speedAddTimer += Speed * Time.deltaTime;//����Ϊʲô��Speed��
		if (speedAddTimer >= speedAddDistance)//speedAddTimer���۵�һ���̶ȣ�speedAddDistance���������ٶ�
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
				MoveLeftRight();//����ˮƽ�ƶ�
				MoveForward();//������ǰ�ƶ�
				UpdateSpeed();
			}
			else
			{
				//TODO������ֹͣ����
			}
			yield return 0;
		}
		//Debug.Log("game over");
		Speed = 0;
		GameController.instance.IsPlay = false;
		//TODO������ɶ��˼��xDirection = Vector3.zero;
		//TODO��������������
		//TODO����dotween���ʵ��ĳ������Ч��
		yield return new WaitForSeconds(3);//�ȴ�3s����UI���
		Debug.Log("restart");
		
		//UIController.instance.ShowRestartUI();
		//UIController.instance.HidePauseUI();
	}

	private void Update()
	{
		UpdateItemTime();
	}
	/// <summary>
	/// ����UI�����ϵļ���CD��ʾ������Ӧ�÷����һ�������
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
		return ((int)time + 1).ToString() + "s";//���ϵ��ַ���ƴ���Ƿ��������ܣ�
	}
}
