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

        string playerName = SessionDetector.GetBestDisplayName(newPlayer);
        string message = MessageFormatter.BuildClimbedWithMessage(playerName, count);
        __instance.AddMessage(message);
    }
}
