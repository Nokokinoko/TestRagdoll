using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasRenderer))]
public class ShakeGraphic : Graphic
{
	[SerializeField] private RectTransform m_TransformCanvas;

	#region VERTEX SETTING
	[Serializable]
	public class VertexSetting
	{
		public bool m_IsShake;
		public Vector2 m_Position;
	}
	
	[SerializeField] private VertexSetting m_SettingLB;
	[SerializeField] private VertexSetting m_SettingRB;
	[SerializeField] private VertexSetting m_SettingLT;
	[SerializeField] private VertexSetting m_SettingRT;
	#endregion

	private VertexInfo m_LB;
	private VertexInfo m_RB;
	private VertexInfo m_LT;
	private VertexInfo m_RT;

	private bool m_Shake = false;
	public bool Shake { set { m_Shake = value; } }

	private const float DURATION = 0.2f;
	private float m_Interval = 0.0f;
	
	protected override void OnValidate()
	{
		m_LB = new VertexInfo(m_SettingLB);
		m_RB = new VertexInfo(m_SettingRB);
		m_LT = new VertexInfo(m_SettingLT);
		m_RT = new VertexInfo(m_SettingRT);

		base.OnValidate();
	}

	private void Update()
	{
		if (!m_Shake)
		{
			return;
		}

		m_Interval += Time.deltaTime;
		
		if (DURATION <= m_Interval)
		{
			m_LB.SetNextVertex();
			m_RB.SetNextVertex();
			m_LT.SetNextVertex();
			m_RT.SetNextVertex();
			
			m_Interval = 0.0f;
		}
		else
		{
			float _Ratio = m_Interval / DURATION;
			m_LB.CalcPosition(_Ratio);
			m_RB.CalcPosition(_Ratio);
			m_LT.CalcPosition(_Ratio);
			m_RT.CalcPosition(_Ratio);
			
			SetVerticesDirty();
		}
	}

	protected override void OnPopulateMesh(VertexHelper pHelper)
	{
		pHelper.Clear();
		
		pHelper.AddVert(m_LB.GetUIVertex());
		pHelper.AddVert(m_RB.GetUIVertex());
		pHelper.AddVert(m_LT.GetUIVertex());
		pHelper.AddVert(m_RT.GetUIVertex());
		
		pHelper.AddTriangle(0, 2, 1);
		pHelper.AddTriangle(1, 2, 3);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.matrix = m_TransformCanvas.localToWorldMatrix;
		
		Gizmos.color = Color.green;
		
		Gizmos.DrawSphere(m_SettingLB.m_Position, 50.0f);
		Gizmos.DrawSphere(m_SettingRB.m_Position, 50.0f);
		Gizmos.DrawSphere(m_SettingLT.m_Position, 50.0f);
		Gizmos.DrawSphere(m_SettingRT.m_Position, 50.0f);
	}
}
