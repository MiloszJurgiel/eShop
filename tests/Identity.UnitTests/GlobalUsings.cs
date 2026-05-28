global using System.Security.Claims;
global using eShop.Identity.API.Controllers;
global using eShop.Identity.API.Models;
global using eShop.Identity.API.Services;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using NSubstitute;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
