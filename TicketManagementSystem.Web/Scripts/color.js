function OnBegin() {
    setButtonLoadingState();
    onToolbarSelectionChanged();
}

// For delete action.
function onDeletingSuccess(data) {
    $('#color-toolbar').html('');
}

function onToolbarSelectionChanged() {
    $('#color-result').html('');
}

function OnComplete(request, status) {
    resetButtonLoadingState();
}
