using System.Threading.Tasks;
using Box.V2.Models;
using BoxyQL.BoxPlatform.Service;
using BoxyQL.GraphQL.Types;
using GraphQL;
using GraphQL.Types;

namespace BoxyQL.GraphQL
{
  public class BoxQuery : ObjectGraphType<object>
  {
    public BoxQuery()
    {
      Name = "Query";

      FieldAsync<BoxFolderType>(
          "folder",
          arguments: new QueryArguments(
              new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the folder" }
            ),
            resolve: async (context) =>
            {
              var userContext = context.UserContext.As<GraphQLUserContext>();
              return await userContext.client.FoldersManager.GetInformationAsync(context.GetArgument<string>("id"));
            }
          );
    }
  }
}