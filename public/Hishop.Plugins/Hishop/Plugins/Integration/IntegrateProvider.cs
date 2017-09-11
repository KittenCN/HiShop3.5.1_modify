namespace Hishop.Plugins.Integration
{
    using System;

    public abstract class IntegrateProvider
    {
        protected IntegrateProvider()
        {
        }

        public abstract void AdminLogin(string username, string password, string returnUrl);
        public abstract void ChangePassword(string username, string password);
        public abstract void DeleteUser(int userId);
        public abstract int GetUserID(string username);
        protected abstract void Init(string configStr);
        public static IntegrateProvider Instance(string applicationType, string configStr)
        {
            IntegrateProvider provider = Activator.CreateInstance(Type.GetType(applicationType)) as IntegrateProvider;
            provider.Init(configStr);
            return provider;
        }

        public abstract void Login(string username, string password, string returnUrl);
        public abstract void Logout();
        public abstract void Register(string username, int gender, string password, string email, string regip, string qq, string msn);
    }
}

