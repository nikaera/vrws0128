using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クリックされたときに他のシーンに遷移するための MonoBehaviour.
/// </summary>
public class SceneHotspot : MonoBehaviour 
{
	/// <summary>
	/// 遷移させたいシーン名
	/// </summary>
	[SerializeField] string m_SceneName;
	private bool m_IsVisible = false;

	/// <summary>
	/// 他のシーンに遷移するのに利用する Method.
	/// </summary>
	/// <remarks>
	/// GameManager のシングルトンの Method を利用して、
	/// Stereo シーンに遷移する. 詳細は Scripts/GameManager.cs 参照. 
	/// </remarks>
	public void PresentScene() 
	{
		GameManager.Instance.LoadScene (m_SceneName);
	}
}