namespace AspnetCoreMvcStarter.Areas.Admin.Models;

public class Product
{
  public long Id { get; set; }
  public string Title { get; set; }
  public string Description { get; set; }
  public decimal Price { get; set; }
  public string ImageUrl { get; set; }
  public int Status { get; set; }
}
