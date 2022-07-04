﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

using CruzeMob.Models;

namespace CruzeMob.Data
{
    public class CruzeServices
    {
		IRestService restService;

		public CruzeServices(IRestService service)
		{
			restService = service;
		}

		public Task<LoginResponse> LoginAsync()
		{
			return restService.LoginAsync();
		}

		public Task<IdentifyResponse> IdentifyAsync(IdentifyRequest request)
		{
			return restService.IdentifyAsync(request);
		}
	}
}

