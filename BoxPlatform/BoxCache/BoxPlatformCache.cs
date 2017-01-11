using System;
using BoxyQL.BoxPlatform.Utilities;

namespace BoxyQL.BoxPlatform.BoxCache
{
  public abstract class BoxPlatformCache : IBoxPlatformCache
  {
    public const string CACHE_PREFIX = "box_platform";

    public const string ENTERPRISE = "enterprise";

    public const string USER = "user";

    public const string EXPIRES_AT = "expires_at";

    public string AccessTokenFieldName { get; private set; } = "access_token";

    public const string CACHE_DELIMITER = "|";

    protected static readonly TimeSpan TOKEN_EXPIRATION_PERIOD = TimeSpan.FromMinutes(45);

    public abstract IBoxCachedToken GetToken(BoxTokenTypes tokenType, string tokenId, Func<string> generateToken);

    protected string ConstructCacheKey(string tokenType, string tokenId)
    {
      return CACHE_PREFIX + CACHE_DELIMITER + tokenType + CACHE_DELIMITER + tokenId;
    }
  }
}