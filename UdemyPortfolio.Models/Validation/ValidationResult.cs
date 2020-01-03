namespace UdemyPortfolio.Models.Validation
{
    public class ValidationResult<T> : Validation
    {
        public T Result { get; set; }
    }
}
