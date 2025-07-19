
using CroBooks.Web.Resources;
using CroBooks.Web.Resources.Elements;
using CroBooks.Web.Resources.Models;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace CroBooks.Web.Helpers
{
    public class TranslationHelper
    {
        private readonly IStringLocalizer<ModelResource> _modelLocalizer;
        private readonly IStringLocalizer<AppResource> _appLocalizer;
        private readonly IStringLocalizer<ElementResource> _elementLocalizer;

        public TranslationHelper(IStringLocalizer<ModelResource> modelLocalizer
            , IStringLocalizer<AppResource> appLocalizer
            , IStringLocalizer<ElementResource> elementLocalizer
            )
        {
            this._modelLocalizer = modelLocalizer;
            this._appLocalizer = appLocalizer;
            this._elementLocalizer = elementLocalizer;
        }
        public string GetModelLabelTranslation<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression.Body is MemberExpression memberExpression)
            {
                var propertyName = memberExpression.Member.Name;

                var translation = _modelLocalizer[propertyName];
                if (translation.ResourceNotFound)
                {
                    return $"TNF: {propertyName}";
                }

                // var requiredAttribute = memberExpression.Member.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
                //
                // if (requiredAttribute != null)
                // {
                //     var mandatoryString = translation.Value;
                //     return mandatoryString += "*";
                // }

                return translation;
            }
            throw new ArgumentException("Invalid property expression");
        }

        public string GetAppTranslation(string key)
        {
            var translation = _appLocalizer[key];
            if (translation.ResourceNotFound)
            {
                return $"TNF: {key}";
            }
            return translation;
        }

        public string GetElementTranslation(string key)
        {
            var translation = _elementLocalizer[key];
            if (translation.ResourceNotFound)
            {
                return $"TNF: {key}";
            }
            return translation;
        }
    }
}
