using LibDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using WebAPIBiblioteca.DTOs;
using WebAPIBiblioteca.Repositories;
using WebAPIBiblioteca.Services;

namespace WebAPIBiblioteca
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // -------- Serilog --------
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            // -------- Connection string --------
            DALPro.ConnectionString = builder.Configuration.GetConnectionString("Biblioteca");
            if (string.IsNullOrEmpty(DALPro.ConnectionString))
                throw new Exception("Connection string Biblioteca não definida");

            // -------- CORS --------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("cors", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // -------- JWT Authentication --------
            var secret_key = builder.Configuration["App:JWT:SECRET_KEY"];
            var key = Encoding.UTF8.GetBytes(secret_key);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "biblioteca",
                        ValidAudience = "biblioteca",
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            builder.Services.AddAuthorization();

            // -------- Swagger com JWT --------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.FullName.Replace("+", "."));

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter 'Bearer {token}'"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // -------- Dependency Injection --------
            builder.Services.AddScoped<AuthService>();

            builder.Services.AddScoped<ITemaRepository, TemaRepository>();
            builder.Services.AddScoped<ITemaService, TemaService>();

            builder.Services.AddScoped<INucleoRepository, NucleoRepository>();
            builder.Services.AddScoped<INucleoService, NucleoService>();

            builder.Services.AddScoped<ILeitorRepository, LeitorRepository>();
            builder.Services.AddScoped<ILeitorService, LeitorService>();

            builder.Services.AddScoped<IObraRepository, ObraRepository>();
            builder.Services.AddScoped<IObraService, ObraService>();

            builder.Services.AddScoped<IExemplarRepository, ExemplarRepository>();
            builder.Services.AddScoped<IExemplarService, ExemplarService>();

            builder.Services.AddScoped<IRequisicaoRepository, RequisicaoRepository>();
            builder.Services.AddScoped<IRequisicaoService, RequisicaoService>();

            builder.Services.AddScoped<IExemplarRequisicaoRepository, ExemplarRequisicaoRepository>();
            builder.Services.AddScoped<IExemplarRequisicaoService, ExemplarRequisicaoService>();

            // -------- Build --------
            var app = builder.Build();

            app.UseCors("cors");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthentication();
            app.UseAuthorization();

            // ====================================
            // ENDPOINTS
            // ====================================

            app.MapGet("/", () => "WebAPI Biblioteca XPTO");

            // -------- Login --------
            app.MapPost("/login", (LoginDTO login, AuthService auth, ILogger<Program> logger) =>
            {
                if (login.Username == "admin" && login.Password == "123")
                {
                    var token = auth.GenerateToken(login.Username, secret_key);
                    logger.LogInformation($"Login bem-sucedido: {login.Username}");
                    return Results.Ok(new { token });
                }

                logger.LogWarning($"Tentativa de login falhada: {login.Username}");
                return Results.Unauthorized();
            });

            // -------- DEV-only --------
#if DEV_MODE
            app.MapGet("/getModel", (string models) =>
            {
                List<string> list = models?
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(m => m.Trim())
                    .ToList();

                return DevService.GetModel(list);
            });
#endif

            // -------- TEMAS --------
            app.MapGet("/temas", (ITemaService s) => s.GetAll())
               .RequireAuthorization();

            app.MapGet("/temas/{id}", (int id, ITemaService s) =>
            {
                var t = s.GetById(id);
                return t == null ? Results.NotFound() : Results.Ok(t);
            }).RequireAuthorization();

            app.MapPost("/temas", (TemaCreateDTO dto, ITemaService s) =>
            {
                int id = s.Create(dto);
                return Results.Created($"/temas/{id}", new { id });
            }).RequireAuthorization();

            app.MapPut("/temas/{id}", (int id, TemaCreateDTO dto, ITemaService s) =>
            {
                s.Update(id, dto);
                return Results.Ok();
            }).RequireAuthorization();

            app.MapDelete("/temas/{id}", (int id, ITemaService s) =>
            {
                s.Delete(id);
                return Results.Ok();
            }).RequireAuthorization();

            // -------- NUCLEOS --------
            app.MapGet("/nucleos", (INucleoService s) => s.GetAll())
               .RequireAuthorization();

            app.MapGet("/nucleos/{id}", (int id, INucleoService s) =>
            {
                var n = s.GetById(id);
                return n == null ? Results.NotFound() : Results.Ok(n);
            }).RequireAuthorization();

            app.MapPost("/nucleos", (NucleoCreateDTO dto, INucleoService s) =>
            {
                int id = s.Create(dto);
                return Results.Created($"/nucleos/{id}", new { id });
            }).RequireAuthorization();

            app.MapPut("/nucleos/{id}", (int id, NucleoCreateDTO dto, INucleoService s) =>
            {
                s.Update(id, dto);
                return Results.Ok();
            }).RequireAuthorization();

            app.MapDelete("/nucleos/{id}", (int id, INucleoService s) =>
            {
                s.Delete(id);
                return Results.Ok();
            }).RequireAuthorization();

            // -------- LEITORES --------
            app.MapGet("/leitores", (ILeitorService s) => s.GetAll())
               .RequireAuthorization();

            app.MapGet("/leitores/{id}", (int id, ILeitorService s) =>
            {
                var l = s.GetById(id);
                return l == null ? Results.NotFound() : Results.Ok(l);
            }).RequireAuthorization();

            app.MapPost("/leitores", (LeitorCreateDTO dto, ILeitorService s) =>
            {
                int id = s.Create(dto);
                return Results.Created($"/leitores/{id}", new { id });
            }).RequireAuthorization();

            app.MapPut("/leitores/{id}", (int id, LeitorCreateDTO dto, ILeitorService s) =>
            {
                s.Update(id, dto);
                return Results.Ok();
            }).RequireAuthorization();

            app.MapDelete("/leitores/{id}", (int id, ILeitorService s) =>
            {
                s.Delete(id);
                return Results.Ok();
            }).RequireAuthorization();

            // -------- OBRAS --------
            app.MapGet("/obras", (IObraService s) => s.GetAll())
               .RequireAuthorization();

            app.MapGet("/obras/{id}", (int id, IObraService s) =>
            {
                var o = s.GetById(id);
                return o == null ? Results.NotFound() : Results.Ok(o);
            }).RequireAuthorization();

            app.MapPost("/obras", (ObraCreateDTO dto, IObraService s) =>
            {
                int id = s.Create(dto);
                return Results.Created($"/obras/{id}", new { id });
            }).RequireAuthorization();

            app.MapPut("/obras/{id}", (int id, ObraCreateDTO dto, IObraService s) =>
            {
                s.Update(id, dto);
                return Results.Ok();
            }).RequireAuthorization();

            app.MapDelete("/obras/{id}", (int id, IObraService s) =>
            {
                s.Delete(id);
                return Results.Ok();
            }).RequireAuthorization();

            // -------- EXEMPLARES --------
            app.MapGet("/exemplares", (IExemplarService s) => s.GetAll())
               .RequireAuthorization();

            app.MapGet("/exemplares/{id}", (int id, IExemplarService s) =>
            {
                var e = s.GetById(id);
                return e == null ? Results.NotFound() : Results.Ok(e);
            }).RequireAuthorization();

            app.MapPost("/exemplares", (ExemplarCreateDTO dto, IExemplarService s) =>
            {
                int id = s.Create(dto);
                return Results.Created($"/exemplares/{id}", new { id });
            }).RequireAuthorization();

            app.MapPut("/exemplares/{id}", (int id, ExemplarCreateDTO dto, IExemplarService s) =>
            {
                s.Update(id, dto);
                return Results.Ok();
            }).RequireAuthorization();

            app.MapDelete("/exemplares/{id}", (int id, IExemplarService s) =>
            {
                s.Delete(id);
                return Results.Ok();
            }).RequireAuthorization();

            // -------- REQUISICOES --------
            app.MapGet("/requisicoes", (IRequisicaoService s) => s.GetAll())
               .RequireAuthorization();

            app.MapGet("/requisicoes/{id}", (int id, IRequisicaoService s) =>
            {
                var r = s.GetById(id);
                return r == null ? Results.NotFound() : Results.Ok(r);
            }).RequireAuthorization();

            app.MapPost("/requisicoes", (RequisicaoCreateDTO dto, IRequisicaoService s) =>
            {
                int id = s.Create(dto);
                return Results.Created($"/requisicoes/{id}", new { id });
            }).RequireAuthorization();

            app.MapDelete("/requisicoes/{id}", (int id, IRequisicaoService s) =>
            {
                s.Delete(id);
                return Results.Ok();
            }).RequireAuthorization();

            // -------- EXEMPLAR-REQUISICAO --------
            app.MapGet("/requisicoes/{id}/exemplares", (int id, IExemplarRequisicaoService s) =>
                s.GetByRequisicao(id))
               .RequireAuthorization();

            app.MapGet("/exemplares/{id}/requisicoes", (int id, IExemplarRequisicaoService s) =>
                s.GetByExemplar(id))
               .RequireAuthorization();

            app.MapPost("/requisicoes/{id}/exemplares", (int id, ExemplarRequisicaoCreateDTO dto, IExemplarRequisicaoService s) =>
            {
                s.Adicionar(id, dto);
                return Results.Ok();
            }).RequireAuthorization();

            app.MapPut("/requisicoes/{id}/exemplares/{exId}/devolver", (int id, int exId, IExemplarRequisicaoService s) =>
            {
                s.MarcarDevolvido(exId, id);
                return Results.Ok();
            }).RequireAuthorization();

            app.MapDelete("/requisicoes/{id}/exemplares/{exId}", (int id, int exId, IExemplarRequisicaoService s) =>
            {
                s.Delete(exId, id);
                return Results.Ok();
            }).RequireAuthorization();

            app.Run();
        }
    }
}