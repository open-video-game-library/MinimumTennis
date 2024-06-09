using UnityEngine;

public interface IConnectionManager
{
    public bool Active { get; set; }
    public bool ManageConnection();
    public InputMethod ReturnInputMethod();
}
