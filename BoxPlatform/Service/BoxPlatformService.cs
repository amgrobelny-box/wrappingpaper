using System;
using System.IO;
using Box.V2;
using Box.V2.Config;
using Box.V2.JWTAuth;
using BoxyQL.BoxPlatform.BoxCache;
using BoxyQL.BoxPlatform.Utilities;
using BoxyQL.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace BoxyQL.BoxPlatform.Service
{
  public class BoxPlatformService : IBoxPlatformService
  {
    public string JwtPrivateKey { get; private set; }

    public BoxConfig BoxPlatformConfig { get; private set; }

    public BoxJWTAuth BoxPlatformAuthorizedClient { get; private set; }

    public IBoxPlatformCache BoxPlatformCache { get; private set; }

    public BoxPlatformService(IHostingEnvironment hostingEnvironment, IOptions<BoxPlatformSettings> boxPlatformSettings, IBoxPlatformCache boxPlatformCache)
    {
      this.BoxPlatformCache = boxPlatformCache;
      boxPlatformSettings.Value.JwtPrivateKeyFilePath = (String.IsNullOrWhiteSpace(boxPlatformSettings.Value.JwtPrivateKeyFilePath)) ? "/private_key.pem" : boxPlatformSettings.Value.JwtPrivateKeyFilePath;
      if (!boxPlatformSettings.Value.JwtPrivateKeyFilePath.StartsWith("/"))
      {
        boxPlatformSettings.Value.JwtPrivateKeyFilePath = "/${this.BoxPlatformSettings.JwtPrivateKeyFilePath}";
      }
      this.JwtPrivateKey = File.ReadAllText(hostingEnvironment.ContentRootPath + boxPlatformSettings.Value.JwtPrivateKeyFilePath);
      this.BoxPlatformConfig = new BoxConfig(boxPlatformSettings.Value.ClientId, boxPlatformSettings.Value.ClientSecret, boxPlatformSettings.Value.EnterpriseId, this.JwtPrivateKey, boxPlatformSettings.Value.JwtPrivateKeyPassword, boxPlatformSettings.Value.JwtPublicKeyId);
      this.BoxPlatformAuthorizedClient = new BoxJWTAuth(this.BoxPlatformConfig);
    }

    public BoxClient AdminClient()
    {
      var token = EnterpriseToken();
      return this.BoxPlatformAuthorizedClient.AdminClient(token);
    }

    public BoxClient UserClient(string boxAppUserId)
    {
      var token = UserToken(boxAppUserId);
      return this.BoxPlatformAuthorizedClient.UserClient(token, boxAppUserId);
    }

    public string EnterpriseToken()
    {
      return this.BoxPlatformCache.GetToken(BoxTokenTypes.Enterprise, this.BoxPlatformConfig.EnterpriseId, () => { return this.BoxPlatformAuthorizedClient.AdminToken(); }).AccessToken;
    }

    public string UserToken(string boxAppUserId)
    {
      return this.BoxPlatformCache.GetToken(BoxTokenTypes.User, boxAppUserId, () => { return this.BoxPlatformAuthorizedClient.UserToken(boxAppUserId); }).AccessToken;
    }

    public IBoxCachedToken EnterpriseAccessTokenAndExpiration()
    {
      return this.BoxPlatformCache.GetToken(BoxTokenTypes.Enterprise, this.BoxPlatformConfig.EnterpriseId, () => { return this.BoxPlatformAuthorizedClient.AdminToken(); });
    }

    public IBoxCachedToken UserAccessTokenAndExpiration(string boxAppUserId)
    {
      return this.BoxPlatformCache.GetToken(BoxTokenTypes.User, boxAppUserId, () => { return this.BoxPlatformAuthorizedClient.UserToken(boxAppUserId); });
    }
  }
}