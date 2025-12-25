namespace Api.Models;

public record BaseResponse(int Id)
{
    public static implicit operator BaseResponse(int id) => new(id);

    public static implicit operator int(BaseResponse response) => response.Id;
};
