namespace HowManyTimesWeClimbedTogether;

internal static class MessageFormatter
{
    internal static string BuildClimbedWithMessage(string playerName, int count)
    {
        string body = HmtcLocalization.FormatClimbedWithMessage(playerName, count);
        return $"<color=#AAAAAA>{body}</color>";
    }
}
