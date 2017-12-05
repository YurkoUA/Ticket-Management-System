$("a#link-tickets-today").each(function (index, element) {
    var href = $(element).attr("href") + "?timezoneOffset=" + new Date().getTimezoneOffset();
    $(element).attr("href", href);
});