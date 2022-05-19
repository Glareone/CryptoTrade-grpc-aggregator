using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcServer;
using Microsoft.AspNetCore.WebUtilities;

namespace grpcServer.Services
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

        public override async Task Subscribe(SubscribeRequest request, IServerStreamWriter<HistoricalReply> responseStream, ServerCallContext context)
        {

            var i = 0;
            while (!context.CancellationToken.IsCancellationRequested && i < 20)
            {
                await Task.Delay(500); // Gotta look busy

                var historicalMessage = new HistoricalReply
                {
                    //Timestamp = Timestamp.FromDateTime(now.AddDays(i++)),
                    Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                    Open = GetPseudoDoubleWithinRange(32000, 38000),
                    Close = GetPseudoDoubleWithinRange(32000, 36000),
                    Low = GetPseudoDoubleWithinRange(32000, 34000),
                    High = GetPseudoDoubleWithinRange(38000, 40000),
                    VolumeBtc = GetPseudoDoubleWithinRange(1, 1000),
                    VolumeCur = GetPseudoDoubleWithinRange(5, 5000),
                    WeightedPrice = GetPseudoDoubleWithinRange(35000, 35300)
                };

                _logger.LogInformation("Sending WeatherData response");
                _logger.LogInformation($"Timestamp: {historicalMessage.Timestamp}");
                _logger.LogInformation($"Open: {historicalMessage.Open}");
                _logger.LogInformation($"Close: {historicalMessage.Close}");
                _logger.LogInformation($"Low: {historicalMessage.Low}");
                _logger.LogInformation($"High: {historicalMessage.High}");
                _logger.LogInformation($"WeightedPrice: {historicalMessage.WeightedPrice}");

                await responseStream.WriteAsync(historicalMessage);
                i++;
            }

            // return new Task(Task.FromResult(new HistoricalReply
            // {
            //     Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
            //     Open = GetPseudoDoubleWithinRange(32000, 38000),
            //     Close = GetPseudoDoubleWithinRange(32000, 36000),
            //     Low = GetPseudoDoubleWithinRange(32000, 34000),
            //     High = GetPseudoDoubleWithinRange(38000, 40000),
            //     VolumeBtc = GetPseudoDoubleWithinRange(1, 1000),
            //     VolumeCur = GetPseudoDoubleWithinRange(5, 5000),
            //     WeightedPrice = GetPseudoDoubleWithinRange(35000, 35300)
            // }));
        }
    }
}
