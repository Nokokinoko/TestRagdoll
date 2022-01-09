using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class ShakeSquare : MonoBehaviour
{
	[SerializeField] private RectTransform m_TransformCanvas;
	[SerializeField] private Vector2 m_LB;
	[SerializeField] private Vector2 m_RB;
	[SerializeField] private Vector2 m_LT;
	[SerializeField] private Vector2 m_RT;
	
	private MeshFilter m_Filter;

	private readonly List<Vector3> m_ListVertex = new List<Vector3>();
	private readonly List<Vector2> m_ListUV = new List<Vector2>();
	private readonly int[] m_Indices = new[] { 0, 2, 1, 1, 2, 3 };

	private void Awake()
	{
		m_Filter = GetComponent<MeshFilter>();
	}

	private void Start()
	{
		Vector2 _Calc = m_TransformCanvas.sizeDelta * -0.5f;
		m_ListVertex.Add(m_LB + _Calc);
		m_ListVertex.Add(m_RB + _Calc);
		m_ListVertex.Add(m_LT + _Calc);
		m_ListVertex.Add(m_RT + _Calc);
		
		m_ListUV.Add(Vector2.zero);
		m_ListUV.Add(Vector2.right);
		m_ListUV.Add(Vector2.up);
		m_ListUV.Add(Vector2.one);

		Mesh _Mesh = new Mesh();
		_Mesh.SetVertices(m_ListVertex);
		_Mesh.SetUVs(0, m_ListUV);
		_Mesh.SetIndices(m_Indices, MeshTopology.Triangles, 0);

		m_Filter.mesh = _Mesh;
	}

	private void OnDrawGizmosSelected()
	{
		Matrix4x4 _Matrix = m_TransformCanvas.localToWorldMatrix;
		_Matrix *= Matrix4x4.Translate(m_TransformCanvas.sizeDelta * -0.5f);
		Gizmos.matrix = _Matrix;
		
		Gizmos.color = Color.green;
		
		Gizmos.DrawSphere(m_LB, 50.0f);
		Gizmos.DrawSphere(m_RB, 50.0f);
		Gizmos.DrawSphere(m_LT, 50.0f);
		Gizmos.DrawSphere(m_RT, 50.0f);
	}
}
