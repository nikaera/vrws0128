using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Main シーン内のユーザによるアクションに応じて処理を実行する MonoBehaviour.
/// </summary>
public class MainSceneController : MonoBehaviour 
{
	private const float WEB_CAM_X_ANGLE = 125f;
	private const float MENU_Z_POSITION = 20f;
	private const float MENU_AUTO_HIDDEN_INTERVAL = 3.0f; // sec

	/// <summary>
	/// シーン内に配置したホットスポットの Transform の配列
	/// </summary>
	[SerializeField] Transform[] m_HotspotTransforms;

	/// <summary>
	/// メインカメラの Transform.
	/// </summary>
	/// <remarks>
	/// カメラの目線に Ray を飛ばしたり、
	/// どの方向を向いているか判別する際に使用する
	/// </remarks>
	[SerializeField] Transform m_CameraTransform;

	/// <summary>
	/// カメラ中央に表示するマーカーの Transform.
	/// ユーザの視線の中心を示すために用いる.(目線クリック時に活用)
	/// </summary>
	[SerializeField] Transform m_CameraCenterTransform;

	/// <summary>
	/// ホットスポットの表示/非表示を切り替えるメニュー の Canvas.
	/// </summary>
	[SerializeField] Canvas m_MenuCanvas;

	/// <summary>
	/// 真下の方向にカメラの映像を投影するために利用するスクリプト.
	/// </summary>
	/// <remakrs>
	/// 詳細は Scripts/BottomCamera.cs 参照
	/// </remarks>
	[SerializeField] BottomCamera m_BottomCamera;

	private Coroutine m_CurrentCoroutine;
	private Transform m_CachedRaycastHitTransform;
	private Vector3 m_CachedCameraCenterScale;

	void Reset () 
	{
		m_CameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").transform;
	}

	void Start()
	{
		DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
		m_CachedCameraCenterScale = m_CameraCenterTransform.localScale;
		m_BottomCamera.StartWebCam ();
	}

	void OnDestroy()
	{
		m_BottomCamera.DestroyWebCam ();
	}

	// Update is called once per frame
	void Update () 
	{
		#if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			RaycastHit hit;
			Ray ray = new Ray (m_CameraTransform.position, m_CameraTransform.rotation * Vector3.forward);
			if (Physics.Raycast (ray, out hit)) 
			{
				OnHitRaycast (hit.transform);
			} 
			else 
			{
				bool isActive = m_MenuCanvas.gameObject.activeSelf;
				m_MenuCanvas.gameObject.SetActive (!isActive);

				if(m_MenuCanvas.gameObject.activeSelf) 
					UpdateMenuCanvasPosition ();
			}
			if (m_MenuCanvas.gameObject.activeSelf) 
			{
				if(m_CurrentCoroutine != null) 
					StopCoroutine (m_CurrentCoroutine);
				
				m_CurrentCoroutine = StartCoroutine (HiddenMenuCanvas ());
			}
		}
		#elif UNITY_IOS || UNITY_ANDROID
		RaycastHit hit;
		Ray ray = new Ray (m_CameraTransform.position, m_CameraTransform.rotation * Vector3.forward);
		if (Physics.Raycast (ray, out hit)) 
		{
			string raycastHitName = hit.transform.name;
			if(m_CachedRaycastHitTransform == null || raycastHitName != m_CachedRaycastHitTransform.name) 
			{
				m_CachedRaycastHitTransform = hit.transform;
				Vector3 scaleVector = new Vector3(1, 1, 1);
				m_CameraCenterTransform.DOScale(scaleVector, 1).SetId("gazeClicked").OnComplete(() => 
				{
					OnHitRaycast (m_CachedRaycastHitTransform);
					m_CameraCenterTransform.localScale = m_CachedCameraCenterScale;
				});
			}
		} 
		else 
		{
			DOTween.Kill("gazeClicked");
			m_CameraCenterTransform.localScale = m_CachedCameraCenterScale;
			m_CachedRaycastHitTransform = null;
		}
		#endif
		float cameraXAngle = Vector3.Angle (m_CameraTransform.forward, Vector3.up); 

		if (cameraXAngle > WEB_CAM_X_ANGLE)
			m_BottomCamera.gameObject.SetActive (true);
		else
			m_BottomCamera.gameObject.SetActive (false);
	}

	public void OnClickShowButton() 
	{
		foreach (Transform hotspotTransform in m_HotspotTransforms) 
		{
			hotspotTransform.gameObject.SetActive (true);
		}
	}

	public void OnClickHiddenButton() 
	{
		foreach (Transform hotspotTransform in m_HotspotTransforms) 
		{
			hotspotTransform.gameObject.SetActive (false);
		}
	}

	public void OnClickChangeSceneButton()
	{
		GameManager.Instance.LoadScene(SceneName.Stereo);
	}

	private IEnumerator HiddenMenuCanvas() 
	{
		yield return new WaitForSeconds (MENU_AUTO_HIDDEN_INTERVAL);
		m_MenuCanvas.gameObject.SetActive (false);
	}

	private void OnHitRaycast(Transform hitRaycastTransform) 
	{
		TextHotspot textHotspot = hitRaycastTransform.GetComponent<TextHotspot> ();
		if (textHotspot != null) 
			textHotspot.ChangeVisiblePopup ();

		SceneHotspot sceneHotspot = hitRaycastTransform.GetComponent<SceneHotspot> ();
		if (sceneHotspot != null)
			sceneHotspot.PresentScene ();
		
		Button button = hitRaycastTransform.GetComponent<Button> ();
		if (button != null) 
			button.onClick.Invoke();
	}

	private void UpdateMenuCanvasPosition() 
	{
		Vector3 menuCanvasForwardPosition = m_CameraTransform.forward * MENU_Z_POSITION;
		m_MenuCanvas.transform.position = m_CameraTransform.position + menuCanvasForwardPosition;
		m_MenuCanvas.transform.LookAt (2 * m_MenuCanvas.transform.position - m_CameraTransform.position);
	}
}
