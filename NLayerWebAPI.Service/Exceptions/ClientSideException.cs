using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerWebAPI.Service.Exceptions
{
	public class ClientSideException : Exception
	{
		// Mesaj Olarak oluşturduk.
		public ClientSideException(string message) : base(message)
		{

		}

	}
}
