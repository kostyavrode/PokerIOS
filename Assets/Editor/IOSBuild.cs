using UnityEditor;
using UnityEngine;
using System.IO;

public static class IOSBuild
{
    [MenuItem("Build/Build iOS")] // Это можно убрать, но оставим для удобства
    public static void BuildIos()
    {
        string buildPath = "build/iOS";

        // Убедимся, что путь для билда существует
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[]
            {
                "Assets/Scenes/MainScene.unity",
                "Assets/Scenes/Blackjack.unity",
                "Assets/Scenes/5CardPoker.unity",
                "Assets/Scenes/HighLo.unity"
            },
            locationPathName = buildPath,
            target = BuildTarget.iOS,
            options = BuildOptions.None
        };

        Debug.Log("🚀 Начинаем сборку iOS...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log($"✅ Сборка iOS завершена! Файлы находятся в {buildPath}");
    }
}