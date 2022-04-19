using evnto.chat.bll.Helpers;
using evnto.chat.bll.Interfaces;
using evnto.chat.dal;

namespace evnto.chat.bll.Implementations
{
    internal class BaseBL
    {
        #region Fields

        private readonly IBLFactory _bLFactory;
        private ISecurityHelper _securityHelper = null;

        #endregion

        #region Properties

        protected IBLFactory BLFactory => _bLFactory;

        protected BLConfiguration Configuration => _bLFactory.Configuration;

        protected ISecurityHelper SecurityHelper
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

        public BaseBL(IBLFactory blFactory)
        {
            _bLFactory = blFactory;
        }

        #endregion

        #region Methods

        protected EvntoChatDBContext CreateDbContext()
        {
            return new EvntoChatDBContext(_bLFactory.Configuration.DBConnectionString);
        }

        #endregion
    }
}
