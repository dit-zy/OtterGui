using ImGuiNET;
using OtterGui.Text.HelperObjects;

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

namespace OtterGui.Text;

public static unsafe partial class ImUtf8
{
    /// <summary> Display a text-based tooltip only when hovering over the last item. </summary>
    /// <param name="text"> The given text as a UTF8 string. Does not have to be null-terminated. </param>
    /// <param name="flags"> Flags to pass to the hover-check. </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void HoverTooltip(ReadOnlySpan<byte> text, ImGuiHoveredFlags flags = ImGuiHoveredFlags.None)
    {
        if (text.Length == 0 || text[0] == 0 || !ImGui.IsItemHovered(flags))
            return;

        using var tt = Tooltip();
        Text(text);
    }

    /// <param name="text"> The given text as a UTF16 string. </param>
    /// <inheritdoc cref="HoverTooltip(ReadOnlySpan{byte},ImGuiHoveredFlags)"/>>
    /// <exception cref="ImUtf8FormatException" />
    /// <remarks> The text is not transcoded when the tooltip is not displayed. </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void HoverTooltip(ReadOnlySpan<char> text, ImGuiHoveredFlags flags = ImGuiHoveredFlags.None)
    {
        if (text.Length == 0 || text[0] == '\0' || !ImGui.IsItemHovered(flags))
            return;

        using var tt = Tooltip();
        Text(text);
    }

    /// <param name="text"> The given text as a format string. </param>
    /// <remarks> The format string is not evaluated when the tooltip is not displayed. </remarks>
    /// <inheritdoc cref="HoverTooltip(ReadOnlySpan{char},ImGuiHoveredFlags)"/>>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void HoverTooltip(ImGuiHoveredFlags flags, [InterpolatedStringHandlerArgument(nameof(flags))] ref HoverUtf8StringHandler text)
    {
        if (!text.IsHovered)
            return;

        if (!text.GetEnd(out var end))
            throw new ImUtf8FormatException();

        if (end == text.Begin || *text.Begin == 0)
            return;

        using var tt = Tooltip();
        ImGuiNative.igTextUnformatted(text.Begin, end);
    }

    /// <inheritdoc cref="HoverTooltip(ImGuiHoveredFlags, ref HoverUtf8StringHandler)"/>>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void HoverTooltip(ref HoverUtf8StringHandler text)
        => HoverTooltip(ImGuiHoveredFlags.None, ref text);
}