using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Pospos.Core.Common;
using Pospos.Core.Helpers;
using Pospos.Core.Modules;
using Pospos.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospos.Api.Filters
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseTraceLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TraceLogMiddleWare>();
        }
    }
    public class TraceLogMiddleWare : IMiddleware
    {
        private readonly LogManager _logger;
        private readonly SecurityHelper _securityHelper;
        private readonly AppSettings _settings;
        public TraceLogMiddleWare(LogManager logger, SecurityHelper securityHelper, AppSettings settings)
        {
            _logger = logger;
            _securityHelper = securityHelper;
            _settings = settings;
        }
        //public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        //{
        //    string path = "";
        //    string headerStr = "";
        //    string request = "";
        //    DateTime startDate = DateTime.Now;
        //    try
        //    {
        //        request = await FormatRequest(context.Request);
        //        var originalBodyStream = context.Response.Body;
        //        path = context.Request.Path;
        //        headerStr = System.Text.Json.JsonSerializer.Serialize(context.Request.Headers.ToList());
        //        if(path.Contains("swagger"))
        //        {
        //            if(!_settings.SwaggerAllowedIpAddresses.Contains(_securityHelper.GetClientIpAddress()))
        //            {
        //                _logger.TraceLog(new Core.Common.LogObject()
        //                {
        //                    FirstLevelCategory = "swagger",
        //                    Request = request,
        //                    Response = "",
        //                    SecondLevelCategory = path,
        //                    ThirdLevelCategory = _securityHelper.GetClientIpAddress(),
        //                    Description = "Swagger- AccessDenied"
        //                });
        //                context.Response.StatusCode = 403;
        //                await context.Response.WriteAsync("You dont have acces to view this page");
        //                return;
        //            }
        //            else
        //            {
        //                _logger.TraceLog(new Core.Common.LogObject()
        //                {
        //                    FirstLevelCategory = "swagger",
        //                    Request = request,
        //                    Response = "",
        //                    SecondLevelCategory = path,
        //                    ThirdLevelCategory = _securityHelper.GetClientIpAddress(),
        //                    Description = "Swagger- Access"
        //                });
        //                await next(context);
        //            }
        //        }
        //        else
        //        {
        //            var responseBody = new MemoryStream();
        //            context.Response.Body = responseBody;
        //            await next(context);
        //            var resp = context.Response;
        //            var response = await GetResponseBody(resp);

        //            _logger.TraceLog(new Core.Common.LogObject()
        //            {
        //                FirstLevelCategory = path,
        //                Request = request,
        //                Response = response,
        //                SecondLevelCategory = context.Response.StatusCode.ToString(),
        //                ThirdLevelCategory = "",
        //                Description = headerStr,
        //                RequestEndDate = DateTime.Now,
        //                RequestStartDate = startDate
        //            });
        //            await responseBody.CopyToAsync(originalBodyStream);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.InternalError(new Core.Common.LogObject()
        //        {
        //            FirstLevelCategory = path,
        //            Request = request,
        //            Response = ex,
        //            SecondLevelCategory = context.Response.StatusCode.ToString(),
        //            ThirdLevelCategory = ex.GetType().Name,
        //            Description = headerStr,
        //            RequestEndDate = DateTime.Now,
        //            RequestStartDate = startDate
        //        });

        //        BaseApiResponse resp = new BaseApiResponse();
        //        resp.ErrorList.Add("Internal Error");
        //        var json = Jil.JSON.SerializeDynamic(resp);
        //        context.Response.ContentType = "application/json";
        //        await context.Response.WriteAsync(json);
        //    }
        //}

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string path = "";
            string headerStr = "";
            string requestStr = "";
            string responseStr = "";
            DateTime startDate = DateTime.Now;
            var requestBody = context.Request.Body;
            var responseBody = context.Response.Body;
            try
            {
                requestStr = await FormatRequest(context.Request);
                path = context.Request.Path;
                headerStr = System.Text.Json.JsonSerializer.Serialize(context.Request.Headers.ToList());
                if (path.Contains("swagger"))
                {
                    if (!_settings.SwaggerAllowedIpAddresses.Contains(_securityHelper.GetClientIpAddress()))
                    {
                        _logger.TraceLog(new Core.Common.LogObject()
                        {
                            FirstLevelCategory = "swagger",
                            Request = requestStr,
                            Response = "",
                            SecondLevelCategory = path,
                            ThirdLevelCategory = _securityHelper.GetClientIpAddress(),
                            Description = "Swagger- AccessDenied"
                        });
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("You dont have acces to view this page");
                        return;
                    }
                    else
                    {
                        _logger.TraceLog(new Core.Common.LogObject()
                        {
                            FirstLevelCategory = "swagger",
                            Request = requestStr,
                            Response = "",
                            SecondLevelCategory = path,
                            ThirdLevelCategory = _securityHelper.GetClientIpAddress(),
                            Description = "Swagger- Access"
                        });
                        await next(context);
                    }
                }
                else
                {
                    using (var newRequest = new MemoryStream())
                    {
                        context.Request.Body = newRequest;

                        using (var newResponse = new MemoryStream())
                        {
                            context.Response.Body = newResponse;

                            using (var writer = new StreamWriter(newRequest))
                            {
                                requestStr = _securityHelper.Decrypt(requestStr);
                                await writer.WriteAsync(requestStr);
                                await writer.FlushAsync();
                                newRequest.Position = 0;
                                context.Request.Body = newRequest;
                                await next(context);
                            }
                            
                            using (var reader = new StreamReader(newResponse))
                            {
                                newResponse.Position = 0;
                                responseStr = await reader.ReadToEndAsync();
                            }
                            _logger.TraceLog(new Core.Common.LogObject()
                            {
                                FirstLevelCategory = path,
                                Request = requestStr,
                                Response = responseStr,
                                SecondLevelCategory = context.Response.StatusCode.ToString(),
                                ThirdLevelCategory = "",
                                Description = headerStr,
                                RequestEndDate = DateTime.Now,
                                RequestStartDate = startDate
                            });

                            if (!string.IsNullOrWhiteSpace(responseStr))
                            {
                                responseStr = _securityHelper.Encrypt(responseStr);
                                responseStr = "{\"data\":\""+responseStr+"\"}";
                            }
                            using (var writer = new StreamWriter(responseBody))
                            {
                                await writer.WriteAsync(responseStr);
                                await writer.FlushAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.InternalError(new Core.Common.LogObject()
                {
                    FirstLevelCategory = path,
                    Request = requestStr,
                    Response = ex,
                    SecondLevelCategory = context.Response.StatusCode.ToString(),
                    ThirdLevelCategory = ex.GetType().Name,
                    Description = headerStr,
                    RequestEndDate = DateTime.Now,
                    RequestStartDate = startDate
                });

                BaseApiResponse resp = new BaseApiResponse();
                resp.ErrorList.Add("Internal Error");
                var json = Jil.JSON.SerializeDynamic(resp);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }




        private async Task<string> FormatRequest(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            request.EnableBuffering();
            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            string bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;
            return bodyAsText;
        }

        private async Task<string> GetResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

    }
}
