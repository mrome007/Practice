using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock : MonoBehaviour 
{
	#region Inspector fields

	/// <summary>
	/// The hours transform.
	/// </summary>
	[SerializeField]
	private Transform hoursTransform;

	/// <summary>
	/// The minutes transform.
	/// </summary>
	[SerializeField]
	private Transform minutesTransform;

	/// <summary>
	/// The seconds transform.
	/// </summary>
	[SerializeField]
	private Transform secondsTransform;

	/// <summary>
	/// The continuous.
	/// </summary>
	[SerializeField]
	private bool continuous = false;

	#endregion

	#region Private fields

	/// <summary>
	/// The degree per hour. won't change
	/// </summary>
	private const float degreePerHour = 30f;

	/// <summary>
	/// The degrees per minute. Minutes and seconds take up six degrees.
	/// </summary>
	private const float degreesPerMinute = 6f;

	///  <summary>
	/// The degrees per second.
	/// </summary>
	private const float degreesPerSecond = 6f;

	#endregion

	#region Unity methods

	private void Awake()
	{
		var time = DateTime.Now;
		//Quaternions are based on complex numbers and are used to represent 3D rotations. While harder
		//to understand than simple 3D vectors, they have some useful characteristics.

		//Our clock has twelve hour indicators set to 30 degree intervals. To make the rotation match that,
		//we have to multiply the hours by 30.
		hoursTransform.localRotation = Quaternion.Euler(0, time.Hour * degreePerHour, 0);
		minutesTransform.localRotation = Quaternion.Euler(0, time.Minute * degreesPerMinute, 0);
		secondsTransform.localRotation = Quaternion.Euler(0, time.Second * degreesPerSecond, 0);
	}

	private void Update()
	{
		if(continuous)
		{
			UpdateContinuous();
		}
		else
		{
			UpdateDiscrete();
		}
	}

	#endregion

	#region Helper functions

	private void UpdateContinuous()
	{
		//Displays fractional hours, minutes, and seconds.
		//DateTime has TimeOfDay property, this gives us a TimeSpan value that contains the data in the format
		//we need.
		var time = DateTime.Now.TimeOfDay;

		hoursTransform.localRotation = Quaternion.Euler(0, (float)time.TotalHours * degreePerHour, 0);
		minutesTransform.localRotation = Quaternion.Euler(0, (float)time.TotalMinutes * degreesPerMinute, 0);
		secondsTransform.localRotation = Quaternion.Euler(0, (float)time.TotalSeconds * degreesPerSecond, 0);
	}

	private void UpdateDiscrete()
	{
		var time = DateTime.Now;

		hoursTransform.localRotation = Quaternion.Euler(0, time.Hour * degreePerHour, 0);
		minutesTransform.localRotation = Quaternion.Euler(0, time.Minute * degreesPerMinute, 0);
		secondsTransform.localRotation = Quaternion.Euler(0, time.Second * degreesPerSecond, 0);
	}

	#endregion
}
