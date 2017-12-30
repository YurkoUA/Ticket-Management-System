function OnBegin() {
    $('#package-result').html('');
    setButtonLoadingState();
}

function OnComplete(request, status) {
    resetButtonLoadingState();
}

// For delete action.
function OnSuccess(data) {
    $('#package-toolbar').html('');
}

// Function to check all checkboxes in "MoveUnallocatedPartial".
function checkAll() {
    $("#move-unallocated-grid input[type='checkbox']").each(function (index, element) {
        element.setAttribute("checked", "true");
    });
}

function onToolbarSelectionChanged() {
    $('#package-result').html('');
}

function OnPackageOpenedSuccess(data) {
    location.reload();
}
