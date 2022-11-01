using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_DotNet_6_WithJWT.DBContext;
using WebAPI_DotNet_6_WithJWT.Models;

namespace WebAPI_DotNet_6_WithJWT.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly JWT_DBContext _context;
        public CustomerController(JWT_DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<TblCustomer> Get()
        {
            return _context.TblCustomers.ToList();
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CustomerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
