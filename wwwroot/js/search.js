function searchUser() {
    var username = $.trim($('#Username').val());
    if (!username) {
        return;
    }
    var form = $('#searchForm');
    $.ajax({
        url: form.attr('action') || '/Home/Index',
        type: 'POST',
        data: { userName: username },
        success: function (data) {
            var resultHtml = $('<div>').append($.parseHTML(data)).find('#searchResults').html();
            $('#searchResults').html(resultHtml || data);
        },
        error: function (xhr, status, error) {
            $('#searchResults').html('<p class="text-danger">An error occurred while searching for users.</p>');
        },
        complete: function () {
            
        }

    });
}