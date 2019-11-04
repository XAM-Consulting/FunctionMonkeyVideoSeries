using System;
using AzureFromTheTrenches.Commanding.Abstractions;
using ServerlessBlog.Commands.Models;

namespace ServerlessBlog.Commands
{
    public class UpdatePostCommand : ICommand<Post>
    {
        //[SecurityProperty]
        //public string UserId { get; set; }

        public Guid PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
