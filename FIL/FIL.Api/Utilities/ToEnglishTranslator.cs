using FIL.Api.Integrations;

namespace FIL.Api.Utilities
{
    public class ToEnglishTranslator : IToEnglishTranslator
    {
        private readonly ILanguageTranslationApi _languageTranslationApi;

        public ToEnglishTranslator(ILanguageTranslationApi languageTranslationApi)
        {
            _languageTranslationApi = languageTranslationApi;
        }

        public string TranslateToEnglish(string text)
        {
            string lang = _languageTranslationApi.DetectTextLanguage(text).Result;
            if (lang == "en")
            {
                return text;
            }
            else
            {
                return _languageTranslationApi.TranslateText(text).Result;
            }
        }
    }
}