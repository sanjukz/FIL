using Google.Cloud.Translation.V2;
using FIL.Configuration;
using FIL.Contracts.Models.Integrations;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Api.Integrations
{
    public interface ILanguageTranslationApi : IService
    {
        IResponse<string> TranslateText(string text, string toLanguage = "en");

        IResponse<string> DetectTextLanguage(string text);
    }

    public class LanguageTranslationApi : Service<string>, ILanguageTranslationApi
    {
        public TranslationClient client = TranslationClient.Create();

        public LanguageTranslationApi(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public IResponse<string> TranslateText(string text, string toLanguage = "en")
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return GetResponse(true, "");
            }
            try
            {
                Detection detection = client.DetectLanguage(text);
                var response = client.TranslateText(
                text: text,
                targetLanguage: toLanguage,
                sourceLanguage: detection.Language);
                return GetResponse(true, response.TranslatedText);
            }
            catch (HttpRequestException ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return GetResponse(false, null);
            }
            catch (TaskCanceledException ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return GetResponse(false, null);
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return GetResponse(false, null);
            }
        }

        public IResponse<string> DetectTextLanguage(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return GetResponse(false, null);
            }

            try
            {
                Detection detection = client.DetectLanguage(text);
                return GetResponse(true, detection.Language);
            }
            catch (HttpRequestException ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return GetResponse(false, null);
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return GetResponse(false, null);
            }
        }
    }
}