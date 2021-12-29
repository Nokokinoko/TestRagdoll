using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class SceneConductor : MonoBehaviour
{
	[SerializeField] private PlayerController m_CtrlPlayer;

	[SerializeField] private Button m_BtnReset;

	private Vector2 m_TouchBegan = Vector2.zero;

	private void Awake()
	{
		m_BtnReset.OnClickAsObservable()
			.Where(_ => m_CtrlPlayer.IsRagdoll)
			.Subscribe(_ => m_CtrlPlayer.ToAnimate())
			.AddTo(this)
		;
		
		this.UpdateAsObservable()
			.Subscribe(_ => {
				switch (InputManager.GetTouch())
				{
					case ENUM_TOUCH.TOUCH_BEGAN:
						m_CtrlPlayer.ToRagdoll();
						m_TouchBegan = InputManager.GetPosition();
						break;
					case ENUM_TOUCH.TOUCH_MOVED:
						Vector2 _Dt = InputManager.GetPosition() - m_TouchBegan;
						if (_Dt.magnitude < 20.0f)
						{
							break;
						}

						float _Rad = Mathf.Atan2(_Dt.y, _Dt.x);
						m_CtrlPlayer.SetAngle(_Rad);
						break;
					case ENUM_TOUCH.TOUCH_ENDED:
						m_TouchBegan = Vector2.zero;
						m_CtrlPlayer.OffControl();
						break;
				}
			})
		;
	}
}
