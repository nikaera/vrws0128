using UnityEngine;
using System.Collections;

/// <summary>
/// 法線を反転させた球を生成する MonoBehaviour.
/// </summary>
/// <remarks>
/// ヒエラルキーの Division_x や Division_y などのパラメータを変更することで、
/// メッシュの分割数や球のスケールを動的に変化させることができます.
/// </remarks>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SphereMeshInvert : MonoBehaviour {
	public int division_x = 12, division_y = 6;
	public float scale = 1.0f;

	private Mesh mesh;
	private Vector3[] vertices;
	private Vector3[] normals;
	private Vector2[] uvs;
	private int[] triangles;
	private int beforeNum;

	// Use this for initialization
	void Start () {
		beforeNum = division_x;
	}

	// Update is called once per frame
	void Update () {
		if(beforeNum != division_x) {
			beforeNum = division_x;
			CreateMesh();
		}
	}

	void Reset() {
		CreateMesh ();
	}

	void CreateMesh () {
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Sphere Screen";

		vertices = new Vector3[ (division_x + 1) * (division_y + 1) + 1 ];
		normals = new Vector3[ (division_x + 1) * (division_y + 1) + 1 ];
		uvs = new Vector2[ (division_x + 1) * (division_y + 1) + 1 ];
		int index = 0;
		for (int y = 0; y<division_y+1; y++) {
			float dy = (float)y / (float)division_y, dy_rad = dy * Mathf.PI;
			float v3y = Mathf.Cos (dy_rad), y_scale = Mathf.Sin (dy_rad);

			for (int x = 0; x<division_x+1; x++) {
				float dx = (float)x / (float)division_x, dx_rad = dx * 2.0f * Mathf.PI;

				vertices[index].Set(Mathf.Sin (dx_rad) * scale * y_scale, v3y * scale, Mathf.Cos (dx_rad) * scale * y_scale);
				normals[index] = vertices[index].normalized * -1.0f;
				uvs[index].Set( dx, 1.0f - dy );

				index++;
			}
		}

		index = 0;
		triangles = new int[ (division_x + 1) * division_y * 2 * 3];
		int[] vbuffer = new int[3];

		for (int y = 0; y<division_y; y++){
			vbuffer[1] = (y + 0) * (division_x+1);
			vbuffer[2] = (y + 1) * (division_x+1);
			for (int x = 2; x<(division_x + 2) * 2; x++) {
				vbuffer[0] = vbuffer[1];
				vbuffer[1] = vbuffer[2];
				vbuffer[2] = x / 2 + (y + (x % 2)) * (division_x+1);

				if( (y==0 && x%2>0) || (y>0 && y<division_y-1) || (y==division_y-1 && x%2==0) ){
					triangles[index * 3 + 0] = vbuffer[1 - x % 2];
					triangles[index * 3 + 1] = vbuffer[    x % 2];
					triangles[index * 3 + 2] = vbuffer[        2];

					index++;
				}
			}
		}

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uvs;
	}
}