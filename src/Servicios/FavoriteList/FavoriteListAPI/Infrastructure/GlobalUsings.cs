// Application Namespaces
global using Application;
global using Application.Common.Interfaces;

// Domain Namespaces
global using Domain.Entities;

// Infrastructure Namespaces
global using Infrastructure.Models;
global using Infrastructure.Services;

// Services.Common Namespaces
global using Services.Common.Interfaces;
global using Services.Common.Services;


global using MassTransit;
global using MediatR;
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization;
global using MongoDB.Bson.Serialization.Serializers;
global using MongoDB.Driver;


global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;

global using System.Text;