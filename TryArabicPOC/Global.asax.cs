using i18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TryArabicPOC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Change from the default of 'en'.
            i18n.LocalizedApplication.Current.DefaultLanguage = "en";

            // Change from the default of 'i18n.langtag'.
            i18n.LocalizedApplication.Current.CookieName = "i18n_langtag";

            // Change from the of temporary redirects during URL localization
            i18n.LocalizedApplication.Current.PermanentRedirects = true;

            // This line can be used to disable URL Localization.
            //i18n.UrlLocalizer.UrlLocalizationScheme = i18n.UrlLocalizationScheme.Void;

            // Change the URL localization scheme from Scheme1.
            i18n.UrlLocalizer.UrlLocalizationScheme = i18n.UrlLocalizationScheme.Scheme2;

            // Change i18n's expectation for the ASP.NET application's virtual application root path on the server, 
            // used by Url Localization. Defaults to "/".
            //i18n.LocalizedApplication.Current.ApplicationPath = "/mysite";

            // Specifies whether the key for a message may be assumed to be the value for
            // the message in the default language. Defaults to true.
            //i18n.LocalizedApplication.Current.MessageKeyIsValueInDefaultLanguage = false;

            // Specifies a custom method called after a nugget has been translated
            // that allows the resulting message to be modified, for instance according to content type.
            // See [Issue #300](https://github.com/turquoiseowl/i18n/issues/300) for example usage case.
            i18n.LocalizedApplication.Current.TweakMessageTranslation = delegate (System.Web.HttpContextBase context, i18n.Helpers.Nugget nugget, i18n.LanguageTag langtag, string message)
            {
                switch (context.Response.ContentType)
                {
                    case "text/html":
                        return message.Replace("\'", "&apos;");
                }
                return message;
            };

            // Blacklist certain URLs from being 'localized' via a callback.
            i18n.UrlLocalizer.IncomingUrlFilters += delegate (Uri url) {
                if (url.LocalPath.EndsWith("sitemap.xml", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                return true;
            };

            // Extend (+=) or override (=) the default handler for Set-PAL event.
            // The default handler applies the setting to both the CurrentCulture and CurrentUICulture
            // settings of the thread, as shown below.
            i18n.LocalizedApplication.Current.SetPrincipalAppLanguageForRequestHandlers = delegate (System.Web.HttpContextBase context, ILanguageTag langtag)
            {
                // Do own stuff with the language tag.
                // The default handler does the following:
                if (langtag != null)
                {
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = langtag.GetCultureInfo();
                }
            };

            // Blacklist certain URLs from being translated using a regex pattern. The default setting is:
            //i18n.LocalizedApplication.Current.UrlsToExcludeFromProcessing = new Regex(@"(?:\.(?:less|css)(?:\?|$))|(?i:i18nSkip|glimpse|trace|elmah)");

            // Whitelist content types to translate. The default setting is:
            //i18n.LocalizedApplication.Current.ContentTypesToLocalize = new Regex(@"^(?:(?:(?:text|application)/(?:plain|html|xml|javascript|x-javascript|json|x-json))(?:\s*;.*)?)$");

            // Change the types of async postback blocks that are localized
            //i18n.LocalizedApplication.Current.AsyncPostbackTypesToTranslate = "updatePanel,scriptStartupBlock,pageTitle";

            // Change which languages are parsed from the request, like skipping  the "Accept-Language"-header value. The default setting is:
            //i18n.HttpContextExtensions.GetRequestUserLanguagesImplementation = (context) => LanguageItem.ParseHttpLanguageHeader(context.Request.Headers["Accept-Language"]);

            // Override the i18n service injection. See source code for more details!
            //i18n.LocalizedApplication.Current.RootServices = new Myi18nRootServices();
        }
    }
}
