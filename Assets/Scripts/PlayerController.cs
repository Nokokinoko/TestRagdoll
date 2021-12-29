using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(BoneRagdollable))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] private Animator m_Animator;
	[SerializeField] private Transform m_Center;
	[SerializeField] private Transform m_TransformRagdoll;

	private class CopyTransform
	{
		private readonly Transform From;
		private readonly Transform To;

		public CopyTransform(Transform pFrom, Transform pTo)
		{
			From = pFrom;
			To = pTo;
		}

		public bool Compare(Transform pTransform)
		{
			return pTransform == To;
		}
		
		public void CopyLocalPosition()
		{
			To.localPosition = From.localPosition;
		}

		public void CopyLocalRotation()
		{
			To.localRotation = From.localRotation;
		}
	}

	private readonly Dictionary<HumanBodyBones, CopyTransform> m_DictTransform =
		new Dictionary<HumanBodyBones, CopyTransform>();

	private BoneRagdollable m_BoneRagdollable;
	private readonly List<Rigidbody> m_ListRigidbody = new List<Rigidbody>();

	private Vector3 m_RotationDefault;
	private Vector2 m_RotationRatio = Vector2.zero;

	private const float ROTATE_CHEST_H = 90.0f;
	private const float ROTATE_CHEST_V = 90.0f;

	public bool IsRagdoll { get; private set; }

	private void Awake()
	{
		m_BoneRagdollable = GetComponent<BoneRagdollable>();
		m_RotationDefault = m_Center.localEulerAngles;
		
		IsRagdoll = false;

		List<Rigidbody> _List = new List<Rigidbody>();
		foreach (Rigidbody _Child in m_TransformRagdoll.GetComponentsInChildren<Rigidbody>())
		{
			_Child.isKinematic = true;
			_List.Add(_Child);
		}

		foreach (HumanBodyBones _Bone in Enum.GetValues(typeof(HumanBodyBones)))
		{
			if (_Bone == HumanBodyBones.LastBone)
			{
				break;
			}
			
			Transform _From = m_Animator.GetBoneTransform(_Bone);
			if (_From == null)
			{
				continue;
			}

			string _Hierarchy = GetHierarchyRecursive(_From);
			Transform _To = m_TransformRagdoll.Find(_Hierarchy);
			if (_To == null)
			{
				continue;
			}

			CopyTransform _Copy = new CopyTransform(_From, _To);
			Rigidbody _Rigidbody = _List.FirstOrDefault(
				_Child => _Copy.Compare(_Child.transform) && m_BoneRagdollable.IsInclude(_Bone)
			);
			if (_Rigidbody != null)
			{
				if (_Bone == HumanBodyBones.Chest || _Bone == HumanBodyBones.Hips)
				{
					_Rigidbody.isKinematic = true; // always true
				}
				else
				{
					m_ListRigidbody.Add(_Rigidbody);
				}
			}
			m_DictTransform.Add(_Bone, _Copy);

			ObservableRotation _Observable = _From.gameObject.AddComponent<ObservableRotation>();
			_Observable.OnChangedRotation()
				.Where(_ => !(IsRagdoll && m_BoneRagdollable.IsInclude(_Bone)))
				.Subscribe(_ => _Copy.CopyLocalRotation())
				.AddTo(this)
			;
		}

		this.UpdateAsObservable()
			.Where(_ => IsRagdoll)
			.Subscribe(_ => {
				Vector3 _ToRotation = m_RotationDefault;
				if (m_RotationRatio != Vector2.zero)
				{
					_ToRotation.y += ROTATE_CHEST_H * m_RotationRatio.x;
					_ToRotation.z += ROTATE_CHEST_V * m_RotationRatio.y;
				}
				
				m_Center.localEulerAngles = Vector3.Lerp(
					NormalizedVec3(m_Center.localEulerAngles),
					NormalizedVec3(_ToRotation),
					0.1f
				);
			})
			.AddTo(this)
		;
	}

	private string GetHierarchyRecursive(Transform pTransform, string pStr = "")
	{
		string _Str = pTransform.name + "/" + pStr;
		
		Transform _Parent = pTransform.parent;
		if (_Parent == null || _Parent.name == "Animation")
		{
			return _Str;
		}

		return GetHierarchyRecursive(_Parent, _Str);
	}

	private static Vector3 NormalizedVec3(Vector3 pVec)
	{
		return new Vector3(
			Mathf.Repeat(pVec.x + 180.0f, 360.0f) - 180.0f,
			Mathf.Repeat(pVec.y + 180.0f, 360.0f) - 180.0f,
			Mathf.Repeat(pVec.z + 180.0f, 360.0f) - 180.0f
		);
	}

	public void ToRagdoll()
	{
		if (IsRagdoll)
		{
			return;
		}
		
		IsRagdoll = true;
		
		m_ListRigidbody.ForEach(_Child => _Child.isKinematic = false);
	}

	public void ToAnimate()
	{
		if (!IsRagdoll)
		{
			return;
		}
		
		IsRagdoll = false;

		m_ListRigidbody.ForEach(_Child => _Child.isKinematic = true);

		foreach (var _Pair in m_DictTransform)
		{
			_Pair.Value.CopyLocalPosition();
		}
	}

	public void SetAngle(float pTheta)
	{
		m_RotationRatio = new Vector2(Mathf.Cos(pTheta), Mathf.Sin(pTheta));
	}

	public void OffControl()
	{
		m_RotationRatio = Vector2.zero;
	}
}
