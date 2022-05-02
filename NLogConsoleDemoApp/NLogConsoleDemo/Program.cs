// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var logger = NLog.LogManager.GetCurrentClassLogger();
logger.Info("App Started {args} {username}",args,"fullstackamigo");
try
{

}
catch (Exception e)
{

    logger.Error(e, "This was unexpected! {args}", 2);
}
NLog.LogManager.Shutdown();
