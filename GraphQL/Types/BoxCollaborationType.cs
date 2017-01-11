using Box.V2.Models;
using GraphQL.Types;

namespace BoxyQL.GraphQL.Types
{
    public class BoxCollaborationType : ObjectGraphType<BoxCollaboration>
    {
        public BoxCollaborationType()
        {
            Name = "Box Collaboration";
            Description = "A collaboration on a Box file or folder.";

            Field(c => c.Status);
            Field<BoxEntityType>("accessibleBy");
        }
    }
}