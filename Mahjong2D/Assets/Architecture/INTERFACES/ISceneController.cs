using System.Collections;
using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ISceneController
{
    UniTask Initialize(DIContainer container);
    UniTask ShutDown();
}
