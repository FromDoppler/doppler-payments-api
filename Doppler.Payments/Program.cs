using System;
using Doppler.Payments.Models;
using Doppler.Payments.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<RaftClient>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["Raft:BaseUrl"]);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/tokens", async (TokenRequest req, RaftClient client) => await client.CreateTokenAsync(req));
app.MapPost("/verified-tokens", async (VerifiedTokenRequest req, RaftClient client) => await client.VerifyTokenAsync(req));
app.MapPost("/payments", async (PaymentRequest req, RaftClient client) => await client.MakePaymentAsync(req));
app.MapGet("/tokens/{tokenId}", async (string tokenId, RaftClient client) => await client.GetTokenAsync(tokenId));
app.MapDelete("/tokens/{tokenId}", async (string tokenId, RaftClient client) => { await client.DeleteTokenAsync(tokenId); return Results.NoContent(); });

app.Run();
