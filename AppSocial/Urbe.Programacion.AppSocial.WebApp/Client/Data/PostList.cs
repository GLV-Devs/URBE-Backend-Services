using System.Collections;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Data;

public class PostList : IReadOnlyCollection<PostViewModel>
{
    private readonly List<PostViewModel> _postList = new();

    public event Action<PostList, PostViewModel>? PostAdded;
    public event Action<PostList>? ListCleared;

    public IEnumerator<PostViewModel> GetEnumerator()
    {
        return ((IEnumerable<PostViewModel>)_postList).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_postList).GetEnumerator();
    }

    public PostViewModel this[int index] 
        => ((IList<PostViewModel>)_postList)[index];

    public void Add(IEnumerable<PostViewModel> items)
    {
        foreach (var item in items)
            Add(item);
    }

    public void Add(PostViewModel item)
    {
        _postList.Add(item);
        PostAdded?.Invoke(this, item);
    }

    public void Clear()
    {
        _postList.Clear();
        ListCleared?.Invoke(this);
    }

    public bool Contains(PostViewModel item)
    {
        return _postList.Contains(item);
    }

    public void CopyTo(PostViewModel[] array, int arrayIndex)
    {
        _postList.CopyTo(array, arrayIndex);
    }

    public int Count => _postList.Count;
}
