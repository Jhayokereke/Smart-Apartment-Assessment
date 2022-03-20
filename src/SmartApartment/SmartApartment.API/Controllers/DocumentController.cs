using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartApartment.Application.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartApartment.API.Controllers
{
    [ApiController]
    [Route("api/v1/documents")]
    public class DocumentController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public DocumentController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [SwaggerOperation(Summary = "fetch documents matching a given word or phrase")]
        [SwaggerResponse(200, Type = typeof(SearchResponse))]
        [HttpGet("search")]
        public async Task<ActionResult<SearchResponse>> Search([FromQuery] SearchRequest request)
        {
            return await _mediatR.Send(request);
        }
    }
}
