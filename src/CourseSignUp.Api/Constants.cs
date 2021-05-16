using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseSignUp.Api
{
    internal static class Constants
    {
        public const string NEW_SIGN_UP = "NewSignUp";
    }

    public static class SystemTexts
    {
        internal static string NotifyUserThatTheyWillBeInformedByEmail(string name)
        {
            //TODO change to get resource for multilanguage
            return "Você será notificado via email";
        }
    }
}
