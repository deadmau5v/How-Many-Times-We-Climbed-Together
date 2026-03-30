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
        PhotonPlayer[] allPlayers = PhotonNetwork.PlayerList;

        foreach (PhotonPlayer player in allPlayers)
            TryCountPlayer(player);

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
        string playerName = GetBestDisplayName(player);

        if (!_countedPlayersThisSession.Add(playerId))
            return false;

        Plugin.Tracker.IncrementSession(playerId, playerName);
        return true;
    }

    internal static string GetStablePlayerId(PhotonPlayer player)
    {
        if (!string.IsNullOrEmpty(player.UserId))
            return player.UserId;

        return $"nick:{player.NickName ?? player.ActorNumber.ToString()}";
    }

    internal static string GetBestDisplayName(PhotonPlayer player)
    {
        string playerId = GetStablePlayerId(player);
        string storedName = Plugin.Tracker.GetLastKnownName(playerId);
        if (!string.IsNullOrWhiteSpace(storedName))
            return storedName;

        if (!string.IsNullOrWhiteSpace(player.NickName))
            return player.NickName;

        if (!string.IsNullOrWhiteSpace(player.UserId))
            return player.UserId;

        return $"Player {player.ActorNumber}";
    }
}
