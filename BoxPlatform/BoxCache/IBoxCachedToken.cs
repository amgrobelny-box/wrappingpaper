namespace BoxyQL.BoxPlatform.BoxCache
{
  public interface IBoxCachedToken
  {
    string AccessToken { get; set; }

    string ExpiresAt { get; set; }
  }
}