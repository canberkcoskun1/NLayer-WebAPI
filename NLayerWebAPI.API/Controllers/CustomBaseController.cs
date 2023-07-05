using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayerWebAPI.Core.DTOs;

namespace NLayerWebAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomBaseController : ControllerBase
	{
		//CustomeResponseDto için baseController oluşturup Status code için action yazdık. Data dönsün diye <T> yazdık. Generic bir method yazdık. Endpoint olmadığını belli etmek için NonAction koyduk.
		[NonAction]
		public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
		{
			if (response.StatusCode == 204)
			{
				return new ObjectResult(null) 
				{
					StatusCode = response.StatusCode,
				};
			}

			return new ObjectResult(response)
			{
				StatusCode = response.StatusCode,
			};

		}
	}
}
