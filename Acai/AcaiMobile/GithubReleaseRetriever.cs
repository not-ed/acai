using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace AcaiMobile;

public class GithubReleaseRetriever : IReleaseRetriever
{
    private string _lastException = null;
    
    public async Task<AcaiRelease> CheckForNewReleases()
    {
        _lastException = null;
        try
        {
            var mostRecentRelease = await GetMostRecentRelease();
            if (mostRecentRelease == null || !ReleaseHasNewerVersion(mostRecentRelease))
            {
                return null;
            }

            return new AcaiRelease()
            {
                Version = mostRecentRelease.Version,
                ReleasePageUrl = mostRecentRelease.ReleasePageUrl,
                DirectDownloadUrl = mostRecentRelease.ReleaseAssets.FirstOrDefault(x => x.ContentType == "application/vnd.android.package-archive")?.DirectDownloadUrl,
            };
        }
        catch (Exception e)
        {
            _lastException = e.GetType().Name;
            return null;
        }
    }
    
    public string GetExceptionMessage()
    {
        return _lastException;
    }

    private bool ReleaseHasNewerVersion(GithubReleaseResponseItem release)
    {
        var releaseVersion = Version.Parse(release.Version.ToLower().Replace("v",""));
        return releaseVersion.Major > AppInfo.Version.Major || releaseVersion.Minor > AppInfo.Version.Minor || releaseVersion.Build > AppInfo.Version.Build;
    }

    private async Task<GithubReleaseResponseItem> GetMostRecentRelease()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.github.com/repos/not-ed/acai/releases"),
            Headers =
            {
                { "User-Agent", $"Acai/{AppInfo.VersionString} ({DeviceInfo.Current.Platform.ToString()} {DeviceInfo.Current.VersionString})" },
            },
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadFromJsonAsync<List<GithubReleaseResponseItem>>();
            return body.OrderByDescending(x => x.PublishTime).FirstOrDefault();
        }
    }
    
    private class GithubReleaseResponseItem()
    {
        [JsonPropertyName("tag_name")] public string Version { get; set; }
        [JsonPropertyName("html_url")] public string ReleasePageUrl { get; set; }
        [JsonPropertyName("assets")] public List<GithubReleaseAsset> ReleaseAssets { get; set; }
        [JsonPropertyName("published_at")] public DateTime PublishTime { get; set; }
    }

    private class GithubReleaseAsset()
    {
        [JsonPropertyName("content_type")] public string ContentType { get; set; }
        [JsonPropertyName("browser_download_url")] public string DirectDownloadUrl { get; set; }
    }
}