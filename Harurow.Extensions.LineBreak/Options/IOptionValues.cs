namespace Harurow.Extensions.LineBreak.Options
{
    internal interface IOptionValues
    {
        LineBreakMode VisibleLineBreakMode { get; }
        LineBreakMode LineBreakWarningMode { get; }
    }
}