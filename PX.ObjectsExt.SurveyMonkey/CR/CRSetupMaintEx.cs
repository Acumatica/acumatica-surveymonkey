﻿using System;
using System.Collections;
using System.Text;
using System.Web;
using PX.Data;
using PX.Objects.CR;
using PXSurveyMonkeyCRExt.DAC;
using PXSurveyMonkeyMKExt;
using PX.SurveyMonkeyReader;

namespace PXSurveyMonkeyCRExt
{
    public class CRSetupMaintEx : PXGraphExtension<CRSetupMaint>
    {
        public PXSelect<CRSetup> Document;

        #region Actions

        public PXAction<CRSetup> login;
        [PXUIField(DisplayName = "Get Access Token")]
        [PXButton]
        public virtual void Login()
        {
            Base.Actions.PressSave();

            var surveyMonkeyOAuthHandler = GetSurveyMonkeyOAuthHandler();
            string authUrl = surveyMonkeyOAuthHandler.GetAuthorizationPageUri() +
                "&state=" + Convert.ToBase64String(Encoding.UTF8.GetBytes("acumaticaUrl=" + GetSiteBaseUrl()));

            string redirectUrl = GetSiteBaseUrl() +
                "?authUrl=" + Uri.EscapeDataString(authUrl);

            throw new PXRedirectToUrlException(redirectUrl, PXBaseRedirectException.WindowMode.NewWindow, "Get Access Token");
        }

        public PXAction<CRSetup> completeAuthentication;
        [PXButton]
        public virtual IEnumerable CompleteAuthentication(PXAdapter adapter)
        {
            var code = SurveyMonkeyCode.GetCode(adapter.CommandArguments);

            //If the user refuses to authorize the request, the code will be blank
            if (!string.IsNullOrEmpty(code.Code))
            {
                var setup = Document.Current;
                var setupExt = setup.GetExtension<CRSetupExt>();

                var surveyMonkeyOAuthHandler = GetSurveyMonkeyOAuthHandler();

                try
                {
                    setupExt.UsrAccessToken = surveyMonkeyOAuthHandler.GetAccessToken(code.Code);
                }
                catch (Exception ex)
                {
                    throw new PXException(ex.Message);
                }

                Document.Update(setup);
                Base.Actions.PressSave();
            }

            return adapter.Get();
        }

        #endregion

        #region Connection Helpers

        public class SurveyMonkeyCode
        {
            public string Code { get; }

            private SurveyMonkeyCode(string code)
            {
                Code = code;
            }

            public static SurveyMonkeyCode GetCode(string queryString)
            {
                return new SurveyMonkeyCode(HttpUtility.ParseQueryString(queryString).Get("code"));
            }
        }

        #endregion

        #region OAuthHandler

        private static SurveyMonkeyOAuthHandler GetSurveyMonkeyOAuthHandler()
        {
            var graph = PXGraph.CreateInstance<CaseSurveyResponseEngine>();
            var setupRecord = (CRSetup)graph.SetupRecord.Select();
            var setupRecordExt = graph.SetupRecord.Cache.GetExtension<CRSetupExt>(setupRecord);
            var redirectUrl = GetSiteBaseUrl();

            return new SurveyMonkeyOAuthHandler(setupRecordExt.UsrSurveyClientID, setupRecordExt.UsrSurveyClientSecret,
                redirectUrl);
        }

        #endregion

        private static string GetSiteBaseUrl()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                          VirtualPathUtility.ToAbsolute("~/") + "Frames/SurveyMonkeyAuthenticator.html";
        }
    }
}
