using System;
using System.Collections.Generic;
using System.Linq;
using Box.V2.Models;
using BoxyQL.BoxPlatform.Service;
using GraphQL;
using GraphQL.Types;

namespace BoxyQL.GraphQL.Types
{
    public class BoxItemType : ObjectGraphType<BoxItem>
    {
        public BoxItemType()
        {
            Name = "Item";
            Description = "An item in Box. Type could be file, folder, or web_link";

            Field(i => i.Id);
            Field(i => i.Type);
            Field(i => i.Name, nullable: true);

            FieldAsync<ListGraphType<BoxCommentType>>("comments", resolve: async (context) =>
            {
                if (context.Source.Type != "file")
                {
                    return new List<BoxComment>();
                }

                var userContext = context.UserContext.As<GraphQLUserContext>();
                BoxCollection<BoxComment> comments;

                try
                {
                    comments = await userContext.client.FilesManager.GetCommentsAsync(context.Source.Id);
                    return comments.Entries;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return new List<BoxComment>();
                }
            });
        }
    }
}