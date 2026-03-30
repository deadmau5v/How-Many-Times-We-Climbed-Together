using System.Collections.Generic;
using Photon.Pun;
using PhotonPlayer = Photon.Realtime.Player;

namespace HowManyTimesWeClimbedTogether;

internal static class SessionDetector
{
    private const string LobbyScene = "Airport";

    private static bool _inExpedition;
    private static readonly HashSet<string> _countedPlayersThisSession = new();

    internal static void Update(string sceneName)
    {
        if (sceneName == LobbyScene)
        {
            _inExpedition = false;
            _countedPlayersThisSession.Clear();
            return;
        }

        if (!PhotonNetwork.InRoom)
        {
            return;
        }

        if (!_inExpedition)
        {
            _inExpedition = true;
            RecordAllCurrentPlayers();
            return;
        }

        if (HasUncountedRemotePlayers())
            RecordAllCurrentPlayers();
    }

    internal static void OnSceneLoaded(string sceneName)
    {
        Update(sceneName);
    }

    private static void RecordAllCurrentPlayers()
    {
        PhotonPlayer[] allPlayers = PhotonNetwork.PlayerList;
        bool anyCounted = false;

        foreach (PhotonPlayer player in allPlayers)
        {
            if (TryCountPlayer(player))
                anyCounted = true;
        }

        if (anyCounted)
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

    private static bool HasUncountedRemotePlayers()
    {
        foreach (PhotonPlayer player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                continue;

            string playerId = GetStablePlayerId(player);
            if (!_countedPlayersThisSession.Contains(playerId))
                return true;
        }

        return false;
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
