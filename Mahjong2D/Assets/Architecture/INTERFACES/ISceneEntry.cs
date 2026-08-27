using System.Collections;
using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ISceneEntry
{
    UniTask Initialize(DIContainer container);
    UniTask BeforeShutdown();
    UniTask ShutDown();
}
