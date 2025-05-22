using System.Runtime.InteropServices;

namespace Audora.Application.Common.Services
{
    public abstract class ResponseAttacher<TSelf, TResponse>
        where TSelf : ResponseAttacher<TSelf, TResponse>
        where TResponse : class
    {
        protected static readonly Dictionary<string, object> SharedArguments = [];
        protected readonly List<Action<TResponse>> Attachments = [];

        protected TResponse SingleResponse = null!;
        protected List<TResponse> ResponseCollection = null!;

        protected void AddAttachment(Action<TResponse> attachment)
        {
            Attachments.Add(attachment);
        }

        protected TSelf Attach(
            Func<Task> singleLogic,
            Func<Task> collectionLogic)
        {
            if (SingleResponse is not null)
                singleLogic().GetAwaiter().GetResult();
            else if (ResponseCollection is not null)
                collectionLogic().GetAwaiter().GetResult();
            else
                throw new InvalidOperationException("No response is set.");

            return (TSelf)this;
        }

        public TSelf AttachTo(TResponse response)
        {
            SingleResponse = response ?? throw new ArgumentNullException(nameof(response));
            return (TSelf)this;
        }

        public TSelf AttachTo(List<TResponse> podcastResponses)
        {
            ResponseCollection = podcastResponses ?? throw new ArgumentNullException(nameof(podcastResponses));
            return (TSelf)this;
        }

        public List<TResponse> GetResponseCollection()
        {
            var responsesSpan = CollectionsMarshal.AsSpan(ResponseCollection);
            var attachmentsSpan = CollectionsMarshal.AsSpan(Attachments);

            for (int i = 0; i < responsesSpan.Length; i++)
            {
                var response = responsesSpan[i];
                for (int j = 0; j < attachmentsSpan.Length; j++)
                {
                    attachmentsSpan[j](response);
                }
            }

            return ResponseCollection;
        }

        public TResponse GetSingleResponse()
        {
            return SingleResponse ?? throw new InvalidOperationException("No single response is set.");
        }
    }
}