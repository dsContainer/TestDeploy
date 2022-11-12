using AutoMapper;
using DigitalSignature.Entities;
using DigitalSignature.Interface;
using DigitalSignature.Mapper;
using DigitalSignature.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddMvc();

//Database
builder.Services.AddDbContext<DigitalSignatureDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SignDatabase")));

//Mapper
builder.Services.AddSingleton(new MapperConfiguration(mc =>
    {
        mc.AddProfile(new MappingProfile());
    }).CreateMapper());

//Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDocumentTypeService, DocumentTypeService>();
builder.Services.AddScoped<ISignatureService, SignatureService>();


var app = builder.Build();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
