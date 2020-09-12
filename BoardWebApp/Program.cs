using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace BoardApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //using (var scope = host.Services.CreateScope())
            //{
            //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            //    await userManager.CreateAsync(new IdentityUser { Id = "Z^kwGY", UserName = "igor" }, "123");
            //    await userManager.CreateAsync(new IdentityUser { Id = "uCBu#f", UserName = "zepsen" }, "123");
            //    await userManager.CreateAsync(new IdentityUser { Id = "R#&vdU", UserName = "marina" }, "123");
            //    await userManager.CreateAsync(new IdentityUser { Id = "IweH&n", UserName = "yulia" }, "123");
            //    await userManager.CreateAsync(new IdentityUser { Id = "WxEE#k", UserName = "stas" }, "123");
            //    await userManager.CreateAsync(new IdentityUser { Id = "CwQQ2%", UserName = "katiya" }, "123");
            //    await userManager.CreateAsync(new IdentityUser { Id = "mdI0@@", UserName = "sasha" }, "123");
            //    await userManager.CreateAsync(new IdentityUser { Id = "pc*Nc@", UserName = "aliona" }, "123");
            //    await userManager.CreateAsync(new IdentityUser { Id = "a$M*Mc", UserName = "vika" }, "123");
            //    await userManager.CreateAsync(new IdentityUser { Id = "FaJ^h1", UserName = "gleb" }, "123");
            //}

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
