

namespace Rush.Common
{
    public class RushConfiguration
    {
        private static RushConfiguration _config;

        private RushConfiguration()
        {
        }

        public static RushConfiguration Config => _config ?? (_config = new RushConfiguration());

        public double Version => 1.1;
    }
}
