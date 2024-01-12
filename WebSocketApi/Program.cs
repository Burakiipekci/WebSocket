using System.Net;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseWebSockets();
app.Map("/ws", async context =>
{
	if (context.WebSockets.IsWebSocketRequest)
	{
	using var ws= await context.WebSockets.AcceptWebSocketAsync();
		while (true)
		{
			var message = "Burak Test Time  :" + DateTime.Now.ToString("HH:mm:ss"); 
		var bytes= Encoding.UTF8.GetBytes(message);
		var arraySegment=new ArraySegment<byte>(bytes,0,bytes.Length);
			if (ws.State==WebSocketState.Open)
			{
				await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
			} else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
			{
				break;
			}
			Thread.Sleep(1000);
		}
		
		

	}
	else
	{
		context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
	}
});


app.UseAuthorization();

app.MapControllers();

await app.RunAsync("https://localhost:6969");

