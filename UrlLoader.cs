namespace ProfileSummaryDemo
{
    public static class UrlLoader
    {
        // This method will return the content of a web page.
        public static async Task<string> Get(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                var page = await client.GetStringAsync(url);
                return page;
            }
            catch
            {
                return null;
            }
        }
    }
}
