using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RectTransform コンポーネントが追加されている GameObject に
/// 適切にコライダーを設定するための MonoBehaviour.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class CreateRectCollider : MonoBehaviour 
{
	// Use this for initialization
	/// <summary>
	/// 自身の GameObject に対して、適切に BoxCollider を設定する.
	/// </summary>
	void Reset() 
	{
		BoxCollider boxCollider = null;
		if (GetComponent<BoxCollider> () == null)
			boxCollider = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
		else 
			boxCollider = GetComponent<BoxCollider> ();
		
		boxCollider.isTrigger = true;
		RectTransform rectTransform = GetComponent<RectTransform>();
		Rect box = rectTransform.rect;
		boxCollider.size = new Vector3(box.width, box.height, 1f);

		Vector2 pivot = rectTransform.pivot;
		float pivotX = 0.5f - pivot.x;
		float pivotY = 0.5f - pivot.y;
		boxCollider.center = new Vector3(box.center.x + pivotX, box.center.y + pivotY, 0);
	}
}