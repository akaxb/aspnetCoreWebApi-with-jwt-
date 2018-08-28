using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using User.Identity.Services;

namespace User.Identity.Authentication
{
    public class SmsCodeValidator : IExtensionGrantValidator
    {
        public string GrantType => "sms_auth_code";

        private readonly IAuthCodeService _authCodeService;

        private readonly IUserService _userService;

        public SmsCodeValidator(IAuthCodeService authCodeService, IUserService userService)
        {
            _authCodeService = authCodeService;
            _userService = userService;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var code = context.Request.Raw["auth_code"];
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(code))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "手机号或验证码不能为空");
                return;
            }
            if (!_authCodeService.Validate(phone, code))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "无效的验证码");
                return;
            }
            int id = await _userService.CreateOrCheckAsync(phone);
            if (id <= 0)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "获取userid失败");
                return;
            }
            context.Result = new GrantValidationResult(id.ToString(),GrantType);
        }
    }
}
