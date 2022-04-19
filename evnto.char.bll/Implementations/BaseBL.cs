using evnto.chat.bll.Helpers;
using evnto.chat.dal;

namespace evnto.chat.bll.Implementations
{
    internal class BaseBL
    {
        #region Fields

        private readonly BLConfiguration _config;
        private ISecurityHelper _securityHelper = null;

        #endregion

        #region Properties

        protected BLConfiguration Configuration => _config;

        public ISecurityHelper SecurityHelper
        {
            get
            {
                if (_securityHelper == null)
                    _securityHelper = new SecurityHelper(Configuration.SaltMinSeed, Configuration.SaltRepeatMin, Configuration.SaltRepeatMax);

                return _securityHelper;
            }
        }

        #endregion

        #region Ctor

        public BaseBL(BLConfiguration config)
        {
            _config = config;
        }

        #endregion

        #region Methods

        protected EvntoChatDBContext CreateDbContext()
        {
            return new EvntoChatDBContext(_config.DBConnectionString);
        }

        #endregion
    }
}
