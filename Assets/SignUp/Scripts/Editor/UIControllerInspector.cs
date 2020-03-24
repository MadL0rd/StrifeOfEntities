
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace GameSparksTutorials
{
    [CustomEditor(typeof(UIController))]
    public class UIControllerInspector : Editor
    {
        private bool showAdditionalElements = false;

        private ReorderableList hideInEditor;

        public override void OnInspectorGUI()
        {
            if (!EditorApplication.isPlaying)
            {
                // Background
                GUI.backgroundColor = Color.green;
                
                var ui = target as UIController;

                // Active Panel DropDown
                GUILayout.Label("Active Panel", InspectorStyles.Text);
                ui.ActivePanel = (UI_Element)EditorGUILayout.EnumPopup(ui.ActivePanel, InspectorStyles.ButtonActive, InspectorStyles.LayoutOptions);

                // Set active Panel
                UIController.SetActivePanel(ui.ActivePanel);

                // Additional Header
                showAdditionalElements = EditorGUILayout.Foldout(showAdditionalElements, "Additional");

                if (showAdditionalElements)
                {
                    ui.BackgroundBlur = (GameObject)EditorGUILayout.ObjectField("Background Blur", ui.BackgroundBlur, typeof(GameObject), true);
                    ui.LoadingCircle = (GameObject)EditorGUILayout.ObjectField("Loading Circle", ui.LoadingCircle, typeof(GameObject), true);


                    serializedObject.Update();
                    hideInEditor.DoLayoutList();
                    serializedObject.ApplyModifiedProperties();

                    foreach (var go in ui.HideInEditor)
                    {
                        if (go) go.SetActive(false);
                    }
                }
            }
        }

        private void OnEnable()
        {
            hideInEditor = new ReorderableList(serializedObject,
                    serializedObject.FindProperty("HideInEditor"),
                    true, true, true, true);

            hideInEditor.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = hideInEditor.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.ObjectField(
                    new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element, typeof(GameObject), GUIContent.none);
            };

            hideInEditor.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Hide in Editor");
            };
        }
    }
}
#endif