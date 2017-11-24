using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {

	public int xSize, ySize, zSize;

	private Mesh mesh;
	private Vector3[] vertices;
	private MeshCollider meshCollider;
	private bool hasCollider = false;

	public Vector3[] GetVertices() {
		return vertices;
	}

	public void UpdateVertices(Vector3[] v) {
		vertices = v;
		mesh.vertices = vertices;
		mesh.RecalculateNormals ();

		if (hasCollider) {
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
	}

	private void Awake () {
		meshCollider = GetComponent<MeshCollider> ();
		if (meshCollider != null) {
			hasCollider = true;
		}
		Generate();
	}

	public Vector3 GetCenter() {
		return new Vector3 (
			transform.position.x + xSize * transform.localScale.x * 0.5f,
			transform.position.y,
			transform.position.z + ySize * transform.localScale.y * 0.5f
		);
	}

	private void Generate () {
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Procedural Grid";
//		transform.position += Vector3.up * zSize;

		vertices = new Vector3[(xSize + 1) * (ySize + 1)];
		Vector2[] uv = new Vector2[vertices.Length];
		Vector4[] tangents = new Vector4[vertices.Length];
		Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
		for (int i = 0, y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++, i++) {
				vertices[i] = new Vector3(x, y, Random.value * zSize);
				uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
				tangents[i] = tangent;
			}
		}
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.tangents = tangents;

		int[] triangles = new int[xSize * ySize * 6];
		for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
			for (int x = 0; x < xSize; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
				triangles[ti + 5] = vi + xSize + 2;
			}
		}
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}
}