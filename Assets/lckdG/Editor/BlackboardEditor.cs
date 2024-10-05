#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace DevToolkit.AI.Editor
{
    [CustomEditor(typeof(Blackboard), true)]
    public class BlackboardEditor : UnityEditor.Editor
    {
        private int contextSize = 0;
        private List<bool> foldouts = new List<bool>();
        private bool foldoutInited = false;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if ( Application.isPlaying == false )
            {
                InspectBlackboardEditorMode();
            }
            else
            {
                InspectBlackboardPlayMode();
            }
        }

        private void InspectBlackboardEditorMode()
        {
            SerializedProperty context = serializedObject.FindProperty("keyMappingList");
            InitFoldout(context.arraySize);

            void InsertElementToContext()
            {
                bool emptyList = context.arraySize == 0;
                context.InsertArrayElementAtIndex(emptyList ? 0 : context.arraySize - 1);
            }

            void RemoveElementFromContext()
            {
                context.DeleteArrayElementAtIndex(context.arraySize - 1);
            }

            using (new EditorGUI.DisabledScope(true))
            {
                SerializedProperty navMeshAgent = serializedObject.FindProperty("agent");
                GUIContent navMeshAgentLabel = new GUIContent("Nav Mesh Agent", "Nav Mesh Agent of this behavior tree, should be assigned at runtime" );

                EditorGUILayout.ObjectField(navMeshAgent, navMeshAgentLabel);

                GUIContent contextSizeLabel = new GUIContent("Context Size");
                EditorGUILayout.IntField(contextSizeLabel, contextSize > 0 ? contextSize : 0);
            }


            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("+"))
            {
                ++contextSize;
            }

            if (GUILayout.Button("-"))
            {
                --contextSize;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            UpdateFoldout(context.arraySize, InsertElementToContext, RemoveElementFromContext);
            EditorGUI.indentLevel++;

            for (int i = 0; i < context.arraySize; i++)
            {
                SerializedProperty _ = context.GetArrayElementAtIndex(i);

                SerializedProperty type = _.FindPropertyRelative("type");
                SerializedProperty keyString = _.FindPropertyRelative("keyString");

                bool keyStringAssigned = KeyStringAssigned(keyString);

                string contextLabel = $"Context {i}";
                if (keyStringAssigned == false)
                {
                    contextLabel = string.Concat(contextLabel, " - NEEDS ASSIGNMENT!");
                } 

                foldouts[i] = EditorGUILayout.BeginFoldoutHeaderGroup( foldouts[i], contextLabel);               
                if (foldouts[i])
                {
                    EditorGUILayout.PropertyField(type);

                    GUIContent keyStringLabel = new GUIContent(keyStringAssigned ? "Key String" : "Key String - REQUIRED");
                    EditorGUILayout.PropertyField(keyString, keyStringLabel);

                    int enumIndex = type.enumValueIndex;
                    DrawPropertyFor(_, enumIndex);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }

        private void InitFoldout(int size)
        {
            if (foldoutInited == false)
            {
                foldoutInited = true;
                for (; foldouts.Count < size; )
                {
                    foldouts.Add(false);
                }

                contextSize = size;
            }
        }

        private void UpdateFoldout(int size, System.Action onAddFoldout = null, System.Action onRemoveFoldout = null)
        {
            if (contextSize != size)
            {
                int diff = contextSize - size;
                if (diff > 0)
                {
                    for (int i = 0; i < diff; i++)
                    {
                        onAddFoldout?.Invoke();
                        foldouts.Add(false);
                    }
                }
                else
                {
                    for ( int i = 0; i < -diff; i++)
                    {
                        if ( foldouts.Count > 0 )
                        {
                            onRemoveFoldout?.Invoke();
                            foldouts.RemoveAt(foldouts.Count - 1);
                        }
                    }
                }
            }
        }

        private void InspectBlackboardPlayMode()
        {
            Blackboard blackboard = target as Blackboard;
            var allKeys = blackboard.GetAllKeyMaps();
            int keyCount = allKeys.Count;

            InitFoldout(keyCount);

            UpdateFoldout(keyCount);
            contextSize = keyCount;

            using (new EditorGUI.DisabledScope(true))
            {
                GUIContent contextSizeLabel = new GUIContent("Context Size");
                EditorGUILayout.IntField(contextSizeLabel, keyCount);
            }

            EditorGUI.indentLevel++;

            for (int i = 0; i < keyCount; i++)
            {
                BlackboardKeyMapping keyMap = allKeys[i];

                string contextLabel = $"Context {i}";

                foldouts[i] = EditorGUILayout.BeginFoldoutHeaderGroup(foldouts[i], contextLabel);               
                if (foldouts[i])
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        BlackboardObjectType type = keyMap.type;
                        EditorGUILayout.EnumPopup("Type", type);    

                        string keyString = keyMap.keyString;
                        EditorGUILayout.TextField("Key String", keyString);

                        DrawKey(keyMap);
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            EditorGUI.indentLevel--;
        }

        private bool KeyStringAssigned(SerializedProperty keyString) => !string.IsNullOrEmpty(keyString.stringValue);

        private void DrawPropertyFor(SerializedProperty element, int typeIndex)
        {
            switch(typeIndex)
            {
                case (int)BlackboardObjectType.String:
                    var stringValue = element.FindPropertyRelative("stringValue");
                    EditorGUILayout.PropertyField(stringValue, new GUIContent("String Value"));
                    break;

                case (int)BlackboardObjectType.Float:
                {
                    var floatValue = element.FindPropertyRelative("floatValue");
                    EditorGUILayout.PropertyField(floatValue, new GUIContent("Float Value"));
                    break;
                }

                case (int)BlackboardObjectType.Int:
                {
                    var intValue = element.FindPropertyRelative("intValue");
                    EditorGUILayout.PropertyField(intValue, new GUIContent("Int Value"));
                    break;
                }

                case (int)BlackboardObjectType.Vector2:
                {
                    var vec2Value = element.FindPropertyRelative("vector2");
                    EditorGUILayout.PropertyField(vec2Value, new GUIContent("Vector2 Value"));
                    break;
                }

                case (int)BlackboardObjectType.Vector3:
                {
                    var vec3Value = element.FindPropertyRelative("vector3");
                    EditorGUILayout.PropertyField(vec3Value, new GUIContent("Vector3 Value"));
                    break;
                }

                case (int)BlackboardObjectType.Bool:
                {
                    var boolValue = element.FindPropertyRelative("boolValue");
                    EditorGUILayout.PropertyField(boolValue, new GUIContent("Bool Value"));
                    break;
                }
            }
        }
    
        private void DrawKey(BlackboardKeyMapping keyMapping)
        {
            BlackboardObjectType type = keyMapping.type;
            switch(type)
            {
                case BlackboardObjectType.String:
                    var stringValue = keyMapping.stringValue;
                    EditorGUILayout.TextField(new GUIContent("String Value"), stringValue);
                    break;

                case BlackboardObjectType.Float:
                {
                    var floatValue = keyMapping.floatValue;
                    EditorGUILayout.FloatField(new GUIContent("Float Value"), floatValue);
                    break;
                }

                case BlackboardObjectType.Int:
                {
                    var intValue = keyMapping.intValue;
                    EditorGUILayout.IntField(new GUIContent("Int Value"), intValue);
                    break;
                }

                case BlackboardObjectType.Vector2:
                {
                    var vec2Value = keyMapping.vector2;
                    EditorGUILayout.Vector2Field(new GUIContent("Vector2 Value"), vec2Value);
                    break;
                }

                case BlackboardObjectType.Vector3:
                {
                    var vec3Value = keyMapping.vector3;
                    EditorGUILayout.Vector3Field(new GUIContent("Vector3 Value"), vec3Value);
                    break;
                }

                case BlackboardObjectType.Bool:
                {
                    var boolValue = keyMapping.boolValue;
                    EditorGUILayout.Toggle(new GUIContent("Bool Value"), boolValue);
                    break;
                }

                case BlackboardObjectType.Object:
                {
                    var objectRef = keyMapping.objRef;
                    EditorGUILayout.IntField(new GUIContent("Object Hash Code" ), objectRef.GetHashCode());
                    break;
                }
            }
        }
    }
}

#endif
