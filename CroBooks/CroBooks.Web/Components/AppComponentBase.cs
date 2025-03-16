using CroBooks.Web.Helpers;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace CroBooks.Web.Components
{
    public abstract class AppComponentBase : ComponentBase//, IDisposable
    {
        [Inject] TranslationHelper TranslationHelper { get; set; }
        
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
