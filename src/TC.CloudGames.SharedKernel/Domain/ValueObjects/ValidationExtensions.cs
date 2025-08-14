namespace TC.CloudGames.SharedKernel.Domain.ValueObjects
{
    public static class ValidationExtensions
    {
        public static void AddErrorsIfFailure<T>(this List<ValidationError> errors, Result<T> result) where T : class
        {
            if (!result.IsSuccess)
                errors.AddRange(result.ValidationErrors);
        }
    }

}
