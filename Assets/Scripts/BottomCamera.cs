using UnityEngine;
using System.Collections;

/// <summary>
/// メインカメラの真下にカメラの映像を出力するための MonoBehaviour.
/// </summary>
/// <remarks>
/// カメラの映像はこのスクリプトを追加した GameObject に投影される.
/// Material には Texture が設定できるようにしておく必要がある.
/// </remarks>
public class BottomCamera : MonoBehaviour
{
	[SerializeField] Transform mainCameraTransform;

	private const int WIDTH = 1280;
	private const int HEIGHT = 720;
	private const int FPS = 15;

	private WebCamTexture m_WebcamTexture;
	private Vector3 m_TempPosition = Vector3.zero;

	/// <summary>
	/// 常に GameObject をメインカメラ前に配置し、
	/// その向きがカメラ前方を向くようにする
	/// </summary>
	void Update()
	{
		m_TempPosition.Set(mainCameraTransform.forward.x * 5, -5.5f, mainCameraTransform.forward.z * 5);
		transform.position = mainCameraTransform.position + m_TempPosition;
		Vector3 cameraForward = -mainCameraTransform.forward;
		cameraForward.y = 0.0f;
		transform.rotation = Quaternion.LookRotation (cameraForward);
	}

	/// <summary>
	/// カメラとして扱えるデバイスを探し出し、
	/// その映像を投影する自身の Material に投影する Method.
	/// </summary>
	public void StartWebCam()
	{
		WebCamDevice[] devices = WebCamTexture.devices;
		m_WebcamTexture = new WebCamTexture(devices[0].name, WIDTH, HEIGHT, FPS);
		GetComponent<Renderer>().material.mainTexture = m_WebcamTexture;
		m_WebcamTexture.Play();
	}

	public void DestroyWebCam()
	{
		m_WebcamTexture.Stop();
	}
}