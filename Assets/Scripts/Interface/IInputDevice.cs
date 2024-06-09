using UnityEngine;

public interface IInputDevice
{
    public Vector2 GetMoveInput(Players player);
    public bool GetNormalShotInput(Players player);
    public bool GetLobShotInput(Players player);
    public bool GetFastShotInput(Players player);
    public bool GetTossInput(Players player);
    public bool GetServeInput(Players player);
    public bool GetDropShotInput(Players player);
    public bool GetEscapeInput(Players player);
}