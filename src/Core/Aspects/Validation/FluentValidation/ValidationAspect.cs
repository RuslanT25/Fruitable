using Castle.DynamicProxy;
using Core.Entities.Concrete;
using Core.Helpers.Interceptors;
using Entities.Abstract;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Core.Aspects.Validation.FluentValidation
{
    public class ValidationAspect<T> : MethodInterception
        where T : class, IEntity, new()
    {
        private readonly Type _validatorType;

        public ValidationAspect(Type type)
        {
            if (!typeof(IValidator).IsAssignableFrom(type))
            {
                throw new ArgumentException("This is not a validator class!", nameof(type));
            }

            _validatorType = type;
        }
            
        protected override void OnBefore(IInvocation invocation)
        {

            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            //UserValidator: AbstractValidator<User>  basetype AbstractValidator<User>, [0] = User

            var entities = invocation.Arguments.Where(t => t != null && t.GetType() == entityType);
            foreach (T entity in entities)
            {
                var validationContext = new ValidationContext<T>(entity);
                var result = validator.Validate(validationContext);
                if (!result.IsValid)
                {
                    throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
                }
            }
        }
    }
}
