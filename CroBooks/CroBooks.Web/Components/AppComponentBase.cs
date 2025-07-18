using CroBooks.Web.Helpers;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace CroBooks.Web.Components
{
    public abstract class AppComponentBase : ComponentBase//, IDisposable
    {
        [Inject] private TranslationHelper TranslationHelper { get; set; } = null!;

        protected bool ComponentBusy;

        protected string LocalizeLabel<T>(Expression<Func<T>> propertyExpression)
        {
            return TranslationHelper.GetModelLabelTranslation(propertyExpression);
        }

        protected string LocalizeElement(string key)
        {
            return TranslationHelper.GetElementTranslation(key);
        }

        protected string Localize(string key)
        {
            return TranslationHelper.GetAppTranslation(key);
        }

        protected void ToggleBusy()
        {
            ComponentBusy = !ComponentBusy;
        }
    }
}
