$(document).ready(function () {
    LoadData(); 
    $('.btn-create-user').on('click', function () {
        var user = {};
        user.Username = $('#userName').val();
        user.Email = $('#userEmail').val();
        user.SchoolName = $('#schoolName').val();
        user.Username = $('#userAddress').val();
        user.Username = $('#phoneNumber').val();
        urldetail = '/Admin/CreateUser';
        $.ajax({
            type: "POST",
            url: urldetail,
            data: 
        });
    });
});