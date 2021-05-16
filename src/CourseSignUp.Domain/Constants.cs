using CourseSignUp.Domain.Models;
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

        public static string AcceptedIntoCourseTemplate(Course course, Student student)
        {
            //TODO get email template from resourse for multilanguage

            return $"{student.Name}, {course.Name} accepted you!";
        }

        public static string NotAcceptedIntoCourseTemplate(Course course, Student student)
        {
            return $"{student.Name}, {course.Name} did not accept you... :(";
        }
    }
}
