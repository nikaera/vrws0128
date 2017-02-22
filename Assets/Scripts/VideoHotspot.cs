using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// クリックされたときにその場所の情報を
/// 動画で表示するための MonoBehaviour.
/// </summary>
public class VideoHotspot : MonoBehaviour 
{
	[SerializeField] Transform m_PopupTransform;
	#if !(UNITY_IOS || UNITY_ANDROID)
	private MovieTexture m_MovieTexture;
	#endif
	private AudioSource m_AudioSource;
	private Coroutine m_CurrentCoroutine;
	private bool m_IsVisible = false;

	void Start () 
	{
		Renderer renderer = m_PopupTransform.GetComponent<Renderer>();
		#if !(UNITY_IOS || UNITY_ANDROID)
		m_MovieTexture = (MovieTexture) renderer.material.mainTexture;
		#endif
		m_AudioSource = m_PopupTransform.GetComponent<AudioSource>();
	}

	void OnEnable() 
	{
		m_IsVisible = false;
	}

	void OnDisable() 
	{
		StopMovie ();
		m_PopupTransform.gameObject.SetActive (false);
	}

	/// <summary>
	/// 現在の表示状況に応じて表示/非表示を切り替える Method.
	/// またその際に表示状況に応じて動画の再生/停止を切り替える.
	/// </summary>
	public void ChangeVisiblePopup() 
	{
		m_IsVisible = !m_IsVisible;
		m_PopupTransform.gameObject.SetActive(m_IsVisible);

		if (m_IsVisible)
			PlayMovie ();
		else 
			StopMovie ();
	}

	/// <summary>
	/// 動画の再生が終了したことを検知するために利用する Coroutine.
	/// </summary>
	private IEnumerator MovieFinished() 
	{
		#if !(UNITY_IOS || UNITY_ANDROID)
		while (m_MovieTexture.isPlaying) 
		{
			yield return null;
		}
		m_PopupTransform.gameObject.SetActive (false);
		#else
		yield return null;
		#endif
	}

	/// <summary>
	/// 動画を再生する Method.
	/// </summary>
	/// <remarks>
	/// 動画を再生する際は Audio も同時に再生していることに注意.
	/// また動画の終了を検知するための Coroutine がスタートする.
	/// </remarks>
	private void PlayMovie() 
	{
		#if !(UNITY_IOS || UNITY_ANDROID)
		m_AudioSource.clip = m_MovieTexture.audioClip;
		m_MovieTexture.Play ();
		m_AudioSource.Play ();
		m_CurrentCoroutine = StartCoroutine (MovieFinished ());
		#endif
	}

	/// <summary>
	/// 動画を停止する Method.
	/// </summary>
	/// <remarks>
	/// 動画を停止する際は Audio も同時に停止させていることに注意.
	/// また動画の終了を検知するための Coroutine が動いていれば停止する.
	/// </remarks>
	private void StopMovie() 
	{
		#if !(UNITY_IOS || UNITY_ANDROID)
		if(m_CurrentCoroutine != null) 
			StopCoroutine (m_CurrentCoroutine);
		m_MovieTexture.Stop ();
		m_AudioSource.Stop ();
		#endif
	}
}