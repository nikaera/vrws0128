using UnityEngine;
using System.Collections;

/// <summary>
/// このジェネリッククラスを継承するとシングルトンになります。
/// Managerクラスのようなインスタンスを一つしか存在させない場合に使います。
/// </summary>
/// <remarks>
/// 参考URL : 
/// http://naichilab.blogspot.jp/2013/11/unitymanager.html
/// </remarks>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	/// <summary>
	/// インスタンス変数 (Singleton)
	/// </summary>
	private static T instance;

	/// <summary>
	/// DontDestroyフラグ
	/// </summary>
	/// <remarks>
	/// スクリプトをアタッチしたGameObjectをDontDestroy状態にするかのフラグです。
	/// </remarks>
	[SerializeField]
	bool dontDestroyOnLoad;

	/// <summary>
	/// インスタンスがあるか
	/// </summary>
	/// <remarks>
	/// 後に生成されたSingletonオブジェクトの方を消すため、
	/// 最初のオブジェクトはtrueフラグにしてDestroyしないようにします。
	/// </remarks>
	protected bool isInstanced = false;

	/// <summary>
	/// インスタンス変数を返します。
	/// </summary>
	/// <remarks>
	/// 初回は検索します。
	/// </remarks>
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));

				if (instance == null)
					Debug.Log(typeof(T) + "is noting");
			}
			return instance;
		}
	}

	/// <summary>
	/// Awakeの仮想関数
	/// 継承先でオーバーライドする時はbase.Awake()します。
	/// </summary>
	/// <remarks>
	/// override void Awake()
	/// {
	/// 	base.Awake();
	/// 	処理
	/// }
	/// </remarks>
	protected virtual void Awake()
	{
		CheckInstance();
	}

	/// <summary>
	/// インスタンスされているか確認します。
	/// DontDestroyフラグを付けるとここで処理されます。
	/// </summary>
	/// <returns>bool値(成功→true,失敗→false)</returns>
	/// <remarks>
	/// (this).CheckInstance();
	/// </remarks>
	protected virtual bool CheckInstance()
	{
		if (this == Instance)
		{
			if (dontDestroyOnLoad) DontDestroyOnLoad(this.gameObject);
			return true;
		}

		if (isInstanced) 
			return true;

		Destroy(this.gameObject);
		isInstanced = true;

		return false;
	}

	/// <summary>
	/// Destroy時にリソースの解放を行います。
	/// </summary>
	void OnDestroy()
	{
		if(instance == this)
			instance = null;
	}
}