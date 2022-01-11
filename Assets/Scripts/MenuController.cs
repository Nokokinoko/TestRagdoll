using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	[SerializeField] private Transform m_UITop;
	[SerializeField] private Button m_BtnMenu;

	[Space]
	[SerializeField] private Transform m_UIMenu;
	[SerializeField] private ShakeGraphic m_ShakeBlack;
	[SerializeField] private ShakeGraphic m_ShakeRed;
	[SerializeField] private Transform m_Star;
	[SerializeField] private Button m_BtnReset;
	[SerializeField] private Button m_BtnNone;
	[SerializeField] private Button m_BtnClose;

	private const float TIME_SWITCH = 0.3f;
	private const float DISABLE_POSITION_TOP = 1920.0f;
	private const float DISABLE_POSITION_MENU = 1080.0f;

	private readonly Subject<Unit> m_RxReset = new Subject<Unit>();
	public IObservable<Unit> RxReset => m_RxReset.AsObservable();

	private void Awake()
	{
		m_UITop.localPosition = Vector3.zero;
		Vector3 _Position = Vector3.zero;
		_Position.x = DISABLE_POSITION_MENU;
		m_UIMenu.localPosition = _Position;

		// top
		m_BtnMenu.OnClickAsObservable()
			.Subscribe(_ => {
				m_UITop.DOLocalMoveY(DISABLE_POSITION_TOP, TIME_SWITCH).SetEase(Ease.Linear);
				m_UIMenu.DOLocalMoveX(0.0f, TIME_SWITCH)
					.SetEase(Ease.Linear)
					.OnComplete(() => m_ShakeBlack.Shake = m_ShakeRed.Shake = true);
			})
			.AddTo(this);

		// menu
		Vector3 _Angle = m_Star.localEulerAngles;
		_Angle.z += 72.0f;
		m_Star.DOLocalRotate(_Angle, 1.0f, RotateMode.FastBeyond360)
			.SetDelay(1.0f)
			.SetEase(Ease.OutBack)
			.SetLoops(-1, LoopType.Restart);
		
		m_BtnReset.OnClickAsObservable()
			.Subscribe(_ => {
				m_RxReset.OnNext(Unit.Default);
			})
			.AddTo(this);
		
		m_BtnClose.OnClickAsObservable()
			.Subscribe(_ => {
				m_UITop.DOLocalMoveY(0.0f, TIME_SWITCH).SetEase(Ease.Linear);
				m_UIMenu.DOLocalMoveX(DISABLE_POSITION_MENU, TIME_SWITCH)
					.SetEase(Ease.Linear)
					.OnComplete(() => m_ShakeBlack.Shake = m_ShakeRed.Shake = false);
			})
			.AddTo(this);
	}
}
