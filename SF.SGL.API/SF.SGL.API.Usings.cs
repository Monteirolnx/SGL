#region Usings Externos
global using AutoMapper;
global using AutoMapper.QueryableExtensions;

global using MediatR;

global using System;
global using System.Data;
global using System.Net.Mail;
global using System.ComponentModel.DataAnnotations;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Text.Json.Serialization;
global using System.Net;
global using System.Text.Json;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.OpenApi.Models;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Hosting;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.AspNetCore.ResponseCompression;
#endregion

#region Usings Internos
global using SF.SGL.API.Dominio.Entidades;
global using SF.SGL.API.Infra.Contexto;
global using SF.SGL.API.Filtros;
global using SF.SGL.API.Dominio.Validacao;
global using SF.SGL.API.Middleware;
global using SF_SGL_WFC.ServiceReference;
global using SF.SGL.API.Hub;
#endregion