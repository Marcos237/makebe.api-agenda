using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using FluentValidation;

namespace api.makebe.agenda.domain.Services
{
    public class UsuarioLojaDomainService : IUsuarioLojaDomainService
    {
        private readonly IUsuarioLojaRepository _usuarioLojaRepository;
        private readonly IValidator<UsuarioLoja> _validator;
        private readonly INotificationContext _notificationContext;
        public UsuarioLojaDomainService(IUsuarioLojaRepository usuarioLojaRepository, IValidator<UsuarioLoja> validator, INotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
            _usuarioLojaRepository = usuarioLojaRepository;
            _validator = validator;
        }
        public Task<UsuarioLoja> Salvar(UsuarioLoja loja)
        {
            throw new NotImplementedException();
        }
    }
}
