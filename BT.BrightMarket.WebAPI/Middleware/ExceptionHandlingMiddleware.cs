﻿using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using FV = FluentValidation;
using BT.BrightMarket.Application.Exceptions;
using System.Diagnostics;

namespace BT.BrightMarket.WebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = new ErrorResponseInfo();
                response.Message = ex.Message;
                switch (ex)
                {
                    case ValidationException:
                    case FV.ValidationException:
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        break;
                    case RelationNotFoundException:
                        response.StatusCode = StatusCodes.Status404NotFound;
                        break;
                    default:
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }
                Debug.WriteLine(ex.Message);
                context.Response.StatusCode = response.StatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }

    public class ErrorResponseInfo
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
