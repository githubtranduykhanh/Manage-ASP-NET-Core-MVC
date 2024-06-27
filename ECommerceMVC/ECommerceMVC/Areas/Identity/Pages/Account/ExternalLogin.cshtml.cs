﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ECommerceMVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ECommerceMVC.Helper.Strings;
using Microsoft.EntityFrameworkCore;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json.Linq;
using ECommerceMVC.Helper.Facebooks;

namespace ECommerceMVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<DbUser> _signInManager;
        private readonly UserManager<DbUser> _userManager;
        private readonly IUserStore<DbUser> _userStore;
        private readonly IUserEmailStore<DbUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<DbUser> signInManager,
            UserManager<DbUser> userManager,
            IUserStore<DbUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        
        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }


            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // Người dùng chưa có tài khoản trong hệ thống, cần liên kết tài khoản
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var phoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone);
                var userName = info.Principal.FindFirstValue(ClaimTypes.Name);
                DbUser user = null;

                if (!string.IsNullOrEmpty(email))
                {
                    user = await _userManager.FindByEmailAsync(email);
                }
                else if (!string.IsNullOrEmpty(phoneNumber))
                {
                    // Kiểm tra người dùng dựa trên số điện thoại
                    user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
                }
                else
                {
                    user = await _userManager.FindByNameAsync(StringHelper.NormalizeUsername(userName) + info.ProviderKey);
                }


                if (user == null && info.LoginProvider.Equals("Facebook", StringComparison.OrdinalIgnoreCase))
                {
                    user = CreateUser();
                    await _userStore.SetUserNameAsync(user, StringHelper.NormalizeUsername(userName) + info.ProviderKey, CancellationToken.None);                  
                    await _emailStore.SetEmailAsync(user, info.ProviderKey + "facebook@example.com", CancellationToken.None);
                    await _emailStore.SetEmailConfirmedAsync(user, true, CancellationToken.None);                 
                    var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);                                    
                    var accessToken = info.AuthenticationProperties.GetTokenValue("access_token");
                    user.DisplayName = userName;
                    user.Avatar = await FacebookHelper.GetPicture(identifier, accessToken);
                    var resultNewUser = await _userManager.CreateAsync(user);
                    if (!resultNewUser.Succeeded)
                    {
                        // Handle errors if create new user fails
                        var errors = resultNewUser.Errors.Select(e => e.Description);
                        ErrorMessage = string.Join(Environment.NewLine, errors);
                        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                    }
                }

                if (user == null && info.LoginProvider.Equals("Google", StringComparison.OrdinalIgnoreCase))
                {

                    user = CreateUser();
                    await _userStore.SetUserNameAsync(user, StringHelper.NormalizeUsername(userName) + info.ProviderKey, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
                    await _emailStore.SetEmailConfirmedAsync(user, true, CancellationToken.None);
                    user.DisplayName = userName;
                    if (info.Principal.HasClaim(c => c.Type == "urn:google:picture"))
                    {
                        user.Avatar = info.Principal.FindFirstValue("urn:google:picture");
                    }
                    else
                    {
                        user.Avatar = "https://i.pinimg.com/originals/f1/0f/f7/f10ff70a7155e5ab666bcdd1b45b726d.jpg";
                    }
                    var resultNewUser = await _userManager.CreateAsync(user);
                    if (!resultNewUser.Succeeded)
                    {
                        // Handle errors if create new user fails
                        var errors = resultNewUser.Errors.Select(e => e.Description);
                        ErrorMessage = string.Join(Environment.NewLine, errors);
                        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                    }
                }

                if (user != null)
                {
                    // Liên kết tài khoản với thông tin đăng nhập bên ngoài
                    var addLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (!addLoginResult.Succeeded)
                    {
                        // Handle errors if adding login fails
                        var errors = addLoginResult.Errors.Select(e => e.Description);
                        ErrorMessage = string.Join(Environment.NewLine, errors);
                        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                    }
                    // Đăng nhập sau khi đã liên kết tài khoản thành công
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                    return LocalRedirect(returnUrl);
                }


                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }


        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }

        private DbUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<DbUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(DbUser)}'. " +
                    $"Ensure that '{nameof(DbUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<DbUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<DbUser>)_userStore;
        }
    }
}
