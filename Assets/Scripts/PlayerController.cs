using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] private Rigidbody m_CenterOfGravity;
	
	private Animator m_Animator;
	private readonly List<Rigidbody> m_ListRigidbody = new List<Rigidbody>();

	public bool IsRagdoll { get; private set; }

	private void Awake()
	{
		m_Animator = GetComponent<Animator>();
		IsRagdoll = false;
		
		foreach (Rigidbody _Child in GetComponentsInChildren<Rigidbody>())
		{
			if (transform != _Child.transform)
			{
				m_ListRigidbody.Add(_Child);
			}
		}
	}

	private void Start()
	{
		m_Animator.Play("Walking@loop");
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
