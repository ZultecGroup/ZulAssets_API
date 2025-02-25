using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZulAssetsBackEnd_API.Settings
{
    /// <summary>
    /// Mail Settings Parameters
    /// </summary>
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string myZultecURL { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
