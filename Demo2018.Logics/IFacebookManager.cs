using System;
using Demo2018.Logics.Model;

namespace Demo2018.Logics
{
    public interface IFacebookManager
    {
        void Login(Action<FacebookUser, string> onLoginComplete);

        void Logout();
    }
}
