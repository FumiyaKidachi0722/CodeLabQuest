// Assets/Scripts/Core/ICommand.cs
using UnityEngine;
using System.Collections;

/// <summary>
/// キャラクターに対する命令を表すインタフェース。
/// Execute はコルーチンとして動作し、Transform actor を操作する。
/// </summary>
public interface ICommand
{
    IEnumerator Execute(Transform actor);
}
