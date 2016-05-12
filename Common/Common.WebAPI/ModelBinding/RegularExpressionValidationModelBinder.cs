using System;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Common.Utils;
using Common.WebApi.Resources;

namespace Common.WebApi.ModelBinding
{
    public class RegularExpressionValidationModelBinder : IModelBinder
    {
        private readonly string _regularExpression;
        private readonly string _errorMessage;
        private readonly bool _allowNull;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="regularExpression">regular expression which will be used to validate the model value, not null or empty.</param>
        /// <param name="errorMessage">the error message if the validation fails, not null or empty.</param>
        /// <param name="allowNull">indicate if null is acceptable, default is false.</param>
        public RegularExpressionValidationModelBinder(string regularExpression, string errorMessage, bool allowNull = false)
        {
            Guard.ArgumentNotNullOrEmpty(regularExpression, "regularExpression");
            Guard.ArgumentNotNullOrEmpty(errorMessage, "errorMessage");

            _regularExpression = regularExpression;
            _errorMessage = errorMessage;
            _allowNull = allowNull;
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            // the binder only supports string type.
            if (bindingContext.ModelType != typeof(string))
            {
                throw new ArgumentException(Messages.Error_ModelTypeShouldBeString, bindingContext.ModelName);
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var obj = valueProviderResult?.RawValue;

            var isSuccess = true;

            if (obj == null)
            {
                // if not allow null and the obj is null, then add the model error and set isSuccess to false.
                if (!_allowNull)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, _errorMessage);
                    isSuccess = false;
                }
            }
            else
            {
                var strValue = Convert.ToString(obj);

                // check if the str value match the regular expression pattern, if not, then add the model error and set isSuccess to false.
                if (!Regex.IsMatch(strValue, _regularExpression))
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, _errorMessage);
                    isSuccess = false;
                }
            }

            // set the obj to model.
            bindingContext.Model = obj;
            return isSuccess;
        }
    }
}
