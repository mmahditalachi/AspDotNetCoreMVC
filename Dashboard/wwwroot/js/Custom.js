function confrimDelete(id, isDeleted) {
    var currentId = 'delete_' + id;
    var confrimId = 'confrimDelete_' + id;

    if (isDeleted) {
        $('#' + confrimId).show();
        $('#' + currentId).hide();
    }
    else {
        $('#' + confrimId).hide();
        $('#' + currentId).show();
    }
}
