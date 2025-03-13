using CroBooks.Web.Helpers;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace CroBooks.Web.Components
{
    public abstract class AppComponentBase : ComponentBase, IDisposable
    {
        [Inject] TranslationHelper TranslationHelper { get; set; }
        [Inject] HttpInterceptorService interceptor { get; set; } = null!;

        public void RegisterInterceptor()
        {
            interceptor.RegisterEvent();
        }

        public void Dispose()
        {
            interceptor.DisposeEvent();
        }

        public string LocalizeLabel<T>(Expression<Func<T>> propertyExpression)
        {
            return TranslationHelper.GetModelLabelTranslation(propertyExpression);
        }

        public string LocalizeElement(string key)
        {
            return TranslationHelper.GetElementTranslation(key);
        }

        public string Localize(string key)
        {
            return TranslationHelper.GetAppTranslation(key);
        }
    }
}
