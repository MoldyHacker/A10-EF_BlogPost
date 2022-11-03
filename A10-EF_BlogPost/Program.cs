using ApplicationTemplate;
using ApplicationTemplate.Services;
using Microsoft.Extensions.DependencyInjection;

namespace A10_EF_BlogPost
{
    public class Program
    {
        /// <summary>
        /// Entry point for the MainService.  It is unlikely you will need to update this.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            try
            {
                var startup = new Startup();
                var serviceProvider = startup.ConfigureServices();
                var service = serviceProvider.GetService<IMainService>();

                service?.Invoke();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }