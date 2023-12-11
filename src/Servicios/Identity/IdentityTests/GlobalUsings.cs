// Application Namespaces
global using Application.Common.DTOs.Auth;
global using Application.Common.DTOs.User;
global using Application.Common.Validators.Auth;
global using Application.Common.Validators.Users;

// Services.Common Namespaces
global using Services.Common.Models;

global using IntegrationTests.Common;


global using FluentValidation.TestHelper;
global using Xunit;


global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.IdentityModel.Tokens;

global using System.Collections;
global using System.IdentityModel.Tokens.Jwt;
global using System.Net;
global using System.Net.Http.Json;
global using System.Net.Http.Headers;
global using System.Security.Claims;
global using System.Text;