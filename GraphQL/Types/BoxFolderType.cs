using System;
using System.Collections.Generic;
using System.Linq;
using Box.V2.Models;
using BoxyQL.BoxPlatform.Service;
using GraphQL;
using GraphQL.Types;

namespace BoxyQL.GraphQL.Types
{
    public class BoxFolderType : ObjectGraphType<BoxFolder>
    {
        public BoxFolderType()
        {
            Name = "Folder";
            Description = "A folder in Box.";

            Field(d => d.Id).Description("The id of the folder.");
            Field(d => d.Name, nullable: true).Description("The name of the folder.");

            FieldAsync<ListGraphType<BoxMetadataType>>(
              "metadata",
              arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "template", Description = "template key for metadata template" }
                  ),
              resolve: async (context) =>
              {
                  var template = context.GetArgument<string>("template");
                  var userContext = context.UserContext.As<GraphQLUserContext>();
                  Dictionary<string, object> metadata;
                  try
                  {
                      metadata = await userContext.client.MetadataManager.GetFolderMetadataAsync(context.Source.Id, "enterprise", template);
                      var list = new List<BoxMetadata>();

                      foreach (var key in metadata.Keys)
                      {
                          list.Add(new BoxMetadata
                          {
                              Field = key,
                              Value = metadata[key].ToString()
                          });
                      }
                      return list;
                  }
                  catch (Exception e)
                  {
                      System.Console.WriteLine(e.Message);
                      return new List<BoxMetadata>();
                  }
              });

            FieldAsync<ListGraphType<BoxCollaborationType>>("collaborations", resolve: async (context) =>
            {
                var userContext = context.UserContext.As<GraphQLUserContext>();
                BoxCollection<BoxCollaboration> collabs;
                try
                {
                    collabs = await userContext.client.FoldersManager.GetCollaborationsAsync(context.Source.Id);
                    return collabs.Entries;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return new List<BoxCollaboration>();
                }
            });

            FieldAsync<ListGraphType<BoxItemType>>("items", resolve: async (context) =>
            {
                var userContext = context.UserContext.As<GraphQLUserContext>();
                BoxCollection<BoxItem> items;

                try
                {
                    items = await userContext.client.FoldersManager.GetFolderItemsAsync(context.Source.Id, 100, autoPaginate: true);
                    return items.Entries;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    return new List<BoxItem>();
                }
            });
        }
    }
}