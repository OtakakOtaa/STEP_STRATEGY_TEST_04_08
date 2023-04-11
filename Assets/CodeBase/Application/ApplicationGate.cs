using UnityEditor;
using UnityEngine;

namespace CodeBase.Application
{
    public sealed class ApplicationGate
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Bootstrap()
        {
#if UNITY_EDITOR
            EditorWindow.focusedWindow.maximized = true;
#endif
        }

        public void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            UnityEngine.Application.Quit();
        }
    }
}