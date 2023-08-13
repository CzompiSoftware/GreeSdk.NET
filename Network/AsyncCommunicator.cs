using System.ComponentModel;

namespace GreeSdk.Network;

public abstract class AsyncCommunicator<TParams, TProgress, TResult> : IDisposable where TParams : class where TProgress : class where TResult : class
{
    private readonly BackgroundWorker backgroundWorker;
    private List<TParams> _parameters = null;
    private TProgress _progress = null;
    private List<TResult> _result = null;

    public AsyncCommunicator()
    {
        backgroundWorker = new BackgroundWorker();
    }

    protected abstract void DoBeforeExecution();
    protected virtual List<TParams> DoInBackground(params TParams[] parameters)
    {
        _parameters = parameters.ToList();
        return _parameters;
    }

    protected virtual void OnProgressUpdate(TProgress progress)
    {
        _progress = progress;
    }
    protected virtual void DoAfterExecution(params TResult[] result)
    {
        _result = result.ToList();
    }

    public void ExecuteAsync()
    {
        DoBeforeExecution();
        backgroundWorker.DoWork += (s, e) => { DoInBackground(_parameters.ToArray()); };
        backgroundWorker.ProgressChanged += (s, e) => { OnProgressUpdate(_progress); };
        backgroundWorker.RunWorkerCompleted += (s, e) => { DoAfterExecution(_result.ToArray()); };
        backgroundWorker.RunWorkerAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _parameters = null;
        _progress = null;
        _result = null;
        backgroundWorker.Dispose();
    }
}