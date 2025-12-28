namespace Api.Models;

public record BaseResponse<T>(T Id)
    where T : struct
{
    public static implicit operator BaseResponse<T>(T id) => new(id);

    public static implicit operator T(BaseResponse<T> response) => response.Id;
};

public record BaseResponse(Guid Id) : BaseResponse<Guid>(Id)
{
    public static implicit operator BaseResponse(Guid id) => new(id);

    public static implicit operator Guid(BaseResponse response) => response.Id;
};
