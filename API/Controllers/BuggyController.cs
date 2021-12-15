using API.Errors;
using API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
         _context=context;   
        }
        
        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest() // Originally : 404 Not Found
        {
            var thing = _context.Products.Find(42); // this will return null as there is no product with this id.

            if(thing==null)
            {
                //return NotFound(); // Without any custom httpresponse generatr
                return NotFound(new ApiResponse(404));
            }
            return Ok();
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = _context.Products.Find(42); // this will be null

            var thingToReturn = thing.ToString(); // 500 Internal Server Error - this will give null reference error.

            return Ok();
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()  // Originally : 400 Bad Request
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetNotFoundRequest(int id) //400 - Generate a validation kind of error by passing string instead of an integer.
        {
            return Ok();
        }
    }
}