using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZulAssetsBackEnd_API.DAL;
using ZulAssetsBackEnd_API.Model;
using ZulAssetsBackEnd_API.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static ZulAssetsBackEnd_API.BAL.RequestParameters;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;

namespace ZulAssetsBackEnd_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        #region Declaration
        string newCountryFlag = string.Empty;

        private readonly IMailService _mailService;
        public EmailController(IMailService mailService)
        {
            _mailService = mailService;
        }


        #endregion

        #region Forget Password Email

        [HttpPost("ForgetPasswordEmail")]
        [Authorize]
        public async Task<IActionResult> ForgetPasswordEmail([FromForm] MailRequest mailRequest)
        {
            Message msg = new Message();
            try
            {
                await _mailService.ForgetPasswordEmail(mailRequest);
                msg.message = "Email sent successfully!";
                msg.status = "200";
                return Ok(msg);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("?p=BadCredentials"))
                {
                    msg.message = "Your requested Leave has been applied but due to wrong sender password you! Kindly check and update it again! Thanks.";
                    return Ok(msg);
                }
                if (ex.Message.Contains("RFC-5321"))
                {
                    msg.message = "The provided email address is not valid. Kindly use correct email address! Thanks.";
                    return Ok(msg);
                }
                else
                {
                    return Ok(ex.Message);
                }

            }
        }


        #endregion

        #region Forget Password Email

        [HttpPost("GeneratePasswordResetTokenEmail")]
        [Authorize]
        public async Task<IActionResult> GeneratePasswordResetTokenEmail([FromForm] MailRequest mailRequest)
        {
            Message msg = new Message();
            try
            {
                await _mailService.ForgetPasswordEmail(mailRequest);
                msg.message = "Email sent successfully!";
                msg.status = "200";
                return Ok(msg);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("?p=BadCredentials"))
                {
                    msg.message = "Your requested Leave has been applied but due to wrong sender password you! Kindly check and update it again! Thanks.";
                    return Ok(msg);
                }
                if (ex.Message.Contains("RFC-5321"))
                {
                    msg.message = "The provided email address is not valid. Kindly use correct email address! Thanks.";
                    return Ok(msg);
                }
                else
                {
                    return Ok(ex.Message);
                }

            }
        }


        #endregion

    }
}
