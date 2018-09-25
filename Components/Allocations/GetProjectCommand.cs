using System;
using System.Threading.Tasks;
using Steeltoe.CircuitBreaker.Hystrix;

namespace Allocations
{
    public class GetProjectCommand : HystrixCommand<ProjectInfo>
    {
        private readonly long _projectId;
        private readonly Func<long, Task<ProjectInfo>> _getProjectFn;
        private readonly Func<long, Task<ProjectInfo>> _getProjectFallbackFn;

        public GetProjectCommand(
            long projectId,
            Func<long, Task<ProjectInfo>> getProjectFn,
            Func<long, Task<ProjectInfo>> getProjectFallBack
        ) : base(HystrixCommandGroupKeyDefault.AsKey("ProjectClientGroup"))
        {
            _projectId = projectId;
            _getProjectFn = getProjectFn;
            _getProjectFallbackFn = getProjectFallBack;
        }

        protected override async Task<ProjectInfo> RunAsync() => await _getProjectFn(_projectId);
        protected override async Task<ProjectInfo> RunFallbackAsync() => await _getProjectFallbackFn(_projectId);
    }
}
