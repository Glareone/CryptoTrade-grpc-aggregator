using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcServer;

namespace grpcServer
{
    public class HistoricalFeedService : HistoricalFeed.HistoricalFeedBase
    {
        private readonly ILogger<HistoricalFeedService> _logger;
        public HistoricalFeedService(ILogger<HistoricalFeedService> logger)
        {
            _logger = logger;
        }

        public static double GetPseudoDoubleWithinRange(double lowerBound, double upperBound)
        {
            var random = new Random();
            var rDouble = random.NextDouble();
            var rRangeDouble = rDouble * (upperBound - lowerBound) + lowerBound;
            return rRangeDouble;
        }

        public override Task<HistoricalReply> Subscribe(SubscribeRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HistoricalReply
            {
                Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                Open = GetPseudoDoubleWithinRange(32000, 38000),
                Close = GetPseudoDoubleWithinRange(32000, 36000),
                Low = GetPseudoDoubleWithinRange(32000, 34000),
                High = GetPseudoDoubleWithinRange(38000, 40000),
                VolumeBtc = GetPseudoDoubleWithinRange(1, 1000),
                VolumeCur = GetPseudoDoubleWithinRange(5, 5000),
                WeightedPrice = GetPseudoDoubleWithinRange(35000, 35300)
            });
        }
    }
}
