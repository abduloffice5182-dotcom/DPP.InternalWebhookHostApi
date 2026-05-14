namespace DPP.InternalWebhookHost.Api.Controllers;

public class BaseController : ControllerBase
{
	public BaseController() { }

	protected IActionResult Success(
		 string message = "Success")
	{
		return Ok(new ApiResponse<object>
		(
		 message, null, null
		));
	}

	protected IActionResult Success<T>(
		T data,
		string? message = null)
	{
		return Ok(new ApiResponse<T>
		(
			message, data,null
		));
	}

	protected IActionResult Failure(
		string errorMessage)
	{
		return BadRequest(new ApiResponse<object>
		(
			null,
			null,
			errorMessage
		));
	}
}
