#if UNITY_EDITOR

using System.Diagnostics;
using System.Drawing;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class BuildTools
{
    private const string BUILD_FOLDER_NAME = "Builds";
    private const string BUILD_NAME = "BuiltGame.exe";

    private static Size FULLHD_SCREEN_SIZE = new Size(1920, 1080);

    [MenuItem("Builds/Create Build")]
    public static void CreateBuild()
    {
        if (!Directory.Exists(BUILD_FOLDER_NAME))
            Directory.CreateDirectory(BUILD_FOLDER_NAME);

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
        {
            locationPathName = Path.Combine(BUILD_FOLDER_NAME, BUILD_NAME),
            target = BuildTarget.StandaloneWindows64
        };

        PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
        PlayerSettings.defaultScreenHeight = FULLHD_SCREEN_SIZE.Height / 2;
        PlayerSettings.defaultScreenWidth = FULLHD_SCREEN_SIZE.Width / 2;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    [MenuItem("Builds/Simulate Game")]
    public static void SimulateGame()
    {
        string buildPath = Path.Combine(BUILD_FOLDER_NAME, BUILD_NAME);

        UnityEngine.Assertions.Assert.IsTrue(File.Exists(buildPath), "No game built created previously");
            
        Process procPlayerOne = new Process();
        procPlayerOne.StartInfo.FileName = buildPath;
        procPlayerOne.Start();

        Process procPlayerTwo = new Process();
        procPlayerTwo.StartInfo.FileName = buildPath;
        procPlayerTwo.Start();
    }
}

#endif
