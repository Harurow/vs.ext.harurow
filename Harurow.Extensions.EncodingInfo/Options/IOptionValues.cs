namespace Harurow.Extensions.EncodingInfo.Options
{
    internal interface IOptionValues
    {
        bool IsEnabledRecommendUtf8Bom { get; }
        bool IsEnabledWarningOtherEncoding { get; }
        bool IsEnabledAutoHide { get; }
    }
}