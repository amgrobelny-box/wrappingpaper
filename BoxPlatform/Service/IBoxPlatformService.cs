using Box.V2;
using Box.V2.Config;
using Box.V2.JWTAuth;
using BoxyQL.BoxPlatform.BoxCache;

namespace BoxyQL.BoxPlatform.Service
{
  public interface IBoxPlatformService
  {
    string JwtPrivateKey { get; }

    BoxConfig BoxPlatformConfig { get; }

    BoxJWTAuth BoxPlatformAuthorizedClient { get; }

    BoxClient AdminClient();

    BoxClient UserClient(string boxAppUserId);

    string EnterpriseToken();

    string UserToken(string boxAppUserId);

    IBoxCachedToken EnterpriseAccessTokenAndExpiration();

    IBoxCachedToken UserAccessTokenAndExpiration(string boxAppUserId);
  }
}