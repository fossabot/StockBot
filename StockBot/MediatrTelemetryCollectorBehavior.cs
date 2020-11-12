﻿using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Application.Services;
using MediatR;

namespace StockBot
{
	public class MediatrTelemetryCollectorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ITelemetryService _telemetryService;

		public MediatrTelemetryCollectorBehavior(ITelemetryService telemetryService)
		{
			_telemetryService = telemetryService;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			var stopWatch = Stopwatch.StartNew();
			var response = await next();
			stopWatch.Stop();
			if (request is null)
				return response;

			_telemetryService.TrackMediatrRequest(request.GetType().Name, stopWatch.ElapsedMilliseconds);

			return response;
		}
	}
}