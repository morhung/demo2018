﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo2018.Logics.Models.Auth;
using Demo2018.Logics.ViewModels;
using Foundation;
using Google.SignIn;
using UIKit;

namespace Demo2018.iOS
{
    public class GoogleManager : NSObject, IGoogleManager, ISignInDelegate, ISignInUIDelegate
    {
        private Action<Logics.Models.Auth.GoogleUser, string> _onLoginComplete;
        private UIViewController _viewController { get; set; }

        public GoogleManager()
        {
            SignIn.SharedInstance.UIDelegate = this;
            SignIn.SharedInstance.Delegate = this;
        }

        public void Login(Action<Logics.Models.Auth.GoogleUser, string> OnLoginComplete)
        {
            _onLoginComplete = OnLoginComplete;

            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            _viewController = vc;

            SignIn.SharedInstance.SignInUser();
        }

        public void Logout()
        {
            SignIn.SharedInstance.SignOutUser();
        }

        public void DidSignIn(SignIn signIn, Google.SignIn.GoogleUser user, NSError error)
        {

            if (user != null && error == null)
                _onLoginComplete?.Invoke(new Logics.Models.Auth.GoogleUser()
                {
                    Name = user.Profile.Name,
                    Email = user.Profile.Email,
                    Picture = user.Profile.HasImage ? new Uri(user.Profile.GetImageUrl(500).ToString()) : new Uri(string.Empty)
                }, string.Empty);
            else
                _onLoginComplete?.Invoke(null, error.LocalizedDescription);
        }

        [Export("signIn:didDisconnectWithUser:withError:")]
        public void DidDisconnect(SignIn signIn, Google.SignIn.GoogleUser user, NSError error)
        {
            // Perform any operations when the user disconnects from app here.
        }

        [Export("signInWillDispatch:error:")]
        public void WillDispatch(SignIn signIn, NSError error)
        {
            //myActivityIndicator.StopAnimating();
        }

        [Export("signIn:presentViewController:")]
        public void PresentViewController(SignIn signIn, UIViewController viewController)
        {
            _viewController?.PresentViewController(viewController, true, null);
        }

        [Export("signIn:dismissViewController:")]
        public void DismissViewController(SignIn signIn, UIViewController viewController)
        {
            _viewController?.DismissViewController(true, null);
        }
    }
}