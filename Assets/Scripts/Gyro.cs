using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ジャイロセンサを用いて追加された GameObject を回転させる MonoBehaviour.
/// </summary>
/// <remarks>
/// iOS 及び Android でのビルド実行時のみ有効.
/// </remarks>
public class Gyro : MonoBehaviour {
	/// <summary>
	/// 方向キーでカメラを回転させる際の変化量.
	/// </summary>
	[SerializeField] float m_RotateSpeed;

	void Reset () 
	{
		m_RotateSpeed = 100;
	}

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		#elif UNITY_IPHONE || UNITY_ANDROID
			Input.gyro.enabled = true;
		#endif
	}
	
	// Update is called once per frame

	void Update () {
		#if UNITY_EDITOR
		RotateCamera();
		#elif UNITY_IPHONE || UNITY_ANDROID
		transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.right) * Input.gyro.attitude * Quaternion.AngleAxis(180.0f, Vector3.forward);
		#endif
	}

	void RotateCamera()
	{
		if (Input.GetKey (KeyCode.UpArrow))
			transform.Rotate(Vector3.left * Time.deltaTime * m_RotateSpeed);

		if (Input.GetKey (KeyCode.DownArrow))
			transform.Rotate(Vector3.left * -Time.deltaTime * m_RotateSpeed);

		if (Input.GetKey (KeyCode.LeftArrow))
			transform.Rotate(Vector3.up * -Time.deltaTime * m_RotateSpeed, Space.World);

		if (Input.GetKey (KeyCode.RightArrow))
			transform.Rotate(Vector3.up * Time.deltaTime * m_RotateSpeed, Space.World);
	}
}
