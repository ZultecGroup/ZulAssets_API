using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;

namespace ZulAssetsBackEnd_API.Model
{
    public class MailRequest
    {
        public string AppliedUserName { get; set; }
        public string EmailTo { get; set; }

        public string PasswordResetToken { get; set; }


    }
}
