using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	public GameObject target;
	public float height;
	public float distance;
	Vector3 pos;
	bool isShaking = false;
	public static CameraManager instance;
	void Start()
	{
		instance = this;
		pos = transform.position;
	}

	public void CameraShake()
	{
		isShaking = true;
		MyTimerManager.Instance.AddFiniteTimer(0.5f,ShakeScreen);
	}

	void ShakeScreen()
	{
		transform.position = new Vector3(
		target.transform.position.x + Random.Range(-0.1f, 0.1f),
		target.transform.position.y + height,
		target.transform.position.z - distance);
		isShaking=false;
	}

	void LateUpdate()
	{
		if (!isShaking && GameController.instance.IsPlay && !GameController.instance.IsPause)
		{
			pos.x = target.transform.position.x;//��������ƶ�
			//ʹ����� y �����𽥽ӽ�Ŀ������ y �������һ���߶�ƫ�ƣ�height����
			pos.y = Mathf.Lerp(pos.y, target.transform.position.y + height, Time.deltaTime * 5);
			pos.z = target.transform.position.z - distance;
			transform.position = pos;
		}
	}
}
