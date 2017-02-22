using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// プロジェクト全体で利用される Singleton の MonoBehaviour.
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager> 
{
	/// <summary>
	/// 他のシーンをロードする Method.
	/// </summary>
	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}
}