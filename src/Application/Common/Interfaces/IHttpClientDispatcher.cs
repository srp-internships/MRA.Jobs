namespace MRA.Jobs.Application.Common.Interfaces;
public interface IHttpClientDispatcher<TCreateRequest, TCreateResponse, TPassEntity>
{
    Task<TCreateResponse> SendTestCreationRequest(TCreateRequest request);
    Task<TPassEntity> SendTestPassRequest(TPassEntity request);
}
