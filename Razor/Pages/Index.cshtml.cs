using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterFunctionality;

namespace Razor.Pages;

public class IndexModel : PageModel
{
  private IWaiterAvailability _waiter;
  private readonly ILogger<IndexModel> _logger;

  public IndexModel(ILogger<IndexModel> logger, IWaiterAvailability waiter)
  {
    _logger = logger;
    _waiter = waiter;
  }
  [BindProperty(SupportsGet = true)]
  public WorkingDays Data { get; set; }

  [BindProperty]
  public string FirstName { get; set; }

  [BindProperty]
  public List<string> Day { get; set; }


  public void OnGet()
  {
    Day = _waiter.GetWeekDays(Data.FirstName!);

  }

  public void OnPostSubmit()
  {
    _waiter.AddingSelectDays(Data.FirstName!, Day);


  }


  public void OnPostRemove()
  {
    _waiter.UpdateWorkingDays(Data.FirstName!);
  }






}
