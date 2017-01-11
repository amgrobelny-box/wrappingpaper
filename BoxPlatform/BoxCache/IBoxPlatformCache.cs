using System;
using BoxyQL.BoxPlatform.Utilities;

namespace BoxyQL.BoxPlatform.BoxCache
{
  public interface IBoxPlatformCache
  {
    string AccessTokenFieldName { get; }
    IBoxCachedToken GetToken(BoxTokenTypes tokenType, string tokenId, Func<string> generateToken);
  }
}