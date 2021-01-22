
namespace RepositoryPattern.Sample.Features.Home.Error
{
    public class ErrorViewModel
    {
        public string RequestId {get; private set;}

        public ErrorViewModel(string requestId){
            if(requestId is null)
                throw new System.ArgumentException("Provide a valid request identifier", nameof(requestId));
            this.RequestId = requestId;
        }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
