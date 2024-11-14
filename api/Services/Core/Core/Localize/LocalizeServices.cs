using Helpers.Cache;
using Services.Core.Contracts;
using Services.Core.Interfaces;
namespace Services.Core.Services
{
    public class LocalizeServices : ILocalizeServices
    {
        private readonly IResourceServices resourceServices;
        private readonly ICacheServices cacheServices;
        public LocalizeServices(IResourceServices _resourceServices, ICacheServices _cacheServices)
        {
            resourceServices = _resourceServices;
            cacheServices = _cacheServices;
        }

        public string Get(string module, string screen, string key)
        {
            var lang = "ja";
            string cacheKey = $"resource_{lang}_{module}_{screen}";
            var data = cacheServices.Get(cacheKey, () =>
            {
                var items = resourceServices.GetByScreen(lang, module, screen);
                return items.Count() > 0 ? items : null;
            });
            if (data != null && data.Count > 0)
            {
                var dataItem = data.Where(x => x.key == key).FirstOrDefault();
                if (dataItem != null)
                {
                    return dataItem.text;
                }
                else
                    return RegisterNewResource(module, screen, key, lang, cacheKey).Result;
            }
            else
                return RegisterNewResource(module, screen, key, lang, cacheKey).Result;
        }

        public async Task<string> GetAsync(string module, string screen, string key)
        {
            var lang = "ja";
            string cacheKey = $"resource_{lang}_{module}_{screen}";
            var data = cacheServices.Get(cacheKey, () =>
            {
                var items = resourceServices.GetByScreen(lang, module, screen);
                return items.Count() > 0 ? items : null;
            });
            if (data != null && data.Count > 0)
            {
                var dataItem = data.Where(x => x.key == key).FirstOrDefault();
                if (dataItem != null)
                {
                    return dataItem.text;
                }
                else
                    return RegisterNewResource(module, screen, key, lang, cacheKey).Result;
            }
            else
                return RegisterNewResource(module, screen, key, lang, cacheKey).Result;
        }

        private async Task<string> RegisterNewResource(string module, string screen, string key, string lang, string cacheKey)
        {
            await resourceServices.Create(new ResourceRequest
            {
                lang = lang,
                module = module,
                screen = screen,
                key = key,
                text = key
            });
            cacheServices.ClearAsync(cacheKey);
            return key;
        }
    }
}
