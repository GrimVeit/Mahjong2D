using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectMaterialPresenter : IParticleEffectMaterialProvider
{
    private readonly ParticleEffectMaterialModel _model;
    private readonly ParticleEffectMaterialView _view;

    public ParticleEffectMaterialPresenter(ParticleEffectMaterialModel model, ParticleEffectMaterialView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    private void ActivateEvents()
    {
        _model.OnActivate += _view.Activate;
        _model.OnDeactivate += _view.Deactivate;
    }

    private void DeactivateEvents()
    {
        _model.OnActivate -= _view.Activate;
        _model.OnDeactivate -= _view.Deactivate;
    }

    #region Input

    public void Activate() => _model.Activate();
    public void Deactivate() => _model.Deactivate();

    #endregion
}

public interface IParticleEffectMaterialProvider
{
    void Activate();
    void Deactivate();
}
