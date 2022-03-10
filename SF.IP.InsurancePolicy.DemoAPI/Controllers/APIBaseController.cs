using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace SF.IY.InsurancePolicy.DemoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class APIBaseController : ControllerBase
{
    private ISender _mediator;
    protected ISender Mediator
    {
        get
        {
            _mediator ??= HttpContext.RequestServices.GetService<ISender>();
            return _mediator;
        }
    }

    protected Dictionary<string, string> GetRequestHeaders()
    {
        Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
        foreach (var header in Request.Headers)
        {
            requestHeaders.Add(header.Key, header.Value);
        }
        return requestHeaders;
    }
}

