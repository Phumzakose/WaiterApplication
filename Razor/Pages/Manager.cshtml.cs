using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterFunctionality;

namespace Razor.Pages
{
  public class ManagerModel : PageModel
  {
    public const string SessionKeyName = "_Name";
    private IWaiterAvailability _waiter;
    private readonly ILogger<IndexModel> _logger;

    public ManagerModel(ILogger<IndexModel> logger, IWaiterAvailability waiter)
    {
      _logger = logger;
      _waiter = waiter;
    }

    [BindProperty]
    public string FirstName { get; set; }
    [BindProperty]
    public List<string> Day { get; set; }
    [BindProperty]
    public string WaiterName { get; set; }

    [BindProperty]
    public Dictionary<string, List<string>> WorkingEmployees { get; set; }



    public bool IsAdmin
    {
      get
      {
        var userName = HttpContext.Session.GetString(SessionKeys.UserNameKey);
        if (userName != null && userName == "Admin")
        {
          return true;
        }
        return false;
       
      }
    }


    public void OnGet()
    {

      var name = HttpContext.Session.GetString(SessionKeys.UserNameKey);
      Day = _waiter.WeekDays(name);
      WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();

    }

    public void OnPostClear()
    {
      _waiter.ResetData();
      TempData["Message2"] = "Your Schedule has been cleared...!";
      WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
    }

    public IActionResult OnPostLogout()
    {
      HttpContext.Session.Remove(SessionKeys.UserNameKey);
      return RedirectToPage("Index");
    }


    public void OnPostBack()
    {
      var name = HttpContext.Session.GetString(SessionKeys.UserNameKey);
      Console.WriteLine(name);
      Day = _waiter.WeekDays(name);
      WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();


    }
    public IActionResult OnPostHome()
    {
      return RedirectToPage("Index");
    }




  }
}
