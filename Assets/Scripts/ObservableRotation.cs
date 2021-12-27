using System;
using UniRx;
using UnityEngine;

public class ObservableRotation : MonoBehaviour
{
	public IObservable<Unit> OnChangedRotation()
	{
		return this.ObserveEveryValueChanged(x => x.transform.rotation)
			.AsUnitObservable()
		;
	}
}
