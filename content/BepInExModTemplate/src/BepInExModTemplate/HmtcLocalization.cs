using Language = LocalizedText.Language;

namespace HowManyTimesWeClimbedTogether;

internal static class HmtcLocalization
{
    internal static Language CurrentLanguage => LocalizedText.CURRENT_LANGUAGE;

    internal static bool IsCjkLanguage(Language language) =>
        language is Language.SimplifiedChinese
            or Language.TraditionalChinese
            or Language.Japanese
            or Language.Korean;

    internal static bool IsCjkLanguage() => IsCjkLanguage(CurrentLanguage);

    internal static string GetStatsButtonLabel() =>
        CurrentLanguage switch
        {
            Language.French => "STATS D'ASCENSION",
            Language.Italian => "STATISTICHE SCALATA",
            Language.German => "KLETTERSTATISTIK",
            Language.SpanishSpain => "ESTADISTICAS DE ESCALADA",
            Language.SpanishLatam => "ESTADISTICAS DE ESCALADA",
            Language.BRPortuguese => "ESTATISTICAS DE ESCALADA",
            Language.Russian => "STATISTIKA VOSKHOZHDENIY",
            Language.Ukrainian => "STATYSTYKA SKHODZHEN",
            Language.SimplifiedChinese => "\u6500\u767B\u7EDF\u8BA1",
            Language.TraditionalChinese => "\u6500\u767B\u7D71\u8A08",
            Language.Japanese => "\u767B\u9802\u7D71\u8A08",
            Language.Korean => "\uB4F1\uBC18 \uD1B5\uACC4",
            Language.Polish => "STATYSTYKI WSPINACZKI",
            Language.Turkish => "TIRMANIS ISTATISTIKLERI",
            _ => "CLIMB STATS",
        };

    internal static string GetStatsTitle() =>
        CurrentLanguage switch
        {
            Language.French => "Avec qui ai-je deja grimpe ?",
            Language.Italian => "Con chi ho gia scalato?",
            Language.German => "Mit wem bin ich schon geklettert?",
            Language.SpanishSpain => "Con quien ya he escalado?",
            Language.SpanishLatam => "Con quien ya he escalado?",
            Language.BRPortuguese => "Com quem eu ja escalei?",
            Language.Russian => "S kem ya uzhe podnimalsya?",
            Language.Ukrainian => "Z kym ya vzhe pidimavsya?",
            Language.SimplifiedChinese => "\u6211\u548C\u8C01\u4E00\u8D77\u722C\u8FC7\uFF1F",
            Language.TraditionalChinese => "\u6211\u548C\u8AB0\u4E00\u8D77\u722C\u904E\uFF1F",
            Language.Japanese => "\u8AB0\u3068\u4E00\u7DD2\u306B\u767B\u3063\u305F\uFF1F",
            Language.Korean => "\uB204\uAD6C\uC640 \uD568\uAED8 \uC62C\uB790\uC744\uAE4C?",
            Language.Polish => "Z kim juz sie wspinalem?",
            Language.Turkish => "Kimlerle birlikte tirmandim?",
            _ => "Who have I climbed with?",
        };

    internal static string GetPlayerColumnLabel() =>
        CurrentLanguage switch
        {
            Language.French => "Joueur",
            Language.Italian => "Giocatore",
            Language.German => "Spieler",
            Language.SpanishSpain => "Jugador",
            Language.SpanishLatam => "Jugador",
            Language.BRPortuguese => "Jogador",
            Language.Russian => "Igrok",
            Language.Ukrainian => "Hravec",
            Language.SimplifiedChinese => "\u73A9\u5BB6",
            Language.TraditionalChinese => "\u73A9\u5BB6",
            Language.Japanese => "\u30D7\u30EC\u30A4\u30E4\u30FC",
            Language.Korean => "\uD50C\uB808\uC774\uC5B4",
            Language.Polish => "Gracz",
            Language.Turkish => "Oyuncu",
            _ => "Player",
        };

    internal static string GetClimbCountColumnLabel() =>
        CurrentLanguage switch
        {
            Language.French => "Fois",
            Language.Italian => "Volte",
            Language.German => "Male",
            Language.SpanishSpain => "Veces",
            Language.SpanishLatam => "Veces",
            Language.BRPortuguese => "Vezes",
            Language.Russian => "Raz",
            Language.Ukrainian => "Raziv",
            Language.SimplifiedChinese => "\u6B21\u6570",
            Language.TraditionalChinese => "\u6B21\u6578",
            Language.Japanese => "\u56DE\u6570",
            Language.Korean => "\uD69F\uC218",
            Language.Polish => "Razy",
            Language.Turkish => "Kez",
            _ => "Times",
        };

    internal static string FormatClimbCount(int count) =>
        CurrentLanguage switch
        {
            Language.French => $"Grimpe ensemble {count} fois",
            Language.Italian => $"Scalato insieme {count} volte",
            Language.German => $"{count} Mal zusammen geklettert",
            Language.SpanishSpain => $"Escalamos juntos {count} veces",
            Language.SpanishLatam => $"Escalamos juntos {count} veces",
            Language.BRPortuguese => $"Escalamos juntos {count} vez(es)",
            Language.Russian => $"Vmeste podnimalis {count} raz",
            Language.Ukrainian => $"Razom pidiimalys {count} raziv",
            Language.SimplifiedChinese => $"\u4E00\u8D77\u722C\u4E86 {count} \u6B21",
            Language.TraditionalChinese => $"\u4E00\u8D77\u722C\u4E86 {count} \u6B21",
            Language.Japanese => $"\u4E00\u7DD2\u306B\u767B\u3063\u305F\u306E\u306F {count} \u56DE",
            Language.Korean => $"\uD568\uAED8 \uC62C\uB780 \uD69F\uC218 {count}\uD68C",
            Language.Polish => $"Wspinaliscie sie razem {count} razy",
            Language.Turkish => $"Birlikte {count} kez tirmandiniz",
            _ => $"Climbed together {count} time{(count == 1 ? string.Empty : "s")}",
        };

    internal static string FormatTrackedPlayers(int count) =>
        CurrentLanguage switch
        {
            Language.French => $"Joueurs suivis: {count}",
            Language.Italian => $"Giocatori registrati: {count}",
            Language.German => $"Erfasste Spieler: {count}",
            Language.SpanishSpain => $"Jugadores registrados: {count}",
            Language.SpanishLatam => $"Jugadores registrados: {count}",
            Language.BRPortuguese => $"Jogadores rastreados: {count}",
            Language.Russian => $"Otslezhivaemye igroki: {count}",
            Language.Ukrainian => $"Vidstezhuvani hravtsi: {count}",
            Language.SimplifiedChinese => $"\u5DF2\u8BB0\u5F55\u73A9\u5BB6: {count}",
            Language.TraditionalChinese => $"\u5DF2\u8A18\u9304\u73A9\u5BB6: {count}",
            Language.Japanese => $"\u8A18\u9332\u6E08\u307F\u30D7\u30EC\u30A4\u30E4\u30FC: {count}",
            Language.Korean => $"\uAE30\uB85D\uB41C \uD50C\uB808\uC774\uC5B4: {count}",
            Language.Polish => $"Sledzeni gracze: {count}",
            Language.Turkish => $"Kayitli oyuncular: {count}",
            _ => $"Tracked players: {count}",
        };

    internal static string GetStatsHint() =>
        CurrentLanguage switch
        {
            Language.French => "Consultez l'historique des joueurs avec qui vous avez grimpe",
            Language.Italian => "Controlla lo storico dei giocatori con cui hai scalato",
            Language.German => "Hier siehst du die Spieler, mit denen du bereits geklettert bist",
            Language.SpanishSpain => "Consulta el historial de jugadores con los que ya escalaste",
            Language.SpanishLatam => "Consulta el historial de jugadores con los que ya escalaste",
            Language.BRPortuguese => "Veja o historico de jogadores com quem voce ja escalou",
            Language.Russian => "Zdes pokazana istoriya igrokov, s kotorymi vy uzhe podnimalis",
            Language.Ukrainian => "Tut pokazano istoriyu hravtsiv, z yakymy vy vzhe pidimalys",
            Language.SimplifiedChinese => "\u8FD9\u91CC\u4F1A\u663E\u793A\u4F60\u4E00\u8D77\u722C\u8FC7\u5C71\u7684\u73A9\u5BB6",
            Language.TraditionalChinese => "\u9019\u88E1\u6703\u986F\u793A\u4F60\u4E00\u8D77\u722C\u904E\u5C71\u7684\u73A9\u5BB6",
            Language.Japanese => "\u3053\u3053\u3067\u4E00\u7DD2\u306B\u767B\u3063\u305F\u30D7\u30EC\u30A4\u30E4\u30FC\u3092\u78BA\u8A8D\u3067\u304D\u307E\u3059",
            Language.Korean => "\uC5EC\uAE30\uC11C \uD568\uAED8 \uC62C\uB790\uB358 \uD50C\uB808\uC774\uC5B4 \uAE30\uB85D\uC744 \uBCFC \uC218 \uC788\uC2B5\uB2C8\uB2E4",
            Language.Polish => "Tutaj zobaczysz graczy, z ktorymi juz sie wspinales",
            Language.Turkish => "Burada birlikte tirmandigin oyuncularin kaydini gorebilirsin",
            _ => "See the players you have already climbed with",
        };

    internal static string GetNoRecordsText() =>
        CurrentLanguage switch
        {
            Language.French => "Aucun historique pour le moment",
            Language.Italian => "Nessun dato registrato per ora",
            Language.German => "Noch keine Eintraege vorhanden",
            Language.SpanishSpain => "Todavia no hay registros",
            Language.SpanishLatam => "Todavia no hay registros",
            Language.BRPortuguese => "Ainda nao ha registros",
            Language.Russian => "Poka net zapisey",
            Language.Ukrainian => "Poki shcho nemaie zapysiv",
            Language.SimplifiedChinese => "\u8FD8\u6CA1\u6709\u7EDF\u8BA1\u8BB0\u5F55",
            Language.TraditionalChinese => "\u9084\u6C92\u6709\u7D71\u8A08\u8A18\u9304",
            Language.Japanese => "\u307E\u3060\u8A18\u9332\u304C\u3042\u308A\u307E\u305B\u3093",
            Language.Korean => "\uC544\uC9C1 \uAE30\uB85D\uC774 \uC5C6\uC2B5\uB2C8\uB2E4",
            Language.Polish => "Nie ma jeszcze zadnych wpisow",
            Language.Turkish => "Henuz kayit yok",
            _ => "No climbing records yet",
        };

    internal static string FormatClimbedWithMessage(string playerName, int count) =>
        CurrentLanguage switch
        {
            Language.French => $"Vous avez gravi avec {playerName} {count} fois",
            Language.Italian => $"Hai scalato con {playerName} {count} volte",
            Language.German => $"Du bist mit {playerName} {count} Mal geklettert",
            Language.SpanishSpain => $"Has escalado con {playerName} {count} veces",
            Language.SpanishLatam => $"Has escalado con {playerName} {count} veces",
            Language.BRPortuguese => $"Voce escalou com {playerName} {count} vez(es)",
            Language.Russian => $"Vy podnimalis s {playerName} {count} raz",
            Language.Ukrainian => $"Vy pidiimalys z {playerName} {count} raziv",
            Language.SimplifiedChinese => $"\u4F60\u548C {playerName} \u4E00\u8D77\u722C\u4E86 {count} \u6B21\u5C71",
            Language.TraditionalChinese => $"\u4F60\u548C {playerName} \u4E00\u8D77\u722C\u4E86 {count} \u6B21\u5C71",
            Language.Japanese => $"{playerName} \u3068\u4E00\u7DD2\u306B\u767B\u3063\u305F\u56DE\u6570: {count}",
            Language.Korean => $"{playerName} \uB2D8\uACFC \uD568\uAED8 \uB4F1\uBC18\uD55C \uD69F\uC218: {count}",
            Language.Polish => $"Wspinaliscie sie z {playerName} {count} razy",
            Language.Turkish => $"{playerName} ile {count} kez tirmandin",
            _ => $"You've climbed with {playerName} {count} time{(count == 1 ? string.Empty : "s")}",
        };
}
