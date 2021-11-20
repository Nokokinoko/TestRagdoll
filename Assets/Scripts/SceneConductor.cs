using UnityEngine;

public class SceneConductor : MonoBehaviour
{
    [SerializeField] private PlayerController m_CtrlPlayer;

    private void Update()
    {
        switch (InputManager.GetTouch())
        {
            case ENUM_TOUCH.TOUCH_BEGAN:
                m_CtrlPlayer.ToRagdoll();
                break;
            case ENUM_TOUCH.TOUCH_ENDED:
                m_CtrlPlayer.ToAnimate();
                break;
        }
    }
}
