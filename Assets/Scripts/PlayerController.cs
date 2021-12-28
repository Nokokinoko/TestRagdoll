using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(BoneRagdollable))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] private Animator m_Animator;
	[SerializeField] private Rigidbody m_CenterOfGravity;
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

		public void CopyRotation()
		{
			To.rotation = From.rotation;
		}
	}

	private readonly Dictionary<HumanBodyBones, CopyTransform> m_DictTransform =
		new Dictionary<HumanBodyBones, CopyTransform>();

	private BoneRagdollable m_BoneRagdollable;
	private readonly List<Rigidbody> m_ListRigidbody = new List<Rigidbody>();

	public bool IsRagdoll { get; private set; }

	private void Awake()
	{
		m_BoneRagdollable = GetComponent<BoneRagdollable>();
		
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
				m_ListRigidbody.Add(_Rigidbody);
			}
			m_DictTransform.Add(_Bone, _Copy);

			ObservableRotation _Observable = _From.gameObject.AddComponent<ObservableRotation>();
			_Observable.OnChangedRotation()
				.Where(_ => !(IsRagdoll && m_BoneRagdollable.IsInclude(_Bone)))
				.Subscribe(_ => _Copy.CopyRotation())
				.AddTo(this)
			;
		}
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

	public void ToRagdoll()
	{
		IsRagdoll = true;
		
		m_ListRigidbody.ForEach(_Child => _Child.isKinematic = false);
	}

	public void ToAnimate()
	{
		IsRagdoll = false;

		m_ListRigidbody.ForEach(_Child => _Child.isKinematic = true);

		foreach (var _Pair in m_DictTransform)
		{
			_Pair.Value.CopyLocalPosition();
		}
	}

	public void AddForce(Vector2 pNormalizedInput)
	{
		m_CenterOfGravity.AddForce(
			new Vector3(pNormalizedInput.x, 0.0f, pNormalizedInput.y) * 20.0f,
			ForceMode.Acceleration
		);
	}
}
