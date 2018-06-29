using System;
using Demo2018.Logics.Model;

namespace Demo2018.Logics
{
    public interface IGoogleManager
    {
        void Login(Action<GoogleUser, string> OnLoginComplete);

        void Logout();
    }
}
