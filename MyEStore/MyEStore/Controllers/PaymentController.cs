using Microsoft.AspNetCore.Mvc;
using MyEStore.Models;

namespace MyEStore.Controllers
{
	public class PaymentController : Controller
	{
		private readonly PaypalClient _paypalClient;

		public PaymentController(PaypalClient paypalClient)
		{
			_paypalClient = paypalClient;
		}

		[HttpPost("/payment/create-paypal-order")]
		public async Task<IActionResult> CreateOrder([FromBody] Amount amount)
		{
			//số tiền có thể lấy từ Session hoặc dưới client gửi lên
			//var value = "1234";
			var value = amount.value;
			//var currency_code = "USD";
			var currency_code = amount.currency_code;

			//Lưu đơn hàng vô database ==> refId chính là mã đơn hàng
			var refId = DateTime.Now.Ticks.ToString();

			try
			{
				var response = await _paypalClient.CreateOrder(value, currency_code, refId);
				return Ok(response);
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}

		[HttpPost("/payment/capture-paypal-order")]
		public IActionResult CaptureOrder(string orderID)
		{
			try
			{
				var response = _paypalClient.CaptureOrder(orderID);


				//Cập nhật trạng thái đơn hàng vô database


				return Ok(response);
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}
	}
}
