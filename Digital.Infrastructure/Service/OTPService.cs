using System;
using AutoMapper;
using Digital.Data.Entities;
using Digital.Infrastructure.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace Digital.Infrastructure.Service
{
	public class OTPService : IOTPService
    {

        private readonly DigitalSignatureDBContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        public OTPService(IHttpContextAccessor contextAccessor, IMapper mapper, DigitalSignatureDBContext context, IUserContextService userContextService, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _userContext = userContextService;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        public async Task ConfirmCode(string verificationCode)
        {
            try
            {
                var accountSid = _configuration["Twilio:AccountSid"];
                var authToken = _configuration["Twilio:AuthToken"];
                TwilioClient.Init(accountSid, authToken);
                var userExist = Guid.Parse(_userContext.UserID.ToString()!);

                if (!string.IsNullOrEmpty(verificationCode))
                {

                    var emailOtp = await _context.Users.Where(x => x.Id == userExist)
                            .Select(x => x.Email).FirstOrDefaultAsync();
                    if (!string.IsNullOrEmpty(emailOtp))
                    {
                        var verificationCheck = VerificationCheckResource.Create(
                        to: emailOtp,
                        code: verificationCode,
                        pathServiceSid: _configuration["Twilio:VerificationServiceSID"]
                        );
                        Console.WriteLine(verificationCheck.Sid);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public async Task SendEmailMessage()
        {
            try
            {
                var accountSid = _configuration["Twilio:AccountSid"];
                var authToken = _configuration["Twilio:AuthToken"];
                TwilioClient.Init(accountSid, authToken);
                var userExist = Guid.Parse(_userContext.UserID.ToString()!);
                if (userExist != null) { 
                    var emailOtp = await _context.Users.Where(x => x.Id ==  userExist)
                        .Select(x => x.Email).FirstOrDefaultAsync();
                    if (!string.IsNullOrEmpty(emailOtp))
                    {
                        var verification = VerificationResource.Create(
                                    to: emailOtp,
                                    channel: "email",
                                    pathServiceSid: _configuration["Twilio:VerificationServiceSID"]
                        );
                        Console.WriteLine(verification.Sid);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}

