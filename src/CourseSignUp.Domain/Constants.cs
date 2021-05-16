using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseSignUp.Domain
{
    public static class Constants
    {
        public const string NEW_SIGN_UP_TOPIC = "NewSignUp";
        public const string ALL_MESSAGES_SUBSCRIPTION = "AllMessagesSubscriptions";

        public const string SIGN_UP_PROCESSED_TOPIC = "SignUpProcessed";
    }

    public static class ChamaSystemTexts
    {
        public static string NotifyUserThatTheyWillBeInformedByEmail(string name)
        {
            //TODO change to get resource for multilanguage
            return $"{name}, você será notificado via email do resultado";
        }
    }
}
