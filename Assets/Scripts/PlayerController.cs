using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Animator m_Animator;
	[SerializeField] private Rigidbody m_CenterOfGravity;
	[SerializeField] private Transform m_TransformRagdoll;

	private readonly Dictionary<Transform, Transform> m_DictTransform = new Dictionary<Transform, Transform>();
	private readonly List<Rigidbody> m_ListRigidbody = new List<Rigidbody>();

	public bool IsRagdoll { get; private set; }

	private void Awake()
	{
		IsRagdoll = false;
		
		foreach (Rigidbody _Child in GetComponentsInChildren<Rigidbody>())
		{
			if (transform != _Child.transform)
			{
				_Child.isKinematic = true;
				m_ListRigidbody.Add(_Child);
			}
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
			
			m_DictTransform.Add(_From, _To);

			ObservableRotation _Observable = _From.gameObject.AddComponent<ObservableRotation>();
			_Observable.OnChangedRotation()
				.Where(_ => !IsRagdoll)
				.Subscribe(_ => _To.rotation = _From.rotation)
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
		
		foreach (Rigidbody _Child in m_ListRigidbody)
		{
			_Child.isKinematic = false;
		}

		m_Animator.enabled = false;
	}

	public void ToAnimate()
	{
		IsRagdoll = false;
		
		foreach (Rigidbody _Child in m_ListRigidbody)
		{
			_Child.isKinematic = true;
		}

		foreach (var _Pair in m_DictTransform)
		{
			_Pair.Value.localPosition = _Pair.Key.localPosition;
		}

		m_Animator.enabled = true;
	}

	public void AddForce(Vector2 pNormalizedInput)
	{
		m_CenterOfGravity.AddForce(
			new Vector3(pNormalizedInput.x, 0.0f, pNormalizedInput.y) * 20.0f,
			ForceMode.Acceleration
		);
	}
}
