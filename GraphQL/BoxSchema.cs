using System;
using BoxyQL.BoxPlatform.Service;
using GraphQL.Types;

namespace BoxyQL.GraphQL
{
    public class BoxSchema : Schema
    {
        public BoxSchema() 
        {
            Query = new BoxQuery();
        }
    }
}