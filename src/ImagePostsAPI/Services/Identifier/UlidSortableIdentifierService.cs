namespace ImagePostsAPI.Services.Identifier;

public class UlidSortableIdentifierService: ISortableIdentifierService
{
    public string Generate()
    {
        return Ulid.NewUlid().ToString();
    }
}