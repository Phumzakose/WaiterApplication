using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterFunctionality;

namespace Razor.Pages;

public class WaiterModel : PageModel
{
  // public const string SessionKeyName1 = "_Waiter";
  //  public const string SessionKeyName = "_Name";

  private readonly ILogger<WaiterModel> _logger;
  private IWaiterAvailability _waiter;

  public WaiterModel(ILogger<WaiterModel> logger, IWaiterAvailability waiter)
  {
    _logger = logger;
    _waiter = waiter;
  }

  [BindProperty]
  public string FirstName { get; set; }

  [BindProperty]
  public string WaiterName { get; set; }

  [BindProperty]
  public List<string> Day { get; set; }

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


  public void OnGet()
  {
    var name = HttpContext.Session.GetString(SessionKeys.UserNameKey);
    Day = _waiter.WeekDays(name);
    WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();

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
    WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
    Day = _waiter.WeekDays(name);
    TempData["AlertMessage"] = "Your days have been submitted successfully...!";

  }
  public void OnPostUpdate()
  {

    var name = GetWaiterName;
    _waiter.UpdateWorkingDays(name, Day);
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
    Day = _waiter.WeekDays(name);
    WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();


  }




}

