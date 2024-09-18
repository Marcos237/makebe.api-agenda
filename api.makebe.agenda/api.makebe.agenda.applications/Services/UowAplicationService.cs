using api.makebe.agenda.infra.data.interfaces;

namespace api.makebe.agenda.applications.Services
{
    public class UowAplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UowAplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void BeginTransaction()
        {
            _unitOfWork.BeginTransaction();
        }
        public void Commit()
        {
            _unitOfWork.Commit();
        }
        public void RollBack()
        {
            _unitOfWork.RollBack();
        }
    }
}
