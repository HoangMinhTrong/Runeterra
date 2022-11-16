// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Test;
using Identity.MVC.Data;
using Identity.MVC.Entity;
using Identity.Services;
using Identity.Services.MainModule.Account;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerHost.Quickstart.UI
{
    /// <summary>
    /// This sample controller implements a typical login/logout/provision workflow for local and external accounts.
    /// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
    /// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
    /// </summary>
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IIdentityProviderStore _identityProviderStore;
        private readonly IEventService _events;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IIdentityProviderStore identityProviderStore,
            IEventService events,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleInManager,
            ApplicationDbContext db)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _identityProviderStore = identityProviderStore;
            _events = events;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleInManager;
            _db = db;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        public async Task < IActionResult > SignIn(LoginInputModel signIn, string ReturnUrl) {
            ApplicationUser user;
            if (signIn.Username.Contains("@")) {
                user = await _userManager.FindByEmailAsync(signIn.Username);
            } else {
                user = await _userManager.FindByNameAsync(signIn.Username);
            }
            if (user == null) {
                ModelState.AddModelError("", "Login fail");
            }
            var result = await
                _signInManager.PasswordSignInAsync(user, signIn.Password, signIn.RememberLogin, true);
            if (!result.Succeeded) {
                ModelState.AddModelError("", "Login fail");
            }
            if (ReturnUrl != null) return LocalRedirect(ReturnUrl);
            return Ok();
        }

        
        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();
                
                // delete local authentication cookie
                await _signInManager.SignOutAsync();


                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        
        // [HttpGet]
        // [Authorize("api")]
        // public async Task<IActionResult> Register(string returnUrl)
        // {
        //     // build a model so we know what to show on the reg page
        //     var vm = await BuildRegisterViewModelAsync(returnUrl);
        //
        //     return View(vm);
        // }

       
        // [HttpPost]
        // [Authorize("api")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        // {
        //     ViewData["ReturnUrl"] = returnUrl;
        //     if (ModelState.IsValid)
        //     {
        //
        //         var user = new ApplicationUser
        //         {
        //             UserName = model.Username,
        //             Email = model.Email,
        //             EmailConfirmed = true,
        //             FirstName = model.FirstName,
        //             LastName = model.LastName,
        //             Position = model.Position
        //         };
        //
        //         var result = await _userManager.CreateAsync(user, model.Password);
        //         if (result.Succeeded)
        //         {
        //             if (!_roleManager.RoleExistsAsync(model.RoleName).GetAwaiter().GetResult())
        //             {
        //                 var userRole = new IdentityRole
        //                 {
        //                     Name = model.RoleName,
        //                     NormalizedName = model.RoleName,
        //
        //                 };
        //                 await _roleManager.CreateAsync(userRole);
        //             }
        //
        //             await _userManager.AddToRoleAsync(user, model.RoleName);
        //
        //             await _userManager.AddClaimsAsync(user, new Claim[]{
        //                     new Claim(JwtClaimTypes.Name, model.Username),
        //                     new Claim(JwtClaimTypes.Email, model.Email),
        //                     new Claim(JwtClaimTypes.FamilyName, model.FirstName),
        //                     new Claim(JwtClaimTypes.GivenName, model.LastName),
        //                     new Claim(JwtClaimTypes.WebSite, "http://"+model.Username+".com"),
        //                     new Claim(JwtClaimTypes.Role,"Staff") });
        //
        //             var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
        //             var loginresult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: true);
        //             if (loginresult.Succeeded)
        //             {
        //                 var checkuser = await _userManager.FindByNameAsync(model.Username);
        //                 await _events.RaiseAsync(new UserLoginSuccessEvent(checkuser.UserName, checkuser.Id, checkuser.UserName, clientId: context?.Client.ClientId));
        //
        //                 if (context != null)
        //                 {
        //                     if (context.IsNativeClient())
        //                     {
        //                         // The client is native, so this change in how to
        //                         // return the response is for better UX for the end user.
        //                         return this.LoadingPage("Redirect", model.ReturnUrl);
        //                     }
        //
        //                     // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
        //                     return Redirect(model.ReturnUrl);
        //                 }
        //
        //                 // request for a local page
        //                 if (Url.IsLocalUrl(model.ReturnUrl))
        //                 {
        //                     return Redirect(model.ReturnUrl);
        //                 }
        //                 else if (string.IsNullOrEmpty(model.ReturnUrl))
        //                 {
        //                     return Redirect("~/");
        //                 }
        //                 else
        //                 {
        //                     // user might have clicked on a malicious link - should be logged
        //                     throw new Exception("invalid return URL");
        //                 }
        //             }
        //
        //         }
        //     }
        //     var vm = await BuildRegisterViewModelAsync(returnUrl);
        //
        //     return View(vm);
        // }
        // private async Task<RegisterViewModel> BuildRegisterViewModelAsync(string returnUrl)
        // {
        //     var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        //     List<string> roles = new List<string>();
        //     roles.Add("Student");
        //     roles.Add("Staff");
        //     ViewBag.message = roles;
        //     if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        //     {
        //         var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;
        //
        //         // this is meant to short circuit the UI and only trigger the one external IdP
        //         var vm = new RegisterViewModel
        //         {
        //             EnableLocalLogin = local,
        //             ReturnUrl = returnUrl,
        //             Username = context?.LoginHint,
        //         };
        //
        //         if (!local)
        //         {
        //             vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
        //         }
        //
        //         return vm;
        //     }
        //
        //     var schemes = await _schemeProvider.GetAllSchemesAsync();
        //
        //     var providers = schemes
        //         .Where(x => x.DisplayName != null)
        //         .Select(x => new ExternalProvider
        //         {
        //             DisplayName = x.DisplayName ?? x.Name,
        //             AuthenticationScheme = x.Name
        //         }).ToList();
        //
        //     var allowLocal = true;
        //     if (context?.Client.ClientId != null)
        //     {
        //         var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
        //         if (client != null)
        //         {
        //             allowLocal = client.EnableLocalLogin;
        //
        //             if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
        //             {
        //                 providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
        //             }
        //         }
        //     }
        //
        //     return new RegisterViewModel
        //     {
        //         AllowRememberLogin = AccountOptions.AllowRememberLogin,
        //         EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
        //         ReturnUrl = returnUrl,
        //         Username = context?.LoginHint,
        //         ExternalProviders = providers.ToArray()
        //     };
        // }

        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var dyanmicSchemes = (await _identityProviderStore.GetAllSchemeNamesAsync())
                .Where(x => x.Enabled)
                .Select(x => new ExternalProvider
                {
                    AuthenticationScheme = x.Scheme,
                    DisplayName = x.DisplayName
                });
            providers.AddRange(dyanmicSchemes);

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        } 
        
        // [HttpGet]
        // [Authorize(Roles = SD.Admin)]
        //  public async Task<IActionResult> Index()
        //  {
        //      var identity = (ClaimsIdentity)User.Identity;
        //      IEnumerable<Claim> claims = identity.Claims;
        //      var userList = _db.Users.Where(u=>u.Id != claims.First().Value);
        //      foreach (var user in userList)
        //      {
        //          var userTemp = await _userManager.FindByIdAsync(user.Id);
        //          var roleTemp = await _userManager.GetRolesAsync(userTemp);
        //          user.Role = roleTemp.FirstOrDefault();
        //      }
        //      ViewData["Message"] = TempData["Message"];
        //      return View(userList);
        //  }

         // [HttpGet]
         // [Authorize(Roles = SD.Admin)]
         // public async Task<IActionResult> ConfirmDelete(string id)
         // {
         //     var applicationUser = _db.Users.Find(id);
         //     return View(applicationUser);
         // }
         
         // [HttpGet]
         // [Authorize(Roles = SD.Admin)]
         // public async Task<IActionResult> Delete(string id)
         // {
         //     var applicationUser = _db.Users.Find(id);
         //     if (applicationUser != null)
         //     {
         //         await _userManager.DeleteAsync(applicationUser);
         //         TempData["Message"] = "Success: Delete successfully";
         //     }
         //
         //     return RedirectToAction(nameof(Index));
         // }
         
        //  [HttpGet]
        //  [Authorize(Roles = SD.Admin)]
        // public async Task<IActionResult> Edit(string id)
        // {
        //     var user = _db.Users.Find(id);
        //     if (user == null)
        //     {
        //         ViewData["Message"] = "Error: User not found";
        //         return NotFound();
        //     }
        //
        //     return View(user);
        // }
        
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // [Authorize(Roles = SD.Admin)]
        // public async Task<IActionResult> Edit(ApplicationUser user)
        // {
        //     if (user == null)
        //     {
        //         ViewData["Message"] = "Error: Data null";
        //         return RedirectToAction(nameof(Index), "Account");
        //     }
        //
        //     var userDb = _db.Users.Find(user.Id);
        //     userDb.FirstName = user.FirstName;
        //     userDb.LastName = user.LastName;
        //     userDb.Position = user.Position;
        //     userDb.PhoneNumber = user.PhoneNumber;
        //     
        //     _db.Users.Update(userDb);
        //     _db.SaveChanges();
        //     return RedirectToAction(nameof(Index), "Account");
        // }
        
        // [Authorize(Roles = SD.Admin)]
        // public async Task<IActionResult> ForgotPassword(string id)
        // {
        //     var user =  _db.Users.Find(id);
        //
        //     if (user == null)
        //     {
        //         return View();
        //     }
        //
        //     ForgotPasswordViewModel UserEmail = new ForgotPasswordViewModel()
        //     {
        //         Email = user.Email
        //     };
        //     return View(UserEmail);
        // }
        // [Authorize(Roles = SD.Admin)]
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var user = await _userManager.FindByEmailAsync(model.Email);
        //         if (user != null)
        //         {
        //             var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //
        //             return RedirectToAction("ResetPassword", "Account", new {email = model.Email, token = token});
        //         }
        //
        //         return View("ForgotPasswordConfirmation");
        //     }
        //     return View(model);
        // }
        // [Authorize(Roles = SD.Admin)]
        // public async Task<IActionResult> ResetPassword(string token, string email)
        // {
        //     if (token == null || email == null)
        //     {
        //         ModelState.AddModelError("","Invalid password reset token");
        //     }
        //
        //     return View();
        // }
        // [Authorize(Roles = SD.Admin)]
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var user = await _userManager.FindByEmailAsync(model.Email);
        //         if (user != null)
        //         {
        //             var result = await _userManager.ResetPasswordAsync(user,model.Token, model.Password);
        //             if (result.Succeeded)
        //             {
        //                 return View("ResetPasswordConfirmation");
        //             }
        //             else
        //             {
        //                 ViewData["Message"] = "Error: Your Password not permitted";
        //                 return View(model);
        //             }
        //
        //             foreach (var error in result.Errors)
        //             {
        //                 ModelState.AddModelError("",error.Description);
        //             }
        //
        //             return View(model);
        //         }
        //     }
        //     return View(model);
        // }

        // [Authorize(Roles = SD.Admin)]
        // [HttpGet]
        // public IActionResult LockUnlock(string id)
        // {
        //     var identity = (ClaimsIdentity)User.Identity;
        //     IEnumerable<Claim> claims = identity.Claims;
        //     var claimUser = _db.Users.FirstOrDefault(u => u.Id == claims.First().Value);
        //
        //     var applicationUser = _db.Users.FirstOrDefault(u => u.Id == id);
        //     if (applicationUser == null)
        //     {
        //         return NotFound();
        //     }
        //     if (claimUser.Id == applicationUser.Id)
        //     {
        //         return NotFound();
        //     }
        //     if (applicationUser.LockoutEnd != null && applicationUser.LockoutEnd > DateTime.Now)
        //     {
        //         //user is currently locked, we will unlock them
        //         applicationUser.LockoutEnd = DateTime.Now;
        //         _db.SaveChanges();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     else
        //     {
        //         applicationUser.LockoutEnd = DateTime.Now.AddYears(1000);
        //         _db.SaveChanges();
        //         return RedirectToAction(nameof(Index));
        //     }
        // }
    }
}
