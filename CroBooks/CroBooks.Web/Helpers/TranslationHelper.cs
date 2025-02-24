
using CroBooks.Web.Resources;
using CroBooks.Web.Resources.Elements;
using CroBooks.Web.Resources.Models;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace CroBooks.Web.Helpers
{
    public class TranslationHelper
    {
        private readonly IStringLocalizer<ModelResource> modelLocalizer;
        private readonly IStringLocalizer<AppResource> appLocalizer;
        private readonly IStringLocalizer<ElementResource> elementLocalizer;

        public TranslationHelper(IStringLocalizer<ModelResource> modelLocalizer
            , IStringLocalizer<AppResource> appLocalizer
            , IStringLocalizer<ElementResource> elementLocalizer
            )
        {
            this.modelLocalizer = modelLocalizer;
            this.appLocalizer = appLocalizer;
            this.elementLocalizer = elementLocalizer;
        }
        public string GetModelLabelTranslation<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression.Body is MemberExpression memberExpression)
            {
                var propertyName = memberExpression.Member.Name;

                var translation = modelLocalizer[propertyName];
                if (translation.ResourceNotFound)
                {
                    return $"Translation not found: {propertyName}";
                }

                var requiredAttribute = memberExpression.Member.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

                if (requiredAttribute != null)
                {
                    var mandatoryString = translation.Value;
                    return mandatoryString += "*";
                }

                return translation;
            }
            throw new ArgumentException("Invalid property expression");
        }

        public string GetAppTranslation(string key)
        {
            var translation = appLocalizer[key];
            if (translation.ResourceNotFound)
            {
                return $"Translation not found: {key}";
            }
            return translation;
        }

        public string GetElementTranslation(string key)
        {
            var translation = elementLocalizer[key];
            if (translation.ResourceNotFound)
            {
                return $"Translation not found: {key}";
            }
            return translation;
        }
    }
}
