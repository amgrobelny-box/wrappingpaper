using Box.V2;
using BoxyQL.BoxPlatform.Service;

namespace BoxyQL.GraphQL
{
    public class GraphQLUserContext
    {
        private readonly IBoxPlatformService _boxService;
        public readonly BoxClient client;
        public GraphQLUserContext(IBoxPlatformService boxService)
        {
            this._boxService = boxService;
            this.client = boxService.AdminClient();
        }
    }
}