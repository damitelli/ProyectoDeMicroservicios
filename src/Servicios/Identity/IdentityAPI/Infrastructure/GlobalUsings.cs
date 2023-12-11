// Application Namespaces
global using Application.Commands.Auth;
global using Application.Common.DTOs.Auth;
global using Application.Common.Interfaces;

// Domain Namespaces
global using Domain.Entities;
global using Domain.Exceptions;

// Infrastructure Namespaces
global using Infrastructure.Extensions;
global using Infrastructure.Persistence;
global using Infrastructure.Services;


global using AutoMapper;
global using MediatR;


global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.IdentityModel.Tokens;

global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;