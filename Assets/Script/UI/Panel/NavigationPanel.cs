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
		float duration = 1.0f;//动画总时长
		Vector3 startPosition = transform.localPosition;
		Vector3 endPosition = startPosition + new Vector3(0, -200, 0);

		float elapsedTime = 0;//动画经过的时间

		while (elapsedTime < duration)
		{
			//用于在两个向量（这里是位置）之间进行平滑过渡。
			//elapsedTime / duration 是插值的进度值，表示动画的完成度（从 0 到 1）。
			transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.localPosition = endPosition;

		base.ClosePanel();
	}
}
