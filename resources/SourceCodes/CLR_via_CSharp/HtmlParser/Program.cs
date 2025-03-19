using System.Collections.Concurrent;

internal interface HtmlParser
{
    IList<string> GetUrls(string html);
}

internal class Solution
{
    public IList<string> Crawl(string startUrl, HtmlParser htmlParser)
    {
        ConcurrentDictionary<string, bool> visited = new();

        visited.TryAdd(startUrl, true);

        var hostSpan = startUrl.AsSpan();
        var schemeEnd = hostSpan.IndexOf("//") + 2;
        var pathStart = hostSpan.Slice(schemeEnd).IndexOf('/');
        var host = pathStart == -1 ? hostSpan.Slice(schemeEnd) : hostSpan.Slice(schemeEnd, pathStart);

        string hostName = host.ToString();
        Task.Run(() => FetchUrls(hostName, startUrl, htmlParser, visited)).Wait();

        return visited.Keys.ToList();
    }

    private void FetchUrls(string host, string url, HtmlParser htmlParser, ConcurrentDictionary<string, bool> visited)
    {
        string[] urls = htmlParser.GetUrls(url).ToArray();
        Span<string> urlSpan = urls.AsSpan();

        var tasks = new List<Task>();

        foreach (string checkUrl in urlSpan)
        {
            ReadOnlySpan<char> checkHost = checkUrl.AsSpan();
            int schemeEnd = checkHost.IndexOf("//") + 2;
            int pathStart = checkHost.Slice(schemeEnd).IndexOf('/');
            var hostSpan = pathStart == -1 ? checkHost.Slice(schemeEnd) : checkHost.Slice(schemeEnd, pathStart);

            if (!hostSpan.SequenceEqual(host.AsSpan()) || !visited.TryAdd(checkUrl, true))
            {
                continue;
            }

            tasks.Add(Task.Run(() => FetchUrls(host, checkUrl, htmlParser, visited)));
        }

        Task.WaitAll(tasks.ToArray());
    }
}