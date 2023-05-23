using log4net;

namespace Logger
{
    public class CommonLogger
    {
        private const string DEFAULT_LOGGER = "DefaultLogger";
        private const string PAYMENT_LOGGER = "PaymentLogger";
        private const string PERFORMANCE_LOGGER = "PerformanceLogger";
        private const string MOBILE_LOGGER = "MobileLogger";
        private const string MOBILE_ERROR_LOGGER = "MobileErrorLogger";

        /// <summary>
        /// Ghi log chung cho toan project
        /// </summary>
        public static ILog DefaultLogger { get; set; }

        /// <summary>
        /// Ghi log su dung trong chuc nang Payment
        /// </summary>
        public static ILog PaymentLogger { get; set; }

        /// <summary>
        /// Ghi log su dung trong chuc nang Payment
        /// </summary>
        public static ILog PerformanceLogger { get; set; }

        /// <summary>
        /// Ghi log su dung trong chuc nang Mobile
        /// </summary>
        public static ILog MobileLogger { get; set; }

        /// <summary>
        /// Ghi log su dung cho Error mobile
        /// </summary>
        public static ILog MobileErrorLogger { get; set; }

        static CommonLogger()
        {
            //log4net.Config.DOMConfigurator.Configure();
            log4net.Config.XmlConfigurator.Configure();
            DefaultLogger = LogManager.GetLogger(DEFAULT_LOGGER);
            PaymentLogger = LogManager.GetLogger(PAYMENT_LOGGER);
            PerformanceLogger = LogManager.GetLogger(PERFORMANCE_LOGGER);
            MobileLogger = LogManager.GetLogger(MOBILE_LOGGER);
            MobileErrorLogger = LogManager.GetLogger(MOBILE_ERROR_LOGGER);
        }
    }
}
