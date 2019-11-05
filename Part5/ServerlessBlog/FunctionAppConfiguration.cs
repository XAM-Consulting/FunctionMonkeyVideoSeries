﻿using System.Net.Http;
using System.Security.Claims;
using AutoMapper;
using AzureFromTheTrenches.Commanding.Abstractions;
using FunctionMonkey.Abstractions;
using FunctionMonkey.Abstractions.Builders;
using FunctionMonkey.FluentValidation;
using FunctionMonkey.TokenValidator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ServerlessBlog.Application;
using ServerlessBlog.Commands;
using ServerlessBlog.CrossCuttingConcerns;

namespace ServerlessBlog
{
    public class FunctionAppConfiguration : IFunctionAppConfiguration
    {
        public const string Domain = "monkeydemo.au.auth0.com";
        public const string Audience = "http://serverlessblog";
        
        public void Build(IFunctionHostBuilder builder)
        {
            builder
                .Setup((serviceCollection, commandRegistry) =>
                {
                    serviceCollection
                        .AddApplication(commandRegistry)
                        .AddAutoMapper(typeof(SubsystemRegistration))
                        .Replace(new ServiceDescriptor(
                            typeof(ICommandDispatcher),
                            typeof(CustomDispatcher),
                            ServiceLifetime.Transient))
                        ;
                })
                .Authorization(authorization => authorization
                    .AuthorizationDefault(AuthorizationTypeEnum.TokenValidation)
                    .AddOpenIdConnectTokenValidator(
                        $"https://{Domain}/.well-known/openid-configuration",
                        Audience
                    )
                    .Claims(claims => claims
                        .MapClaimToCommandProperty<AddPostCommand>(
                            ClaimTypes.NameIdentifier,
                            cmd => cmd.UserId
                        )
                        .MapClaimToCommandProperty<UpdatePostCommand>(
                            ClaimTypes.NameIdentifier,
                            cmd => cmd.UserId
                        )
                    )
                )
                .OpenApiEndpoint(openApi => openApi
                    .Title("Serverless Blog")
                    .Version("0.0.1")
                    .UserInterface()
                )
                .AddFluentValidation()
                .DefaultHttpResponseHandler<HttpResponseHandler>()
                .Functions(functions => functions
                    .HttpRoute("/api/v1/post", route => route
                        
                        .HttpFunction<AddPostCommand>(HttpMethod.Post)
                        .Options(options => options.ClaimsPrincipalAuthorization<IsApprovedAuthorClaimsAuthorization>())
                        
                        .HttpFunction<GetPostQuery>(
                            "/{postId}",
                            AuthorizationTypeEnum.Anonymous,
                            HttpMethod.Get)

                        .HttpFunction<GetHelloWorldQuery>(
                            "/",
                            AuthorizationTypeEnum.Anonymous,
                            HttpMethod.Get)

                        .HttpFunction<UpdatePostCommand>(HttpMethod.Put)
                        .Options(options => options.ClaimsPrincipalAuthorization<IsApprovedAuthorClaimsAuthorization>())
                    )
                );
        }
    }
}
