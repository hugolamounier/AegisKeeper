using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AegisKeeper.Client.Components.Common;

public partial class InsertFormModal<TEntity> : ComponentBase
{
    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public TEntity Entity { get; set; }
    
    
    

    private void Cancel() => MudDialog.Cancel();
}