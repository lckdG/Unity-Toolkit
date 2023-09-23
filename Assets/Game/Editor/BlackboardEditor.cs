#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace AI.Tree.Editor
{
    // TODO: add pre-defined keys
    [CustomEditor(typeof(Blackboard), true)]
    public class BlackboardEditor : UnityEditor.Editor
    {
        private int contextSize = 0;
        private List<bool> foldouts = new List<bool>();
        private bool foldoutInited = false;
        public override void OnInspectorGUI()
        {
            Blackboard blackboard = target as Blackboard;
            serializedObject.Update();

            SerializedProperty context = serializedObject.FindProperty("keyMappingList");
            
            if ( !foldoutInited )
            {
                foldoutInited = true;
                for (; foldouts.Count < context.arraySize; )
                {
                    foldouts.Add( false );
                }

                contextSize = context.arraySize;
            }

            GUIContent contextSizeLabel = new GUIContent("Context Size");
            contextSize = EditorGUILayout.IntField( contextSizeLabel, contextSize > 0 ? contextSize : 0 );

            if ( contextSize != context.arraySize )
            {
                int diff = contextSize - context.arraySize;
                if ( diff > 0)
                {
                    for ( int i = 0; i < diff; i++)
                    {
                        bool emptyList = context.arraySize == 0;
                        context.InsertArrayElementAtIndex( emptyList ? 0 : context.arraySize - 1 );
                        foldouts.Add( false );
                    }
                }
                else
                {
                    for ( int i = 0; i < -diff; i++)
                    {
                        if ( foldouts.Count > 0)
                        {
                            context.DeleteArrayElementAtIndex( context.arraySize - 1 );
                            foldouts.RemoveAt( foldouts.Count - 1);
                        }
                    }
                }
            }

            EditorGUI.indentLevel++;

            for (int i = 0; i < context.arraySize; i++)
            {
                SerializedProperty _ = context.GetArrayElementAtIndex( i );

                SerializedProperty type = _.FindPropertyRelative("type");
                SerializedProperty keyString = _.FindPropertyRelative("keyString");

                bool keyStringAssigned = KeyStringAssigned( keyString );

                string contextLabel = $"Context {i}";
                if ( !keyStringAssigned )
                {
                    contextLabel = string.Concat(contextLabel, " - NEEDS ASSIGNMENT!");
                } 

                foldouts[i] = EditorGUILayout.BeginFoldoutHeaderGroup( foldouts[i], contextLabel );               
                if ( foldouts[i] )
                {
                    EditorGUILayout.PropertyField( type );

                    GUIContent keyStringLabel = new GUIContent(keyStringAssigned ? "Key String" : "Key String - REQUIRED");
                    EditorGUILayout.PropertyField( keyString, keyStringLabel );

                    int enumIndex = type.enumValueIndex;
                    DrawPropertyFor( _, enumIndex );
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }

        private bool KeyStringAssigned( SerializedProperty keyString ) => !string.IsNullOrEmpty( keyString.stringValue );

        private void DrawPropertyFor( SerializedProperty element, int typeIndex )
        {
            switch( typeIndex )
            {
                case (int)BlackboardObjectType.String:
                    var stringValue = element.FindPropertyRelative("stringValue");
                    EditorGUILayout.PropertyField( stringValue, new GUIContent("String Value") );
                    break;

                case (int)BlackboardObjectType.Float:
                {
                    var floatVal = element.FindPropertyRelative("floatValue");
                    EditorGUILayout.PropertyField( floatVal, new GUIContent("Float Value") );
                    break;
                }

                case (int)BlackboardObjectType.Int:
                {
                    var intVal = element.FindPropertyRelative("intValue");
                    EditorGUILayout.PropertyField( intVal, new GUIContent("Int Value") );
                    break;
                }

                case (int)BlackboardObjectType.Vector2:
                {
                    var vec2Val = element.FindPropertyRelative("vector2");
                    EditorGUILayout.PropertyField( vec2Val, new GUIContent("Vector2 Value") );
                    break;
                }

                case (int)BlackboardObjectType.Vector3:
                {
                    var vec3Val = element.FindPropertyRelative("vector3");
                    EditorGUILayout.PropertyField( vec3Val, new GUIContent("Vector3 Value") );
                    break;
                }

                case (int)BlackboardObjectType.Bool:
                {
                    var boolVal_ = element.FindPropertyRelative("boolValue");
                    EditorGUILayout.PropertyField( boolVal_, new GUIContent("Bool Value") );
                    break;
                }
            }
        }
    }
}

#endif
