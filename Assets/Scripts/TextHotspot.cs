using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クリックされたときにその場所の注釈を
/// Text で表示するための MonoBehaviour.
/// </summary>
public class TextHotspot : MonoBehaviour 
{
	/// <summary>
	/// 表示/非表示を切り替える Text の Transform.
	/// </summary>
	[SerializeField] Transform m_PopupTransform;
	private bool m_IsVisible = false;

	void OnEnable() 
	{
		m_IsVisible = false;
	}

	void OnDisable() 
	{
		if(m_PopupTransform)
			m_PopupTransform.gameObject.SetActive (false);
	}

	/// <summary>
	/// 現在の表示状況に応じて表示/非表示を切り替える Method.
	/// </summary>
	public void ChangeVisiblePopup() 
	{
		m_IsVisible = !m_IsVisible;
		m_PopupTransform.gameObject.SetActive(m_IsVisible);
	}
}