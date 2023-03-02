using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterFunctionality;

namespace Razor.Pages;

public class WaiterModel : PageModel
{

  private readonly ILogger<WaiterModel> _logger;
  private IWaiterAvailability _waiter;

  public WaiterModel(ILogger<WaiterModel> logger, IWaiterAvailability waiter)
  {
    _logger = logger;
    _waiter = waiter;
  }

  [BindProperty]
  public string WaiterName { get; set; }

  [BindProperty]
  public List<string> Day { get; set; }

  [BindProperty]
  public Dictionary<string, List<string>> WorkingEmployees { get; set; }
  [BindProperty]
  public Dictionary<string, DayOfWeek> WeekDaysDate { get; set; }


  public int days { get; set; }


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

  public string GetWaiterName
  {
    get
    {
      if (IsAdmin)
      {
        if (WaiterName != null && WaiterName.Length > 0)
        {
          return WaiterName;

        }
      }

      return HttpContext.Session.GetString(IndexModel.SessionKeyName);

    }
  }

  DateTime today = DateTime.Now;

  public List<string> WaiterDays()
  {
    var name = HttpContext.Session.GetString(SessionKeys.UserNameKey);
    List<string> waiterDays = new List<string>();

    foreach (var item in _waiter.DaysOfTheWeek(today, WorkingDays.Week))
    {
      foreach (var items in _waiter.WeekDays(name))
      {
        if (items.Contains(item.Key))
        {
          waiterDays.Add(item.Key);
        }

      }
    }
    return waiterDays;
  }

  public void OnGet()
  {
    WorkingDays.DecreasedWeeks();
    var name = HttpContext.Session.GetString(SessionKeys.UserNameKey);
    WeekDaysDate = _waiter.DaysOfTheWeek(today, WorkingDays.Week);
    WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
    Day = _waiter.WeekDays(name);

  }


  public IActionResult OnPostLogout()
  {
    HttpContext.Session.Remove(SessionKeys.UserNameKey);
    return RedirectToPage("Index");
  }

  public void OnPostSubmit()
  {
    var name = GetWaiterName;
    _waiter.AddingSelectedDays(name, Day);
    WeekDaysDate = _waiter.DaysOfTheWeek(today, WorkingDays.Week);
    WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
    Day = _waiter.WeekDays(name);

    TempData["AlertMessage"] = "Your days have been submitted successfully...!";

  }

  public void OnPostUpdate()
  {
    //WorkingDays.IncreasedWeeks();
    var name = GetWaiterName;
    _waiter.UpdateWorkingDays(name, Day, WorkingDays.Week);
    WeekDaysDate = _waiter.DaysOfTheWeek(today, WorkingDays.Week);
    WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
    TempData["AlertMessage"] = "Your days have been updated successfully..!";

    if (HttpContext.Session.GetString(SessionKeys.UserNameKey) == "Admin")
    {
      TempData["AlertMessage"] = name + " " + "days have been updated successfully..!";

    }

  }

  public void OnPostManager()
  {
    var name = GetWaiterName;
    WeekDaysDate = _waiter.DaysOfTheWeek(today, WorkingDays.Week);
    WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
    Day = _waiter.WeekDays(name);

  }
  public IActionResult OnPostNext()
  {
    days = WorkingDays.Week;
    WorkingDays.IncreasedWeeks();
    WeekDaysDate = _waiter.DaysOfTheWeek(today, WorkingDays.Week);
    WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
    return Page();

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


}

