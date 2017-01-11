using System.Collections.Generic;
using GraphQL.Types;

namespace BoxyQL.GraphQL.Types
{
  public class BoxMetadataType : ObjectGraphType
  {
    public BoxMetadataType()
    {
      Field<StringGraphType>("field");
      Field<StringGraphType>("value");
    }
  }

  public class BoxMetadata
  {
    public string Field { get; set; }
    public string Value { get; set; }
  }
}