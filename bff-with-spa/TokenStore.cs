namespace bffwithspa
{
    public static class TokenStore
    {
        private static readonly Dictionary<string, string> _tokenCache = new();

        public static void SaveAccessToken(string userId, string accessToken)
        {
            _tokenCache[userId] = accessToken;
        }

        public static string? GetAccessToken(string userId)
        {
            return _tokenCache.TryGetValue(userId, out var token) ? token : null;
        }
    }
}
