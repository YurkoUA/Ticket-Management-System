function OnBegin() {
    onToolbarSelectionChanged();
    setButtonLoadingState();
}

function OnComplete(request, status) {
    resetButtonLoadingState();
}

// For delete action.
function onDeletingSuccess(data) {
    $('#serial-toolbar').html('');
}

function onToolbarSelectionChanged() {
    $('#serial-result').html('');
}
