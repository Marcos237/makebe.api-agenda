using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.infra.crosscutting.Notifications;

namespace api.makebe.agenda.applications.Helpers
{
    public static class ResponseModelHelper<T> where T : class
    {
        public static ResponseModel<T> RetornarResponseModel(T objeto, IEnumerable<T> objetos,  IEnumerable<Notification> notifications)
        {
            return new ResponseModel<T>
            {
                data = objeto,
                notifications = notifications,
                datas = objetos
            };
        }

        public static ResponseModel<T> RetornarResponseModel(T objeto, IEnumerable<Notification> notifications)
        {
            return new ResponseModel<T>
            {
                data = objeto,
                notifications = notifications
            };
        }
    }
}
