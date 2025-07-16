using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Area("Frontend")]
public class AccountController : Controller
{
    [HttpPost]
    public IActionResult ExternalLogin(string provider)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { Area = "Frontend" });
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
  
        return Challenge(properties, provider);
    }

    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // 如果沒有成功驗證，可能原因：使用者取消登入、網路問題等
        if (!authenticateResult.Succeeded)
        {
            // 導回登入頁面或其他錯誤頁面，並顯示錯誤訊息
            TempData["ErrorMessage"] = "外部登入失敗。";
            return RedirectToAction("LoginC", "Access", new { Area = "Frontend" });
        }

        var externalClaims = authenticateResult.Principal.Claims;

        string externalUserId = externalClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        string email = externalClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        string name = externalClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        string providerName = authenticateResult.Properties?.Items[".AuthScheme"];

        var claimsIdentity = new ClaimsIdentity(externalClaims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        TempData["LoginSuccessMessage"] = $"您已成功使用 {providerName} 登入！歡迎 {name}。";
        return RedirectToAction("Index", "Home", new { Area = "Frontend" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("LoginC", "Access", new { Area = "Frontend" });
    }

    [HttpGet]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult Profile()
    {
        return View();
    }
}
