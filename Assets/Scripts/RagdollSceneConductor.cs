using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class RagdollSceneConductor : MonoBehaviour
{
	[SerializeField] private RagdollController m_CtrlRagdoll;

	[SerializeField] private Button m_BtnReset;

	private Vector2 m_TouchBegan = Vector2.zero;

	private void Awake()
	{
		m_BtnReset.OnClickAsObservable()
			.Subscribe(_ => m_CtrlRagdoll.ToAnimate())
			.AddTo(this)
		;
		
		this.UpdateAsObservable()
			.Subscribe(_ => {
				switch (InputManager.GetTouch())
				{
					case ENUM_TOUCH.TOUCH_BEGAN:
						m_CtrlRagdoll.ToRagdoll();
						m_TouchBegan = InputManager.GetPosition();
						break;
					case ENUM_TOUCH.TOUCH_MOVED:
						Vector2 _Dt = InputManager.GetPosition() - m_TouchBegan;
						if (_Dt.magnitude < 20.0f)
						{
							break;
						}

						float _Rad = Mathf.Atan2(_Dt.y, _Dt.x);
						m_CtrlRagdoll.SetAngle(_Rad);
						break;
					case ENUM_TOUCH.TOUCH_ENDED:
						m_TouchBegan = Vector2.zero;
						m_CtrlRagdoll.OffControl();
						break;
				}
			})
		;
	}
}
