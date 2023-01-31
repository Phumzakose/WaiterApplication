using System.ComponentModel.DataAnnotations;
public class WorkingDays
{
  [Required(ErrorMessage = "Please select your working days")]
  public List<int>? WeekDays { get; set; }
  [Required(ErrorMessage = "Please enter a your name")]
  public string? FirstName { get; set; }

}