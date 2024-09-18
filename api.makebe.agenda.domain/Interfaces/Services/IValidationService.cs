namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IValidationService<T> where T : class
    {
        Task<bool> Validar(T item);
        void RetornarListaVazia(string entidade, string mensagem);
    }
}
