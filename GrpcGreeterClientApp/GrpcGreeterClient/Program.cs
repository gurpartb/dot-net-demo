using Grpc.Net.Client;
using GrpcGreeterClient;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:7298");
var client = new Greeter.GreeterClient(channel);
var replyHello = await client.SayHelloAsync(
                  new HelloRequest { Name = "John Macroe" });
Console.WriteLine("Greeting: " + replyHello.Message);
var replyThankYou = await client.SayThankYouAsync(
                  new ThankYouRequest { Name = "Abby Winehouse" });
Console.WriteLine("Greeting: " + replyThankYou.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();