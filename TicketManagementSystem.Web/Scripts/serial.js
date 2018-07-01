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

function onToolbarSelectionChangedSuccess(data) {
    var title = $("#partial-view-title").val();

    if (title != undefined) {
        $("h2#page-title").html(title);
    }
}
