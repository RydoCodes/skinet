using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // 404 NOT FOUND: handle error related to endpoint that does not exist
    // 401 Error:  If the token is not valided in the code nser service.AddAuthentication(JWT Stuff) then 401 error is created from here.
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            var statuscoderesult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (code)
            {
                case 404:
                    return new ObjectResult(new ApiResponse(code, $"404 Error Occurred. This Path {statuscoderesult.OriginalPath} with Query String Parameters {statuscoderesult.OriginalQueryString} was not available"));
                default:
                    return new ObjectResult(new ApiResponse(code));

            }
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using API.Errors;
//using Microsoft.AspNetCore.Mvc;

//namespace API.Controllers
//{
//    // 404 NOT FOUND: handle error related to endpoint that does not exist
//    // 401 Error:  If the token is not valided in the code nser service.AddAuthentication(JWT Stuff) then 401 error is created from here.
//    [Route("errors/{code}")]
//    [ApiExplorerSettings(IgnoreApi = true)]
//    public class ErrorController : BaseApiController
//    {
//        public IActionResult Error(int code)
//        {
//            return new ObjectResult(new ApiResponse(code));
//        }
//    }
//}