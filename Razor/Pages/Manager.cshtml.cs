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
    public List<string> Day { get; set; }
    [BindProperty]
    public string WaiterName { get; set; }

    [BindProperty]
    public Dictionary<string, List<string>> WorkingEmployees { get; set; }
    [BindProperty]
    public Dictionary<string, DayOfWeek> WeekDaysDate { get; set; }
    DateTime today = DateTime.Now;
    public int days = 7;



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
      WorkingDays.DecreasedWeeks();
      var name = HttpContext.Session.GetString(SessionKeys.UserNameKey);
      Day = _waiter.WeekDays(name);
      WeekDaysDate = _waiter.DaysOfTheWeek(today, WorkingDays.Week);
      WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();

    }

    public void OnPostClear()
    {
      _waiter.ResetData(WorkingDays.Week);
      if (WorkingDays.Week == 0)
      {
        TempData["Message2"] = "Week 1 Schedule has been cleared...!";

      }
      else
      {
        TempData["Message2"] = "Week 2 Schedule has been cleared...!";
      }
      WeekDaysDate = _waiter.DaysOfTheWeek(today, WorkingDays.Week);
      WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
    }

    public IActionResult OnPostLogout()
    {
      HttpContext.Session.Remove(SessionKeys.UserNameKey);
      return RedirectToPage("Index");
    }


    public void OnPostBack()
    {
      WorkingDays.DecreasedWeeks();
      var name = HttpContext.Session.GetString(SessionKeys.UserNameKey);
      Day = _waiter.WeekDays(name);
      WeekDaysDate = _waiter.DaysOfTheWeek(today, WorkingDays.Week);
      WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();


    }
    public IActionResult OnPostHome()
    {
      return RedirectToPage("Index");
    }
    public IActionResult OnPostNextWeek()
    {
      WorkingDays.IncreasedWeeks();
      var name = HttpContext.Session.GetString(SessionKeys.UserNameKey);
      Day = _waiter.WeekDays(name);
      WeekDaysDate = _waiter.DaysOfTheWeek(today, WorkingDays.Week);
      WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
      return Page();

    }

  }
}
