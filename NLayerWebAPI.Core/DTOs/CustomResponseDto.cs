using System.Text.Json.Serialization;

namespace NLayerWebAPI.Core.DTOs
{
	public class CustomResponseDto<T>
	{ 
		public T Data { get; set; }
		[JsonIgnore]
		//Clientlara dönmesin diye StatusCode ignore'ladık.
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }

		// Static metotlar oluşturup Success ve Fail nesneler oluştursun.
		public static CustomResponseDto<T> Success(int statusCode, T data)
		{
			return new CustomResponseDto<T> { Data = data, StatusCode = statusCode, Errors = null };
		}
		// Success illaki data dönmemize gerek yok, Update olsun Datayı doldurmamıza gerek yok sadece durum kodu verebiliriz.
		public static CustomResponseDto<T> Success(int statusCode)
		{
			return new CustomResponseDto<T> {  StatusCode = statusCode };
		}
		// Error (Birden fazla.) Static Factory Method
		public static CustomResponseDto<T> Fail(int statusCode ,List<string> errors) 
		{
			return new CustomResponseDto<T> { StatusCode = statusCode, Errors = errors };
		}
		// Static Factory Method
		public static CustomResponseDto<T> Fail(int statusCode, string error)
		{
			return new CustomResponseDto<T> { StatusCode = statusCode, Errors = new List<string> { error } };
		}
		
	}
}
