namespace AcaiMobile;

public interface IReleaseRetriever
{
    public Task<AcaiRelease> CheckForNewReleases();
    public string GetExceptionMessage();
}

public class AcaiRelease
{
    public string Version { get; set; }
    public string ReleasePageUrl { get; set; }
    public string DirectDownloadUrl { get; set; }
}