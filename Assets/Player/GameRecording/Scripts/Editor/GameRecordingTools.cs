using System.Linq;
using UnityEditor;

public static class GameRecordingTools
{
    const string RECORDING_SYMBOL = "RECORDING";

    [MenuItem("HexaLinks/Recording/Enable Game Recording")]
    private static void EnableGameRecording()
    {
        PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, out string[] symbolsDefined);
        symbolsDefined = symbolsDefined.Append(RECORDING_SYMBOL).ToArray();
        PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, symbolsDefined);
    }

    [MenuItem("HexaLinks/Recording/Disable Game Recording")]
    private static void DisableGameRecording()
    {
        PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, out string[] symbolsDefined);
        symbolsDefined = symbolsDefined.Except(new string[] { RECORDING_SYMBOL }).ToArray();
        PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, symbolsDefined);
    }
}
