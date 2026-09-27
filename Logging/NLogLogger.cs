using NLog;

namespace Logging {
    public class NLogLogger {
        private static readonly Logger Logger =
            LogManager.GetCurrentClassLogger();

        public void Info(string message){
            Logger.Info(message);
        }

        public void Error(Exception ex, string message){
            Logger.Error(ex, message);
        }
    }
}