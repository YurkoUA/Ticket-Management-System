function OnBegin() {
    $('#package-result').html('');
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
