using UnityEngine;

public class VertexInfo
{
    private readonly ShakeGraphic.VertexSetting m_Setting;
    private Vector2 m_From;
    private Vector2 m_To;

    private Vector2 m_Draw;

    public VertexInfo(ShakeGraphic.VertexSetting pSetting)
    {
        m_Setting = pSetting;
        m_From = m_To = m_Draw = m_Setting.m_Position;
    }

    public void SetNextVertex()
    {
        if (!m_Setting.m_IsShake)
        {
            return;
        }

        m_From = m_To;

        m_To = m_Setting.m_Position;
        m_To.x += Random.Range(-20, 20);
        m_To.y += Random.Range(-20, 20);
    }

    public void CalcPosition(float pRatio)
    {
        m_Draw = Vector2.Lerp(m_From, m_To, pRatio);
    }

    public UIVertex GetUIVertex()
    {
        UIVertex _Vertex = UIVertex.simpleVert;
        _Vertex.position = m_Draw;
        return _Vertex;
    }
}
