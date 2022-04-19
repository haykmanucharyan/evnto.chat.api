namespace evnto.chat.bll.Helpers
{
    internal interface ISecurityHelper
    {
        int GenerateSalt();

        int GenerateSaltCount();

        string ComputePassowrdHash(string password, int salt, int saltCount);        
    }
}
