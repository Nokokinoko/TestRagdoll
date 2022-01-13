using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif

public class ButtonMenu : Button
{
	public ShakeGraphic GraphicRainbow { get; set; }
	public ShakeGraphic GraphicBase { get; set; }

	protected override void Awake()
	{
		GraphicRainbow.gameObject.SetActive(false);
		GraphicBase.gameObject.SetActive(false);
		
		base.Awake();
	}

	public void OnSelect()
	{
		GraphicRainbow.gameObject.SetActive(true);
		GraphicBase.gameObject.SetActive(true);
	}

	public void OnDeselect()
	{
		GraphicRainbow.gameObject.SetActive(false);
		GraphicBase.gameObject.SetActive(false);
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(ButtonMenu))]
public class ButtonMenuEditor : ButtonEditor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("----- Shake Graphic -----");

		ButtonMenu _Component = (ButtonMenu)target;
		_Component.GraphicRainbow = (ShakeGraphic)EditorGUILayout.ObjectField(
			"Graphic Rainbow",
			_Component.GraphicRainbow,
			typeof(ShakeGraphic),
			true
		);
		_Component.GraphicBase = (ShakeGraphic)EditorGUILayout.ObjectField(
			"Graphic Base",
			_Component.GraphicBase,
			typeof(ShakeGraphic),
			true
		);
	}
}
#endif
