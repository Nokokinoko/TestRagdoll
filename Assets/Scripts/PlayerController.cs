using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private Animator m_Animator;
    private readonly List<Rigidbody> m_ListRigidbody = new List<Rigidbody>();

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        
        foreach (Rigidbody _Child in GetComponentsInChildren<Rigidbody>())
        {
            if (transform != _Child.transform)
            {
                m_ListRigidbody.Add(_Child);
            }
        }
    }

    public void ToRagdoll()
    {
        foreach (Rigidbody _Child in m_ListRigidbody)
        {
            _Child.isKinematic = false;
        }

        m_Animator.enabled = false;
    }

    public void ToAnimate()
    {
        foreach (Rigidbody _Child in m_ListRigidbody)
        {
            _Child.isKinematic = true;
        }

        m_Animator.enabled = true;
    }
}
