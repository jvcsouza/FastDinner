using System;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastDinner.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FastDinner.Api.Controllers
{
    public class EnumController : ApiController
    {
        public EnumController(ISender mediator, IUnitOfWork unitOfWork) : base(mediator, unitOfWork)
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
