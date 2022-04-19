using evnto.chat.bll.Helpers;
using evnto.chat.dal;

namespace evnto.chat.bll.Implementations
{
    internal class BaseBL
    {
        private readonly BLConfiguration _config;
        private ISecurityHelper _securityHelper = null;

        protected BLConfiguration Configuration => _config;

        public BaseBL(BLConfiguration config)
        {
            _config = config;
        }

        protected EvntoChatDBContext CreateDbContext()
        {
            return new EvntoChatDBContext(_config.DBConnectionString);
        }

        public ISecurityHelper SecurityHelper
        {
            get
            {
                if (_securityHelper == null)
                    _securityHelper = new SecurityHelper(Configuration.SaltMinSeed, Configuration.SaltRepeatMin, Configuration.SaltRepeatMax);

                return _securityHelper;
            }
        }
    }
}
