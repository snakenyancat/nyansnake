#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Gists.Editor
{
    public class HideSplashScreen : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            PlayerSettings.SplashScreen.show = false;
            Debug.Log("Splash screen disabled (only works with a non-free licence)");
        }
    }
}
#endif
