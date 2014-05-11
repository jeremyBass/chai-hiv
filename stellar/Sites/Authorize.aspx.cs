﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetOpenAuth.OAuth.Messages;
using System.Security.Cryptography;
using DotNetOpenAuth.OAuth;
using stellar.Code;
using stellar;

namespace stellar {
    public partial class Authorize : System.Web.UI.Page {
        private static readonly RandomNumberGenerator CryptoRandomDataGenerator = new RNGCryptoServiceProvider();

        private string AuthorizationSecret {
            get { return Session["OAuthAuthorizationSecret"] as string; }
            set { Session["OAuthAuthorizationSecret"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                if (GlobalApplication.PendingOAuthAuthorization == null) {
                    //no pending authorization
                } else {
                    ITokenContainingMessage pendingToken = GlobalApplication.PendingOAuthAuthorization;
                    var token = GlobalApplication.AuthTokens.Single(t => t.Token == pendingToken.Token);
                    this.desiredAccessLabel.Text = token.Scope;
                    this.consumerLabel.Text = GlobalApplication.TokenManager.GetConsumerForToken(token.Token).Key;

                    // Generate an unpredictable secret that goes to the user agent and must come back
                    // with authorization to guarantee the user interacted with this page rather than
                    // being scripted by an evil Consumer.
                    byte[] randomData = new byte[8];
                    CryptoRandomDataGenerator.GetBytes(randomData);
                    this.AuthorizationSecret = Convert.ToBase64String(randomData);
                    this.OAuthAuthorizationSecToken.Value = this.AuthorizationSecret;

                    this.OAuth10ConsumerWarning.Visible = GlobalApplication.PendingOAuthAuthorization.IsUnsafeRequest;
                }
            }
        }

        protected void Authtorize_Click(object sender, EventArgs e) {
            if (this.AuthorizationSecret != this.OAuthAuthorizationSecToken.Value) {
                throw new ArgumentException(); // probably someone trying to hack in.
            }
            this.AuthorizationSecret = null; // clear one time use secret
            var pending = GlobalApplication.PendingOAuthAuthorization;
            GlobalApplication.AuthorizePendingRequestToken();
            this.multiView.ActiveViewIndex = 1;

            ServiceProvider sp = new ServiceProvider(Constants.SelfDescription, GlobalApplication.TokenManager);
            var response = sp.PrepareAuthorizationResponse(pending);
            if (response != null) {
                sp.Channel.Send(response);
            } else {
                if (pending.IsUnsafeRequest) {
                    this.verifierMultiView.ActiveViewIndex = 1;
                } else {
                    string verifier = ServiceProvider.CreateVerificationCode(VerificationCodeFormat.AlphaNumericNoLookAlikes, 10);
                    this.verificationCodeLabel.Text = verifier;
                    ITokenContainingMessage requestTokenMessage = pending;
                    var requestToken = GlobalApplication.TokenManager.GetRequestToken(requestTokenMessage.Token);
                    requestToken.VerificationCode = verifier;
                    GlobalApplication.TokenManager.UpdateToken(requestToken);
                }
            }
        }
    }
}