using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// トップダウン形式のパノラマ画像の上部分と下部分を
/// それぞれ球に貼り付けるための MonoBehaviour.
/// </summary>
public class StereoSphereMapping : MonoBehaviour 
{
	/// <summary>
	/// トップダウン形式のパノラマ画像の上部分を貼り付ける Transform.
	/// </summary>
	[SerializeField] Transform m_LeftSphereTransform;

	/// <summary>
	/// トップダウン形式のパノラマ画像の下部分を貼り付ける Transform.
	/// </summary>
	[SerializeField] Transform m_RightSphereTransform;

	// Use this for initialization
	void Start () 
	{
		var top_mesh = m_LeftSphereTransform.GetComponent<MeshFilter> ().mesh;
		var uvs = top_mesh.uv;
		for (int i = 0; i < uvs.Length; i++) 
		{
			Vector2 v = uvs [i];
			v.Set (v.x, (v.y * 0.5f + 0.5f));
			uvs [i] = v;
		}
		top_mesh.uv = uvs;

		Mesh down_mesh = m_RightSphereTransform.GetComponent<MeshFilter> ().mesh;
		Vector2[] down_uvs = down_mesh.uv;
		for (int i = 0; i < down_uvs.Length; i++) 
		{
			Vector2 v = down_uvs [i];
			v.Set (v.x, (v.y * 0.5f));
			down_uvs [i] = v;
		}
		down_mesh.uv = down_uvs;
	}
}