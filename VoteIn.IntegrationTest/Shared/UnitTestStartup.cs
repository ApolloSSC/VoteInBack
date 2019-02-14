using Microsoft.AspNetCore.Hosting;

namespace VoteIn.IntegrationTest.Shared
{
    public class UnitTestStartup : Startup
    {
        public static UnitTestStartup Instance { get; private set; }

        public UnitTestStartup(IHostingEnvironment env)
            : base(env)
        {
            Instance = this;
        }

        protected override bool IsUnitTestRunning()
        {
            return true;
        }
    }
}
