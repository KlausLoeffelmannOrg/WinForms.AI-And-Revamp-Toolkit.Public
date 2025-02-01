namespace CommunityToolkit.WinForms.AsyncSupport;

public readonly struct AsyncEventInvoker<TAsyncEventArgs>(
    AsyncEventHandler<TAsyncEventArgs> asyncEventHandler,
    bool autoDisableWhenBusy,
    bool parallelInvoke)
    where TAsyncEventArgs : EventArgs
{
    public async Task RaiseEventAsync(
        object sender,
        TAsyncEventArgs e)
    {
        var control = (Control)sender;

        if (asyncEventHandler is null)
            return;

        bool previousEnabled = true;

        if (autoDisableWhenBusy)
        {
            if (control.InvokeRequired)
            {
                await control.InvokeAsync(
                    () => previousEnabled = control.Enabled);
            }
            else
            {
                previousEnabled = control.Enabled;
                control.Enabled = false;
            }
        }

        try
        {
            IEnumerable<Task> eventHandlerTasks = asyncEventHandler
                .GetInvocationList()
                .OfType<AsyncEventHandler<TAsyncEventArgs>>()
                .Select(handler => handler.DynamicInvoke(sender, e) as Task)
                .OfType<Task>();

            if (parallelInvoke)
            {
                // Invoke each handler in parallel
                await Task.WhenAll(eventHandlerTasks).ConfigureAwait(false);
            }
            else
            {
                // Invoke each handler safely
                foreach (var task in eventHandlerTasks)
                {
                    await task.ConfigureAwait(false);
                }
            }
        }
        finally
        {
            if (autoDisableWhenBusy)
            {
                // We do not check for cross-thread-ness here, because we want 
                // to post and not send the message here for better WinForms responsiveness.
                await control.InvokeAsync(
                    () => control.Enabled = previousEnabled);
            }
        }
    }
}
