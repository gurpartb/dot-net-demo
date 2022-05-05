using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<ThankYouReply> SayThankYou(ThankYouRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ThankYouReply
            {
                Message = "My Pleasure " + request.Name
            });
        }
    }
}