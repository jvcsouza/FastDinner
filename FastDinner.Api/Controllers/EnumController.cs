using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using FastDinner.Domain;
using MediatR;

namespace FastDinner.Api.Controllers
{
    public class EnumController : ApiController
    {
        public EnumController(ISender mediator) 
            : base(mediator)
        {

        }

        [HttpGet("StatusOrder")]
        public async Task<IActionResult> GetStatusOrder()
        {
            var status = Enum.GetValues<OrderPrepareStatus>()
                .Select(x => new { Key = (int)x, Value = x.ToString() });

            //var status = await Task.FromResult(Enum.GetNames(typeof(OrderPrepareStatus)));

            return Ok(await Task.FromResult(status));
        }
    }
}
