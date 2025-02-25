using ZulAssetsBackEnd_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;

namespace ZulAssetsBackEnd_API.Services
{
    /// <summary>
    /// Mail Service
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Forget Password Email Function
        /// </summary>
        /// <param name="mailRequest"></param>
        /// <returns></returns>
        Task ForgetPasswordEmail(MailRequest mailRequest);

        /// <summary>
        /// Generate Password Reset Token Email Function
        /// </summary>
        /// <param name="mailRequest"></param>
        /// <returns></returns>
        Task GeneratePasswordResetTokenEmail(MailRequest mailRequest);
    }
}
