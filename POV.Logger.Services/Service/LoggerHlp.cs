using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
namespace POV.Logger.Service
{
    public class LoggerHlp
    {
        private Dictionary<Type, ILog> loggers = new Dictionary<Type, ILog>();
        private bool logInitialized = false;
        private object lockObject = new object();
       
        private static LoggerHlp singleton;

        private LoggerHlp() { }

        static LoggerHlp() { singleton = new LoggerHlp(); }

        public static LoggerHlp Default
        {
            get { return singleton; }
        }

        private ILog GetLogger(Type source)
        {
            lock (lockObject)
            {
                if (loggers.ContainsKey(source))
                {
                    return loggers[source];
                }
                else
                {
                    ILog logger = LogManager.GetLogger(source);
                    loggers.Add(source, logger);
                    return logger;
                }

            }
        }
        
        public void Error(Type source, Exception ex)
        {
            GetLogger(source).Error(ex.Message, ex);

            if (ex.InnerException != null)
                GetLogger(source).Error(ex.InnerException.Message, ex.InnerException);
        }

        public void Debug(Type source, Exception ex)
        {
            GetLogger(source).Debug(ex.Message, ex);
            if (ex.InnerException != null)
                GetLogger(source).Debug(ex.InnerException.Message, ex.InnerException);
        }

        public void Info(object source, Exception exception)
        {
            Info(source.GetType(), exception);
        }

        public void Info(Type source, Exception exception)
        {
            GetLogger(source).Info(exception);
            if (exception.InnerException != null)
                GetLogger(source).Info(exception.InnerException.Message, exception.InnerException);
        }

        public void Warn(object source, Exception exception)
        {
            Warn(source.GetType(), exception);
        }

        public void Warn(Type source, Exception exception)
        {
            GetLogger(source).Warn(exception);
            if (exception.InnerException != null)
                GetLogger(source).Warn(exception.InnerException.Message, exception.InnerException);
        }

        public void Error(object source, Exception exception)
        {
            Error(source.GetType(), exception);
        }

        public void Fatal(object source, Exception exception)
        {
            Fatal(source.GetType(),  exception);
        }

        public void Fatal(Type source, Exception exception)
        {
            GetLogger(source).Fatal( exception);
            if (exception.InnerException != null)
                GetLogger(source).Fatal(exception.InnerException.Message, exception.InnerException);
        }

        public void Debug(object source, object message)
        {
            Debug(source.GetType(), message);
        }

        public void Debug(Type source, object message)
        {
            GetLogger(source).Debug(message);

        }

        public void Info(object source, object message)
        {
            Info(source.GetType(), message);
        }

        public void Info(Type source, object message)
        {
            GetLogger(source).Info(message);
        }

        public void Warn(object source, object message)
        {
            Warn(source.GetType(), message);
        }

        public void Warn(Type source, object message)
        {
            GetLogger(source).Warn(message);
        }

        public void Error(object source, object message)
        {
            Error(source.GetType(), message);
        }

        public void Error(Type source, object message)
        {
            GetLogger(source).Error(message);
        }

        public void Fatal(object source, object message)
        {
            Fatal(source.GetType(), message);
        }

        public void Fatal(Type source, object message)
        {
            GetLogger(source).Fatal(message);
        }

        
    }
}
