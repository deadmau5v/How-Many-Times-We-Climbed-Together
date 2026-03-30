using System.Collections.Generic;
using Photon.Pun;
using PhotonPlayer = Photon.Realtime.Player;

namespace HowManyTimesWeClimbedTogether;

internal static class SessionDetector
{
    private const string LobbyScene = "Airport";

    private static bool _wasInLobby;
    private static bool _inExpedition;
    private static readonly HashSet<string> _countedPlayersThisSession = new();

    internal static void OnSceneLoaded(string sceneName)
    {
        if (sceneName == LobbyScene)
        {
            _wasInLobby = true;
            _inExpedition = false;
            _countedPlayersThisSession.Clear();
            Plugin.Log.LogDebug("Entered lobby (Airport). Session tracking reset.");
            return;
        }

        if (!_wasInLobby || _inExpedition)
            return;

        if (!PhotonNetwork.InRoom)
            return;

        _inExpedition = true;
        RecordAllCurrentPlayers();
    }

    private static void RecordAllCurrentPlayers()
    {
        PhotonPlayer localPlayer = PhotonNetwork.LocalPlayer;
        PhotonPlayer[] allPlayers = PhotonNetwork.PlayerList;

        Plugin.Log.LogInfo($"New expedition started! Room: {PhotonNetwork.CurrentRoom.Name}, Players: {allPlayers.Length}");

        foreach (PhotonPlayer player in allPlayers)
        {
            TryCountPlayer(player);
        }

        Plugin.Tracker.Save();
    }

    internal static void OnPlayerJoinedDuringExpedition(PhotonPlayer newPlayer)
    {
        if (!_inExpedition)
            return;

        if (TryCountPlayer(newPlayer))
            Plugin.Tracker.Save();
    }

    private static bool TryCountPlayer(PhotonPlayer player)
    {
        if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            return false;

        string playerId = GetStablePlayerId(player);
        string playerName = player.NickName ?? "Unknown";

        if (!_countedPlayersThisSession.Add(playerId))
        {
            Plugin.Log.LogDebug($"  Player {playerName} already counted this session, skipping (reconnect/duplicate).");
            return false;
        }

        Plugin.Tracker.IncrementSession(playerId, playerName);
        Plugin.Log.LogInfo(
            $"  Climbed with {playerName} - Total: {Plugin.Tracker.GetSessionCount(playerId)} times"
        );
        return true;
    }

    internal static string GetStablePlayerId(PhotonPlayer player)
    {
        if (!string.IsNullOrEmpty(player.UserId))
            return player.UserId;

        return $"nick:{player.NickName ?? player.ActorNumber.ToString()}";
    }
}
