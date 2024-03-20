using Microsoft.CodeAnalysis;

namespace _2_uzduotis_c_sharp.Repo
{
    public static class TokenRepo
    {
        private static readonly Dictionary<string, string> _tokens = new();

        public static void SetToken(string platform, string token)
        {
            _tokens[platform] = token;
        }

        public static string GetToken(string platform)
        {
            if (_tokens.TryGetValue(platform, out var token))
            {
                return token;
            }
            return null;
        }

        public static void RemoveToken(string platform)
        {
            _tokens.Remove(platform);
        }
    }
}
