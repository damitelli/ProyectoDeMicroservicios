// Application Namespaces
global using Application.Commands.Auth;
global using Application.Common.DTOs.Auth;
global using Application.Common.DTOs.User;
global using Application.Common.Interfaces;
global using Application.Common.Mappings;
global using Application.Common.Validators.Auth;
global using Application.Common.Validators.Users;

// Domain Namespaces
global using Domain.Entities;
global using Domain.Exceptions;

// Services.Common Namespaces
global using Services.Common.Behaviours;
global using Services.Common.Interfaces;
global using Services.Common.Models;
global using Services.Common.Services;
global using Services.Common.Wrappers;


global using AutoMapper;
global using FluentValidation;
global using MediatR;
global using NLog;
global using NLog.Web;


global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.OpenApi.Models;

global using System.ComponentModel.DataAnnotations;
global using System.Reflection;
global using System.Text.Json;
global using System.Text.Json.Serialization;