using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos;

public class Response<T> where T : class
{
	public T Data { get; set; }
	public int StatusCode { get; private set; }
	public ErrorDto Error { get; set; }


	// Daha tez response-un ugurlu olub olmamasini yoxlamaq
	[JsonIgnore]
	public bool IsSuccessfull { get; set; }


	public static Response<T> Success(T data, int statusCode) => new Response<T> { Data = data, StatusCode = statusCode, IsSuccessfull = true };
	public static Response<T> Success(int statusCode) => new Response<T> { Data = default, StatusCode = statusCode, IsSuccessfull = true };
	public static Response<T> Fail(ErrorDto dto, int statusCode) => new Response<T> { Error = dto, StatusCode = statusCode, IsSuccessfull = false };
	public static Response<T> Fail(string errorMessage, int statutCode, bool isShow)
	{
		var errorDto = new ErrorDto(errorMessage, isShow);
		return new Response<T> { Error = errorDto, StatusCode = statutCode, IsSuccessfull = false };
	}

}
