using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterFunctionality;


namespace Razor.Pages;

public class IndexModel : PageModel
{
  public const string SessionKeyName = "_Name";
  private IWaiterAvailability _waiter;
  private readonly ILogger<IndexModel> _logger;

  public IndexModel(ILogger<IndexModel> logger, IWaiterAvailability waiter)
  {
    _logger = logger;
    _waiter = waiter;
  }

  [BindProperty]
  public string FirstName { get; set; }

  // [BindProperty]
  // public WorkingDays Employee { get; set; }

  public string name;

  public void OnGet()
  {
    HttpContext.Session.Clear();
  }
  public IActionResult OnPost()
  {
    name = _waiter.CheckName(FirstName);
    if (name.Equals(FirstName))
    {
      HttpContext.Session.SetString(SessionKeyName, FirstName);
      return RedirectToPage("Waiter");
    }
    else if (FirstName.Equals("Admin"))
    {
      HttpContext.Session.SetString(SessionKeyName, FirstName);
      return RedirectToPage("Manager");
    }
    else if (!name.Equals(FirstName))
    {
      TempData["Message1"] = "Invalid user !!";
    }

    FirstName = "";
    ModelState.Clear();
    return Page();

  }







}
