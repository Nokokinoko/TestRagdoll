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
						if (m_CtrlPlayer.IsRagdoll)
						{
							m_TouchBegan = InputManager.GetPosition();
						}
						else
						{
							m_CtrlPlayer.ToRagdoll();
						}
						break;
					case ENUM_TOUCH.TOUCH_MOVED:
						if (m_TouchBegan != Vector2.zero)
						{
							m_CtrlPlayer.AddForce((InputManager.GetPosition() - m_TouchBegan).normalized);
						}
						break;
					case ENUM_TOUCH.TOUCH_ENDED:
						m_TouchBegan = Vector2.zero;
						break;
				}
			})
		;
	}
}
