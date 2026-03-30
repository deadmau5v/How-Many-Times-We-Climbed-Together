using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using PEAKLib.Core;
using PEAKLib.UI;
using UnityEngine.SceneManagement;

namespace HowManyTimesWeClimbedTogether;

[BepInDependency(CorePlugin.Id)]
[BepInDependency(UIPlugin.Id)]
[BepInPlugin("com.github.d5v.HowManyTimesWeClimbedTogether", "How Many Times We Climbed Together", "0.1.0")]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;
    internal static SessionTracker Tracker { get; private set; } = null!;

    private void Awake()
    {
        Log = Logger;

        try
        {
            Tracker = new SessionTracker();
        }
        catch (Exception ex)
        {
            Log.LogError($"[HMTC] Failed to initialize SessionTracker: {ex}");
            return;
        }

        try
        {
            var harmony = new Harmony("com.github.d5v.HowManyTimesWeClimbedTogether");

            var original = typeof(PlayerConnectionLog).GetMethod(
                "OnPlayerEnteredRoom",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );

            if (original == null)
            {
                Log.LogError("[HMTC] Could not find PlayerConnectionLog.OnPlayerEnteredRoom method!");
                foreach (var m in typeof(PlayerConnectionLog).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    Log.LogWarning($"[HMTC] Method: {m.Name}");
                }
                return;
            }

            Log.LogInfo($"[HMTC] Found target method: {original.Name}");

            var postfix = typeof(PlayerConnectionLogPatches).GetMethod(
                "OnPlayerEnteredRoom_Postfix",
                BindingFlags.Static | BindingFlags.NonPublic
            );

            harmony.Patch(original, postfix: new HarmonyMethod(postfix));
        }
        catch (Exception ex)
        {
            Log.LogError($"[HMTC] Failed to apply Harmony patch: {ex}");
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        StatsMenuUi.Register();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SessionDetector.OnSceneLoaded(scene.name);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StatsMenuUi.Dispose();
    }
}
