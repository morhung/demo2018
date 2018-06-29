using Demo2018.Logics.Models.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo2018.Logics.ViewModels
{
    public interface IGoogleManager
    {
        void Login(Action<GoogleUser, string> OnLoginComplete);

        void Logout();
    }
}
