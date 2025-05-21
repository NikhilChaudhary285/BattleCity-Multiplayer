#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TagFieldAttribute))]
public class TagFieldDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (property.propertyType == SerializedPropertyType.String)
		{
			property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
		}
		else
		{
			EditorGUI.LabelField(position, label.text, "Use [TagField] with string.");
		}
	}
}
#endif
