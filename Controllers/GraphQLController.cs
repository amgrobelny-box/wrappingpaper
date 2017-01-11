using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BoxyQL.BoxPlatform.Service;
using BoxyQL.GraphQL;
using GraphQL;
using GraphQL.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoxyQL.Controllers
{
  [Route("api/[controller]")]
  public class GraphQLController : Controller
  {
    private DocumentExecuter _executer = new DocumentExecuter();
    private BoxSchema _schema;
    private GraphQLUserContext _user;

    public GraphQLController(GraphQLUserContext user)
    {
      this._schema = new BoxSchema();
      this._user = user;
    }

    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }

    [HttpPost]
    public async Task<ExecutionResult> Post([FromBody] GraphQLRequest request)
    {
      var inputs = request.Variables.ToInputs();
      var result = await _executer.ExecuteAsync(_ =>
      {
        _.Schema = _schema;
        _.Query = request.Query;
        _.Inputs = inputs;
        _.UserContext = this._user;
      }).ConfigureAwait(false);
      
      return result;
    }
  }

  public class GraphQLRequest
  {
    public string Query { get; set; }
    public string Variables { get; set; }
  }
}