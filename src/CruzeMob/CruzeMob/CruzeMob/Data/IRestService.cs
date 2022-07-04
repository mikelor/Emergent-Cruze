using System.Collections.Generic;
using System.Threading.Tasks;
using CruzeMob.Models;

namespace CruzeMob.Data
{
	public interface IRestService
	{
		Task<LoginResponse> LoginAsync();

		Task <IdentifyResponse> IdentifyAsync(IdentifyRequest request);

	}
}