using log4net;
using log4net.Config;
using System;

namespace AirportPanel
{
    public class Log
    {
        private static ILog logger = LogManager.GetLogger("AirportPanel");

        static Log()
        {
            XmlConfigurator.Configure();
        }


        public static ILog For(object LoggedObject)
        {
            if (LoggedObject != null)
            {
                return For(LoggedObject.GetType());
            }
            else
            {
                return For(null);
            }
        }


        public static ILog For(Type ObjectType)
        {
            if (ObjectType != null)
            {
                return LogManager.GetLogger(ObjectType.Name);
            }
            else
            {
                return LogManager.GetLogger(string.Empty);
            }
        }


        public static void Info(string msg)
        {
            logger.Info(msg);
            Console.WriteLine(msg);
        }


        public static void Error(string msg)
        {
            logger.Error(msg);
            Console.WriteLine(msg);
        }


        public static void Error(string msg, Exception ex)
        {
            logger.Error(msg, ex);
            Console.WriteLine(msg);
        }

    }
}
