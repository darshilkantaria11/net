namespace FirstReadAPI.Models
{
   public class Book
{
    public int BookId { get; set; }
    public string BookName { get; set; } = string.Empty;  // Providing default value
    public int Copies { get; set; }
}

}
