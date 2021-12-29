using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Uneditable))]
public class UneditableDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginDisabledGroup(true);

		EditorGUI.PropertyField(position, property, label);
		
		EditorGUI.EndDisabledGroup();
	}
}
