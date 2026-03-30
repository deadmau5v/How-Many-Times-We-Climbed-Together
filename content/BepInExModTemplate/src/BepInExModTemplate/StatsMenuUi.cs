using System.Collections.Generic;
using System.Linq;
using PEAKLib.UI;
using PEAKLib.UI.Elements;
using pworld.Scripts.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;
using Zorro.UI;

namespace HowManyTimesWeClimbedTogether;

internal static class StatsMenuUi
{
    private static readonly List<GameObject> Rows = new();
    private static bool _registered;
    private static bool _subscribedToLanguageChanged;
    private static PauseMenuHandler? _pauseMenuHandler;
    private static PauseMenuMainPage? _parentPage;
    private static PeakChildPage? _statsPage;
    private static PeakMenuButton? _openButton;
    private static PeakMenuButton? _backButton;
    private static RectTransform? _rowsContent;
    private static TextMeshProUGUI? _titleText;
    private static TextMeshProUGUI? _summaryText;
    private static TextMeshProUGUI? _hintText;
    private static TextMeshProUGUI? _emptyText;
    private static TMP_FontAsset? _vanillaFont;
    private static TMP_FontAsset? _cjkFont;

    internal static void Register()
    {
        if (_registered)
            return;

        _registered = true;
        MenuAPI.AddToPauseMenu(BuildPauseMenu);

        if (_subscribedToLanguageChanged)
            return;

        LocalizedText.OnLangugageChanged += OnLanguageChanged;
        _subscribedToLanguageChanged = true;
    }

    internal static void Dispose()
    {
        if (!_subscribedToLanguageChanged)
            return;

        LocalizedText.OnLangugageChanged -= OnLanguageChanged;
        _subscribedToLanguageChanged = false;
    }

    private static void BuildPauseMenu(Transform parent)
    {
        if (_statsPage != null && _statsPage)
            return;

        _pauseMenuHandler = parent.GetComponentInParent<PauseMenuHandler>();
        _parentPage = parent.GetComponent<PauseMenuMainPage>();

        if (_pauseMenuHandler == null || _parentPage == null)
        {
            Plugin.Log.LogError("[HMTC] Failed to build stats page because pause menu references were missing.");
            return;
        }

        EnsureFonts();

        _statsPage = MenuAPI.CreateChildPage("HMTC_StatsPage", _parentPage)
            .CreateBackground(new Color(0f, 0f, 0f, 0.8667f));
        _statsPage.SetOnOpen(RefreshPage);

        BuildHeader();
        BuildContent();
        BuildButtons(parent);

        _statsPage.gameObject.SetActive(false);
        RefreshStaticText();
    }

    private static void BuildHeader()
    {
        if (_statsPage == null)
            return;

        var headerContainer = new GameObject("Header")
            .ParentTo(_statsPage)
            .AddComponent<PeakElement>()
            .SetAnchorMinMax(new Vector2(0.5f, 1f))
            .SetPosition(new Vector2(0f, -42f))
            .SetPivot(new Vector2(0.5f, 1f))
            .SetSize(new Vector2(900f, 120f));

        _titleText = CreateText(
            headerContainer.transform,
            "Title",
            54,
            FontStyles.Normal,
            TextAlignmentOptions.Center
        );
        StretchToParent(_titleText.rectTransform);
        _titleText.fontSizeMax = 54;
        _titleText.fontSizeMin = 30;
        _titleText.enableAutoSizing = true;
    }

    private static void BuildContent()
    {
        if (_statsPage == null)
            return;

        var content = new GameObject("Content")
            .AddComponent<PeakElement>()
            .ParentTo(_statsPage)
            .SetPivot(new Vector2(0.5f, 1f))
            .SetAnchorMinMax(new Vector2(0.5f, 1f))
            .SetPosition(new Vector2(0f, -150f))
            .SetSize(new Vector2(1420, 840));

        var panel = new GameObject("StatsPanel")
            .AddComponent<PeakElement>()
            .ParentTo(content)
            .ExpandToParent();

        panel.gameObject.AddComponent<Image>().color = new Color(0.11f, 0.09f, 0.08f, 0.76f);

        _summaryText = CreateText(
            panel.transform,
            "Summary",
            32,
            FontStyles.Normal,
            TextAlignmentOptions.Center
        );
        ConfigureTopText(_summaryText.rectTransform, new Vector2(90f, -18f), new Vector2(-90f, -62f));

        _hintText = CreateText(
            panel.transform,
            "Hint",
            22,
            FontStyles.Normal,
            TextAlignmentOptions.Center
        );
        _hintText.color = new Color(0.88f, 0.84f, 0.76f, 0.92f);
        ConfigureTopText(_hintText.rectTransform, new Vector2(90f, -68f), new Vector2(-90f, -104f));

        var divider = new GameObject("Divider")
            .AddComponent<PeakElement>()
            .ParentTo(panel)
            .SetAnchorMin(new Vector2(0, 1))
            .SetAnchorMax(new Vector2(1, 1))
            .SetPivot(new Vector2(0.5f, 1f))
            .SetPosition(new Vector2(0f, -122f))
            .SetSize(new Vector2(-120f, 2f));
        divider.gameObject.AddComponent<Image>().color = new Color(0.78f, 0.63f, 0.35f, 0.9f);

        var scrollableContent = MenuAPI.CreateScrollableContent("StatsRows")
            .ParentTo(panel)
            .SetAnchorMin(new Vector2(0f, 0f))
            .SetAnchorMax(new Vector2(1f, 1f))
            .SetOffsetMin(new Vector2(56f, 110f))
            .SetOffsetMax(new Vector2(-56f, -146f));

        _rowsContent = scrollableContent.Content;

        _emptyText = CreateText(
            panel.transform,
            "Empty",
            30,
            FontStyles.Normal,
            TextAlignmentOptions.Center
        );
        _emptyText.color = new Color(0.94f, 0.90f, 0.82f, 0.95f);
        _emptyText.rectTransform.anchorMin = new Vector2(0f, 0f);
        _emptyText.rectTransform.anchorMax = new Vector2(1f, 1f);
        _emptyText.rectTransform.offsetMin = new Vector2(120f, 160f);
        _emptyText.rectTransform.offsetMax = new Vector2(-120f, -170f);
    }

    private static void BuildButtons(Transform parent)
    {
        if (_statsPage == null || _pauseMenuHandler == null || _parentPage == null)
            return;

        _backButton = MenuAPI.CreateMenuButton("Back")
            .SetLocalizationIndex("BACK")
            .SetColor(new Color(0.5189f, 0.1297f, 0.1718f))
            .ParentTo(_statsPage)
            .SetAnchorMinMax(new Vector2(0.5f, 0f))
            .SetPivot(new Vector2(0.5f, 0f))
            .SetPosition(new Vector2(0f, 36f))
            .SetWidth(220f)
            .OnClick(() => _pauseMenuHandler.TransistionToPage(_parentPage, new SetActivePageTransistion()));

        _statsPage.SetBackButton(_backButton.Button);

        _openButton = MenuAPI.CreatePauseMenuButton("CLIMB STATS")
            .ParentTo(parent)
            .OnClick(() =>
            {
                RefreshPage();
                _pauseMenuHandler.TransistionToPage(_statsPage, new SetActivePageTransistion());
            });

        PositionOpenButton(_openButton);
    }

    private static void PositionOpenButton(PeakMenuButton button)
    {
        RectTransform rectTransform = button.RectTransform;
        rectTransform.anchorMin = new Vector2(0.5f, 0f);
        rectTransform.anchorMax = new Vector2(0.5f, 0f);
        rectTransform.pivot = new Vector2(0.5f, 0f);
        rectTransform.sizeDelta = new Vector2(MenuAPI.OPTIONS_WIDTH, rectTransform.sizeDelta.y);
        rectTransform.anchoredPosition = new Vector2(0f, 36f);
    }

    private static void RefreshPage()
    {
        RefreshStaticText();
        RefreshRows();
    }

    private static void RefreshStaticText()
    {
        EnsureFonts();

        if (_titleText != null)
        {
            _titleText.text = HmtcLocalization.GetStatsTitle();
            ApplyFont(_titleText, preferCjk: HmtcLocalization.IsCjkLanguage());
        }

        if (_summaryText != null)
        {
            _summaryText.text = HmtcLocalization.FormatTrackedPlayers(Plugin.Tracker.GetAllRecords().Count);
            ApplyFont(_summaryText, preferCjk: HmtcLocalization.IsCjkLanguage());
        }

        if (_hintText != null)
        {
            _hintText.text = HmtcLocalization.GetStatsHint();
            ApplyFont(_hintText, preferCjk: HmtcLocalization.IsCjkLanguage());
        }

        if (_emptyText != null)
        {
            _emptyText.text = HmtcLocalization.GetNoRecordsText();
            ApplyFont(_emptyText, preferCjk: HmtcLocalization.IsCjkLanguage());
        }

        RefreshButtonText();
    }

    private static void RefreshRows()
    {
        if (_rowsContent == null)
            return;

        foreach (GameObject row in Rows)
            Object.Destroy(row);

        Rows.Clear();

        var records = Plugin.Tracker
            .GetAllRecords()
            .Values
            .OrderByDescending(record => record.SessionCount)
            .ThenBy(record => record.LastKnownName)
            .ToList();

        bool hasRows = records.Count > 0;
        if (_emptyText != null)
            _emptyText.gameObject.SetActive(!hasRows);

        foreach (PlayerRecord record in records)
        {
            string displayName = string.IsNullOrWhiteSpace(record.LastKnownName)
                ? record.PlayerId
                : record.LastKnownName;

            GameObject row = CreateRow(displayName, record.SessionCount, Rows.Count);
            row.transform.SetParent(_rowsContent, false);
            Rows.Add(row);
        }
    }

    private static GameObject CreateRow(string playerName, int count, int index)
    {
        GameObject row = new("Row", typeof(RectTransform), typeof(Image), typeof(LayoutElement));
        row.GetComponent<Image>().color = index % 2 == 0
            ? new Color(0.20f, 0.15f, 0.11f, 0.72f)
            : new Color(0.16f, 0.12f, 0.09f, 0.72f);

        LayoutElement layout = row.GetComponent<LayoutElement>();
        layout.preferredHeight = 72f;
        layout.minHeight = 72f;

        TextMeshProUGUI playerText = CreateText(
            row.transform,
            "PlayerName",
            30,
            FontStyles.Normal,
            TextAlignmentOptions.Left
        );
        ConfigureTopText(playerText.rectTransform, new Vector2(24f, -10f), new Vector2(-240f, -58f));
        playerText.text = playerName;
        ApplyFont(playerText, preferCjk: ContainsCjk(playerName));

        TextMeshProUGUI countText = CreateText(
            row.transform,
            "Count",
            24,
            FontStyles.Normal,
            TextAlignmentOptions.Right
        );
        ConfigureTopText(countText.rectTransform, new Vector2(820f, -12f), new Vector2(-24f, -58f));
        countText.text = HmtcLocalization.FormatClimbCount(count);
        countText.enableAutoSizing = true;
        countText.fontSizeMin = 18f;
        countText.fontSizeMax = 24f;
        ApplyFont(countText, preferCjk: HmtcLocalization.IsCjkLanguage());

        return row;
    }

    private static void RefreshButtonText()
    {
        if (_openButton != null)
        {
            _openButton.SetText(HmtcLocalization.GetStatsButtonLabel());
            ApplyFont(_openButton.Text, preferCjk: HmtcLocalization.IsCjkLanguage());
        }

        if (_backButton != null)
            ApplyFont(_backButton.Text, preferCjk: HmtcLocalization.IsCjkLanguage());
    }

    private static void OnLanguageChanged()
    {
        RefreshStaticText();
        RefreshRows();
    }

    private static TextMeshProUGUI CreateText(
        Transform parent,
        string objectName,
        float fontSize,
        FontStyles fontStyle,
        TextAlignmentOptions alignment
    )
    {
        GameObject textObject = new(
            objectName,
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(TextMeshProUGUI)
        );
        textObject.transform.SetParent(parent, false);

        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.font = _vanillaFont;
        text.fontSize = fontSize;
        text.fontStyle = fontStyle;
        text.alignment = alignment;
        text.color = Color.white;
        text.overflowMode = TextOverflowModes.Ellipsis;
        text.textWrappingMode = TextWrappingModes.NoWrap;
        return text;
    }

    private static void EnsureFonts()
    {
        if (_vanillaFont == null)
        {
            _vanillaFont = Templates.ButtonTemplate?
                .GetComponentInChildren<TextMeshProUGUI>(true)?
                .font;

            _vanillaFont ??= Resources
                .FindObjectsOfTypeAll<TextMeshProUGUI>()
                .Select(text => text.font)
                .FirstOrDefault(font => font != null);
        }

        _cjkFont ??= CreateCjkFontAsset();
    }

    private static TMP_FontAsset? CreateCjkFontAsset()
    {
        string[] candidates =
        [
            "Microsoft YaHei UI",
            "Microsoft YaHei",
            "SimHei",
            "Microsoft JhengHei UI",
            "Microsoft JhengHei",
            "Malgun Gothic",
            "Yu Gothic UI"
        ];

        foreach (string fontName in candidates)
        {
            try
            {
                Font osFont = Font.CreateDynamicFontFromOSFont(fontName, 20);
                if (osFont == null)
                    continue;

                return TMP_FontAsset.CreateFontAsset(
                    osFont,
                    20,
                    4,
                    GlyphRenderMode.SDFAA,
                    1024,
                    1024,
                    AtlasPopulationMode.Dynamic,
                    true
                );
            }
            catch
            {
            }
        }

        return null;
    }

    private static void ApplyFont(TMP_Text text, bool preferCjk)
    {
        if (preferCjk && _cjkFont != null)
            text.font = _cjkFont;
        else if (_vanillaFont != null)
            text.font = _vanillaFont;
    }

    private static void StretchToParent(RectTransform rectTransform)
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    private static void ConfigureTopText(
        RectTransform rectTransform,
        Vector2 offsetMin,
        Vector2 offsetMax
    )
    {
        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(1f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);
        rectTransform.offsetMin = new Vector2(offsetMin.x, offsetMax.y);
        rectTransform.offsetMax = new Vector2(offsetMax.x, offsetMin.y);
    }

    private static bool ContainsCjk(string text) => text.Any(character => character >= 0x2E80);
}
