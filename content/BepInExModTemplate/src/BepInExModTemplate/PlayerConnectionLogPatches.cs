using HarmonyLib;
using PhotonPlayer = Photon.Realtime.Player;

namespace HowManyTimesWeClimbedTogether;

internal static class PlayerConnectionLogPatches
{
    [HarmonyPatch(typeof(PlayerConnectionLog), nameof(PlayerConnectionLog.OnPlayerEnteredRoom))]
    [HarmonyPostfix]
    static void OnPlayerEnteredRoom_Postfix(PlayerConnectionLog __instance, PhotonPlayer newPlayer)
    {
        if (newPlayer.IsLocal)
            return;

        SessionDetector.OnPlayerJoinedDuringExpedition(newPlayer);

        string playerId = SessionDetector.GetStablePlayerId(newPlayer);
        int count = Plugin.Tracker.GetSessionCount(playerId);

        if (count <= 0)
            return;

        string message = $"<color=#AAAAAA>You've climbed with {newPlayer.NickName} {count} time{(count > 1 ? "s" : "")}</color>";
        __instance.AddMessage(message);
    }
}
