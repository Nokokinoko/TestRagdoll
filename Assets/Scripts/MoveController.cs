using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(FootPrint))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class MoveController : MonoBehaviour
{
	[SerializeField] private Animator m_Animator;
	[SerializeField] private Transform m_TransformRagdoll;
	
	private FootPrint m_FootPrint;

	private Rigidbody m_Rigidbody;
	private Collider m_Collider;
	private Vector2 m_NormalizedDirection = Vector2.zero;
	
	private readonly List<Rigidbody> m_ListRigidbody = new List<Rigidbody>();

	private bool IsRagdoll { get; set; }

	private const float MOVE_RATIO = 0.04f;
	
	private void Awake()
	{
		m_FootPrint = GetComponent<FootPrint>();
		m_Rigidbody = GetComponent<Rigidbody>();
		
		m_Collider = GetComponent<Collider>();
		m_Collider.OnTriggerEnterAsObservable()
			.Where(_Collider => _Collider.CompareTag("Wall"))
			.Subscribe(_ => ToRagdoll())
			.AddTo(this)
		;
		
		IsRagdoll = false;
		
		foreach (Rigidbody _Child in m_TransformRagdoll.GetComponentsInChildren<Rigidbody>())
		{
			_Child.isKinematic = true;
			m_ListRigidbody.Add(_Child);
		}

		this.FixedUpdateAsObservable()
			.Where(_ => !IsRagdoll && m_NormalizedDirection != Vector2.zero)
			.Subscribe(_ => {
				Vector3 _Before = m_Rigidbody.position;
				
				Vector3 _After = _Before;
				_After.x += MOVE_RATIO * m_NormalizedDirection.x;
				_After.z += MOVE_RATIO * m_NormalizedDirection.y;
				m_Rigidbody.position = _After;

				m_Rigidbody.rotation = Quaternion.LookRotation(_Before - _After);
			})
			.AddTo(this)
		;

		Observable.Interval(TimeSpan.FromSeconds(0.3f))
			.Where(_ => !IsRagdoll && m_NormalizedDirection != Vector2.zero)
			.Subscribe(_ => m_FootPrint.Leave())
			.AddTo(this)
		;
	}

	private void ToRagdoll()
	{
		if (IsRagdoll)
		{
			return;
		}

		IsRagdoll = true;
		
		m_Animator.enabled = false;
		m_Collider.enabled = false;
		m_ListRigidbody.ForEach(_Child => _Child.isKinematic = false);

		m_NormalizedDirection = Vector2.zero;
	}

	public void Reset()
	{
		transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
		
		IsRagdoll = false;

		m_Animator.enabled = true;
		m_Collider.enabled = true;
		m_ListRigidbody.ForEach(_Child => _Child.isKinematic = true);
		
		Stand();
	}

	public void Stand()
	{
		if (IsRagdoll)
		{
			return;
		}

		m_NormalizedDirection = Vector2.zero;
		m_Animator.SetBool("Move", false);
	}

	public void Move(Vector2 pDirection)
	{
		if (IsRagdoll)
		{
			return;
		}

		m_NormalizedDirection = pDirection;
		m_Animator.SetBool("Move", true);
	}
}
