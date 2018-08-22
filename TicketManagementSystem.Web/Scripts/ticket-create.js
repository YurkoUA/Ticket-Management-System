var notesArray = [];

$("#note-dropdown").on('click', 'li', function () {
    var text = $(this).attr('text-to-append');

    console.log('to append: ' + text);

    if (text !== undefined) {
        $('#Note').val(text);
    }
});

function OnBegin() {
    $('#create-result').html("");
    setButtonLoadingState();
}

function OnComplete(request, status) {
    resetButtonLoadingState();
}

function OnSuccess(data) {
    pushNoteToArray($("#Note").val());

    $('#Number').val('');
    $('#SerialNumber').val('');
    $('#Note').val('');
    $('#Date').val('');

    $('#Number').focus();
}

function pushNoteToArray(note) {
    if (note !== undefined && notesArray.indexOf(note) < 0) {
        note = note.replace(new RegExp('#', 'g'), '№');

        notesArray.push(note);
        appendToDropdown(note);

        console.log('pushed: ' + note);
    }
}

function appendToDropdown(note) {
    $('#note-dropdown').prepend('<li text-to-append="' + note + '"><a>' + getShortNoteText(note) + '</a></li>');
}

function getShortNoteText(note) {
    var maxLength = 20;

    if (note.length > maxLength)
        note = note.substr(0, maxLength) + "...";

    return note;
}
