using System;
using System.Collections.Generic;
using System.Linq;
using Box.V2.Models;
using BoxyQL.BoxPlatform.Service;
using GraphQL;
using GraphQL.Types;

namespace BoxyQL.GraphQL.Types
{
    public class BoxUserType : ObjectGraphType<BoxUser>
    {
        public BoxUserType()
        {
            Name = "User";
            Description = "A user in Box.";

            Field(u => u.Id);
            Field(u => u.Name, nullable: true);
            Field(u => u.Login);
        }
    }
}