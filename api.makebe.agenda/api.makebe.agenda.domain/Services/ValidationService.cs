using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using FluentValidation;

namespace api.makebe.agenda.domain.Services
{
    public class ValidationService<T> : IValidationService<T> where T : class
    {
        private readonly INotificationContext _notificationContext;
        private readonly IValidator<T> _validator;
        public ValidationService(INotificationContext notificationContext, IValidator<T> validator)
        {
            _notificationContext = notificationContext;
            _validator = validator;
        }

        public void RetornarListaVazia(string entidade, string mensagem)
        {
            _notificationContext.AddNotification(entidade,mensagem);
        }

        public async Task<bool> Validar(T item)
        {
            var isValid = await _validator.ValidateAsync(item);
            if (!isValid.IsValid)
            {
                isValid.Errors.ForEach(x =>
                {
                    _notificationContext.AddNotification(x.PropertyName,
                    x.ErrorMessage.ToString(), isValidate: true);
                });
                return isValid.IsValid;
            }
            return isValid.IsValid;
        }
    }
}
