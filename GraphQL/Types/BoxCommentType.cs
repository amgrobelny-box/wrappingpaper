using System;
using System.Collections.Generic;
using System.Linq;
using Box.V2.Models;
using BoxyQL.BoxPlatform.Service;
using GraphQL;
using GraphQL.Types;

namespace BoxyQL.GraphQL.Types
{
    public class BoxCommentType : ObjectGraphType<BoxComment>
    {
        public BoxCommentType()
        {
            Name = "Comment";
            Description = "A comment on an item in Box.";

            Field(c => c.Id);
            Field(c => c.Type);
            Field(c => c.Message, nullable: true);
            //Field(c => c.CreatedBy);
        }
    }
}