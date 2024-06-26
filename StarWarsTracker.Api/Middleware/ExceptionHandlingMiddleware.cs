﻿using StarWarsTracker.Domain.Constants.LogConfigs;
using StarWarsTracker.Domain.Exceptions;
using StarWarsTracker.Logging.Abstraction;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace StarWarsTracker.Api.Middleware
{
    [ExcludeFromCodeCoverage]
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly IClassLogger _logger;

        private readonly ILogConfigReader _logConfigReader;

        public ExceptionHandlingMiddleware(IClassLoggerFactory loggerFactory, ILogConfigReader logConfigReader)
        {
            _logger = loggerFactory.GetLoggerFor(this);
            _logConfigReader = logConfigReader;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.ContentType = "application/json";

            try
            {
                await next(context);
            }
            catch (ValidationFailureException e)
            {
                _logger.IncreaseLevel(_logConfigReader.GetCustomLogLevel(Section.ExceptionLogging, Key.ValidationFailureExceptionLogLevel) ?? Domain.Enums.LogLevel.None, 
                    "ValidationFailureException Caught", new { e.GetType().Name, e.ValidationFailureMessages, e.StackTrace });

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsync(JsonSerializer.Serialize(e.ValidationFailureMessages));
            }
            catch (DoesNotExistException e)
            {
                _logger.IncreaseLevel(_logConfigReader.GetCustomLogLevel(Section.ExceptionLogging, Key.DoesNotExistExceptionLogLevel) ?? Domain.Enums.LogLevel.None, 
                    "DoesNotExistException Caught", new { e.GetType().Name, e.NameOfObjectNotExisting, e.ValuesSearchedBy, e.StackTrace });

                context.Response.StatusCode = StatusCodes.Status404NotFound;

                await context.Response.WriteAsync(JsonSerializer.Serialize(new { e.NameOfObjectNotExisting, e.ValuesSearchedBy }));
            }
            catch (AlreadyExistsException e)
            {
                _logger.IncreaseLevel(_logConfigReader.GetCustomLogLevel(Section.ExceptionLogging, Key.AlreadyExistsExceptionLogLevel) ?? Domain.Enums.LogLevel.None, 
                    "AlreadyExistsException Caught", new { e.GetType().Name, e.NameOfObjectAlreadyExisting, e.Conflicts, e.StackTrace });

                context.Response.StatusCode = StatusCodes.Status409Conflict;

                await context.Response.WriteAsync(JsonSerializer.Serialize(new { e.Conflicts }));
            }
            catch (Exception e)
            {
                _logger.IncreaseLevel(_logConfigReader.GetCustomLogLevel(Section.ExceptionLogging, Key.DefaultExceptionLogLevel) ?? Domain.Enums.LogLevel.None, 
                    "Exception Caught", new { e.GetType().Name, e.Message, e.StackTrace });
                
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync("Unexpected Error.");
            }
        }
    }
}
