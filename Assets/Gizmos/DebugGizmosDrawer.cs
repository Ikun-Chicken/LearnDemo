using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugGizmosDrawer
{
	public static void DrawGroundCheckLine(Vector3 start, Vector3 direction, float distance)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(start, start + direction * distance);
	}

	public static void DrawGroundCheckSphere(Vector3 center, float radius)
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(center, radius);
	}

	public static void DrawRay(Vector3 start, Vector3 direction)
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(start, direction);
	}
}
