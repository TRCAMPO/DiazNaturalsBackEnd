using BACK_END_DIAZNATURALS.Model;
using BACK_END_DIAZNATURALS.Services;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSqlServer<DiazNaturalsContext>(builder.Configuration.GetConnectionString("DefaultConnection"));


var credeentials = GoogleCredential.FromJson(@"{
  ""type"": ""service_account"",
  ""project_id"": ""diaznaturals-e056b"",
  ""private_key_id"": ""03a87a40ba7e6f32cbee523431eb5fe3a0bebef4"",
  ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDP8U4IaDSurrQk\nB0tiTXvJ4Qv56ETmRiuNZLG0p0U4jtBPbaGIsh6IExhIrP7YuIRlV3WtQnVusfJy\n4csnjuX5K+Ufq/l0C0o3rtCZ9+hYZNtJ+9MyDLqgw9flktSOAVE6ZiL2QlsLqz57\n6DyAMEQwga9jNYHB9rF6HRA8A4V3zE9SWK1TgE/qzynCLTJf5FiUsVlwJubH7mSw\nSNyyZWhrA8GUlNyPlT4Oh9jYW2gNA4aHRUfV+U/qeiH032vKxCWPcfvoKV2xZP18\ncw8w1i4Cp+S4LI5PmQQv0xWkBHmpEY8FmdhUH7qV/nmQnNRF3xplEf5B7F6mnycF\nS+tb5pBJAgMBAAECggEAFKAEO8Z9OjnTHZM5JY8WqQmYvuEMS3RYj8nV6/ue3Ksi\nq4wt1mGHcHsHYLcDAJM9s5ETF5w0ymXdTMqLVFlCw34Yd/WedKWFhyQgZfMuUS0w\nyIPxOODFFprSRF+11D96c3LLwzYF8pB5zj+014iiDNciIrKdFBRwWlK9aFx3jbAQ\n8MQI1bjAcB35Qdksm81h2PvCriLxyYxz62REIgRGXuwBDds3IpXlJF81FD6aLP4P\nkSjolsGUh8ek1e++lbdo2NWiKrAuJv6Eff5wO9OjWP6UDaHB0hV+JHPlUqfxvFYA\n4SrWl5a9aWDV4AZUsAmntD/9iRp14DSCJMeTPSKx8QKBgQD02vWuba1Ap+Hgms+T\niTbIvSdiIoVQQFYnyyJkNtoKhetjGHySA/pc4sg+NjzXeWMND+5kW/GCQtOQoR3S\npP8slNf9wirEzPKUXfqTntuzZoxDjbB6nBnSx60ZIqXuuRfyqjRpQRFTgHU+gLS/\nXvNLWbRg+salyGUtLc8njzEo2QKBgQDZaD2BMald+2g5AlRNpdvncYPERxL1P9hp\no7vbU3i5QlU4fb+ImFqVd678kvrGNjFZlRf+ukp3Y3GD3rk5RdOt69vJLSS9imW9\naVvgkhEyGMVOdvAtj9pz4aa4pwcq26n9qf/uMZRK81y1donnuc9XMB8WrNt6Ns6N\nQhn8/8Z88QKBgC5NTZIriUylspAPlls24lY5dr4W+xC+6cvinOFIghjlVCrMEZpn\nCCwScn1ZMk4o1TM+JP1zaYsRagJ6hTI1I0/h4apJ0l6exuyJjP3nV3JoXPPVUl8N\nL9DtE22iYLBw7fdkej7BIC6jJwinvAZIRUelcfe41GG7MG7Hr9myOUCZAoGBAL9F\n+c6/nY4FEYYOqZMDGH0AnvhBu8kv2bvfhPiK8My1MnsYDzojKJcWDEtFGQLoTPTO\nuqSuy7NMN2PYwUdFSt5agyz03b0wlSBXILFscVqM9pJ5DAhZ5s1LOz58HU80odN+\nDKBI2Ho0sF0qpFvOW0API1r0y8gzjfgzrt+rPLtRAoGAPZLpY+eHAPPOnr9pNfSg\nt8l94ULp524BhG3jyU9fDaVVxlMvK22HOlxxYmWBSa8xoRXwlFH9typW2fnoLB/Q\n3r3iiG8KtyXTDRyUXz9K6AVE0FuL65Yeu5EnfQRbrFkGRH1HKC2+GIprBsREpX70\ns2d8Ncns2QpHcCnsyAAvEiM=\n-----END PRIVATE KEY-----\n"",
  ""client_email"": ""firebase-adminsdk-glax3@diaznaturals-e056b.iam.gserviceaccount.com"",
  ""client_id"": ""115367257596424236856"",
  ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
  ""token_uri"": ""https://oauth2.googleapis.com/token"",
  ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
  ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-glax3%40diaznaturals-e056b.iam.gserviceaccount.com"",
  ""universe_domain"": ""googleapis.com""
}");

FirebaseApp appp = FirebaseApp.Create(new AppOptions
{
Credential = credeentials,
});
try
{
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    string apiKey = configuration["Firebase:ApiKey"];
    string authDomain = configuration["Firebase:AuthDomain"];
    string projectId = configuration["Firebase:ProjectId"];
    string storageBucket = configuration["Firebase:StorageBucket"];
    var storage = StorageClient.Create(credeentials);
}
catch (Exception ex) { Console.WriteLine("Excepción no controlada: " + ex.ToString()); }



builder.Services.AddScoped<FirebaseStorageService>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.IgnoreNullValues = true;
});



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(optionss =>
{
    optionss.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes((builder.Configuration["Jwt:Key"])))
    };
});



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Inserte Bearer",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});


var app = builder.Build();


app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DiazNaturals");
    });
}



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
