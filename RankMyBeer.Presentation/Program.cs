using Amazon;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using RankMyBeerApplication;
using RankMyBeerInfrastructure;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;

builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (!builder.Environment.IsDevelopment())
{
    builder.Configuration.AddSecretsManager(region: RegionEndpoint.USEast2,
        configurator: options =>
        {
            options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}_");
            options.KeyGenerator = (_, s) => s
                .Replace($"{env}_{appName}_", string.Empty)
                .Replace("__", ":");
            options.PollingInterval = TimeSpan.FromSeconds(10);
        });
}

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var projectId = builder.Configuration["Firebase:ProjectId"];
        options.Authority = $"https://securetoken.google.com/{projectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{projectId}",
            ValidateAudience = true,
            ValidAudience = projectId,
            ValidateLifetime = true
        };
    });

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors(x => x.AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
