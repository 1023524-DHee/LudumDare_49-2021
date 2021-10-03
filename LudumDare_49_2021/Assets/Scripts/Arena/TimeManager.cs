using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	private void SetTimeScale(float scaleAmount)
	{
		Time.timeScale = scaleAmount;
	}
}
