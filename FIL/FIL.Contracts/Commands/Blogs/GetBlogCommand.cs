using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Blogs
{
    public class GetBlogCommand : ICommandWithResult<GetBlogCommandResult>
    {
        public Guid ModifiedBy { get; set; }
    }

    public class GetBlogCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public List<BlogResponseModel> BlogResponseModelList { get; set; }
    }
}