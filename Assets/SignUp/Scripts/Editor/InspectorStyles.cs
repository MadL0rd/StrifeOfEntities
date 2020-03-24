
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace GameSparksTutorials
{
    public class InspectorStyles : Editor
    {
        public static GUIStyle ButtonNormal
        {
            get
            {
                return new GUIStyle(GUI.skin.button)
                {
                    padding = new RectOffset(5, 5, 5, 5),
                    margin = new RectOffset(30, 30, 5, 5),
                    fontStyle = FontStyle.Normal,
                    fontSize = 16
                };
            }
        }

        public static GUIStyle ButtonActive
        {
            get
            {
                return new GUIStyle(GUI.skin.button)
                {
                    padding = new RectOffset(5, 5, 5, 5),
                    margin = new RectOffset(30, 30, 5, 5),
                    fontStyle = FontStyle.Bold,
                    fontSize = 18
                };
            }
        }

        public static GUIStyle Text
        {
            get
            {
                return new GUIStyle(GUI.skin.textArea)
                {
                    padding = new RectOffset(5, 5, 5, 5),
                    margin = new RectOffset(30, 30, 5, 5),
                    fontStyle = FontStyle.Bold,
                    fontSize = 16,
                    alignment = TextAnchor.MiddleCenter
                };
            }
        }

        public static GUILayoutOption[] LayoutOptions
        {
            get
            {
                return new GUILayoutOption[]
                {
                    GUILayout.MinHeight(30),
                    GUILayout.MinWidth(100)
                };
            }
        }

        public static GUIStyle GSBackground
        {
            get
            {
                var backgroundTexture = Resources.Load<Texture2D>("GameSparksOrange");
                var styleState = new GUIStyleState
                {
                    background = backgroundTexture
                };

                return new GUIStyle
                {
                    normal = styleState
                };
            }
        }
    }
}
#endif
