using FIL.Messaging.Models;
using FIL.Utilities.Extensions;

namespace FIL.Messaging.Extensions
{
    public static class RepsonseExtensions
    {
        public static IResponse Merge(this IResponse currentReponse, IResponse newResponse)
        {
            string newErrorMessage;
            if (currentReponse.ErrorMessage.IsNullOrBlank())
            {
                newErrorMessage = newResponse.ErrorMessage;
            }
            else
            {
                newErrorMessage = (currentReponse.ErrorMessage.Contains(newResponse.ErrorMessage) || newResponse.ErrorMessage.IsNullOrBlank())
                    ? currentReponse.ErrorMessage
                    : $"{currentReponse.ErrorMessage}, {newResponse.ErrorMessage}";
            }
            return new Response
            {
                Success = currentReponse.Success && newResponse.Success,
                ErrorMessage = newErrorMessage
            };
        }
    }
}