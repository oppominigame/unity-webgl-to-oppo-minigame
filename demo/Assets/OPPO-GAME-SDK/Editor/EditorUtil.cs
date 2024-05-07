using UnityEngine;
using UnityEditor;

namespace QGMiniGame
{
    public static class EditorUtil
    {
#if UNITY_2019_2_OR_NEWER
        public static GUIStyle linkLabel => EditorStyles.linkLabel;
#else
        private static GUIStyle m_linkLabel;
        public static GUIStyle linkLabel
        {
            get
            {
                if (m_linkLabel == null)
                {
                    m_linkLabel = new GUIStyle(EditorStyles.label);
                    m_linkLabel.normal.textColor = new Color(0.45f, 0.62f, 0.85f);
                    m_linkLabel.fontSize = 12;
                }
                return m_linkLabel;
            }
        }
#endif

        public static float lineHeightWithMargin =>
            EditorGUIUtility.singleLineHeight +
            EditorGUIUtility.standardVerticalSpacing;

        private static string GetKey(string title, string category)
        {
            return $"{nameof(QGMiniGame)}-{category}-{title}";
        }

        private static string GetFoldOutKey(string title)
        {
            return GetKey(title, "FoldOut");
        }

        public static bool EditorOnlyToggle(string title, string category, bool initialState)
        {
            var keyTitle = title.Replace(" ", "_");
            var key = GetKey(keyTitle, category);
            var value = EditorPrefs.GetBool(key, initialState);
            var newValue = EditorGUILayout.Toggle(title, value);
            if (newValue != value)
            {
                EditorPrefs.SetBool(key, newValue);
            }
            return newValue;
        }

        public static bool IsFoldOutOpened(string title, bool initialState = false, string additionalKey = "")
        {
            var key = GetFoldOutKey(title + additionalKey);
            if (!EditorPrefs.HasKey(key)) return initialState;
            return EditorPrefs.GetBool(key);
        }

        public static bool Foldout(string title, bool initialState, string additionalKey = "")
        {
            var style = new GUIStyle("ShurikenModuleTitle")
            {
                font = new GUIStyle(EditorStyles.boldLabel).font,
                fontSize = 13,
                border = new RectOffset(15, 7, 4, 4),
                fixedHeight = 26,
                contentOffset = new Vector2(20f, -2f),
                margin = new RectOffset((EditorGUI.indentLevel + 1) * 16, 2, 0, 0)
            };

            var key = GetFoldOutKey(title + additionalKey);
            bool display = EditorPrefs.GetBool(key, initialState);

            var rect = GUILayoutUtility.GetRect(16f, 26f, style);
            GUI.Box(rect, title, style);

            var e = Event.current;

            var toggleRect = new Rect(rect.x + 4f, rect.y + 6f, 13f, 13f);
            if (e.type == EventType.Repaint)
            {
                EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            {
                EditorPrefs.SetBool(key, !display);
                e.Use();
            }

            return display;
        }

        public static bool SimpleFoldout(Rect rect, string title, bool initialState, string additionalKey = "")
        {
            var key = GetFoldOutKey(title + additionalKey);
            bool display = EditorPrefs.GetBool(key, initialState);
            bool newDisplay = EditorGUI.Foldout(rect, display, title);
            if (newDisplay != display) EditorPrefs.SetBool(key, newDisplay);
            return newDisplay;
        }

        public static bool SimpleFoldout(string title, bool initialState, string additionalKey = "")
        {
            var key = GetFoldOutKey(title + additionalKey);
            bool display = EditorPrefs.GetBool(key, initialState);
            var newDisplayStyle =
#if UNITY_2019_1_OR_NEWER
                EditorStyles.foldoutHeader;
#else
                EditorStyles.label;
#endif
            bool newDisplay = EditorGUILayout.Foldout(display, title, newDisplayStyle);
            if (newDisplay != display) EditorPrefs.SetBool(key, newDisplay);
            return newDisplay;
        }

        public static bool LinkButton(string label, params GUILayoutOption[] options)
        {
#if UNITY_2021_1_OR_NEWER
            return EditorGUILayout.LinkButton(label, options);
#else
            return LinkButton(EditorGUIUtility.TrTempContent(label), options);
#endif
        }

        public static bool LinkButton(GUIContent label, params GUILayoutOption[] options)
        {
#if UNITY_2021_1_OR_NEWER
            return EditorGUILayout.LinkButton(label, options);
#else
            Rect position = GUILayoutUtility.GetRect(label, linkLabel, options);
            Handles.color = linkLabel.normal.textColor;
            Handles.DrawLine(new Vector3(position.xMin + linkLabel.padding.left, position.yMax), new Vector3(position.xMax - linkLabel.padding.right, position.yMax));
            Handles.color = Color.white;
            EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);
            return GUI.Button(position, label, linkLabel);
#endif
        }

        public static void Space(float width = 6f)
        {
            GUILayoutUtility.GetRect(width, width, GUILayout.ExpandWidth(true));
        }

        public static void DrawProperty(SerializedObject obj, string propName)
        {
            var prop = obj.FindProperty(propName);
            if (prop == null) return;
            EditorGUILayout.PropertyField(prop);
        }

        public static void DrawBackgroundRect(Rect rect, Color bg, Color line)
        {
            Handles.DrawSolidRectangleWithOutline(rect, bg, line);
        }

        public static void DrawBackgroundRect(Rect rect)
        {
            DrawBackgroundRect(
                rect,
                new Color(0f, 0f, 0f, 0.2f),
                new Color(1f, 1f, 1f, 0.2f));
        }
    }
}