$(document).ready(function () {
    $('.delete a').on('click', function (e) {
        var $tr = $(e.target).closest('tr');
        var playerName = $tr.find('.playerName').text().trim();
        
        var dialog = confirm('Are you sure you want to delete ' + playerName + '\'s record?');
        if (dialog == true) {
            var id = $(e.target).data('id');
            var url = location.pathname + '/ModeratorDeleteRecord?id=' + id;
            window.location.href = url;
        }
    });
});