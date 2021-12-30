using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class FootPrint : MonoBehaviour
{
	[SerializeField] private GameObject m_Prefab;

	public void Leave()
	{
		Transform _Transform = transform;
		Vector3 _Position = _Transform.position;
		_Position.y += 0.001f;
		Vector3 _Rotation = _Transform.rotation.eulerAngles;
		_Rotation.y += 180.0f;
		GameObject _Obj = Instantiate(m_Prefab, _Position, Quaternion.Euler(_Rotation));
		
		Material _Mat = _Obj.GetComponent<MeshRenderer>().material;
		Color _Color = _Mat.color;
		_Color.a = 1.0f;
		_Mat.color = _Color;

		Observable.Timer(TimeSpan.FromSeconds(1.0f))
			.Subscribe(_ => _Mat.DOFade(0.0f, 1.0f).SetEase(Ease.Linear).OnComplete(() => Destroy(_Obj)))
			.AddTo(this)
		;
	}
}
