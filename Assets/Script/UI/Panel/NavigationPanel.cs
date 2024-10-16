using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationPanel : BasePanel
{
	public void MoveOutOfScreen()
	{
		StartCoroutine(MovePanelOutOfScreenAndDestroy());
	}
	private IEnumerator MovePanelOutOfScreenAndDestroy()
	{
		float duration = 1.0f;//������ʱ��
		Vector3 startPosition = transform.localPosition;
		Vector3 endPosition = startPosition + new Vector3(0, -200, 0);

		float elapsedTime = 0;//����������ʱ��

		while (elapsedTime < duration)
		{
			//����������������������λ�ã�֮�����ƽ�����ɡ�
			//elapsedTime / duration �ǲ�ֵ�Ľ���ֵ����ʾ��������ɶȣ��� 0 �� 1����
			transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.localPosition = endPosition;

		base.ClosePanel();
	}
}
