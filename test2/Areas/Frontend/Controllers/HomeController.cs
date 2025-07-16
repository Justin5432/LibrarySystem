using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using test2.Models;
using test2.Areas.Frontend.Models.ViewModels;
using test2.Areas.Frontend.Models.Dtos;
using test2.Services;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace test2.Areas.Frontend.Controllers
{
    [Area("Frontend")]
    public class HomeController : Controller
    {
        #region field
        public const string sk1 = "query1";
        public const string sk2 = "type1";
        public const string sk3 = "query2";
        public const string sk4 = "year1";
        public const string sk5 = "year2";
        public const string sk6 = "lang";
        public const string sk7 = "type2";
        public const string sk8 = "status";
        #endregion

        private readonly ILogger<HomeController> _logger;
        private readonly ActivityService _activityService;
        private readonly AnnouncementService _announcementService;
        private readonly UserService _userService;

        public HomeController(ILogger<HomeController> logger, ActivityService activityService, AnnouncementService announcementService, UserService userService)
        {
            _logger = logger;
            _activityService = activityService;
            _announcementService = announcementService;
            _userService = userService;
        }

        #region action
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string displayType = "", string searchQuery="")
        {
            var viewModel = new HomeIndexViewModel();

            try
            {
                // 從 AnnouncementService 獲取公告資料
                viewModel = await _announcementService.GetPagedAnnouncementsAsync(pageNumber, pageSize, displayType, searchQuery); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入 Home Index 資料時發生錯誤。");
                ViewData["Error"] = $"無法載入首頁資料: {ex.Message}";
                // 即使出錯，ViewModel 還是會有空列表，避免 View 報錯
            }

            return View(viewModel);
        }

        [HttpPost]
        /// <summary>
        /// 透過 AJAX POST 請求取得公告列表的資料，並可根據頁碼、公告類型進行篩選。
        /// </summary>
        /// <param name="pageNumber">當前頁碼。</param>
        /// <param name="pageSize">每頁顯示公告數量</param>
        /// <param name="displayType">要篩選的公告類型Id。</param>
        /// <param name="searchQuery">要搜尋的活動標題</param>
        public async Task<IActionResult> UpdateAnnouncementList(
            [FromForm] int pageNumber,
            [FromForm] int pageSize,
            [FromForm] string displayType,
            [FromForm] string searchQuery)
        {
            try
            {

                // 確保 displayType 有預設值，避免 null 參考錯誤
                displayType ??= "";  // 預設全部類型

                HomeIndexViewModel viewModel;

                // 呼叫 GetPagedActivitiesAsync
                // 如果 searchQuery 是 null，searchQuery 就傳 null
                viewModel = await _announcementService.GetPagedAnnouncementsAsync(
                    pageNumber,
                    pageSize,
                    displayType,
                    searchQuery == "null" ? null : searchQuery);

                return PartialView("~/Areas/Frontend/Views/Shared/_Partial/_announcementList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新活動列表時發生錯誤。 傳入參數: Page = {pageNumber}, pageSize = {pageSize}, displayType = {displayType}, searchQuery = {searchQuery ?? "null"}");
                return StatusCode(500, $"更新活動列表失敗，請稍後再試: {ex.Message}");
            }
        }

        /// <summary>
        /// 顯示活動列表頁，支援分頁和顯示模式切換。
        /// </summary>
        /// <param name="page">當前頁碼，預設為 1。</param>
        /// <param name="pageSize">每頁顯示筆數，預設為 4。</param>
        /// <param name="displayMode">顯示模式 ("image" 或 "table")，預設為 "image"。</param>
        public async Task<IActionResult> Activity(int page = 1, int pageSize = 4, string displayMode = "image") // 預設第一頁，每頁4筆，圖片式顯示
        {
            var viewModel = new ActivityPagedViewModel();

            try
            {
                viewModel = await _activityService.GetPagedActivitiesAsync(page, pageSize, displayMode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入 Home Activity 資料時發生錯誤。");
                ViewData["Error"] = $"無法載入活動列表頁資料: {ex.Message}";
                // 即使出錯，ViewModel 還是會有空列表，避免 View 報錯
            }

            return View(viewModel); // 將包含完整分頁資訊的 ViewModel 傳遞給 View
        }

        [HttpPost]
        /// <summary>
        /// 透過 AJAX POST 請求取得活動列表的資料，並可根據頁碼、顯示模式和活動類型進行篩選。
        /// </summary>
        /// <param name="page">當前頁碼。</param>
        /// <param name="displayMode">要切換到的顯示模式 ("image" 或 "table")。</param>
        /// <param name="displayType">要篩選的活動類型 ("全部"、"講座" 等)。</param>
        /// <param name="searchQuery">要搜尋的活動標題</param>
        public async Task<IActionResult> UpdateActivityList(
            [FromForm] int page,
            [FromForm] string displayMode,
            [FromForm] string displayType,
            [FromForm] string searchQuery)
        {
            try {
            
                // 確保 displayMode 和 displayType 有預設值，避免 null 參考錯誤
                displayMode ??= "image"; // 預設圖片模式
                displayType ??= "全部";  // 預設全部類型

                // 根據 displayMode 決定 pageSize
                int pageSize = displayMode switch
                {
                    "image" => 4,
                    "table" => 6,
                    _ => -1 // 如果 displayMode 不是預期的模式就把 pageSize 設為 -1
                };

                if (pageSize == -1) // 如果 pageSize 為無效值 直接返回錯誤
                {
                    return BadRequest("無效的顯示模式");
                }

                ActivityPagedViewModel viewModel;

                // 呼叫 GetPagedActivitiesAsync
                // 如果 displayType 是 "全部"，activityTypeName 就傳 null
                // 如果 searchQuery 是 null，searchQuery 就傳 null
                viewModel = await _activityService.GetPagedActivitiesAsync(
                    page,
                    pageSize,
                    displayMode,
                    displayType == "全部" ? null : displayType,
                    searchQuery == "null" ? null : searchQuery);

                string? partialViewPath = displayMode switch
                {
                    "image" => "~/Areas/Frontend/Views/Shared/_Partial/_activityList_image.cshtml",
                    "table" => "~/Areas/Frontend/Views/Shared/_Partial/_activityList_table.cshtml",
                    _ => null
                };

                // 如果 partialViewPath 不再預期內 表示 displayMode 不在預期內
                if (partialViewPath == null)
                {
                    return BadRequest("無效的顯示模式，無法載入對應的視圖");
                }

                return PartialView(partialViewPath, viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新活動列表時發生錯誤。 傳入參數: Page = {page}, displayType = {displayType}, displayMode = {displayMode}, searchQuery = {searchQuery ?? "null"}");
                return StatusCode(500, $"更新活動列表失敗，請稍後再試: {ex.Message}");
            }
        }

        public async Task<IActionResult> ActivityInfo(string activityTitle)
        {
            try
            {
                // 呼叫時，型別會自動推斷為 ActivityViewModel?
                var viewModel = await _activityService.GetActivityByTitleAsync(activityTitle);

                if (viewModel == null) // <-- 這裡就是編譯器強制你檢查 null 的地方
                {
                    _logger.LogWarning($"找不到標題為 '{activityTitle}' 的活動。");
                    return NotFound($"找不到要查詢的活動: {activityTitle}");
                }

                return View(viewModel); // 如果 viewModel 不為 null，就可以安全地傳遞給 View
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"載入 Home ActivityInfo 資料時發生錯誤，活動標題: {activityTitle}。");
                ViewData["Error"] = $"無法載入活動資訊頁面資料: {ex.Message}";
                return View(new ActivityViewModel()); // 即使出錯，回傳一個空的 ViewModel
            }
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new UserRegistrationDto());
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Email == null || model.Password == null)
            {
                return View(model);
            }

            var registerResult = await _userService.UserRegister(model.PhoneNumber, model.Email, model.Password);

            if (registerResult.IsSuccess)
            {
                TempData["Result"] = "success";
                TempData["ShowModal"] = true;
                TempData["ResultMessage"] = registerResult.SuccessMessage;
                return RedirectToAction("Register");
            }

            else
            {
                TempData["Result"] = "fail";
                TempData["ShowModal"] = true;
                TempData["ResultMessage"] = registerResult.FailMessage;
                return View(model);
            }
        }

        public IActionResult Query(string query1, string type1, string query2, string year1, string year2, string lang, string type2, string status)
        {
            if (string.IsNullOrEmpty(query1)) { query1 = string.Empty; }
            if (string.IsNullOrEmpty(type1)) { type1 = string.Empty; }
            if (string.IsNullOrEmpty(query2)) { query2 = string.Empty; }
            if (string.IsNullOrEmpty(year1)) { year1 = string.Empty; }
            if (string.IsNullOrEmpty(year2)) { year2 = string.Empty; }
            if (string.IsNullOrEmpty(lang)) { lang = string.Empty; }
            if (string.IsNullOrEmpty(type2)) { type2 = string.Empty; }
            if (string.IsNullOrEmpty(status)) { status = string.Empty; }

            HttpContext.Session.SetString(sk1, query1);
            HttpContext.Session.SetString(sk2, type1);
            HttpContext.Session.SetString(sk3, query2);
            HttpContext.Session.SetString(sk4, year1);
            HttpContext.Session.SetString(sk5, year2);
            HttpContext.Session.SetString(sk6, lang);
            HttpContext.Session.SetString(sk7, type2);
            HttpContext.Session.SetString(sk8, status);

            return View();
        }
        public IActionResult Collection() { return View(); }
        #endregion
    }
}