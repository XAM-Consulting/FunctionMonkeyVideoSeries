using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AzureFromTheTrenches.Commanding.Abstractions;
using ServerlessBlog.Application.Models.Document;
using ServerlessBlog.Application.Repositories;
using ServerlessBlog.Commands;
using ServerlessBlog.Commands.Models;

namespace ServerlessBlog.Application.Handlers
{
    internal class GetHelloWorldQueryHandler : ICommandHandler<GetHelloWorldQuery, string>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetHelloWorldQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<string> ExecuteAsync(GetHelloWorldQuery command, string previousResult)
        {
            return "hello world";
        }
    }
}
