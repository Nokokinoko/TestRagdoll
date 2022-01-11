using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class MoveSceneConductor : MonoBehaviour
{
	[SerializeField] private MoveController m_CtrlMove;
	[SerializeField] private MenuController m_CtrlMenu;
	
	private Vector2 m_TouchBegan = Vector2.zero;
	
	private void Awake()
	{
		m_CtrlMenu.RxReset
			.Subscribe(_ => m_CtrlMove.Reset())
			.AddTo(this)
		;
		
		this.UpdateAsObservable()
			.Where(_ => m_CtrlMenu.EnableCtrl)
			.Subscribe(_ => {
				switch (InputManager.GetTouch())
				{
					case ENUM_TOUCH.TOUCH_BEGAN:
						m_TouchBegan = InputManager.GetPosition();
						break;
					case ENUM_TOUCH.TOUCH_MOVED:
						Vector2 _Dt = InputManager.GetPosition() - m_TouchBegan;
						if (_Dt.magnitude < 20.0f)
						{
							m_CtrlMove.Stand();
							break;
						}

						m_CtrlMove.Move(_Dt.normalized);
						break;
					case ENUM_TOUCH.TOUCH_ENDED:
						m_TouchBegan = Vector2.zero;
						m_CtrlMove.Stand();
						break;
				}
			})
		;
	}
}
