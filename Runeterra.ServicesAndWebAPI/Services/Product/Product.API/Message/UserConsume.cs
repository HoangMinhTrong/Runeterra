using System.Diagnostics;
using MassTransit;
using Product.API.Entity;

namespace Product.API.Message;

public class UserConsume : IConsumer<ApplicationUser>
{

    public async Task Consume(ConsumeContext<ApplicationUser> context)
    {
        var data = context.Message;
        Console.WriteLine(data);
    }

}