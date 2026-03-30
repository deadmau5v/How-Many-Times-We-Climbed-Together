using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace HowManyTimesWeClimbedTogether;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;
    internal static SessionTracker Tracker { get; private set; } = null!;

    private void Awake()
    {
        Log = Logger;
        Tracker = new SessionTracker();

        var harmony = new Harmony(Id);
        harmony.PatchAll(typeof(PlayerConnectionLogPatches));

        SceneManager.sceneLoaded += OnSceneLoaded;

        Log.LogInfo($"Plugin {Name} v{Version} loaded!");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SessionDetector.OnSceneLoaded(scene.name);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
