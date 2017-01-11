using Box.V2.Models;
using GraphQL.Types;

namespace BoxyQL.GraphQL.Types
{
    public class BoxEntityType : ObjectGraphType<BoxEntity>
    {
        public BoxEntityType()
        {
            Field(e => e.Id);
            Field(e => e.Type);
        }
    }
}